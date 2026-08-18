#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]

public static class EditorReset
{
    private const string HealthSOPath = "Assets/Scripts/ScriptableObjects/HealthSO.asset";
    private const string LevelsSurvivedSOPath = "Assets/Scripts/ScriptableObjects/LevelsSurvivedSO.asset";

    static EditorReset()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            var hso = AssetDatabase.LoadAssetAtPath<PlayerHealthSO>(HealthSOPath);
            var lso = AssetDatabase.LoadAssetAtPath<LevelsSurvivedSO>(LevelsSurvivedSOPath);

            if (hso != null && lso != null)
            {
                hso.ResetState();
                lso.ResetState();

                EditorUtility.SetDirty(hso);
                EditorUtility.SetDirty(lso);
            }
        }
    }
}
#endif