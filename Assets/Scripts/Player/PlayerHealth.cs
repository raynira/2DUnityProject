using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Parameters")]
    public Health health;
    public int _maxHealth = 3;
    private int _currentHealth;

    [Header("Grace Period Parameters")]
    public float _invincibilityDuration = 0.5f;
    private float _lastHitTime = -999f;
    public bool _isInvincible => Time.time - _lastHitTime < _invincibilityDuration;

    public static event Action OnPlayerDied;

    void Start()
    {
        _currentHealth = _maxHealth;
        health.SetMaxHearts(_maxHealth);

        PlayerHealthItem.OnHealthCollect += Heal;
    }

    public bool IsMaxHealth()
    {
        return _currentHealth >= _maxHealth;
    }

    void Heal(int amount)
    {
        if (_isInvincible) return;

        _lastHitTime = Time.time;
        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        health.UpdateHearts(_currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (_isInvincible) return;

        _lastHitTime = Time.time;
        _currentHealth -= damage;

        health.UpdateHearts(_currentHealth);

        if (_currentHealth == 0)
        {
            OnPlayerDied.Invoke();
            gameObject.SetActive(false);
        }
    }
}