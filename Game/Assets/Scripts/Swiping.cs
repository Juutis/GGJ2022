using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        host = GetComponent<TargetEntity>();
    }

    public void NormalSwipe()
    {
        // particles.Play();
        handAnimation.TriggerQuickAttack = true;
        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hitData;
        bool hit = Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hitData, 3.5f);

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
                        killable.DealDamage(3);
                        // hitParticles.Play();
                    }
                }
            }
        }
    }

    public void HeavySwipe()
    {
        // particles.Play();
        handAnimation.TriggerPowerAttack = true;
        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hitData;
        bool hit = Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hitData, 3.5f);

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
                        killable.DealDamage(10);
                        // hitParticles.Play();
                    }
                }
            }
        }
    }
}
