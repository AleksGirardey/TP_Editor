using UnityEngine;

public class GameObjectTagFilterAttribute : PropertyAttribute {
    public string TagFilter { get; }

    public GameObjectTagFilterAttribute(string tagFilter) {
        TagFilter = tagFilter;
    }
}