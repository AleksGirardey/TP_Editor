using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(MoveAround2D))]
public class MoveAroundInspector : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        ToolboxGuiLayout.BeginBox("Move Around Properties");
        
        SerializedProperty radiusProperty = serializedObject.FindProperty("radius");
        EditorGUILayout.Slider(radiusProperty, 1f, 100f);
        SerializedProperty speedProperty = serializedObject.FindProperty("speed");
        EditorGUILayout.Slider(speedProperty, 0f, 500f);
        
        ToolboxGuiLayout.EndBox();

        SerializedProperty useGuiDebug = serializedObject.FindProperty("guiDebug");
        SerializedProperty useGizmosDebug = serializedObject.FindProperty("gizmosDebug");
        
        useGuiDebug.boolValue = EditorGUILayout.Toggle("Display GUI", useGuiDebug.boolValue);
        if (useGuiDebug.boolValue) DisplayGuiDebug();
        useGizmosDebug.boolValue = EditorGUILayout.Toggle("Display Gizmos", useGizmosDebug.boolValue);
        if (useGizmosDebug.boolValue) DisplayGizmosDebug();
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayGuiDebug() {
        ToolboxGuiLayout.BeginBox("GUI Debug Properties");
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("guiFontSize"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("guiColor"));

        ToolboxGuiLayout.EndBox();
    }

    private void DisplayGizmosDebug() {
        ToolboxGuiLayout.BeginBox("Gizmos Debug Properties");
        
        EditorGUILayout.Slider(serializedObject.FindProperty("gizmosSize"), 0.1f, 1f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosCenterColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosDestinationColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosRadiusColor"));
        
        ToolboxGuiLayout.EndBox();
    }

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
    private static void DrawGizmos(MoveAround2D moveAround2D, GizmoType gizmoType) {
        if (!moveAround2D.gizmosDebug) return;
        
        Vector3 position = moveAround2D.transform.position;

        //Draw Center
        Gizmos.color = moveAround2D.gizmosCenterColor;
        Gizmos.DrawWireSphere(moveAround2D.Center, moveAround2D.gizmosSize);
        
        //Draw destination
        Gizmos.color = moveAround2D.gizmosDestinationColor;
        Gizmos.DrawWireSphere(position, moveAround2D.gizmosSize);
        
        //Draw radius
        Gizmos.color = moveAround2D.gizmosRadiusColor;
        Gizmos.DrawLine(moveAround2D.Center, position);
    }
}