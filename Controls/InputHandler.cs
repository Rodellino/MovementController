using UnityEngine.InputSystem;
using UnityEngine;

// TODO: Singleton pattern

/// <summary>
/// Receives and handles input to be passed to the <see cref="MovementController"/>
/// </summary>
public class InputHandler
{
    // Reference to the controller
    private MovementController _controller;

    // Reference to the Unity's Input System custom control set
    private Controls _controls;


    public InputHandler(MovementController controller)
    {
        this._controller = controller;
        Init();
    }

    private void Init()
    {
        _controls = new Controls();
        _controls.Enable();

        _controls.Gameplay.Move.started += OnMove;
        _controls.Gameplay.Move.performed += OnMove;
        _controls.Gameplay.Move.canceled += OnMove;

        _controls.Gameplay.Dash.started += OnDash;
        _controls.Gameplay.Dash.performed += OnDash;
        _controls.Gameplay.Dash.canceled += OnDash;

    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var action = PlayerInput.Action.Move;

        var status = TranslateStatus(context);

        var value = context.ReadValue<Vector2>();

        var input = new PlayerInput.Input(action, status, value);

        _controller.HandleInput(input);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        var action = PlayerInput.Action.Dash;

        var status = TranslateStatus(context);

        var input = new PlayerInput.Input(action, status);

        _controller.HandleInput(input);
    }

    private PlayerInput.Status TranslateStatus(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            return PlayerInput.Status.Started;
        }
        else if (context.performed)
        {
            return PlayerInput.Status.Performed;
        }
        else
        {
            return PlayerInput.Status.Canceled;
        }
    }


    ~InputHandler()
    {
        _controls.Disable();
        _controls.Gameplay.Move.Disable();
        _controls.Gameplay.Dash.Disable();
    }
}
