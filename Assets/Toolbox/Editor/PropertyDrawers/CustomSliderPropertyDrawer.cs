using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CustomSliderPropertyAttribute))]
public class CustomSliderPropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (!(attribute is CustomSliderPropertyAttribute sliderAttribute)) return;
        
        EditorGUI.IntSlider(position, property, sliderAttribute.MinValue, sliderAttribute.MaxValue);
    }
}