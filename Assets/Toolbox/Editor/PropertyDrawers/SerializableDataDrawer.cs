using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableData))]
public class SerializableDataDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        GUI.Box(position, GUIContent.none);

        Rect labelPosition = position;
        labelPosition.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(labelPosition, label, EditorStyles.boldLabel);
        
        Rect intValuePosition = labelPosition;
        intValuePosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(intValuePosition, property.FindPropertyRelative("intValue"));

        Rect floatValuePosition = intValuePosition;
        floatValuePosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(floatValuePosition, property.FindPropertyRelative("floatValue"));
        
        Rect stringValuePosition = floatValuePosition;
        stringValuePosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(stringValuePosition, property.FindPropertyRelative("stringValue"));
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 4f;
    }
}