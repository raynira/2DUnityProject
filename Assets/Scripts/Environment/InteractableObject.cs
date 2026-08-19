using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public bool _isPulled { get; private set; }
    public Sprite _pulledSprite;
    public TextMesh _keyCollected;
    public string _nextLevelName;

    public FinishFlag finish;
    public LevelsSurvivedSO survivedSO;

    public bool CanInteract()
    {
        if (_isPulled) return false;

        return _keyCollected != null && _keyCollected.text == "1";
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        SFXManager.Play("Interact");

        PullLever(true);

        if (finish != null)
        {
            finish.LeverPulled();
        }
        else
        {
            FadeTransition();
        }
    }

    async void FadeTransition()
    {
        await ScreenFadeTransition.Instance.FadeOut();

        survivedSO.Value++;
        SceneManager.LoadScene(_nextLevelName);

        await ScreenFadeTransition.Instance.FadeIn();
    }

    public void PullLever(bool pulled)
    {
        if (_isPulled = pulled)
        {
            GetComponent<SpriteRenderer>().sprite = _pulledSprite;
        }
    }
}
