using System.Threading.Tasks;
using UnityEngine;

public class ScreenFadeTransition : MonoBehaviour
{
    public static ScreenFadeTransition Instance;
    [SerializeField] CanvasGroup _canvas;
    [SerializeField] float _fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async Task Fade(float targetTransparency)
    {
        float start = _canvas.alpha, t = 0;

        while (t < _fadeDuration)
        {
            t += Time.deltaTime;

            _canvas.alpha = Mathf.Lerp(start, targetTransparency, t / _fadeDuration);

            await Task.Yield();
        }

        _canvas.alpha = targetTransparency;
    }

    public async Task FadeOut()
    {
        await Fade(1);
    }

    public async Task FadeIn()
    {
        await Fade(0);
    }
}