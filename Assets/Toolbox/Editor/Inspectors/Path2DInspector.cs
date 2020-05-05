using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Path2D))]
public class Path2DInspector : Editor {
    private ReorderableList _pointsList;
    private GUIStyle _pointsLabelStyle;
    private Tool _lastUsedTool = Tool.None;
    
    private void OnEnable() {
        _pointsList = new ReorderableList(
            serializedObject,
            serializedObject.FindProperty("points")) {
            drawElementCallback = _OnDrawPointsListElement
        };

        _pointsLabelStyle = new GUIStyle {normal = {textColor = Color.yellow}, fontSize = 16};
        _lastUsedTool = Tools.current;
        Tools.current = Tool.None;
    }

    private void OnDisable() {
        _pointsList = null;

        if (Tools.current == Tool.None)
            Tools.current = _lastUsedTool;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "points");
        
        _pointsList.DoLayoutList();
        
        serializedObject.ApplyModifiedProperties();
    }

    private void _OnDrawPointsListElement(Rect rect, int index, bool isActive, bool isFocused) {
        EditorGUI.PropertyField(rect, _pointsList.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
    }

    private void OnSceneGUI() {
        Path2D path2D = target as Path2D;
        
        if (path2D == null) return;
        
        // Edit Points
        for (int i = 0; i < path2D.points.Length; ++i) {
            Vector2 point = path2D.points[i];
            EditorGUI.BeginChangeCheck();
            //point = Handles.PositionHandle(point, Quaternion.identity);
            point = Handles.FreeMoveHandle(point, Quaternion.identity, .5f, Vector3.one, Handles.CircleHandleCap);
            if (!EditorGUI.EndChangeCheck()) continue;
            
            Undo.RecordObject(path2D, "Edit Path Point");
            path2D.points[i] = point;
        }
        
        // Draw points
        Handles.color = Color.yellow;
        for (int i = 0; i < path2D.points.Length; ++i) {
            Vector2 point = path2D.points[i];
            Handles.DrawSolidDisc(point, Vector3.forward, 0.1f);
            Vector2 labelPos = point;
            labelPos.y -= 0.2f;
            Handles.Label(labelPos, (i + 1).ToString(), _pointsLabelStyle);

            if (i <= 0) continue;
            
            Vector2 previousPoint = path2D.points[i - 1];
            Handles.DrawLine(previousPoint, point);
        }

        if (path2D.isLooping && path2D.points.Length > 1) {
            Handles.DrawLine(path2D.points[path2D.points.Length - 1], path2D.points[0]);
        }
        
        //Disable Scene Interactions
        if (Event.current.type == EventType.Layout) {
            HandleUtility.AddDefaultControl(0);
        }
    }
}