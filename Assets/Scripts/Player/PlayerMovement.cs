using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speed Parameters")]
    [SerializeField] private float _gravity = -20;
    [SerializeField] private float _runSpeed = 8;
    [SerializeField] private float _jumpSpeed = 8;
    [SerializeField] private float _acceleration = 60;
    [SerializeField] private float _deceleration = 70;
    [SerializeField] private float _airAcceleration = 20;
    [SerializeField] private float _airDeceleration = 10f;
    private float _defaultSpeed;
    private bool _isFacingRight = true;

    [Header("Jump Parameters")]
    [SerializeField] private float _coyoteTime = 0.1f;
    [SerializeField] private float _earlyJumpTime = 0.1f;

    // [SerializeField] private AudioClip _jumpSFX;

    [Header("Ground Check Parameters")]
    [SerializeField] private Transform _groundCheckTarget;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundLayerMask;

    private Rigidbody2D _rigidBody;
    public Animator animator;

    private bool _grounded = false;
    private bool _jumpPressed = false;
    private bool _earlyJumpTimerActive = false;

    private float _timeSinceLeftGround = 0;
    private float _timeSinceJumpPressed = float.MaxValue;

    private bool _doubleJump = false;

    public bool IsGrounded { get { return _grounded; } }
    public Vector2 Velocity { get { return _rigidBody.linearVelocity; } }

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _defaultSpeed = _runSpeed;
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

        animator.SetFloat("YVelocity", _rigidBody.linearVelocity.y);
        animator.SetFloat("Magnitude", Mathf.Abs(_rigidBody.linearVelocity.x));
    }

    private void Flip(float direction)
    {
        if ((_isFacingRight && direction < 0) || (!_isFacingRight && direction > 0))
        {
            _isFacingRight = !_isFacingRight;

            Vector3 scale = transform.localScale;

            scale.x *= -1f;

            transform.localScale = scale;
        }
    }

    private void OnEnable()
    {
        InputManager.Instance.JumpInputPressed += OnJumpPressed;
    }

    private void OnDisable()
    {
        InputManager.Instance.JumpInputPressed -= OnJumpPressed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _runSpeed = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _runSpeed = _defaultSpeed;
        }
    }

    private void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheckTarget.position, _groundCheckRadius, _groundLayerMask);
        Vector2 velocity = _rigidBody.linearVelocity;

        velocity.y += _gravity * Time.fixedDeltaTime;

        if (_grounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

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

        if ((_grounded || coyote) && (_jumpPressed || earlyJump))
        {
            animator.SetTrigger("Jump");

            // SfxManagar.Instance.PlaySFX(_jumpSFX);
            velocity.y = _jumpSpeed;
            _grounded = false;
            _jumpPressed = false;
            _earlyJumpTimerActive = false;
            _timeSinceJumpPressed = float.MaxValue;
        }

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

        Flip(InputManager.Instance.HorizontalInput);

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

        _earlyJumpTimerActive = true;
        _timeSinceJumpPressed = 0;
    }
}