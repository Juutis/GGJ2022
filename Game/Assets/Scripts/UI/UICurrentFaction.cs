using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurrentFaction : MonoBehaviour
{
    public static UICurrentFaction main;
    private void Awake() {
        main = this;
    }

    [SerializeField]
    private Image imgIcon;
    public void SetFaction(TargetEntityType faction) {
        imgIcon.sprite = TargetEntityManager.main.GetFactionSprite(faction);
    }
}
