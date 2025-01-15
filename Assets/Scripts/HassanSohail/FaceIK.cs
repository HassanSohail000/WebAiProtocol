using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]

public class FaceIK : MonoBehaviour {

    protected Animator animator;

    public bool ikActive = false;
    public Transform lookObj = null;
    
    [SerializeField] List<LookAngle> lookAngles;

    void Start ()
    {
        animator = GetComponent<Animator>();
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if(animator) {

            //if the IK is active, set the position and rotation directly to the goal.
            if(ikActive) {

                // Set the look target position, if one has been assigned
                if(lookObj != null) {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }    
            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else {          
                animator.SetLookAtWeight(0);
            }
        }
    }    

    public void SetLookPos(int id){ 
        lookObj = lookAngles[id].obj.transform;
        lookObj.position= lookAngles[id].obj.transform.position;
    }
}

[Serializable]
class LookAngle{ 
    public string name;    
    public GameObject obj;
}