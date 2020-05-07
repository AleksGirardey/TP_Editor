using UnityEngine;

public class TestProperty : MonoBehaviour {
    [GameObjectTagFilter("TestTag")]
    public GameObject testTag;

    public SerializableData testData;
}
