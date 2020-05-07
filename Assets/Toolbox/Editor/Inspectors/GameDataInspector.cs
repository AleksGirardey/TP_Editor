using UnityEditor;

[CustomEditor(typeof(GameData))]
public class GameDataInspector : Editor {
    private SerializedProperty _nbPlayersProperty;
    private SerializedProperty _player1SpeedProperty;
    private SerializedProperty _player2SpeedProperty;
    private SerializedProperty _player3SpeedProperty;
    private SerializedProperty _player4SpeedProperty;

    private void OnEnable() {
        _nbPlayersProperty = serializedObject.FindProperty("nbPlayers");
        _player1SpeedProperty = serializedObject.FindProperty("player1Speed");
        _player2SpeedProperty = serializedObject.FindProperty("player2Speed");
        _player3SpeedProperty = serializedObject.FindProperty("player3Speed");
        _player4SpeedProperty = serializedObject.FindProperty("player4Speed");
    }

    private void OnDisable() {
        _nbPlayersProperty = null;
        _player1SpeedProperty = null;
        _player2SpeedProperty = null;
        _player3SpeedProperty = null;
        _player4SpeedProperty = null;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        //EditorGUILayout.IntSlider(_nbPlayersProperty, 0, 4);
        EditorGUILayout.PropertyField(_nbPlayersProperty);


        int nbPlayers = _nbPlayersProperty.intValue;
        if (nbPlayers >= 1) EditorGUILayout.PropertyField(_player1SpeedProperty);
        if (nbPlayers >= 2) EditorGUILayout.PropertyField(_player2SpeedProperty);
        if (nbPlayers >= 3) EditorGUILayout.PropertyField(_player3SpeedProperty);
        if (nbPlayers >= 4) EditorGUILayout.PropertyField(_player4SpeedProperty);
        
        serializedObject.ApplyModifiedProperties();
    }
}