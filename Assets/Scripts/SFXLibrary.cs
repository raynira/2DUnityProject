using System.Collections.Generic;
using UnityEngine;

public class SFXLibrary : MonoBehaviour
{
    [SerializeField] private SFXGroup[] _SFXGroups;
    private Dictionary<string, List<AudioClip>> _sfxDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        _sfxDictionary = new Dictionary<string, List<AudioClip>>();

        foreach (SFXGroup sfxGroup in _SFXGroups)
        {
            _sfxDictionary[sfxGroup.name] = sfxGroup.audioClips;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (_sfxDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = _sfxDictionary[name];

            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }

        return null;
    }
}

[System.Serializable]
public struct SFXGroup
{
    public string name;
    public List<AudioClip> audioClips;
}
