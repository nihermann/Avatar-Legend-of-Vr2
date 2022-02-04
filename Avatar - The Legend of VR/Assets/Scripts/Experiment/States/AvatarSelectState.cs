using UnityEngine;
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
        _vrPlayer.rightHand.laserPointer.LaserPointerEnabled = true;
    }

    private void OnAvatarSelected(GameObject go)
    {
        if (go.CompareTag("Avatar"))
        {
            _response.levelOfMatchChosen = go.GetComponent<Avatar>().QuestionnaireMatch;
            
            var avatarField = _player.currentField;
            var leftLofM = avatarField.leftOption.questionnareMatch;
            var rightLofM = avatarField.rightOption.questionnareMatch;

            var choseLeft = _response.levelOfMatchChosen == leftLofM;
            _response.levelOfMatchOther = choseLeft? rightLofM : leftLofM;

            _trialManager.companion = go.GetComponent<Player>();
            AvatarSelected = true;
        }
    }

    public void Tick() { }

    public void OnStateExit()
    {
        _vrPlayer.rightHand.laserPointer.LaserPointerEnabled = false;
        _vrPlayer.rightHand.laserPointer.onPointerClick.RemoveListener(OnAvatarSelected);
        
        // end timer and add the response data to the trial info
        _response.EndTimer();
        _trialInfo.avatarsChosen.Add(_response);
        _response = null;
    }
}
