using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEntityManager : MonoBehaviour
{

    public static TargetEntityManager main;

    [SerializeField]
    public Color IsSeenColor;
    [SerializeField]
    public Color IsNearbyColor;

    [SerializeField]
    private bool debugRays = true;
    public bool DebugRays { get { return debugRays; } }

    [SerializeField]
    private bool debugEntityMaterials = true;
    public bool DebugEntityMaterials { get { return debugEntityMaterials; } }

    [SerializeField]
    private EntityFoVSettings humanFov;
    public EntityFoVSettings HumanFoV { get { return humanFov; } }

    [SerializeField]
    private EntityFoVSettings werewolfFov;
    public EntityFoVSettings WerewolfFoV { get { return werewolfFov; } }

    [SerializeField]
    private Sprite werewolfSprite;
    [SerializeField]
    private Sprite humanSprite;


    void Awake()
    {
        main = this;
    }
    private List<TargetEntity> targets = new List<TargetEntity>();
    public List<TargetEntity> Targets { get { return targets; } }
    public void RegisterTarget(TargetEntity target)
    {
        targets.Add(target);
    }

    public void KillTarget(TargetEntity target)
    {
        targets.Remove(target);
        target.Kill();
    }

    public LayerMask GetOpposingFactionLayer(TargetEntity target)
    {
        string targetLayerName = target.TargetType == TargetEntityType.Human ? "Werewolf" : "Human";
        return LayerMask.NameToLayer(targetLayerName);
    }

    public LayerMask GetFriendlyFactionLayer(TargetEntity target)
    {
        string targetLayerName = target.TargetType == TargetEntityType.Human ? "Human" : "Werewolf";
        return LayerMask.NameToLayer(targetLayerName);
    }
    public Sprite GetFactionSprite(TargetEntityType faction)
    {
        return faction == TargetEntityType.Human ? humanSprite : werewolfSprite;
    }

}

[System.Serializable]
public class EntityFoVSettings
{
    [SerializeField]
    private float distance = 5f;
    [SerializeField]
    private float viewAngle = 45f;
    [SerializeField]
    private float detectInterval = 0.2f;
    public float Distance { get { return distance; } }
    public float ViewAngle { get { return viewAngle; } }
    public float DetectInterval { get { return detectInterval; } }
}