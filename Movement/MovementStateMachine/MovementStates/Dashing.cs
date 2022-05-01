using UnityEngine;

public class Dashing : MovementState
{
    private float _dashDistance = 7f;
    private float _dashTime = 0.5f;

    private float _elapsedTime;
    private Vector3 _dashDirection;
    
    public Dashing(MovementController controller) : base(controller) { }
    
    public override void OnEnter()
    {
        // Reset the timer
        _elapsedTime = 0f;

        // Dash direction stored locally, in order to keep the movement in a straight line. 
        _dashDirection =
            controller.InputDirection == Vector3.zero || controller.InputDirection == null ?
            controller.transform.forward : controller.InputDirection;
    }

    public override Vector3 HandleExternalForces()
    {
        // Currently only accounting for gravity
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

    public override Vector3 UpdateDirection()
    {
        // To keep the dash in a straight line, the stored vector is passed
        return _dashDirection;
    }

    public override Vector3 UpdateVelocity()
    {
        // update elapsed time
        _elapsedTime += Time.deltaTime;

        // Switch to walking state if dash time has been reached
        if (_elapsedTime > _dashTime) controller.ChangeState(new Walking(controller));

        // normalized [0,1] timestep value calculated
        var timeStep = 1 - _elapsedTime / _dashTime;

        return _dashDistance * SmoothStop(timeStep, 4) * _dashDirection;
    }

    // Smoothing formula taken from GDC talk:
    // "Math for Game Programmers: Fast and Funky 1D Nonlinear Transformation"
    // - https://www.youtube.com/watch?v=mr5xkf6zSzk video on YouTube
    private float SmoothStop(float timeStep, float smoothFactor)
    {
        return 1 - (Mathf.Pow(timeStep, smoothFactor));
    }
    
}
