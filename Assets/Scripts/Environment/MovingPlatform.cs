using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform _pointA;
    public Transform _pointB;
    public float _moveSpeed = 2f;

    private Vector3 _nextPosition;
    
    void Start()
    {
        _nextPosition = _pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _nextPosition, _moveSpeed * Time.deltaTime);

        if (transform.position == _nextPosition)
        {
            _nextPosition = (_nextPosition == _pointA.position) ? _pointB.position : _pointA.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
