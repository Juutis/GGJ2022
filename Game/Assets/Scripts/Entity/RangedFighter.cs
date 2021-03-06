using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedFighter : MonoBehaviour
{
    
    [SerializeField]
    private float damage = 1;

    private CharacterAnimator charAnim;

    private TargetEntity target;

    private bool aiming = false;
    private bool firing = false;
    private Vector3 lastKnownTargetPos;

    private bool alive = true;

    [SerializeField]
    private ParticleSystem muzzle;

    [SerializeField]
    private ParticleSystem bloodEffect;

    [SerializeField]
    private ParticleSystem hitEffect;

    [SerializeField]
    private TrailRenderer bulletTrail;

    // Start is called before the first frame update
    void Start()
    {
        charAnim = GetComponentInChildren<CharacterAnimator>();
    }

    public void Die() {
        alive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!alive) {
            if (charAnim != null) {
                charAnim.StopAim();
            }
            return;
        }
        if (target != null) {
            if (!firing) {
                firing = true;
                aiming = true;
                Invoke("StartFiring", Random.Range(1.5f, 3.0f));
            }

            if(aiming) {
                lastKnownTargetPos = target.Position;
            }
        }

        if (firing) {
            if (charAnim != null) {
                charAnim.AimAt(lastKnownTargetPos);
            }
        } else {
            if (charAnim != null) {
                charAnim.StopAim();
            }
        }
    }

    public void SetTarget(TargetEntity target) {
        this.target = target;
    }

    public void StartFiring() {
        Invoke("Fire", 0.25f);
        aiming = false;
    }

    public void Fire() {
        if (!alive) return;

        firing = false;

        Vector3 rayOrigin = transform.position;
        RaycastHit hitData;
        var rayDirection = lastKnownTargetPos - transform.position;
        bool hit = Physics.Raycast(rayOrigin, rayDirection, out hitData);

        muzzle.Play();
        SoundManager.main.PlaySound(GameSoundType.Gunshot, transform.position, true);

        if (hit)
        {
            bulletTrail.transform.position = hitData.point;
            bulletTrail.AddPosition(muzzle.transform.position);
            
            if (hitData.collider != null)
            {
                TargetEntity targetEntity = hitData.collider.gameObject.GetComponentInParent<TargetEntity>();
                
                if (targetEntity != null) {
                    var effect = Instantiate(bloodEffect);
                    effect.transform.position = hitData.point;
                } else {
                    var effect = Instantiate(hitEffect);
                    effect.transform.position = hitData.point;
                }

                if (targetEntity == null) return;
                if (targetEntity.TargetType == TargetEntityType.Werewolf)
                {
                    GameObject target = hitData.collider.gameObject;
                    Killable killable = target.GetComponentInParent<Killable>();
                    if (killable != null)
                    {
                        killable.DealDamage(damage, hitData.point, rayDirection, 100.0f);
                    }
                }
            }
        }
        else {
            bulletTrail.transform.position = rayOrigin + rayDirection * 100.0f;
            bulletTrail.AddPosition(muzzle.transform.position);
        }
    }
}
