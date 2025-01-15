using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreStackHandler : MonoBehaviour
{
    public static StoreStackHandler obj;

    private void Awake()
    {
        if (obj == null)
            obj = this;
        else if (obj != null)
            Destroy(this);
    }

    public enum ActiveAvatarPanel
    {
        HairPanel, FacePanel, EyeBrowPanel, EyePanel, NosePanel, LipsPanel, BodyPanel, SkinPanel,
        EyeLashesPanel, MakeupPanel, AccessaryPanel
    };
    public ActiveAvatarPanel activeAvatarPanel;

    public enum ActiveColorPanel
    {
        HairColorPanel, EyeBrowColorPanel, EyeColorPanel, LipsColorPanel
    };
    public ActiveColorPanel activeColorPanel;

    public enum CallBy { Btn, Other };
    [Space(5)]
    public CallBy callby;


    // Start is called before the first frame update
    void Start()
    {

    }


    public void UpdatePanelStatus(int num, bool callByBtn)
    {
        //Debug.Log("Update panel Number: " + num);
        activeAvatarPanel = (ActiveAvatarPanel)(int)num;
        callby = (CallBy)(int)(callByBtn ? 0 : 1);
    }

    public void ResetValue()
    {
        //Debug.Log("Reset Click");
        callby = (CallBy)(int)1;
    }
    
    public bool IsCallByBtn()
    {
        if (callby.Equals(CallBy.Btn))
            return true;
        else
            return false;
    }

    public void UpdateColorPanelStatus(int num, bool callByBtn)
    {
        //Debug.Log("Update Color panel Number: " + num);
        activeColorPanel = (ActiveColorPanel)(int)num;
        callby = (CallBy)(int)(callByBtn ? 0 : 1);
    }

    //public void EnableCallByBtn()
    //{
    //    callby = (CallBy)(int)0;
    //}

}
