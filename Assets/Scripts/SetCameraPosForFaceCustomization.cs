using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraPosForFaceCustomization : MonoBehaviour
{
    public static SetCameraPosForFaceCustomization instance;
    CinemachineBrain cineMachineBrainScript;
    Camera workingCamera;
    public Camera switchCamera;
    public Transform sidePosition,frontPosition;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        cineMachineBrainScript = this.GetComponent<CinemachineBrain>();
        workingCamera = this.GetComponent<Camera>();
    }

    public void ChangeCameraToIsometric()
    {
        cineMachineBrainScript.enabled = false;
        workingCamera.enabled = false;
        switchCamera.enabled = true;
        ChangePosition(true);
        //this.transform.rotation = Quaternion.identity;
    }

    public void ChangePosition(bool frontSide)
    {
        if (frontSide)
        {
            //switchCamera.gameObject.transform.position = frontPosition.position;
            //switchCamera.gameObject.transform.rotation = frontPosition.rotation;
            switchCamera.gameObject.transform.DOMove( frontPosition.position,.3f);
            switchCamera.gameObject.transform.rotation = frontPosition.rotation;
        } else
        {
            switchCamera.gameObject.transform.DOMove(sidePosition.position, .3f);
            //switchCamera.gameObject.transform.localPosition = sidePosition.localPosition;
            switchCamera.gameObject.transform.rotation = sidePosition.rotation;
            //switchCamera.gameObject.transform.DOLocalRotate( new Vector3( sidePosition.rotation.x, sidePosition.rotation.y, sidePosition.rotation.z),.3f);
        }
    }

    public void ChangeCameraToProspective()
    {
        
        switchCamera.enabled = false;
        workingCamera.enabled = true;
        cineMachineBrainScript.enabled = true;
        ChangePosition(true);
    }
}
