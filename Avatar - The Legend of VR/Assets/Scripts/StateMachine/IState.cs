/// <summary>
/// Defines a state used with a <see cref="StateMachine"/>.
/// </summary>
public interface IState
{
    /// <summary>
    /// Will be called when the state is entered.
    /// Reset all flags and setup needed dependencies.
    /// </summary>
    public void OnStateEnter();
    
    /// <summary>
    /// Called every MonoBehaviour.Update().
    /// </summary>
    public void Tick();
    
    /// <summary>
    /// Called when the state is left.
    /// Free up memory here.
    /// Don't reset flags here it sometimes leads to unexpected behaviours/ state transitions.
    /// </summary>
    public void OnStateExit();
}
