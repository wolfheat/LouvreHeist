using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

// Assets/Editor/PlayModeSceneCleaner.cs

[InitializeOnLoad]
public static class PlayModeSceneCleaner
{
    private static SceneSetup[] cachedSetup;

    static PlayModeSceneCleaner()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        switch (state) {
            case PlayModeStateChange.ExitingEditMode:
                SaveAndClean();
                break;

            case PlayModeStateChange.EnteredEditMode:
                Restore();
                break;
        }
    }

    private static void SaveAndClean()
    {
        cachedSetup = EditorSceneManager.GetSceneManagerSetup();

        foreach (var setup in cachedSetup) {
            if (!setup.isLoaded) {
                var scene = EditorSceneManager.GetSceneByPath(setup.path);

                if (scene.IsValid()) {
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
        }
    }

    private static void Restore()
    {
        if (cachedSetup != null && cachedSetup.Length > 0) {
            EditorSceneManager.RestoreSceneManagerSetup(cachedSetup);
            cachedSetup = null;
        }
    }
}
