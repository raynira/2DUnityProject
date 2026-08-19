using System.Collections;
using UnityEngine;

public class EntityFlashRed : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator FlashRed()
    {
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        _spriteRenderer.color = Color.white;
    }
}
