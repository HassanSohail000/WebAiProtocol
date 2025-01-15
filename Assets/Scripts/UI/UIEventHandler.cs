using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject m_Camera;

    public void OnPointerDown(PointerEventData eventData)
    {
        //PlayerCameraController.instance.m_PressCounter++;
      //  m_Camera.GetComponent<PlayerCameraController>().m_PressCounter++;
        //print("inside");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //PlayerCameraController.instance.m_PressCounter--;
       // m_Camera.GetComponent<PlayerCameraController>().m_PressCounter--;
    }
}
