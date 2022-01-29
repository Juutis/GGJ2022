using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar main;
    private void Awake() {
        main = this;
    }
    [SerializeField]
    private Image imgHealthBar;
    public void SetHealth(float value) {
        imgHealthBar.fillAmount = value;
    }
}
