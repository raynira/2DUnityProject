using UnityEngine;

public class KillEntityOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            GameController.Instance.GameOverScreen();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
