using UnityEngine;

[CreateAssetMenu]

public class LevelsSurvivedSO : ScriptableObject
{
    public int Value;

    public void ResetState() => Value = 0;
}
