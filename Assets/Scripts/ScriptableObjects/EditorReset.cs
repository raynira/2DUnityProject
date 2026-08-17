#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]

public static class EditorReset
{
    private const string HealthSOPath = "Assets/Scripts/ScriptableObjects/HealthSO.asset";

    static EditorReset()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            var so = AssetDatabase.LoadAssetAtPath<PlayerHealthSO>(HealthSOPath);

            if (so != null)
            {
                so.ResetState();
                EditorUtility.SetDirty(so);
            }
        }
    }
}
#endif