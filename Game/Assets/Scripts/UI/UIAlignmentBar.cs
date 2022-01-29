using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAlignmentBar : MonoBehaviour
{
    public static UIAlignmentBar main;
    private void Awake() {
        main = this;
    }

    [SerializeField]
    private Slider slider;

    public void Initialize(int min, int max, int currentValue) {
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = currentValue;
    }
    public void SetAlignment(int value) {
        slider.value = value;
    }
}
