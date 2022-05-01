using UnityEngine;

public class Walking : MovementState
{
    bool _inputEnded;
    float _velocity;
    float _targetSpeed;

    // Min velocity threshold. Below that, we consider ourselves not moving.
    readonly float _minVelocity = 0.01f;

    public Walking(MovementController controller) : base(controller) { }

    public override void OnEnter()
    {
        // Store current horizontal linear velocity
        var velocity = new Vector3(controller.CurrentVelocity.x, 0 ,controller.CurrentVelocity.z).sqrMagnitude;     // Using squared magnitude to avoid square root operations
        _velocity = velocity > (controller.MaxWalkingSpeed * controller.MaxWalkingSpeed) ? controller.MaxWalkingSpeed : Mathf.Sqrt(velocity);

        // Assume no move input is happening on enter. Will be updated at ManageMove otherwise.
        _inputEnded = true;

        // Target speed set to either zero or max speed, depending on the stored input value
        _targetSpeed = controller.InputDirection == Vector3.zero ? 0f : controller.MaxWalkingSpeed;
    }

    public override Vector3 HandleExternalForces()
    {
        // Scale current velocity depending on the slope of the surface we're on
        var floorNormal = controller.GetFloorNormal();
        // Dot product by the controller.transform.forward gives a negative value on descending slopes, and positive for ascending.
        // Dot product by the controller.transform.up gives only positive
        //Debug.Log("Dot.UP: " + Vector3.Dot(controller.transform.up, floorNormal) + " |Dot.FW: " + Vector3.Dot(controller.transform.forward, floorNormal));
        var slopeScaling = Vector3.Dot(controller.transform.up, floorNormal);
        var velocity = controller.CurrentVelocity * slopeScaling;

        return new Vector3(velocity.x, controller.Gravity, velocity.z);
    }

    public override void ManageMove(PlayerInput.Input input)
    {
        if (input.Performed || input.Started)
        {
            var value = input.GetValue<Vector2>();
            var direction = new Vector3(value.x, 0, value.y);
            controller.InputDirection = direction;

            if (input.Started)
            {
                _inputEnded = false;
                _targetSpeed = controller.MaxWalkingSpeed;
            }
        } 
        else if (input.Canceled)
        {
            controller.InputDirection = Vector3.zero;
            _inputEnded = true;
            _targetSpeed = 0f;

        }
    }

    public override void ManageDash(PlayerInput.Input input)
    {
        if (input.Started || input.Performed)
        {
            controller.ChangeState(new Dashing(controller));
        }
    }

    public override Vector3 UpdateDirection()
    {
        // when walking, update the direction to the stored input, as long as it's not zero
        return controller.InputDirection == Vector3.zero ? controller.TargetDirection : controller.InputDirection;
    }

    public override Vector3 UpdateVelocity()
    {
        // Update linear velocity towards target speed, scaled by acceleration over time
        _velocity = Mathf.MoveTowards(_velocity, _targetSpeed, controller.Acceleration * Time.deltaTime);

        // Switch to Idle State if velocity drops below threshold
        if (_velocity < _minVelocity && _inputEnded) controller.ChangeState(new Idle(controller));

        // Velocity Vector3 set towards input direction
        return controller.TargetDirection * _velocity;
    }
}
