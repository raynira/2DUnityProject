using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public LevelsSurvivedSO progress;
    public GameObject _playButton;
    public Sprite _continueSprite;

    void Start()
    {
        StartTransition();

        if (progress.Value > 0)
        {
            _playButton.GetComponent<Image>().overrideSprite = _continueSprite;
        }
    }

    async void StartTransition()
    {
        await ScreenFadeTransition.Instance.FadeIn();
    }

    public void OnPlayClick()
    {
        SFXManager.Play("Select");

        if (progress.Value == 0)
        {
            PlayTransition();
            SceneManager.LoadScene("FoolsCreek");
        }
        else if (progress.Value == 1)
        {
            PlayTransition();
            SceneManager.LoadScene("HiddenOasis");
        }
        else if (progress.Value == 2)
        {
            PlayTransition();
            SceneManager.LoadScene("LoversRest");
        }
    }

    async void PlayTransition()
    {
        await ScreenFadeTransition.Instance.FadeOut();
    }

    public void OnExitClick()
    {
        SFXManager.Play("Select");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
