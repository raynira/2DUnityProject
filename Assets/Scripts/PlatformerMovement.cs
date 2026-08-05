using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    [Header ("Movement Speed Parameters")]
    [SerializeField] private float _gravity = -20;
    [SerializeField] private float _runSpeed = 8;
    [SerializeField] private float _jumpSpeed = 15;
    [SerializeField] private float _acceleration = 60;
    [SerializeField] private float _deceleration = 70;
    [SerializeField] private float _airAcceleration = 20;
    [SerializeField] private float _airDeceleration = 10f;

    [Header ("Jump Parameters")]
    [SerializeField] private float _coyoteTime = 0.1f;
    [SerializeField] private float _earlyJumpTime = 0.1f;
    [SerializeField] private bool _canDoubleJump = true;

    // [SerializeField] private AudioClip _jumpSFX;

    [Header ("Ground Check Parameters")]
    [SerializeField] private Transform _groundCheckTarget;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundLayerMask;

    private Rigidbody2D _rigidBody;

    private bool _grounded = false;
    private bool _jumpPressed = false;
    private bool _jumpHeld = false;
    private bool _earlyJumpTimerActive = false;

    private float _timeSinceLeftGround = 0;
    private float _timeSinceJumpPressed = float.MaxValue;

    private bool _doubleJump = false;

    public bool IsGrounded { get { return _grounded; } }
    public Vector2 Velocity { get { return _rigidBody.linearVelocity; } }

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!_grounded)
        {
            _timeSinceLeftGround += Time.deltaTime;

            if (_earlyJumpTimerActive)
            {
                _timeSinceJumpPressed += Time.deltaTime;
                if (_timeSinceJumpPressed > _earlyJumpTime)
                {
                    _earlyJumpTimerActive = false;
                }
            }
        }
        else
            _timeSinceLeftGround = 0;
    }

    private void OnEnable()
    {
        InputManager.Instance.JumpInputPressed += OnJumpPressed;
        InputManager.Instance.JumpInputReleased += OnJumpReleased;
    }

    private void OnDisable()
    {
        InputManager.Instance.JumpInputPressed -= OnJumpPressed;
        InputManager.Instance.JumpInputReleased -= OnJumpReleased;
    }

    private void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheckTarget.position, _groundCheckRadius, _groundLayerMask);
        Vector2 velocity = _rigidBody.linearVelocity;

        velocity.y += _gravity * Time.fixedDeltaTime;

        float horizontalDirection = Mathf.Clamp(InputManager.Instance.HorizontalInput, -1, 1) * _runSpeed;
        float acceleration = 0;

        if (Mathf.Abs(horizontalDirection) > 0.01f)
        {
            if (_grounded)
                acceleration = _acceleration;
            else
                acceleration = _airAcceleration;
        }
        else
        {
            if (_grounded)
                acceleration = _deceleration;
            else
                acceleration = _airDeceleration;
        }

        float velocityDifference = horizontalDirection - velocity.x;
        float deltaAccleration = acceleration * Time.fixedDeltaTime;
        float finalAcceleration = Mathf.Clamp(velocityDifference, -deltaAccleration, deltaAccleration);
        velocity.x += finalAcceleration;

        bool coyote = _timeSinceLeftGround <= _coyoteTime;
        bool earlyJump = _timeSinceJumpPressed <= _earlyJumpTime;

        // jump
        if ((_grounded || coyote) && (_jumpPressed || earlyJump))
        {
            // SfxManagar.Instance.PlaySFX(_jumpSFX);
            velocity.y = _jumpSpeed;
            _grounded = false;
            _jumpPressed = false;
            _earlyJumpTimerActive = false;
            _timeSinceJumpPressed = float.MaxValue;
        }

        // double jump
        if (_grounded && !_doubleJump)
        {
            _doubleJump = true;
        }

        if (!_grounded && _doubleJump && _jumpPressed)
        {
            // SfxManagar.Instance.PlaySFX(_jumpSFX);
            _doubleJump = false;
            velocity.y = _jumpSpeed;
            _jumpPressed = false;
        }

        _rigidBody.linearVelocity = velocity;

        _jumpPressed = false;
    }

    private void OnDrawGizmos()
    {
        if (_groundCheckTarget != null)
        {
            Gizmos.color = _grounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(_groundCheckTarget.position, _groundCheckRadius);
        }
    }

    private void OnJumpPressed()
    {
        _jumpPressed = true;
        _jumpHeld = true;

        _earlyJumpTimerActive = true;
        _timeSinceJumpPressed = 0;
    }

    private void OnJumpReleased()
    {
        _jumpHeld = false;
    }
}
