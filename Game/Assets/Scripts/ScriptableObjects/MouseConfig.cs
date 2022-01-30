using UnityEngine;

[CreateAssetMenu(fileName = "MouseConfig", menuName = "ScriptableObjects/new MouseConfig")]
public class MouseConfig : ScriptableObject
{
    public int Sensitivity = 10;
    public int MaxSensitivity = 30;
    public int MinSensitivity = 1;
    public int SensitivityStep = 1;
    public float SensitivityFactor { get {return Sensitivity * 0.01f;}}
    public bool IncreaseSensitivity() {
        int oldSens = Sensitivity;
        Sensitivity = System.Math.Clamp(Sensitivity + SensitivityStep, MinSensitivity, MaxSensitivity);
        return oldSens != Sensitivity;
    }
    public bool DecreaseSensitivity() {
        int oldSens = Sensitivity;
        Sensitivity = System.Math.Clamp(Sensitivity - SensitivityStep, MinSensitivity, MaxSensitivity);
        return oldSens != Sensitivity;
    }
}