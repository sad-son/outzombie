using M3.UnityToolbarExtender;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad]
    public static class StartSceneToolbar
    {
        private static SceneAsset startScene
        {
            get
            {
                var path = "Assets/Scenes/Entry.unity";
                return AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            }
        }

        static StartSceneToolbar()
        {
            ToolbarExtender.LeftToolbarGUI.Remove(OnToolbarGUI);
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

            EditorApplication.playModeStateChanged += ResetStartScene;
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent($"{startScene.name}", $"Start {startScene.name} Scene"),
                    EditorStyles.toolbarButton))
            {
                EditorSceneManager.playModeStartScene = startScene;
                EditorApplication.isPlaying = true;
            }

            return;
        }

        private static void ResetStartScene(PlayModeStateChange state)
        {
            if (EditorSceneManager.playModeStartScene != startScene ||
                state != PlayModeStateChange.ExitingPlayMode) return;

            EditorSceneManager.playModeStartScene = null;
        }
    }
}