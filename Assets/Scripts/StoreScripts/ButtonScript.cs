//using Crosstales;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InventoryManager;

public class ButtonScript : MonoBehaviour
{
    public EnumClass.CategoryEnum categoryEnum;

    public int Index;
    public Text BtnTxt;

    // Start is called before the first frame update
    InventoryManager inventoryManager;
    void Start()
    {
        inventoryManager = InventoryManager.instance;

        this.gameObject.GetComponent<Button>().onClick.AddListener(BtnClicked);
        this.gameObject.GetComponent<Button>().onClick.AddListener(ButtonPressed);
        //InventoryManager.instance.UpdateXanaConstants();
        //inventoryManager.UpdateXanaConstants();
    }

    public void BtnClicked()
    {
        //// AR changes start
        if (BtnTxt.color != GetComponentInParent<SubBottons>().HighlightedColor)
        {
            if (!StoreUndoRedo.obj.addToList)
                StoreUndoRedo.obj.addToList = true;
            else
            {
                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "BtnClicked", StoreUndoRedo.ActionType.ChangePanel, Color.white, categoryEnum);
            }
        }
        //// AR changes end


        if (ConstantsHolder.xanaConstants.currentButtonIndex == Index)
        {
            //    if (Index == 0)                                                  // AR Changes
            //        InventoryManager.instance.ForcellySetLastClickedBtnOfHair();     // AR Changes

            if (inventoryManager.CloseColorPanel(Index) == true)
            {
                //if (inventoryManager.ParentOfBtnsAvatarEyeBrows.transform.childCount != 0)
                if (inventoryManager.AllCategoriesData[10].parentObj.transform.childCount != 0) // AvatarEyeBrow
                {
                    for (int i = 0; i < inventoryManager.AllCategoriesData[10].parentObj.transform.childCount; i++)
                    {
                        inventoryManager.AllCategoriesData[10].parentObj.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                if (inventoryManager.AllCategoriesData[8].parentObj.transform.childCount != 0) // AvatarHairs
                {
                    for (int i = 0; i < inventoryManager.AllCategoriesData[8].parentObj.transform.childCount; i++)
                    {
                        inventoryManager.AllCategoriesData[8].parentObj.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                //InventoryManager.instance.SubmitAllItemswithSpecificSubCategory(InventoryManager.instance.SubCategoriesList[Index + 8].id, true);      // AR changes
                
            }
            inventoryManager.UpdateStoreSelection(Index);
            // If click on the same panel Do Nothing & return
            return;
        }
        // Items which are not downloaded stop them to download
        // because new category is opened
        inventoryManager.StopAllCoroutines();
        //inventoryManager.eyeBrowTapButton.SetActive(false);

        // InventoryManager.instance.DeletePreviousItems();            // AR changes
        
        if (gameObject.transform.parent.name != "Accesary")
        {
            if (PlayerPrefs.GetInt("presetPanel") == 1)
            {
                PlayerPrefs.SetInt("presetPanel", 0);  // was loggedin as account 
            }
        }


        ConstantsHolder.xanaConstants.currentButtonIndex = Index;
        inventoryManager.UpdateXanaConstants();
        inventoryManager.DisableColorPanels();
        //ResetSelectedItems();


        if (Index == 7 && inventoryManager.panelIndex == 1)
            inventoryManager.OnColorButtonClicked(ConstantsHolder.xanaConstants.currentButtonIndex);
        else
            inventoryManager.UpdateStoreSelection(Index);

        //if (LoadingHandler.Instance)
        //    LoadingHandler.Instance.storeLoadingScreen.SetActive(false);
        //if (this.gameObject.activeInHierarchy)
        GetComponentInParent<SubBottons>().ClickBtnFtn(Index);


    }


    void ResetSelectedItems()
    {
        // Get Reference of all Clicked Items
        int count = StoreUndoRedo.obj.data.Count;
        for (int i = 0; i < count; i++)
        {
            var data = StoreUndoRedo.obj.data[i];
            if (data.actionObject)
            {
                var avatarBtn = data.actionObject.GetComponent<AvatarBtn>();
                var presetBtn = data.actionObject.GetComponent<PresetData_Jsons>();
                var image = data.actionObject.GetComponent<Image>();

                if (presetBtn != null)
                {
                    presetBtn.transform.GetChild(0).GetComponent<Image>().enabled = false;
                }
                else if (avatarBtn != null && image != null)
                {
                    image.color = new Color(1, 1, 1, 0);
                }
                else if (!data.methodName.Equals("BtnClicked") && image != null)
                {
                    image.enabled = false;
                }
            }
           
        }
    }
    private void ButtonPressed()
    {
        StoreStackHandler.obj.UpdatePanelStatus(Index, true);    // AR changes
    }

}
