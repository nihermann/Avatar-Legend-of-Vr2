using UnityEngine;
using VRAvatar;

public class AvatarSelectState : IState
{
    private readonly VRPlayer _vrPlayer;
    private readonly Player _player;
    private readonly TrialManager _trialManager;
    private readonly TrialInfo _trialInfo;

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
        _vrPlayer.rightHand.laserPointer.onPointerClick.AddListener(OnAvatarSelected);
        _vrPlayer.rightHand.laserPointer.LaserPointerEnabled = true;
    }

    private void OnAvatarSelected(GameObject go)
    {
        if (go.CompareTag("Avatar"))
        {
            _trialManager.companion = go.GetComponent<Player>();
            // todo save to trial info
            
            AvatarSelected = true;
        }
    }

    public void Tick() { }

    public void OnStateExit()
    {
        _vrPlayer.rightHand.laserPointer.LaserPointerEnabled = false;
        _vrPlayer.rightHand.laserPointer.onPointerClick.RemoveListener(OnAvatarSelected);
    }
}
