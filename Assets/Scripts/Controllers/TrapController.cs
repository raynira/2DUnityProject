using UnityEngine;

public class TrapController : MonoBehaviour
{
    [Header("Damage Parameters")]
    public PlayerHealth player;
    public EnemyAI enemy;
    public enum EnemyType
    {
        Trap,
        Enemy
    }
    public EnemyType _enemyType;
    public int _trapDamage = 1;
    public int _enemyDamage = 1;

    [Header("Bounce Parameters")]
    public float _bounceForce = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (_enemyType)
            {
                case EnemyType.Trap:
                    player.TakeDamage(_trapDamage);

                    HandleBounce(collision.gameObject);
                    break;
                case EnemyType.Enemy:
                    player.TakeDamage(_enemyDamage);
                    break;
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (_enemyType == EnemyType.Trap)
            {
                enemy.TakeDamage(_trapDamage);
                HandleBounce(collision.gameObject);
            }
        }
    }

    private void HandleBounce(GameObject entity)
    {
        Rigidbody2D _rigidbody = entity.GetComponent<Rigidbody2D>();

        if (_rigidbody)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0f);

            _rigidbody.AddForce(Vector2.up * _bounceForce, ForceMode2D.Impulse);
        }
    }
}