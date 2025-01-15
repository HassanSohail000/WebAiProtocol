using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIObjectSwitcher : MonoBehaviour
{
    public Vector2 landscapeImgsize;
    public RectTransform landscapeImg;
    public Vector2 portraitImgsize;

    public GameObject[] landscapeUIObjs;
    public GameObject[] portraitUIObjs;


    private void OnEnable()
    {
        ScreenOrientationManager.switchOrientation += OrientationChanged;
    }
    private void OnDisable()
    {
        ScreenOrientationManager.switchOrientation -= OrientationChanged;
    }


    void OrientationChanged(bool IsPortrait)
    {
        if (IsPortrait)
        {
            foreach (GameObject landscapeAction in landscapeUIObjs)
                landscapeAction.SetActive(false);
            foreach (GameObject portraitAction in portraitUIObjs)
                portraitAction.SetActive(true);

            if (landscapeImg)
                landscapeImg.sizeDelta = portraitImgsize;
               // portraitImg.overrideSprite = landscapeImg.sprite;
        }
        else
        {
            foreach (GameObject landscapeAction in landscapeUIObjs)
                landscapeAction.SetActive(true);
            foreach (GameObject portraitAction in portraitUIObjs)
                portraitAction.SetActive(false);

            if (landscapeImg)
                landscapeImg.sizeDelta = landscapeImgsize;
        }
    }
}