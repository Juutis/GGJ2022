using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlignment : MonoBehaviour
{
    public static PlayerAlignment main;
    private void Awake()
    {
        main = this;
    }

    private int step = 20;

    private int maxAligment = 100;
    private int minAlignment = 0;

    private int currentAlignment = 50;

    private void Start()
    {
        UIAlignmentBar.main.Initialize(minAlignment, maxAligment, currentAlignment);
    }

    public void MoveAlignment(TargetEntity host, GameObject killedTarget)
    {

        TargetEntity killedTargetEntity = killedTarget.GetComponent<TargetEntity>();
        bool towardsLycantrophy = killedTargetEntity.TargetType == TargetEntityType.Human;
        int aligmentChange = towardsLycantrophy ? -step : step;
        currentAlignment = System.Math.Clamp(currentAlignment + aligmentChange, minAlignment, maxAligment);
        if (currentAlignment == maxAligment)
        {
            Debug.Log("<color=gray><i>You are finally rid of lycantrophy and have become a boring full-ass human again...</i></color>");
        }
        else if (currentAlignment == minAlignment)
        {
            Debug.Log("<color=red><b>You have become a FULL BLOWN WEREWOLF! AAAAAAAH!!!</b></color>");
        }
        UIAlignmentBar.main.SetAlignment(currentAlignment);
    }
}
