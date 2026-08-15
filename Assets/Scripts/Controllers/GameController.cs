using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public GameObject _gameOverScreen;
    public GameObject _gameHealthUI;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayerHealth.OnPlayerDied += GameOverScreen;
        _gameOverScreen.SetActive(false);
        _gameHealthUI.SetActive(true);
    }

    public void GameOverScreen()
    {
        _gameHealthUI.SetActive(false);
        _gameOverScreen.SetActive(true);

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        _gameHealthUI.SetActive(true);
        _gameOverScreen.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}