using UnityEngine;

public class MovementController : MonoBehaviour
{
    //--- Inspector settings
    #region Settings
    // Capsule size
    [Header("Character capsule dimensions")]
    [Tooltip("Height of the capsule containing the Character")]
    [SerializeField] private float _height = 2f;
    [Tooltip("Radius of the capsule containing the Character")]
    [SerializeField] private float _radius = 0.25f;

    // Movement
    [Header("Movement settings")]
    [Tooltip("Top walking velocity")]
    [SerializeField] [Min(0f)] private float _maxWalkSpeed = 7;
    [Tooltip("Rate of change in the velocity. Affects both acceleration and deceleration")]
    [SerializeField] [Min(0f)] private float _acceleration = 30;

    // Gravity
    [Header("Gravity")]
    [Tooltip("Downwards force. Must be negative")]
    [SerializeField] private float _gravity = -10;

    // Ground layer
    [Header("Ground Layer")]
    [Tooltip("Layer to be considered ground for the moving object")]
    [SerializeField] LayerMask _groundLayer;
    #endregion

    //--- General fields and properties
    #region Fields and properties
    public Vector3 InputDirection { get => _inputDirection; set => _inputDirection = value; }
    private Vector3 _inputDirection;    // Direction a given input is pointing towards

    public Vector3 TargetDirection { get => _targetDirection; private set => _targetDirection = value; }
    private Vector3 _targetDirection;   // Direction the object will be moving towards

    public Vector3 CurrentVelocity { get => _currentVelocity; private set => _currentVelocity = value; }
    private Vector3 _currentVelocity;

    public float MaxWalkingSpeed { get => _maxWalkSpeed; }
    public float Acceleration { get => _acceleration; }
    public float Gravity { get => _gravity; }
    public MovementState CurrentState { get => _movementSM.CurrentState; }
    #endregion

    //--- Components references
    private MovementStateMachine _movementSM;
    private CharacterController _moveSystem;
    // Unity's character controller is used as the default movement and collision manager

    private void Awake()
    {
        // Component instantiation
        _moveSystem = gameObject.AddComponent<CharacterController>();
        _moveSystem.height = _height;
        _moveSystem.radius = _radius;
        
        _movementSM = new MovementStateMachine(new Idle(this));

        new InputHandler(this);
    }

    private void Start()
    {
        // Initialization of the player's velocity V3 at zero, and the player's target direction V3 pointing forward
        _currentVelocity = Vector3.zero;
        _targetDirection = transform.forward;
    }

    private void Update()
    {
        // Movement loop
        HandleGrounded();

        UpdateDirection();

        UpdateVelocity();

        HandleExternalForces();

        Move();

        FacingUpdate();
    }
    private void HandleGrounded()
    {
        // Behaviour controlled by the movement state machine
        _movementSM.CurrentState.ManageGrounding();
    }

    private float _groundedOffset = -0.1f;    // Must be negative and small enough to properly interact with stairs and slopes
    private Vector3 _objectBottom => this.transform.position + new Vector3(0, -(_height / 2));
    public bool Grounded => GroundCheck.Grounded(_objectBottom, _radius, _groundedOffset, _groundLayer);
    public Vector3 GetFloorNormal() => GroundCheck.GetFloorNormal(this.transform, _radius, _height);

    private void UpdateDirection()
    {
        // Behaviour controlled by the movement state machine
        _targetDirection = _movementSM.CurrentState.UpdateDirection();
    }

    private void UpdateVelocity()
    {
        // Behaviour controlled by the movement state machine
        _currentVelocity = _movementSM.CurrentState.UpdateVelocity();
    }

    private void HandleExternalForces()
    {
        // Behaviour controlled by the movement state machine
        _currentVelocity = _movementSM.CurrentState.HandleExternalForces();
    }

    private void Move()
    {
        // Move to a new position, obtained integrating velocity over time
        _moveSystem.Move(CurrentVelocity * Time.deltaTime);
    }

    private readonly float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    /// <summary>
    /// Updates the player rotation towards the <see cref="TargetDirection"/> using a smoothing function
    /// taken from https://www.youtube.com/watch?v=4HpC--2iowE (THIRD PERSON MOVEMENT in Unity) - tutorial by Brackeys on YouTube
    /// </summary>
    private void FacingUpdate()
    {
        if (_targetDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(_targetDirection.x, _targetDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    /// <summary>
    /// Switches the current state of the movement state machine to a given one
    /// </summary>
    /// <param name="state">Movement state to switch to</param>
    public void ChangeState(MovementState state) => _movementSM.ChangeState(state);

    /// <summary>
    /// Receives inputs to be processed by the movement controller
    /// </summary>
    /// <param name="input">Player input to be processed</param>
    public void HandleInput(PlayerInput.Input input)
    {
        switch (input.Action)
        {
            case PlayerInput.Action.Move:
                _movementSM.CurrentState.ManageMove(input);
                break;
            case PlayerInput.Action.Dash:
                _movementSM.CurrentState.ManageDash(input);
                break;
            default:
                Debug.Log("An invalid input was handed, and wasn't handled");
                break;
        }
    }

    //--- Gizmos
    #region Gizmos drawing
    private void OnDrawGizmos()
    {
        // Grounded sphere
        Gizmos.color = Grounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(_objectBottom - new Vector3(0, _groundedOffset), _radius);
    }
    #endregion
}