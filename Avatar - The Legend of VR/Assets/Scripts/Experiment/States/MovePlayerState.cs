using System.Linq;

public class MovePlayerState : IState
{
    private enum PlayerState
    {
        NotDone,
        FinishedMovement,
        ReachedGoalField,
        ReachedAvatarSelection
    }
    private readonly TrialInfo _currentTrial;
    private readonly Player _player;
    private readonly TrialManager _trialManager;

    private bool _companionExitedMove;
    private PlayerState _movementState;
    public bool FinishedMovement => _companionExitedMove && _movementState == PlayerState.FinishedMovement;
    public bool ReachedGoalField => _movementState == PlayerState.ReachedGoalField;

    public bool ReachedAvatarSelection => _companionExitedMove && _movementState == PlayerState.ReachedAvatarSelection;

    public MovePlayerState(Player player, TrialManager trialManager, TrialInfo currentTrial)
    {
        _player = player;
        _trialManager = trialManager;
        _currentTrial = currentTrial;
    }

    public void OnStateEnter()
    {
        // _c = false;
        // _s = PlayerState.NotDone;
        _companionExitedMove = false;
        _movementState = PlayerState.NotDone;
        var previouslySelectedCard = _currentTrial.cardsPicked.Last();

        _player.onMoveExit.AddListener(OnMoveDone);
        _player.onGoalFieldReached.AddListener(OnGoalReached);
        _player.onAvatarSelectFieldReached.AddListener(OnAvatarSelectReached);

        _player.Move((int)previouslySelectedCard);
        if (_trialManager.companion != null)
        {
            _player.StartCoroutine(_trialManager.companion.Move((int)previouslySelectedCard, _player));
            _trialManager.companion.onMoveExit.AddListener(OnCompanionMoveDone);
        }
        else _companionExitedMove = true;
    }

    private void OnCompanionMoveDone() => _companionExitedMove = true;

    private void OnMoveDone() => _movementState = PlayerState.FinishedMovement;

    private void OnGoalReached() => _movementState = PlayerState.ReachedGoalField;

    private void OnAvatarSelectReached() => _movementState = PlayerState.ReachedAvatarSelection;

    // private bool _c;
    // private PlayerState _s;
    // private int _i;
    public void Tick()
    {
        // if (_s != _movementState)
        // {
        //     _s = _movementState;
        //     Debug.Log($"MOVE: {{{_i++:00}}}: {_s}");
        // }
        //
        // if (_companionExitedMove)
        // {
        //     _c = _companionExitedMove;
        //     Debug.Log($"MOVE: {{{_i++:00}}}Comp Move Done: {_c}");
        // }
    }

    public void OnStateExit()
    {
        _player.onMoveExit.RemoveListener(OnMoveDone);
        _player.onGoalFieldReached.RemoveListener(OnGoalReached);
        _player.onAvatarSelectFieldReached.RemoveListener(OnAvatarSelectReached);
        if (_trialManager.companion != null)
        {
            _trialManager.companion.onMoveExit.RemoveListener(OnCompanionMoveDone);
        }

        _player.StopAllCoroutines();
    }


}
