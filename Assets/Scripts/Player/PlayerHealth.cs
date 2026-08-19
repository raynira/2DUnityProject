using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Parameters")]
    public PlayerHealthSO healthSO;
    public Health health;
    public GameController vignette;
    public int _maxHealth = 3;
    private int _currentHealth;

    public int CurrentHealth => _currentHealth;

    [Header("Grace Period Parameters")]
    public float _invincibilityDuration = 0.5f;
    private float _lastHitTime = -999f;
    public bool _isInvincible => Time.time - _lastHitTime < _invincibilityDuration;

    public static event Action OnPlayerDied;

    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        if (healthSO.IsInitialized)
        {
            _currentHealth = healthSO.Value;
        }
        else
        {
            _currentHealth = _maxHealth;
            healthSO.Value = _currentHealth;
            healthSO.IsInitialized = true;
        }
    }

    void Start()
    {
        health.SetHearts(_maxHealth, _currentHealth);

        PlayerHealthItem.OnHealthCollect += Heal;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsMaxHealth()
    {
        return _currentHealth >= _maxHealth;
    }

    void Heal(int amount)
    {
        if (_isInvincible) return;
        if (_currentHealth <= 0) return;

        _lastHitTime = Time.time;
        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        health.UpdateHearts(_currentHealth);
        vignette.UpdateVignette(_currentHealth);

        Debug.Log("Player healed " + amount + "health, current health: " + _currentHealth);

        healthSO.Value = _currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_isInvincible) return;
        if (_currentHealth <= 0) return;

        SFXManager.Play("Hit");

        _lastHitTime = Time.time;
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        health.UpdateHearts(_currentHealth);
        vignette.UpdateVignette(_currentHealth);

        if (damage > 0) StartCoroutine(FlashRed());

        if (_currentHealth <= 0)
        {
            OnPlayerDied?.Invoke();
            gameObject.SetActive(false);
        }

        Debug.Log("Player took " + damage + ", current health: " + _currentHealth);

        healthSO.Value = _currentHealth;
    }

    private IEnumerator FlashRed()
    {
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        _spriteRenderer.color = Color.white;
    }

    void OnDestroy()
    {
        PlayerHealthItem.OnHealthCollect -= Heal;
    }
}