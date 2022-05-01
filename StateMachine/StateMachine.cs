/// <summary>
/// A State Machine that takes and use <see cref="IState"/> objects to change its behavior
/// </summary>
public class StateMachine
{
    protected IState currentState;

    /// <summary>
    /// Switches the current <see cref="IState"/> in the state machine. This will call 
    /// <see cref="IState.OnExit"/> from the exiting state and <see cref="IState.OnEnter"/> 
    /// from the new one once it takes over
    /// </summary>
    /// <param name="state">State to be used by the state machine</param>
    public virtual void ChangeState(IState state)
    {
        if (currentState != null) currentState.OnExit();
        currentState = state;
        currentState.OnEnter();
    }
}