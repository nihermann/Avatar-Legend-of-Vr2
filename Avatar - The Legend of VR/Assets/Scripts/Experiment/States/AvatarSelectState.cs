using UnityEngine;
using Valve.VR.Extras;
using VRAvatar;

/// <summary>
/// This State expects the participant staying on an <see cref="AvatarField"/>.
/// It will handle the logic of selecting an avatar.
/// </summary>
public class AvatarSelectState : IState
{
    #region State Context
    
    /// <summary>
    /// VR dependencies of the participant.
    /// </summary>
    private readonly VRPlayer _vrPlayer;
    
    /// <summary>
    /// Experiment specific logic of the participant.
    /// </summary>
    private readonly Player _player;
    
    /// <summary>
    /// ref to the trial manager as he knows about the current companion of the participant.
    /// </summary>
    private readonly TrialManager _trialManager;
    
    /// <summary>
    /// ref to the all information about the current trial.
    /// Used to set data about the selection process.
    /// </summary>
    private readonly TrialInfo _trialInfo;
    
    /// <summary>
    /// Cam used to track where the participant is looking approximately.
    /// </summary>
    private readonly Transform _cam;

    #endregion
    
    #region Data

    /// <summary>
    /// DataObject which stores all relevant information about the selection process.
    /// </summary>
    private AvatarSelectionResponse _response;

    #endregion
    
    /// <summary>
    /// Will be set to true if the participant completed the selection successfully so the next state can be entered.
    /// </summary>
    public bool AvatarSelected { get; private set; }
    
    public AvatarSelectState(VRPlayer vrPlayer, Player player, TrialManager trialManager, TrialInfo trialInfo)
    {
        _vrPlayer = vrPlayer;
        _player = player;
        _trialManager = trialManager;
        _trialInfo = trialInfo;
        // cache the camera from the VR player.
        _cam = vrPlayer.GetComponentInChildren<Camera>()?.transform;
    }

    /// <inheritdoc/>
    public void OnStateEnter()
    {
        #region Reset Flags And Variables

        AvatarSelected = false;

        _response = new();
        _response.StartTimer();
        
        #endregion
        
        #region Enable Laser Pointer And Subscribe To Its Events
        
        _vrPlayer.rightHand.laserPointer.onPointerClick.AddListener(OnClicked);
        _vrPlayer.rightHand.laserPointer.onAvatarHovered.AddListener(ChangePointerColorOnHover);
        _vrPlayer.rightHand.laserPointer.LaserPointerEnabled = true;
        
        #endregion
    }

    /// <summary>
    /// Listens to <see cref="LaserPointer"/>.onAvatarHover event.
    /// Changes the color of the laser pointer when it hits one of the two presented avatars.
    /// </summary>
    /// <param name="avatar">The avatar that was hit.</param>
    /// <param name="laserPointer">The underlying steam_vr laser pointer.</param>
    private void ChangePointerColorOnHover(Avatar avatar, SteamVR_LaserPointer laserPointer)
    {
        #region Get Selectable Avatar Options
        
        // As assumed - participant is on an avatar field which holds references to both choose able avatars.
        var avatarField = _player.currentField as AvatarField;
        var leftLoD = avatarField!.leftOption.questionnaireMatch;
        var rightLoD = avatarField!.rightOption.questionnaireMatch;
        
        #endregion
        
        // Check if the hit avatar is one of the two options (Each avatar match is only present once on the board)
        if(avatar.QuestionnaireMatch == leftLoD || avatar.QuestionnaireMatch == rightLoD)
            laserPointer.color = Color.blue;
    }
    

    /// <summary>
    /// Subscribes to <see cref="LaserPointer"/>.onPointerClick event.
    /// If the hit <see cref="go"/> is one of the selectable avatars it will record some variables and finalize the selection.
    /// </summary>
    /// <param name="go">The hit game object</param>
    private void OnClicked(GameObject go)
    {
        // if it's not tagged as an Avatar we're done.
        if (!go.CompareTag("Avatar")) return;
        
        // get the avatar script.
        var chosenAvatar = go.GetComponent<Avatar>();

        // get both level of matches from each avatar.
        var avatarField = _player.currentField as AvatarField;
        var leftLofM = avatarField!.leftOption.questionnaireMatch;
        var rightLofM = avatarField!.rightOption.questionnaireMatch;

        // if hit avatar is not one of the options return.
        if (chosenAvatar.QuestionnaireMatch != leftLofM && chosenAvatar.QuestionnaireMatch != rightLofM)
            return;
        
        #region Record Variables
        
        // save the match level of chosen avatar.
        _response.levelOfMatchChosen = chosenAvatar.QuestionnaireMatch;

        // check if we matched the right or left and save the opposing avatars level of match.
        var choseLeft = chosenAvatar.QuestionnaireMatch == leftLofM;
        _response.levelOfMatchOther = choseLeft? rightLofM : leftLofM;
        _response.wasLeftMatch = choseLeft;
            
        // check if we selected the same avatar as before if there is one.
        _response.keptSameAvatar = _trialManager.companion != null && _trialManager.companion.QuestionnaireMatch == chosenAvatar.QuestionnaireMatch;
        
        #endregion

        // change the companion of the participant and signal that we're done.
        _trialManager.companion = chosenAvatar;
        AvatarSelected = true;
    }

    /// <inheritdoc/>
    public void Tick()
    {
        if (_cam == null) return;
        
        #region Simple Eye Tracking
        
        // Cast a ray from the view point of the player and see if it hits one of the selectable avatars.
        if(Physics.Raycast(new(_cam.position, _cam.forward), out var hit))
        {
            if(hit.collider.CompareTag("Avatar"))
            {
                #region Get Selectable Avatars
                
                var avatar = hit.collider.GetComponent<Avatar>();
                // Again using the assumption that the player is on a AvatarField while selection.
                var avatarField = _player.currentField as AvatarField;
                var leftLoD = avatarField!.leftOption.questionnaireMatch;
                var rightLoD = avatarField!.rightOption.questionnaireMatch;
                
                #endregion
                
                
                // Record when one of the selectable Avatars was looked at.
                if (avatar.QuestionnaireMatch == leftLoD)
                    _response.LookedAtLeftThisFrame();
                else if (avatar.QuestionnaireMatch == rightLoD)
                    _response.LookedAtRightThisFrame();
            }
        }
        
        #endregion
    }

    /// <inheritdoc/>
    public void OnStateExit()
    {
        #region Unsubscribe From LaserPointer Events And Disable It
        
        _vrPlayer.rightHand.laserPointer.LaserPointerEnabled = false;
        _vrPlayer.rightHand.laserPointer.onPointerClick.RemoveListener(OnClicked);
        _vrPlayer.rightHand.laserPointer.onAvatarHovered.RemoveListener(ChangePointerColorOnHover);

        #endregion
        
        // end timer and add the response data to the trial info
        _response.EndTimer();
        _trialInfo.avatarsChosen.Add(_response);
        _response = null;
    }
}
