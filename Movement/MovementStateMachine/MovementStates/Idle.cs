using UnityEngine;

public class Idle : MovementState
{
    public Idle(MovementController controller) : base(controller) { }

    public override Vector3 HandleExternalForces()
    {
        // Currently only accounting for gravity
        var velocity = new Vector3(controller.CurrentVelocity.x, controller.Gravity, controller.CurrentVelocity.z);
        return velocity;
    }

    public override void ManageMove(PlayerInput.Input input)
    {
        if(input.Started || input.Performed)
        {
            var value = input.GetValue<Vector2>();
            var direction = new Vector3(value.x, 0, value.y);
            controller.InputDirection = direction;

            controller.ChangeState(new Walking(controller));
        }
    }

    public override void ManageDash(PlayerInput.Input input)
    {
        if (input.Started || input.Performed)
        {
            controller.ChangeState(new Dashing(controller));
        }
    }
}
