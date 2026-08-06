using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int _damage = 1;
    public PlayerHealth player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(_damage);
        }
    }
}
