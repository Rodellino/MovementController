using UnityEngine;

public class Falling : MovementState
{

    public Falling(MovementController controller) : base(controller) { }
    
    public override void ManageGrounding()
    {
        if (controller.Grounded)
        {
            if (new Vector3(controller.CurrentVelocity.x, 0, controller.CurrentVelocity.z) == Vector3.zero)
            {
                // Switch to Idle if there was no horizontal velocity once grounded
                controller.ChangeState(new Idle(controller));
            }
            else
            {
                // Else switch to walking
                controller.ChangeState(new Walking(controller));
            }
        }
    }

    public override Vector3 HandleExternalForces()
    {
        //Currently only accounting for gravity
        var velocity = new Vector3(controller.CurrentVelocity.x, controller.Gravity, controller.CurrentVelocity.z);
        return velocity;
    }

    public override void ManageMove(PlayerInput.Input input)
    {
        if (input.Started || input.Performed)
        {
            var value = input.GetValue<Vector2>();
            var direction = new Vector3(value.x, 0, value.y);
            controller.InputDirection = direction;

        }
        else if (input.Canceled)
        {
            controller.InputDirection = Vector3.zero;
        }
    }

    public override Vector3 UpdateVelocity()
    {
        // TODO airborne movement, with reduced maneuverability. Currently returns current velocity
        return base.UpdateVelocity();
    }  
}
