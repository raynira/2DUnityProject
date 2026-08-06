using System;
using UnityEngine;

public class PlayerHealthItem : MonoBehaviour, IItem
{
    public PlayerHealth player;

    public int healAmount = 1;

    public static event Action<int> OnHealthCollect;

    public void Collect()
    {
        if (player.IsMaxHealth()) return;

        OnHealthCollect.Invoke(healAmount);
        Destroy(gameObject);
    }
}
