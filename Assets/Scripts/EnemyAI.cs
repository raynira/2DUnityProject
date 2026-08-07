using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int Health = 10;

    [Header("Movement")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _movementSpeed = 1.75f;
    [SerializeField] private float _acceleration = 60;
    [SerializeField] private float _deceleration = 70;
    [SerializeField] private float _gravity = -40;

    [Header("Ground check")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private Transform _groundCheckTarget;
    [SerializeField] private Transform _groundCheckLeft;
    [SerializeField] private Transform _groundCheckRight;
    [SerializeField] private Transform _wallCheckLeft;
    [SerializeField] private Transform _wallCheckRight;
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

    private void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheckTarget.position, _groundCheckRadius, _groundLayerMask);
        Vector2 velocity = _rigidbody.linearVelocity;

        velocity.y += _gravity * Time.fixedDeltaTime;

        bool leftGrounded = Physics2D.OverlapCircle(_groundCheckLeft.position, _ledgeCheckRadius, _groundLayerMask);
        bool rightGrounded = Physics2D.OverlapCircle(_groundCheckRight.position, _ledgeCheckRadius, _groundLayerMask);
        bool leftWall = Physics2D.OverlapCircle(_wallCheckLeft.position, _ledgeCheckRadius);
        bool rightWall = Physics2D.OverlapCircle(_wallCheckRight.position, _ledgeCheckRadius);

        bool ledgeOnLeft = !leftGrounded && rightGrounded && _canStayOnPlatform;
        bool ledgeOnRight = !rightGrounded && leftGrounded && _canStayOnPlatform;

        if (ledgeOnLeft || leftWall)
        {
            Debug.Log("Left ledge detected!");

            _direction = 1;

            if (ledgeOnLeft) velocity.x = 0f;
        }
        else if (ledgeOnRight || rightWall)
        {
            Debug.Log("Right ledge detected!");

            _direction = -1;

            if (ledgeOnRight) velocity.x = 0f;
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
            Gizmos.DrawWireSphere(_wallCheckLeft.position, 0.05f);
            Gizmos.DrawWireSphere(_wallCheckRight.position, 0.05f);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _playerSensorRadius);
        }
    }
}