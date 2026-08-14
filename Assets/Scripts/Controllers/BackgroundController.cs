using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float _startPosition, _length;
    public GameObject _camera;
    public float _parallaxFX;

    void Start()
    {
        _startPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float distance = _camera.transform.position.x * _parallaxFX;
        float movement = _camera.transform.position.x * (1 - _parallaxFX);

        transform.position = new Vector3(_startPosition + distance, transform.position.y, transform.position.z);

        if (movement > _startPosition + _length)
        {
            _startPosition += _length;
        }
        else if (movement < _startPosition - _length)
        {
            _startPosition -= _length;
        }
    }
}
