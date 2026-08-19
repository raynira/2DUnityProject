using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private static SFXManager Instance;

    private static SFXLibrary _library;
    private static AudioSource _source;

    public float _volume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _source = GetComponent<AudioSource>();
            _library = GetComponent<SFXLibrary>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip clip = _library.GetRandomClip(soundName);

        if (clip != null)
        {
            _source.PlayOneShot(clip);
        }
    }
    
    void Start()
    {
        _source.volume = _volume;
    }
}
