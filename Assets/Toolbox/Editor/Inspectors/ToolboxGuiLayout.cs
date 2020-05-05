using UnityEditor;
using UnityEngine;

public static class ToolboxGuiLayout {
    public static void BeginBox(string label) {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        EditorGUILayout.Space();
    }

    public static void EndBox() {
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }
}