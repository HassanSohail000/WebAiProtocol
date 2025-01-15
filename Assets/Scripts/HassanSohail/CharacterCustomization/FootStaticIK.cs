using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStaticIK : MonoBehaviour
{
    protected Animator animator;
    public float weight;
    public bool ikActive = false;
    public Transform rightFeetObj = null;
    public Transform leftFeetObj = null;
    //public Transform lookObj = null;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (animator)
        {

            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                //if (lookObj != null)
                //{
                //    animator.SetLookAtWeight(1);
                //    animator.SetLookAtPosition(lookObj.position);
                //}

                // Set the right hand target position and rotation, if one has been assigned
                if (rightFeetObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weight);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weight);
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFeetObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFeetObj.rotation);
                }
                // Set the right hand target position and rotation, if one has been assigned
                if (leftFeetObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weight);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weight);
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFeetObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFeetObj.rotation);
                }
            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
}
