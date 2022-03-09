using UnityEngine;
using VRAvatar;


/// <summary>
/// Manages the flow of each trial repeatedly.
/// </summary>
public class TrialManager : MonoBehaviour
{
    /// <summary>
    /// The sate machine responsible for the correct transitions between states.
    /// </summary>
    private readonly StateMachine _stateMachine = new();
    
    /// <summary>
    /// The VR logic of the participant.
    /// </summary>
    [SerializeField] private VRPlayer vrPlayer;
    /// <summary>
    /// The game board logic of the player.
    /// </summary>
    [SerializeField] private Player player;
    
    /// <summary>
    /// Reference to the accompanying Avatar of the participant.
    /// </summary>
    public Avatar companion;
    
    /// <summary>
    /// All relevant information about the current trial. 
    /// </summary>
    private TrialInfo _currentTrial;

    /// <summary>
    /// Instantiate the trial, avatars, all states and define their transitions and conditions.
    /// </summary>
    public void Awake()
    {
        // Get all relevant information for this new trial.
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
        
        
        // move vrPlayer --(if enough hand cards)--> select card
        _stateMachine.AddTransition(movePlayer, selectCard, () => movePlayer.FinishedMovement && vrPlayer.leftHand.handCards.NumberOfCards > 0);
        // move vrPlayer --(if avatar selection reached)--> avatarSelection
        _stateMachine.AddTransition(movePlayer, avatarSelection, () => movePlayer.ReachedAvatarSelection);
        // move vrPlayer --(if no more cards on hand)--> refill hand cards
        _stateMachine.AddTransition(movePlayer, refillHandCards, () => movePlayer.FinishedMovement && vrPlayer.leftHand.handCards.NumberOfCards <= 0);
        // move vrPlayer --(if goal reached)--> save trial
        _stateMachine.AddTransition(movePlayer, done, () => movePlayer.ReachedGoalField);
        
        // avatar selection --(if enough hand cards)--> select card
        _stateMachine.AddTransition(avatarSelection, selectCard, () => avatarSelection.AvatarSelected && vrPlayer.leftHand.handCards.NumberOfCards > 0);
        // avatar selection --(no more hand cards)--> refill hand cards
        _stateMachine.AddTransition(avatarSelection, refillHandCards, () => avatarSelection.AvatarSelected && vrPlayer.leftHand.handCards.NumberOfCards <= 0);
        
        _stateMachine.AddAnyTransition(done, () => Input.GetKeyDown(KeyCode.S));
        
        // Start state
        // --()--> refill hand cards
        _stateMachine.SetState(refillHandCards);
    }

    /// <summary>
    /// Updates <see cref="StateMachine"/>.
    /// </summary>
    private void Update() => _stateMachine.Tick();
    
}

