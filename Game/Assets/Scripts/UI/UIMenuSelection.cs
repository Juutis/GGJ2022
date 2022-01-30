using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuSelection : MonoBehaviour
{
    [SerializeField]
    private Image imgSelector;
    [SerializeField]
    private Text txtTitle;

    [SerializeField]
    private UISelectionType selectionType;

    public UISelectionType SelectionType { get { return selectionType; } }

    [SerializeField]
    private Image imgRight;
    [SerializeField]
    private Image imgLeft;

    [SerializeField]
    private Text txtValue;
    [SerializeField]
    private float flashDuration = 0.2f;

    [SerializeField]
    private Color originalColor;
    [SerializeField]
    private Color flashColor;
    private bool isSelected = false;

    private float flashLeftTimer = 0f;
    private float flashRightTimer = 0f;
    private bool flashRight = false;
    private bool flashLeft = false;


    public bool IsSelected { get { return isSelected; } }

    private void Start()
    {
        txtTitle.text = selectionType == UISelectionType.Continue ? "Continue" : (
            selectionType == UISelectionType.MeinMenu ? "Main menu" : (
                selectionType == UISelectionType.Start ? "Start game" : (
                    selectionType == UISelectionType.MouseSensitivity ? "Sensitivity" : "Restart"
                )
            )
        );
        RightReset();
        LeftReset();
    }

    public void Right() {
        if (imgRight == null) {
            return;
        }
        imgRight.color = flashColor;
        flashRight = true;
        //Invoke("RightReset", flashDuration);
    }

    public void RightReset() {
        imgRight.color = originalColor;
    }
    public void Left() {
        if (imgLeft == null) {
            return;
        }
        imgLeft.color = flashColor;
        //Invoke("LeftReset", flashDuration);
        flashLeft = true;
    }

    public void LeftReset() {
        imgLeft.color = originalColor;
    }

    public void SetValue(int value) {
        if (txtValue != null) {
            txtValue.text = value.ToString();
        }
    }

    public void SetSprite(Sprite sprite)
    {
        imgSelector.sprite = sprite;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        imgSelector.enabled = selected;
    }

    void Update() {
        if (flashLeft) {
            flashLeftTimer += Time.unscaledDeltaTime;
            if (flashLeftTimer >= flashDuration) {
                flashLeftTimer = 0f;
                flashLeft = false;
                LeftReset();
            }
        }
        if (flashRight) {
            flashRightTimer += Time.unscaledDeltaTime;
            if (flashRightTimer >= flashDuration) {
                flashRightTimer = 0f;
                flashRight = false;
                RightReset();
            }
        }
    }
}

