using UnityEngine;

public class DamageMapping : MonoBehaviour
{
    [Header("Damage Parameters")]
    public PlayerHealth player;
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

                    HandlePlayerBounce(collision.gameObject);
                    break;
                case EnemyType.Enemy:
                    player.TakeDamage(_enemyDamage);
                    break;
            }
        }
    }

    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D _rigidbody = player.GetComponent<Rigidbody2D>();

        if (_rigidbody)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0f);

            _rigidbody.AddForce(Vector2.up * _bounceForce, ForceMode2D.Impulse);
        }
    }
}