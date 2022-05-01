/// <summary>
/// Interface for states to be used in a <see cref="MovementStateMachine"/>.
/// </summary>
public interface IState
{
    /// <summary>
    /// This method is called once, when switching from another state to this one,
    /// or on initialization.
    /// </summary>
    void OnEnter();
    /// <summary>
    /// This method is called once, when switching from this state to another one.
    /// </summary>
    void OnExit();

}
