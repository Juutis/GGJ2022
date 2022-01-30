using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator anim;

    public MeleeFighter Melee;

    private DudeAim aim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        aim = GetComponent<DudeAim>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMoving(bool moving) {
        anim.SetBool("Walk", moving);
    }

    public void Attack() {
        anim.SetTrigger("Attack");
    }

    public void Hit() {
        if (Melee != null) Melee.DealDamage();
    }

    public void AimAt(Vector3 target) {
        aim.SetTarget(target);
        aim.SetAiming(true);
    }

    public void StopAim() {
        if (aim != null) {
            aim.SetAiming(false);
        }
    }
    
}
