using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float _damage = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAI enemy = collision.GetComponent<EnemyAI>();

        if (enemy != null)
        {
            enemy.TakeDamage((int) _damage);
        }
    }
}