using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfhandsAnimation : MonoBehaviour
{
    public bool TriggerQuickAttack = false;
    public bool TriggerPowerAttack = false;

    private Animator anim;

    private string animAttackProp = "Attack";
    private int quickAttacks = 2;
    private int powerAttacks = 1;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TriggerQuickAttack) {
            TriggerQuickAttack = false;
            QuickAttack();
        }
        if (TriggerPowerAttack) {
            TriggerPowerAttack = false;
            PowerAttack();
        }
    }

    public void PowerAttack() {
        triggerAttack(true);
    }

    public void QuickAttack() {
        triggerAttack(false);
    }

    private void triggerAttack(bool isPowerAttack) {
        if (isAttacking) return;

        var attackIndex = 0;
        if (isPowerAttack) {
            attackIndex = 10 + Random.Range(0, powerAttacks);
        } else {
            attackIndex = 1 + Random.Range(0, quickAttacks);
        }
        anim.SetInteger(animAttackProp, attackIndex);
        isAttacking = true;
        Invoke("ResetAttacking", 0.5f);
    }

    public void ResetAttacking() {
        isAttacking = false;
        anim.SetInteger(animAttackProp, 0);
    }

    public bool ReadyToAttack() {
        return !isAttacking;
    }
}
