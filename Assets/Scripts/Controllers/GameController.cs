using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject _gameOverScreen;
    public GameObject _gameHealthUI;

    void Start()
    {
        PlayerHealth.OnPlayerDied += GameOverScreen;
        _gameOverScreen.SetActive(false);
        _gameHealthUI.SetActive(true);
    }

    void GameOverScreen()
    {
        _gameHealthUI.SetActive(false);
        _gameOverScreen.SetActive(true);

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        _gameHealthUI.SetActive(true);
        _gameOverScreen.SetActive(false);

        // implement logic for buttons later
    }
}