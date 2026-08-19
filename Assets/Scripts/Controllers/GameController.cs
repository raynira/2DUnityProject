using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header ("UI Parameters")]
    public GameObject _gameOverScreen;
    public GameObject _gameVictoryScreen;
    public GameObject _gameHealthUI;
    public GameObject _gameKeyUI;
    public PlayerHealthSO healthSO;

    [Header("Overlay Parameters")]
    public Tilemap _vignetteTilemap;
    public PlayerHealth _playerHealth;

    [Header("Health Vignette Alpha Values")]
    [SerializeField] private float _fullHealthAlpha = 0f;
    [SerializeField] private float _twoHealthAlpha = 0.5f;
    [SerializeField] private float _oneHealthAlpha = 0.75f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        StartTransition();
    }

    async void StartTransition()
    {
        PlayerHealth.OnPlayerDied += GameOverScreen;
        _gameOverScreen.SetActive(false);
        _gameVictoryScreen.SetActive(false);
        _gameHealthUI.SetActive(true);
        _gameKeyUI.SetActive(true);

        UpdateVignette(healthSO.Value);

        await ScreenFadeTransition.Instance.FadeIn();
    }

    public void UpdateVignette(int health)
    {
        float _targetAlpha = health switch
        {
            >= 3 => _fullHealthAlpha,
            2 => _twoHealthAlpha,
            1 => _oneHealthAlpha,
            _ => 1f
        };

        Color _color = _vignetteTilemap.color;
        _color.a = _targetAlpha;
        _vignetteTilemap.color = _color;
    }

    public void GameOverScreen()
    {
        MusicManager.PauseBackgroundMusic();
        SFXManager.Play("Death");

        _gameHealthUI.SetActive(false);
        _gameKeyUI.SetActive(false);
        _gameOverScreen.SetActive(true);

        Time.timeScale = 0;
    }

    public void VictoryScreen()
    {
        MusicManager.PauseBackgroundMusic();
        SFXManager.Play("Victory");

        _gameHealthUI.SetActive(false);
        _gameKeyUI.SetActive(false);
        _gameVictoryScreen.SetActive(true);

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SFXManager.Play("Select");
        MusicManager.PlayBackgroundMusic(true);

        ResetTransition();

        Time.timeScale = 1f;
    }

    async void ResetTransition()
    {
        await ScreenFadeTransition.Instance.FadeOut();

        _gameHealthUI.SetActive(true);
        _gameKeyUI.SetActive(true);
        _gameOverScreen.SetActive(false);

        healthSO.ResetState();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        await ScreenFadeTransition.Instance.FadeIn();
    }

    public void ExitToMainMenu()
    {
        SFXManager.Play("Select");
        MusicManager.PlayBackgroundMusic(false);

        ExitTransition();

        Time.timeScale = 1f;
    }

    async void ExitTransition()
    {
        await ScreenFadeTransition.Instance.FadeOut();

        healthSO.ResetState();

        SceneManager.LoadScene("MainMenu");

        await ScreenFadeTransition.Instance.FadeIn();
    }

    void OnDestroy()
    {
        PlayerHealth.OnPlayerDied -= GameOverScreen;
    }
}