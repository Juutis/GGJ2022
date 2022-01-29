using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator anim;

    public MeleeFighter Melee;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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

}
