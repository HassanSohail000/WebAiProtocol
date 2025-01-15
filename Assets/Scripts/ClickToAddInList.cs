using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InventoryManager;

public class ClickToAddInList : MonoBehaviour
{
    public int panelIndex = 0;
    public GameObject messageReceiverObject;
    public GameObject selectedLine;

    private void OnEnable()
    {
        InventoryManager.instance.storeOpen += AddToList;
    }
    private void OnDisable()
    {
        InventoryManager.instance.storeOpen -= AddToList;
    }

    void AddToList()
    {
        Invoke("Delay", 0.05f);
    }
    void Delay()
    {
        if (panelIndex == 1)
        {
            StoreUndoRedo.obj.ActionWithParametersAdd(messageReceiverObject, panelIndex, "SelectPanel", StoreUndoRedo.ActionType.ChangeCategory, Color.white, EnumClass.CategoryEnum.Avatar);
            StoreUndoRedo.obj.panelType = StoreUndoRedo.PanelType.Avatar;
        }
    }

    void Start()
    {
        //this.gameObject.GetComponent<Button>().onClick.AddListener(AddIntoList);
    }

    public void AddIntoList()
    {
        if (selectedLine.activeSelf) return;

        if (!StoreUndoRedo.obj.addToList)
            StoreUndoRedo.obj.addToList = true;
        else
        {
            if (panelIndex == 1)
            {
                StoreUndoRedo.obj.ActionWithParametersAdd(messageReceiverObject, panelIndex, "SelectPanel", StoreUndoRedo.ActionType.ChangeCategory, Color.white, EnumClass.CategoryEnum.Avatar);
                StoreUndoRedo.obj.panelType = StoreUndoRedo.PanelType.Avatar;
            }
            else if (panelIndex == 0)
            {
                StoreUndoRedo.obj.ActionWithParametersAdd(messageReceiverObject, panelIndex, "SelectPanel", StoreUndoRedo.ActionType.ChangeCategory, Color.white, EnumClass.CategoryEnum.Wearable);
                StoreUndoRedo.obj.panelType = StoreUndoRedo.PanelType.Wearable;
            }
            //Debug.Log("<color=red> Added Into List: " + this.gameObject.name + "</color>");
        }
    }

}
