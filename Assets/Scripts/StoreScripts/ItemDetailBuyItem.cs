using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InventoryManager;

public class ItemDetailBuyItem : MonoBehaviour
{
     public Text PriceTxt;
    public Text CategoryTxt;
    public Toggle btnToggle;

  
    public string assetLinkAndroid;
    public string assetLinkIos;
    public string assetLinkWindows;
    public string createdAt;
    public string createdBy;
    public string iconLink;
    public string id;
    public string isFavourite;
    public string isOccupied;
    public string isPaid;
    public string isPurchased;
    public string name;
    public string price;
    public string categoryId;
    public string subCategory;
    public string updatedAt;
    public string[] itemTags;
    public Image SelectImg;
     public int MyIndex;
    public bool SelectedBool;
    public EnumClass.CategoryEnum CategoriesEnumVar;

     // Start is called before the first frame update
    //void Start()
    //{
    //    this.gameObject.GetComponent<Button>().onClick.AddListener(ItemBtnClicked);
    //    print("in start toggle value is " +btnToggle.isOn);
    //}
     // Update is called once per frame
   
    //void ItemBtnClicked()
    //{
    //    print("Item Btn Clicked");
    //}
   //public void OnValueChanged(Toggle change)
   // {
   //   print(  btnToggle.isOn);
   //     if(btnToggle.isOn)
   //     {
   //         InventoryManager.instance.AddItemsFromCheckOut(this.gameObject);
   //     }  
   //     else
   //     {
   //         InventoryManager.instance.RemoveItemsFromCheckOut(this.gameObject);
   //      }
   //  }
}

 