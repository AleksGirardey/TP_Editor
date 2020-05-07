using UnityEngine;

public class CustomSliderPropertyAttribute : PropertyAttribute {
    public int MinValue { get; }
    public int MaxValue { get; }

    public CustomSliderPropertyAttribute(int min, int max) {
        MinValue = min;
        MaxValue = max;
    }
}
