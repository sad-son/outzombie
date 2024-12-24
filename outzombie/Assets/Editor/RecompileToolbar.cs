using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace M3.UnityToolbarExtender
{
    [InitializeOnLoad]
    public static class RecompileToolbar
    {
        static RecompileToolbar()
        {
            ToolbarExtender.RightToolbarGUI.Remove(OnToolbarGUI);
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload))
            {
                EditorGUILayout.LabelField(EditorGUIUtility.TrTextContentWithIcon(
                        "Domain Reload is disabled", "Static changes may still be present", MessageType.Info),
                    EditorStyles.miniBoldLabel);
            }

            if (GUILayout.Button(new GUIContent("Recompile", "Request Script Compilation"),
                    EditorStyles.toolbarButton))
            {
                CompilationPipeline.RequestScriptCompilation();
            }
        }
    }
}