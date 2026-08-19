using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;

    private AudioSource _source;

    public AudioClip _backgroundMusic;
    public float _volume;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _source = GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (_backgroundMusic != null)
        {
            PlayBackgroundMusic(false, _backgroundMusic);
        }

        Instance._source.volume = _volume;
    }

    public static void PlayBackgroundMusic(bool resetSong, AudioClip clip = null)
    {
        if (clip != null)
        {
            Instance._source.clip = clip;

            Instance._source.Play();
        }
        else if (Instance._source.clip != null)
        {
            if (resetSong)
            {
                Instance._source.Stop();
            }

            Instance._source.Play();
        }
    }

    public static void PauseBackgroundMusic()
    {
        Instance._source.Pause();
    }
}
