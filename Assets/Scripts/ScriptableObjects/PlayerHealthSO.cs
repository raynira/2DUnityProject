using UnityEngine;

[CreateAssetMenu]

public class PlayerHealthSO : ScriptableObject
{
    public int Value;
    public bool IsInitialized;

    public void ResetState()
    {
        IsInitialized = false;
        Value = 0;
    }
}