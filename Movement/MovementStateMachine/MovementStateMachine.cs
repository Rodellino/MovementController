/// <summary>
/// Derived from <see cref="StateMachine"/>, takes and use <see cref="MovementState"/> objects to switch its behavior
/// </summary>
public class MovementStateMachine : StateMachine
{    public MovementStateMachine(MovementState state)
    {
        ChangeState(state);
    }

    /// <summary>
    /// Returns the current <see cref="MovementState"/> being used by the State Machine
    /// </summary>
    public virtual MovementState CurrentState
    {
        get { return (MovementState)currentState; }
    }
}
