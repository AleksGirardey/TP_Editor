using UnityEngine;

[CreateAssetMenu(menuName = "Toolbox/Game/GameData")]
public class GameData : ScriptableObject {
    [CustomSliderProperty(0, 4)] public int nbPlayers = 1;

    public float player1Speed = 15f;
    public float player2Speed = 15f;
    public float player3Speed = 15f;
    public float player4Speed = 15f;
}
