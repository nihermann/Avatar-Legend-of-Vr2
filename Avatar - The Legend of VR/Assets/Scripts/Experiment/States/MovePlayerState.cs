using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlayerState : IState
{
    private readonly TrialInfo _currentTrial;
    private readonly Player _player;
    private readonly TrialManager _trialManager;
    public bool FinishedMovement { get; private set; }
    public bool ReachedGoalField { get; private set; }

    public AudioSource GoalField;
    
    public bool ReachedAvatarSelection { get; private set; }

    public MovePlayerState(Player player, TrialManager trialManager, TrialInfo currentTrial)
    {
        _player = player;
        _trialManager = trialManager;
        _currentTrial = currentTrial;
    }

    public void OnStateEnter()
    {
        FinishedMovement = ReachedGoalField = ReachedAvatarSelection = false;
        var previouslySelectedCard = _currentTrial.cardsPicked.Last();
        
        _player.onMoveExit.AddListener(OnMoveDone);
        _player.onGoalFieldReached.AddListener(OnGoalReached);
        _player.onAvatarSelectFieldReached.AddListener(OnAvatarSelectReached);
        
        _player.Move((int)previouslySelectedCard);
    }

    private void OnMoveDone() => FinishedMovement = true;

    private void OnGoalReached() => ReachedGoalField = true;

    private void OnAvatarSelectReached() => ReachedAvatarSelection = true;
    
    public void Tick() { }

    public void OnStateExit()
    {
        _player.onMoveExit.RemoveListener(OnMoveDone);
        _player.onGoalFieldReached.RemoveListener(OnGoalReached);
        _player.onAvatarSelectFieldReached.RemoveListener(OnAvatarSelectReached);
        
        _player.StopAllCoroutines();
    }
    
    
}