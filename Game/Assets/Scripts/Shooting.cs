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

    [SerializeField]
    private int pellets = 8;

    [SerializeField]
    private float inaccuracyAngle = 10;

    [SerializeField]
    private float dispersionAngle = 5;

    [SerializeField]
    private ParticleSystem hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        host = GetComponent<TargetEntity>();
    }

    public void Shoot()
    {
        //particles.Play();
        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        var aimDir = playerCamera.transform.forward;
        var randomAimDir = Quaternion.AngleAxis(Random.Range(0, inaccuracyAngle), playerCamera.transform.up) * aimDir;
        var rayBaseDirection = Quaternion.AngleAxis(Random.Range(0, 360), aimDir) * randomAimDir;

        for (var i = 0; i < pellets; i++) {

            Debug.Log("raycasting " + i + " " + pellets);

            RaycastHit hitData;
            
            var dispersedDir = Quaternion.AngleAxis(Random.Range(0, dispersionAngle), playerCamera.transform.up) * rayBaseDirection;
            var rayDirection = Quaternion.AngleAxis(Random.Range(0, 360), rayBaseDirection) * dispersedDir;

            bool hit = Physics.Raycast(rayOrigin, rayDirection, out hitData);

            if (hit)
            {
                var effect = Instantiate(hitEffect);
                effect.transform.position = hitData.point;

                if (hitData.collider != null)
                {
                    TargetEntity targetEntity = hitData.collider.gameObject.GetComponentInParent<TargetEntity>();
                    
                    if (targetEntity == null) continue;
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
}
