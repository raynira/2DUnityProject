using TMPro;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public bool _isPulled { get; private set; }
    public Sprite _pulledSprite;
    public TMP_Text _keyCollected;

    public bool CanInteract()
    {
        if (_isPulled) return false;

        return _keyCollected != null && _keyCollected.text == "1";
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        PullLever(true);

        // implement load next level logic here
    }

    public void PullLever(bool pulled)
    {
        if (_isPulled = pulled)
        {
            GetComponent<SpriteRenderer>().sprite = _pulledSprite;
        }
    }
}
