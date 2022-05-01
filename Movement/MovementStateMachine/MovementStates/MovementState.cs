using UnityEngine;
public class MovementState : IState
{
    protected MovementController controller;
    public MovementState(MovementController controller)
    {
        this.controller = controller;
    }
    public virtual void OnEnter() { }
    public virtual void ManageGrounding()
    {
        if (!controller.Grounded) controller.ChangeState(new Falling(controller));
    }
    public virtual Vector3 UpdateDirection() { return controller.TargetDirection; }
    public virtual Vector3 UpdateVelocity() { return controller.CurrentVelocity; }
    public virtual Vector3 HandleExternalForces() { return controller.CurrentVelocity; }
    public virtual void ManageMove(PlayerInput.Input input) { }
    public virtual void ManageDash(PlayerInput.Input input) { }
    public virtual void OnExit() { }
}
