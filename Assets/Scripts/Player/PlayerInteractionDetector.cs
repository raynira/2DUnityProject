using UnityEngine;

public class PlayerInteractionDetector : MonoBehaviour
{
    private IInteractable _interactibleInRange = null;
    public GameObject _interactIcon;
    
    void Start()
    {
        _interactIcon.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            _interactibleInRange?.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            _interactibleInRange = interactable;
            _interactIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == _interactibleInRange)
        {
            _interactibleInRange = null;
            _interactIcon.SetActive(false);
        }
    }
}
