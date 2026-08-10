using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public DamageMapping damage;
    public int _maxEnemyHealth = 2;
    private int _currentEnemyHealth;

    [Header("Movement Speed Parameters")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _movementSpeed = 1.75f;
    [SerializeField] private float _acceleration = 60;
    [SerializeField] private float _deceleration = 70;

    [Header("Jump Parameters")]
    [SerializeField] private float _gravity = -35;
    [SerializeField] private float _jumpSpeed = 8f;
    [SerializeField] private float _jumpCooldown = 0.3f;
    [SerializeField] private Transform _jumpCheckLeft;
    [SerializeField] private Transform _jumpCheckRight;
    [SerializeField] private float _jumpCheckRadius = 0.1f;
    [SerializeField] private Transform _backUpCheckLeft;
    [SerializeField] private Transform _backUpCheckRight;
    [SerializeField] private float _backUpCheckRadius = 0.1f;
    private float _jumpCooldownTimer = 0f;

    [Header("Ground Check Parameters")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _wallLayerMask;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private Transform _groundCheckTarget;
    [SerializeField] private Transform _groundCheckLeft;
    [SerializeField] private Transform _groundCheckRight;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _ledgeCheckRadius = 0.05f;

    [SerializeField] private ContactFilter2D _contactFilter;

    [Header("AI")]
    [SerializeField] private bool _isFollowingPlayer;
    [SerializeField] private bool _canStayOnPlatform;
    [SerializeField] private float _playerSensorRadius = 5.25f;
    [SerializeField] private float _chaseDeadZone = 0.05f;
    [SerializeField] private float _seekDuration = 2f;

    private bool _grounded = false;
    private float _direction = -1;

    private bool _hasLastKnownPlayerPosition = false;
    private Vector2 _lastKnownPlayerPosition;
    private float _seekTimer = 0f;

    void Start()
    {
        _currentEnemyHealth = _maxEnemyHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentEnemyHealth -= damage;

        if (_currentEnemyHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(damage._trapDamage);
        }
    }

    private void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheckTarget.position, _groundCheckRadius, _groundLayerMask);
        Vector2 velocity = _rigidbody.linearVelocity;

        velocity.y += _gravity * Time.fixedDeltaTime;

        if (_jumpCooldownTimer > 0f) _jumpCooldownTimer -= Time.fixedDeltaTime;

        bool leftGrounded = Physics2D.OverlapCircle(_groundCheckLeft.position, _ledgeCheckRadius, _groundLayerMask);
        bool rightGrounded = Physics2D.OverlapCircle(_groundCheckRight.position, _ledgeCheckRadius, _groundLayerMask);

        bool jumpLeft = Physics2D.OverlapCircle(_jumpCheckLeft.position, _jumpCheckRadius, _wallLayerMask);
        bool jumpRight = Physics2D.OverlapCircle(_jumpCheckRight.position, _jumpCheckRadius, _wallLayerMask);

        bool backUpLeft = Physics2D.OverlapCircle(_backUpCheckLeft.position, _backUpCheckRadius, _wallLayerMask);
        bool backUpRight = Physics2D.OverlapCircle(_backUpCheckRight.position, _backUpCheckRadius, _wallLayerMask);

        bool ledgeOnLeft = !leftGrounded && rightGrounded;
        bool ledgeOnRight = !rightGrounded && leftGrounded;

        if (ledgeOnLeft)
        {
            _direction = 1;

            velocity.x = 0f;
        }
        else if (ledgeOnRight)
        {
            _direction = -1;

            velocity.x = 0f;
        }

        bool seesPlayer = false;

        if (_isFollowingPlayer)
        {
            List<Collider2D> colliders = new();
            Physics2D.OverlapCircle(transform.position, _playerSensorRadius, _contactFilter, colliders);

            for (int i = 0; i < colliders.Count; i++)
            {
                if (!colliders[i].CompareTag("Player"))
                    continue;

                seesPlayer = true;

                Vector2 playerPos = colliders[i].transform.position;
                _lastKnownPlayerPosition = playerPos;
                _hasLastKnownPlayerPosition = true;
                _seekTimer = _seekDuration;

                float diffX = playerPos.x - transform.position.x;

                if (Mathf.Abs(diffX) > _chaseDeadZone)
                    _direction = diffX < 0 ? -1 : 1;
                else
                    _direction = 0;

                break;
            }

            if (!seesPlayer && _hasLastKnownPlayerPosition)
            {
                float diffX = _lastKnownPlayerPosition.x - transform.position.x;

                if (Mathf.Abs(diffX) > _chaseDeadZone)
                {
                    _direction = diffX < 0 ? -1 : 1;
                }
                else
                {
                    _hasLastKnownPlayerPosition = false;
                }

                _seekTimer -= Time.fixedDeltaTime;
                if (_seekTimer <= 0f)
                    _hasLastKnownPlayerPosition = false;
            }
        }

        bool isChasing = _isFollowingPlayer && (seesPlayer || _hasLastKnownPlayerPosition);
        bool isApproachingWall = (jumpLeft && _direction < 0) || (jumpRight && _direction > 0);
        bool canJump = isChasing && _grounded && _jumpCooldownTimer <= 0f;

        if (backUpLeft && _direction < 0)
        {
            canJump = false;
            _direction = 1;
        }
        else if (backUpRight && _direction > 0)
        {
            canJump = false;
            _direction = -1;
        }

        if (canJump && isApproachingWall)
        {
            velocity.y = _jumpSpeed;
            _jumpCooldownTimer = _jumpCooldown;
        }

        float horizontalDirection = _direction * _movementSpeed;

        float acceleration = 0;

        if (Mathf.Abs(horizontalDirection) > 0.01f)
        {
            if (_grounded)
                acceleration = _acceleration;
        }
        else
        {
            if (_grounded)
                acceleration = _deceleration;
        }

        float velocityDifference = horizontalDirection - velocity.x;
        float deltaAccleration = acceleration * Time.fixedDeltaTime;
        float finallAcceleration = Mathf.Clamp(velocityDifference, -deltaAccleration, deltaAccleration);
        velocity.x += finallAcceleration;

        _rigidbody.linearVelocity = velocity;
    }

    private void OnDrawGizmos()
    {
        if (_groundCheckTarget != null)
        {
            Gizmos.color = _grounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(_groundCheckTarget.position, _groundCheckRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_groundCheckLeft.position, 0.05f);
            Gizmos.DrawWireSphere(_groundCheckRight.position, 0.05f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(_jumpCheckLeft.position, _jumpCheckRadius);
            Gizmos.DrawWireSphere(_jumpCheckRight.position, _jumpCheckRadius);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_backUpCheckLeft.position, _backUpCheckRadius);
            Gizmos.DrawWireSphere(_backUpCheckRight.position, _backUpCheckRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _playerSensorRadius);
        }
    }
}