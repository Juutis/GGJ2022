using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEntity : MonoBehaviour
{
    [SerializeField]
    private TargetEntityType targetType;
    public TargetEntityType TargetType { get { return targetType; } }

    public Vector3 Position { get { return transform.position; } }
    public Vector3 Direction { get { return transform.forward; } }

    public Vector3 ViewPosition { get { return viewOrigin.position; } }
    public Vector3 ViewDirection { get { return viewOrigin.forward; } }

    private Color originalColor;

    [SerializeField]
    private Transform viewTargetContainer;
    private List<Transform> viewTargets = new List<Transform>();
    public List<Transform> ViewTargets { get { return viewTargets; } }

    [SerializeField]
    private Transform viewOrigin;

    [SerializeField]
    private MeshRenderer mesh;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (viewOrigin == null)
        {
            Debug.LogWarning($"Entity '{name}' has null <b>viewOrigin</b>!");
            return;
        }
        if (viewTargetContainer == null)
        {
            Debug.LogWarning($"Entity '{name}' has null <b>viewTargetContainer</b>!");
            return;
        }
        if (viewTargetContainer.childCount == 0)
        {
            Debug.LogWarning($"Entity '{name}' doesn't have any view targets! Add some under <b>viewTargetContainer</b>!");
        }
        foreach (Transform child in viewTargetContainer)
        {
            viewTargets.Add(child);
            foreach (Transform targetChild in child)
            {
                viewTargets.Add(targetChild);
            }
        }
        originalColor = mesh.material.color;
        TargetEntityManager.main.RegisterTarget(this);
    }

    public EntityFoVSettings GetFoVSettings()
    {
        if (TargetType == TargetEntityType.Human)
        {
            return TargetEntityManager.main.HumanFoV;
        }
        return TargetEntityManager.main.WerewolfFoV;
    }

    public void Kill()
    {

    }

    public void DebugSetIsNearby(bool isNearby)
    {
        mesh.material.color = isNearby ? TargetEntityManager.main.IsNearbyColor : originalColor;
    }


    public void DebugSetIsSeen(bool isSeen)
    {
        mesh.material.color = isSeen ? TargetEntityManager.main.IsSeenColor : originalColor;
    }

    public void TogglePlayerTargetType()
    {
        targetType = targetType == TargetEntityType.Human ? TargetEntityType.Werewolf : TargetEntityType.Human;
    }

}

public enum TargetEntityType
{
    Human,
    Werewolf
}
