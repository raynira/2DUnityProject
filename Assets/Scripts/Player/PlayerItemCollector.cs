using TMPro;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private int _keyCounter = 0;
    public TMP_Text _counterText;
    public GameObject _keyIcon;

    void Start()
    {
        _keyIcon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IItem item = collision.GetComponent<IItem>();

        if (item != null)
        {
            item.Collect();
        }

        if (collision.CompareTag("Key") && collision.gameObject.activeSelf)
        {
            _keyIcon.SetActive(true);
            Destroy(collision.gameObject);

            if (_keyCounter == 1) return;

            _keyCounter += 1;
            _counterText.text = _keyCounter.ToString();
        }
    }
}