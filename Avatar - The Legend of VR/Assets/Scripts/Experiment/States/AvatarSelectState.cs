using UnityEngine;
using Valve.VR.Extras;
using VRAvatar;

public class AvatarSelectState : IState
{
    private readonly VRPlayer _vrPlayer;
    private readonly Player _player;
    private readonly TrialManager _trialManager;
    private readonly TrialInfo _trialInfo;

    private AvatarSelectionResponse _response;
    
    public bool AvatarSelected { get; private set; }
    
    public AvatarSelectState(VRPlayer vrPlayer, Player player, TrialManager trialManager, TrialInfo trialInfo)
    {
        _vrPlayer = vrPlayer;
        _player = player;
        _trialManager = trialManager;
        _trialInfo = trialInfo;
    }

    public void OnStateEnter()
    {
        AvatarSelected = false;
        _response = new();
        _response.StartTimer();
        _vrPlayer.rightHand.laserPointer.onPointerClick.AddListener(OnAvatarSelected);
        _vrPlayer.rightHand.laserPointer.onAvatarHovered.AddListener(ChangePointerColorOnHover);
        _vrPlayer.rightHand.laserPointer.LaserPointerEnabled = true;
    }

    private void ChangePointerColorOnHover(Avatar avatar, SteamVR_LaserPointer laserPointer)
    {
        var avatarField = _player.currentField as AvatarField;
        var leftLoD = avatarField!.leftOption.questionnaireMatch;
        var rightLoD = avatarField!.rightOption.questionnaireMatch;
        if(avatar.QuestionnaireMatch == leftLoD || avatar.QuestionnaireMatch == rightLoD)
            laserPointer.color = Color.blue;
    }
    

    private void OnAvatarSelected(GameObject go)
    {
        if (go.CompareTag("Avatar"))
        {
            var chosenAvatar = go.GetComponent<Avatar>();

            // get both level of matches from each avatar
            var avatarField = _player.currentField as AvatarField;
            var leftLofM = avatarField!.leftOption.questionnaireMatch;
            var rightLofM = avatarField!.rightOption.questionnaireMatch;

            // if avatar was shot which is not a current option
            if (chosenAvatar.QuestionnaireMatch != leftLofM && chosenAvatar.QuestionnaireMatch != rightLofM)
                return;
            
            // save the match level of chosen avatar
            _response.levelOfMatchChosen = chosenAvatar.QuestionnaireMatch;

            // check if we matched the right or left and save the opposing avatars level of match.
            var choseLeft = chosenAvatar.QuestionnaireMatch == leftLofM;
            _response.levelOfMatchOther = choseLeft? rightLofM : leftLofM;
            
            // check if we selected the same avatar as before if there is one.
            _response.keptSameAvatar = _trialManager.companion != null && _trialManager.companion.QuestionnaireMatch == chosenAvatar.QuestionnaireMatch;

            _trialManager.companion = chosenAvatar;
            AvatarSelected = true;
        }
    }

    public void Tick() { }

    public void OnStateExit()
    {
        _vrPlayer.rightHand.laserPointer.LaserPointerEnabled = false;
        _vrPlayer.rightHand.laserPointer.onPointerClick.RemoveListener(OnAvatarSelected);
        _vrPlayer.rightHand.laserPointer.onAvatarHovered.RemoveListener(ChangePointerColorOnHover);

        
        // end timer and add the response data to the trial info
        _response.EndTimer();
        _trialInfo.avatarsChosen.Add(_response);
        _response = null;
    }
}
