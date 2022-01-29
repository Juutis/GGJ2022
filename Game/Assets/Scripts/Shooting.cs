using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particles;
    [SerializeField]
    private Camera playerCamera;
    private TargetEntity host;

    // Start is called before the first frame update
    void Start()
    {
        host = GetComponent<TargetEntity>();
    }

    public void Shoot()
    {
        particles.Play();
        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hitData;
        var rayDirection = playerCamera.transform.forward;
        bool hit = Physics.Raycast(rayOrigin, rayDirection, out hitData);

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
                        killable.DealDamage(2, hitData.point, rayDirection, 100.0f);
                    }
                }
            }
        }
    }
}
