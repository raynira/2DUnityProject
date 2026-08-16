using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header ("UI Parameters")]
    public GameObject _gameOverScreen;
    public GameObject _gameHealthUI;
    public GameObject _gameKeyUI;

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
        PlayerHealth.OnPlayerDied += GameOverScreen;
        _gameOverScreen.SetActive(false);
        _gameHealthUI.SetActive(true);
        _gameKeyUI.SetActive(true);

        UpdateVignette(_playerHealth.CurrentHealth);
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
        _gameHealthUI.SetActive(false);
        _gameKeyUI.SetActive(false);
        _gameOverScreen.SetActive(true);

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        _gameHealthUI.SetActive(true);
        _gameKeyUI.SetActive(true);
        _gameOverScreen.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    void OnDestroy()
    {
        PlayerHealth.OnPlayerDied -= GameOverScreen;
    }
}