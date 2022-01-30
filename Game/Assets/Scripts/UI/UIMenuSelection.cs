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
    private bool isSelected = false;

    public bool IsSelected { get { return isSelected; } }

    private void Start()
    {
        txtTitle.text = selectionType == UISelectionType.Continue ? "Continue" : (
            selectionType == UISelectionType.MeinMenu ? "Main menu" : (
                selectionType == UISelectionType.Start ? "Start game" : "Restart"
            )
        );
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
}

