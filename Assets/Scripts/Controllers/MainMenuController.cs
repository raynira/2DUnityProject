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
        if (progress.Value > 0)
        {
            _playButton.GetComponent<Image>().overrideSprite = _continueSprite;
        }
    }
    
    public void OnPlayClick()
    {
        if (progress.Value == 0)
        {
            SceneManager.LoadScene("FoolsCreek");
        }
        else if (progress.Value == 1)
        {
            SceneManager.LoadScene("HiddenOasis");
        }
        else if (progress.Value == 2)
        {
            SceneManager.LoadScene("LoversRest");
        }
    }

    public void OnExitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
