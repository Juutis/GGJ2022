using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeAim : MonoBehaviour
{
    public Animator anim;

    private Vector3 target;
    private bool aim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAnimatorIK()
    {
        // Set the look target position, if one has been assigned
        if(aim) {

            var targetDir = target - transform.position;
            var aimRotation = Quaternion.LookRotation(targetDir, Vector3.up);

            anim.SetLookAtWeight(1);
            anim.SetLookAtPosition(target);
            
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
            anim.SetIKPosition(AvatarIKGoal.RightHand, target);
            anim.SetIKRotation(AvatarIKGoal.RightHand, aimRotation);
        }    
        
        //if the IK is not active, set the position and rotation of the hand and head back to the original position
        else {          
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
            anim.SetLookAtWeight(0);
        }
    }

    public void SetTarget(Vector3 target) {
        this.target = target;
    }

    public void SetAiming(bool aiming) {
        this.aim = aiming;
    }
}
