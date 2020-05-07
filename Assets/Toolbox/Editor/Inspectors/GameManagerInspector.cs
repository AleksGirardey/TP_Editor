using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        SerializedProperty gameDataProperty = serializedObject.FindProperty("gameData");
        
        if (gameDataProperty.objectReferenceValue) return;
        
        serializedObject.Update();
        gameDataProperty.objectReferenceValue = _FindGameDataInProject();
        serializedObject.ApplyModifiedProperties();
    }

    private GameData _FindGameDataInProject() {
        string[] fileGuidArr = AssetDatabase.FindAssets("t:" + typeof(GameData));

        if (fileGuidArr.Length <= 0)
            return _CreateGameDataInProject();
        
        string assetPath = AssetDatabase.GUIDToAssetPath(fileGuidArr[0]);
        return AssetDatabase.LoadAssetAtPath<GameData>(assetPath);
    }

    private GameData _CreateGameDataInProject() {
        GameData gameData = CreateInstance<GameData>();
        AssetDatabase.CreateAsset(gameData, "Assets/GameData.asset");
        AssetDatabase.SaveAssets();

        return gameData;
    }
}