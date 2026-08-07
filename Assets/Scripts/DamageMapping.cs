using UnityEngine;

public class DamageMapping : MonoBehaviour
{
    public PlayerHealth player;

    public enum EnemyType
    {
        Spike,
        Enemy
    }
    public EnemyType _enemyType;

    private int _spikeDamage = 1;
    private int _enemyDamage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (_enemyType)
            {
                case EnemyType.Spike:
                    player.TakeDamage(_spikeDamage);
                    break;
                case EnemyType.Enemy:
                    player.TakeDamage(_enemyDamage);
                    break;
            }
        }
    }
}