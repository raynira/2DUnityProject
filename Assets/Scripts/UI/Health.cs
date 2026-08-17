using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image _heartPrefab;
    public Sprite _fullHeartSprite;
    public Sprite _emptyHeartSprite;

    private List<Image> hearts = new List<Image>();

    public void SetHearts(int maxHealth, int currentHealth)
    {
        foreach (Image heart in hearts)
        {
            Destroy(heart.gameObject);
        }

        hearts.Clear();

        for (int i = 0; i < maxHealth; i++)
        {
            Image newHeart = Instantiate(_heartPrefab, transform);
            newHeart.sprite = (i < currentHealth) ? _fullHeartSprite : _emptyHeartSprite;
            hearts.Add(newHeart);
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].sprite = (i < currentHealth) ? _fullHeartSprite : _emptyHeartSprite;
        }
    }
}