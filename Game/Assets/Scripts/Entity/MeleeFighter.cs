using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeleeFighter : MonoBehaviour
{
    [SerializeField]
    private float damage = 1;

    [SerializeField]
    private float range = 1;

    [SerializeField]
    private float damageRadius = 1;

    [SerializeField]
    private Transform damagePosition;

    private CharacterAnimator charAnim;

    private TargetEntity target;

    // Start is called before the first frame update
    void Start()
    {
        charAnim = GetComponentInChildren<CharacterAnimator>();
        charAnim.Melee = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) {
            if (Vector3.Distance(damagePosition.position, target.transform.position) < range) {
                   
                SoundManager.main.PlaySound(GameSoundType.Growl, transform.position);
        
                charAnim.Attack();
            }
        }
    }

    public void SetTarget(TargetEntity target) {
        this.target = target;
    }

    public void DealDamage() {
        var killables = TargetEntityManager.main.Targets
            .FindAll(target => target.TargetType == TargetEntityType.Human && IsWithinDamageRange(target))
            .Select(it => it.GetComponent<Killable>())
            .ToList();

        foreach (var killable in killables) {
            var dir = killable.transform.position - transform.position;
            killable.DealDamage(damage, damagePosition.position, dir, 150f);
        }
    }

    private bool IsWithinDamageRange(TargetEntity candidate) {
        var distance = Vector3.Distance(damagePosition.position, candidate.transform.position);
        return distance < damageRadius;
    }
}
