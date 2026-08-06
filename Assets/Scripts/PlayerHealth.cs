using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [SerializeField] Transform _lifeContainer;
    [SerializeField] GameObject _lifePrefab;
    [SerializeField] Transform _deathContainer;
    [SerializeField] GameObject _deathPrefab;

    private int _livesCount = 3;
    private int _deathsCount = 0;

    List<GameObject> _lives = new List<GameObject>();
    List<GameObject> _deaths = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddLife();
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < _livesCount; i++)
        {
            _lives.Add(Instantiate(_lifePrefab, _lifeContainer));
        }
        for (int i = 0; i < _deathsCount; i++)
        {
            _deaths.Add(Instantiate(_deathPrefab, _deathContainer));
        }
    }

    public void AddLife()
    {
        _lives.Add(Instantiate(_lifePrefab, _lifeContainer));
        _livesCount++;

        Destroy(_deaths[0]);
        _deaths.RemoveAt(0);
        _deathsCount--;
    }

    public void RemoveLife()
    {
        if (_livesCount <= 0) return;

        Destroy(_lives[0]);
        _lives.RemoveAt(0);
        _livesCount--;

        _deaths.Add(Instantiate(_deathPrefab, _deathContainer));
        _deathsCount++;
    }

    void Update()
    {
        
    }
}
