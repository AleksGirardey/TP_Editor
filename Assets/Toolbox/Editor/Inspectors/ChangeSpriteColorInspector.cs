using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CustomEditor(typeof(ChangeSpriteColor))]
public class ChangeSpriteColorInspector : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Change Color Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        SerializedProperty changeModeProperty = serializedObject.FindProperty("changeMode");
        EditorGUILayout.PropertyField(changeModeProperty);

        ChangeSpriteColor.ChangeMode changeMode = (ChangeSpriteColor.ChangeMode) changeModeProperty.intValue;

        if (changeMode == ChangeSpriteColor.ChangeMode.Custom) {
            SerializedProperty customColorProperty = serializedObject.FindProperty("customColor");
            EditorGUILayout.PropertyField(customColorProperty);
        }
        
        EditorGUILayout.EndVertical();
        
        serializedObject.ApplyModifiedProperties();
        
        EditorGUILayout.Space();

        if (changeMode == ChangeSpriteColor.ChangeMode.Random) {
            EditorGUILayout.HelpBox("Color will be randomize. Be careful !", MessageType.Info);
            EditorGUILayout.Space();
        }

        ChangeSpriteColor changeSpriteColor = target as ChangeSpriteColor;

        if (!GUILayout.Button("Change Color") || changeSpriteColor == null) return;
        Color color;
        
//        Undo.RecordObject(changeSpriteColor, "Change Color");
        
        switch (changeMode) {
            case ChangeSpriteColor.ChangeMode.Random:
                color = new Color {
                    r = Random.Range(0f, 1f),
                    g = Random.Range(0f, 1f),
                    b = Random.Range(0f, 1f),
                    a = 1f
                };
                break;
            case ChangeSpriteColor.ChangeMode.Custom:
                color = changeSpriteColor.customColor;
                break;
            default:
                color = Color.white;
                break;
        }

        changeSpriteColor.GetComponent<SpriteRenderer>().color = color;
    }
}
