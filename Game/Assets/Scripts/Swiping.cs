using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Swiping : MonoBehaviour
{
    [SerializeField]
    WolfhandsAnimation handAnimation;

    [SerializeField]
    private ParticleSystem particles;

    [SerializeField]
    private ParticleSystem hitParticles;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private bool isPlayer;
    private TargetEntity host;

    [SerializeField]
    private Transform heavySwipePoint;

    [SerializeField]
    private float heavySwipeRadius;



    // Start is called before the first frame update
    void Start()
    {
        host = GetComponent<TargetEntity>();
    }

    public void NormalSwipe()
    {
        if (!handAnimation.ReadyToAttack()) return;
        // particles.Play();
        handAnimation.QuickAttack();
        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hitData;
        var rayDirection = playerCamera.transform.forward;
        bool hit = Physics.Raycast(rayOrigin, rayDirection, out hitData, 3.5f);

        if (hit)
        {
            if (hitData.collider != null)
            {
                TargetEntity targetEntity = hitData.collider.gameObject.GetComponentInParent<TargetEntity>();
                if (targetEntity == null) return;
                if (targetEntity.TargetType != host.TargetType)
                {
                    GameObject target = hitData.collider.gameObject;
                    Killable killable = target.GetComponentInParent<Killable>();
                    if (killable != null)
                    {
                        killable.DealDamage(3, hitData.point, rayDirection, 150.0f);
                        // hitParticles.Play();
                        bool targetDied = killable.DealDamage(2, hitData.point, rayDirection, 100.0f);
                        if (host.IsPlayer && targetDied) {
                            PlayerAlignment.main.MoveAlignment(host, killable.gameObject);
                        }
                    }
                }
            }
        }
    }

    public void HeavySwipe()
    {
        // particles.Play();
        handAnimation.PowerAttack();
        SwipeFront();
    }

    public void SwipeFront() {
        var killables = CheckFront();
        foreach (var killable in killables) {
            var dir = killable.transform.position - transform.position;
            killable.DealDamage(10, heavySwipePoint.position, dir, 150f);
        }
    }

    public void IsEnemyInFront() {

    }

    public List<Killable> CheckFront() {
        return TargetEntityManager.main.Targets
            .FindAll(target => target.TargetType == TargetEntityType.Human && IsWithinDamageRange(target))
            .Select(it => it.GetComponent<Killable>())
            .ToList();
    }

    private bool IsWithinDamageRange(TargetEntity candidate) {
        var distance = Vector3.Distance(heavySwipePoint.position, candidate.transform.position);
        return distance < heavySwipeRadius;
    }
}
