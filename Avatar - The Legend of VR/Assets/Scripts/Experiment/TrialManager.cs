using UnityEngine;
using VRAvatar;

public class TrialManager : MonoBehaviour
{
    private readonly StateMachine _stateMachine = new();
    
    [SerializeField] private VRPlayer vrPlayer;
    [SerializeField] private Player player;
    public Player companion;
    
    private TrialInfo _currentTrial;

    public void Awake()
    {
        _currentTrial = ExperimentController.Instance.InitNewTrial();
        FindObjectOfType<InstantiateAvatars>().Instantiate(_currentTrial.participantPreferences, _currentTrial.avatarSetupInfos);

        // give him cards
        var refillHandCards = new RefillHandCards(
            _currentTrial.cardSetupInfos.cardStaple,
            vrPlayer,
            _currentTrial.cardSetupInfos.numberOfHandCardsAtStart,
            0.5f);

        // see cards - preview them - select one
        var selectCard = new PlayerSelectCardState(vrPlayer, _currentTrial);

        // Move VRPlayer - activate navmesh from potentially following avatars
        var movePlayer = new MovePlayerState(player, this, _currentTrial);
        
        // if reached selection state, select/swap
        var avatarSelection = new AvatarSelectState(vrPlayer, player, this, _currentTrial);
        
        // if not reached but cards are empty, refill hand cards
        
        // done state
        var done = new RecordState(_currentTrial);

        // refill hand cards --()--> select card
        _stateMachine.AddTransition(refillHandCards, selectCard, () => refillHandCards.AreRefilled);
        // select card --()--> move vrPlayer
        _stateMachine.AddTransition(selectCard, movePlayer, () => selectCard.CardSelected);
        
        
        // move vrPlayer --(if enough handcards)--> select card
        _stateMachine.AddTransition(movePlayer, selectCard, () => movePlayer.FinishedMovement && vrPlayer.leftHand.handCards.NumberOfCards > 0);
        // move vrPlayer --(if avatar selection reached)--> avatarSelection
        _stateMachine.AddTransition(movePlayer, avatarSelection, () => movePlayer.ReachedAvatarSelection);
        // move vrPlayer --(if no more cards on hand)--> refill hand cards
        _stateMachine.AddTransition(movePlayer, refillHandCards, () => movePlayer.FinishedMovement && vrPlayer.leftHand.handCards.NumberOfCards <= 0);
        // move vrPlayer --(if goal reached)--> save trial
        _stateMachine.AddTransition(movePlayer, done, () => movePlayer.ReachedGoalField);
        
        // avatar selection --(if enough handcards)--> select card
        _stateMachine.AddTransition(avatarSelection, selectCard, () => vrPlayer.leftHand.handCards.NumberOfCards > 0);
        // avatar selection --(no more handcards)--> refill hand cards
        _stateMachine.AddTransition(avatarSelection, refillHandCards, () => vrPlayer.leftHand.handCards.NumberOfCards <= 0);
        
        
        // Start state
        // --()--> refill hand cards
        _stateMachine.SetState(refillHandCards);
    }

    private void Start()
    {
        // Other setups which are not related to state machine
    }

    private void Update() => _stateMachine.Tick();
    
}

