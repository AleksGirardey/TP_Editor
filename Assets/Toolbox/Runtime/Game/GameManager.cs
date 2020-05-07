using UnityEngine;

public class GameManager : MonoBehaviour {
    private const string GameObjectName = "GameManager";
    public GameData gameData;

    private void OnValidate() {
        if (gameObject.name != GameObjectName)
            gameObject.name = GameObjectName;
    }
}
