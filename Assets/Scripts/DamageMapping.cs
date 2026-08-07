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

    public static int _spikeDamage = 1;
    public static int _enemyDamage = 1;

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