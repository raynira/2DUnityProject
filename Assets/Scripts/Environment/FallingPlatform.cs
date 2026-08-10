using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float _fallWait = 0.5f;
    public float _destroyWait = 1f;

    bool _isFalling;
    private Rigidbody2D _rigidBody;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        _isFalling = true;

        yield return new WaitForSeconds(_fallWait);

        _rigidBody.bodyType = RigidbodyType2D.Dynamic;

        Destroy(gameObject, _destroyWait);
    }
}
