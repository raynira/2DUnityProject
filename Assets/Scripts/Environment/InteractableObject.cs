using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public bool _isPulled { get; private set; }
    public Sprite _pulledSprite;
    public TMP_Text _keyCollected;
    public string _nextLevelName;

    public bool CanInteract()
    {
        if (_isPulled) return false;

        return _keyCollected != null && _keyCollected.text == "1";
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        PullLever(true);

        LoadNextLevel();
    }

    public void PullLever(bool pulled)
    {
        if (_isPulled = pulled)
        {
            GetComponent<SpriteRenderer>().sprite = _pulledSprite;
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(_nextLevelName);
    }
}
