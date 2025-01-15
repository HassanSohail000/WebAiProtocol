using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelEnumUpdate : MonoBehaviour
{
    public enum LocalPanelType { AvatarPanel, WearablePanel};
    public LocalPanelType localPanelType;

    private void OnEnable()
    {
        if (localPanelType.Equals(LocalPanelType.AvatarPanel))
            StoreUndoRedo.obj.panelType = StoreUndoRedo.PanelType.Avatar;
        else if (localPanelType.Equals(LocalPanelType.WearablePanel))
            StoreUndoRedo.obj.panelType = StoreUndoRedo.PanelType.Wearable;
    }

    void Start()
    {
        
    }


}
