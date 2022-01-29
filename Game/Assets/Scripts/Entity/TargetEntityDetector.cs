using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetEntityDetector : MonoBehaviour
{
    /*
    [SerializeField]
    private float viewDistance = 5f;
    [SerializeField]
    private float viewAngle = 45f;

    [SerializeField]

    [SerializeField]
    private Color debugRayColor = Color.cyan;

    [SerializeField]
    private float detectInterval = 0.01f;*/
    private float debugRayDuration = 0.01f;
    private float detectTimer = 0f;

    private TargetEntity host;
    private EntityFoVSettings fovSettings;

    [SerializeField]
    private bool IsEnabled = true;

    private LayerMask targetMask;
    private LayerMask everythingExceptTargetMask;

    private List<TargetEntity> oldTargets = new List<TargetEntity>();
    private List<TargetEntity> nearbyTargets = new List<TargetEntity>();
    private List<TargetEntity> seenTargets = new List<TargetEntity>();

    private void Start() {
        Initialize();
    }
    public void Initialize()
    {
        host = GetComponent<TargetEntity>();
        fovSettings = host.GetFoVSettings();
        debugRayDuration = fovSettings.DetectInterval;
        targetMask = TargetEntityManager.main.GetOpposingFactionLayer(host);
        everythingExceptTargetMask = ~targetMask;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsEnabled)
        {
            return;
        }
        detectTimer += Time.deltaTime;
        if (detectTimer >= fovSettings.DetectInterval)
        {
            detectTimer = 0f;
            Look();
        }
    }

    public void Look()
    {
        FindNearbyTargets();
        ProcessNearbyTargets();
        FindSeenTargets();
        ProcessSeenTargets();
    }


    private void ProcessSeenTargets()
    {
        if (TargetEntityManager.main.DebugEntityMaterials)
        {
            DebugSeenTargets();
        }
        foreach(TargetEntity seenTarget in seenTargets) {
            //ShootAt(seenTarget);
        }
    }


    private void ProcessNearbyTargets()
    {
        if (TargetEntityManager.main.DebugEntityMaterials)
        {
            DebugNearbyTargets();
        }
    }

    private bool IsNearby(TargetEntity target)
    {
        return fovSettings.Distance >= Mathf.Abs(Vector3.Distance(target.Position, host.ViewPosition));
    }

    private void FindNearbyTargets()
    {
        nearbyTargets = TargetEntityManager.main.Targets
            .FindAll(target => target.TargetType != host.TargetType && IsNearby(target))
            .ToList();
    }


    private GameObject CastRay(Vector3 direction)
    {
        Vector3 pos = host.ViewPosition;

        Physics.Raycast(
            pos,
            direction,
            out RaycastHit hitInfo,
            fovSettings.Distance
        );
        return hitInfo.collider ? hitInfo.collider.gameObject : null;
    }

    private bool DirectionIsWithinFoV(Vector3 lookForward, Vector3 direction)
    {
        float angle = Vector3.Angle(
            new Vector3(direction.x, 0f, direction.z),
            new Vector3(lookForward.x, 0f, lookForward.z)
        );
        return Mathf.Abs(angle) <= fovSettings.ViewAngle;
    }

    private TargetEntity FindTargetWithinFov(TargetEntity targetEntity)
    {
        TargetEntity foundEntity = null;

        foreach (Transform viewTarget in targetEntity.ViewTargets)
        {
            Vector3 direction = viewTarget.position - host.ViewPosition;
            GameObject hitObject = CastRay(direction.normalized);
            RayHitInfo rayInfo = new RayHitInfo(hitObject, targetMask, targetEntity);
            if (rayInfo.ObjectEqualsTarget)
            {
                foundEntity = rayInfo.HitEntity;
            }
            DebugTargetWithinFoV(rayInfo, direction);
        }
        return foundEntity;
    }


    private void FindSeenTargets()
    {
        Vector3 lookForward = host.ViewDirection * fovSettings.Distance;
        seenTargets = new List<TargetEntity>();
        foreach (TargetEntity nearbyTarget in nearbyTargets)
        {
            Vector3 hostToTarget = nearbyTarget.Position - host.ViewPosition;
            if (DirectionIsWithinFoV(lookForward, hostToTarget))
            {
                TargetEntity foundTarget = FindTargetWithinFov(nearbyTarget);
                if (foundTarget == nearbyTarget)
                {
                    seenTargets.Add(nearbyTarget);
                }
            }
            else
            {
                DebugVector(host.ViewPosition, hostToTarget, Color.yellow);
            }
        }
        DebugFoV(lookForward);
    }


    // DEBUG METHODS
    private void DebugNearbyTargets()
    {
        foreach (TargetEntity nearbyTarget in nearbyTargets)
        {
            nearbyTarget.DebugSetIsNearby(true);
        }
        foreach (TargetEntity oldNearbyTarget in oldTargets)
        {
            if (!nearbyTargets.Contains(oldNearbyTarget))
            {
                oldNearbyTarget.DebugSetIsNearby(false);
            }
        }
    }

    private void DebugSeenTargets()
    {
        foreach (TargetEntity oldTarget in oldTargets)
        {
            if (!seenTargets.Contains(oldTarget))
            {
                oldTarget.DebugSetIsSeen(false);
            }
        }
        foreach (TargetEntity seenTarget in seenTargets)
        {
            seenTarget.DebugSetIsSeen(true);
        }
        oldTargets = new List<TargetEntity>(seenTargets);
    }

    private void DebugTargetWithinFoV(RayHitInfo rayInfo, Vector3 direction)
    {
        if (!TargetEntityManager.main.DebugRays)
        {
            return;
        }
        if (rayInfo.ObjectEqualsTarget)
        {
            DebugVector(host.ViewPosition, direction, Color.magenta);
        }
        else if (rayInfo.ObjectIsCorrectLayer)
        {
            DebugVector(host.ViewPosition, direction, Color.blue);
        }
        else if (rayInfo.ObjectWasHit)
        {
            DebugVector(host.ViewPosition, direction, Color.black);
        }
        else
        {
            DebugVector(host.ViewPosition, direction, Color.gray);
        }

    }

    private void DebugFoV(Vector3 lookForward)
    {
        if (!TargetEntityManager.main.DebugRays)
        {
            return;
        }
        DebugVector(host.ViewPosition, lookForward, Color.white);
        Vector3 lookLeft = Quaternion.AngleAxis(-fovSettings.ViewAngle, Vector3.up) * lookForward;
        DebugVector(host.ViewPosition, lookLeft, Color.white);
        Vector3 lookRight = Quaternion.AngleAxis(fovSettings.ViewAngle, Vector3.up) * lookForward;
        DebugVector(host.ViewPosition, lookRight, Color.white);
    }

    private void DebugVector(Vector3 origin, Vector3 vector, Color color)
    {
        if (!TargetEntityManager.main.DebugRays)
        {
            return;
        }
        Debug.DrawRay(origin, vector, color, debugRayDuration);
    }

}


public class RayHitInfo
{
    public bool ObjectWasHit = false;
    public bool ObjectIsCorrectLayer = false;
    public bool ObjectEqualsTarget = false;
    public TargetEntity HitEntity;
    public RayHitInfo(GameObject hitObject, LayerMask targetMask, TargetEntity targetEntity)
    {
        ObjectWasHit = hitObject != null;
        if (ObjectWasHit)
        {
            ObjectIsCorrectLayer = hitObject.layer == targetMask;
            if (ObjectIsCorrectLayer)
            {
                HitEntity = hitObject.GetComponentInParent<TargetEntity>();
                ObjectEqualsTarget = HitEntity == targetEntity;
            }
        }
    }
}