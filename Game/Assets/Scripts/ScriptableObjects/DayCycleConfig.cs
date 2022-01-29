using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DayCycleConfig", menuName = "ScriptableObjects/new DayCycleConfig")]
public class DayCycleConfig : ScriptableObject
{
    public float DayLength;

    public Color DawnAmbientColor;
    public Color DayAmbientColor;
    public Color DuskAmbientColor;
    public Color NightAmbientColor;

    public Color DawnSkyColor;
    public Color DaySkyColor;
    public Color DuskSkyColor;
    public Color NightSkyColor;
}
