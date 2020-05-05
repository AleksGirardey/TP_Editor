using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChangeSpriteColor : MonoBehaviour {
    public enum ChangeMode {
        Random = 0,
        Custom
    }

    public ChangeMode changeMode = ChangeMode.Random;
    public Color customColor = Color.white;
}
