using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Health health;

    public int _maxHealth = 3;
    private int _currentHealth;

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
        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        health.UpdateHearts(_currentHealth);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        health.UpdateHearts(_currentHealth);

        if (_currentHealth <= 0)
        {
            OnPlayerDied.Invoke();
        }
    }
}