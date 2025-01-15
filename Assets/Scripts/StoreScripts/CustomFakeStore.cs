using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Purchasing;
using UnityEngine.EventSystems;

public class CustomFakeStore : MonoBehaviour
{  
    [Header("Store Information")]
    public Text productInformation;
    public Text productReward;  

    [Header("Store Buttons")]
   //  public IAPButton buyBtn;
    public Button cancelBtn;      
   /*
    public void UpdateFakeStore()
    {
        productInformation.text = buyBtn.productId;
        for (int i = 0; i < CreditShopManager.instance.m_ItemList.Count; i++)
        {
            if (CreditShopManager.instance.m_ItemList[i].InAppKey == buyBtn.productId)
            {
                productReward.text = CreditShopManager.instance.m_ItemList[i].ProductAmount.text.ToString();
            }
        }
    }
      
    public void PurchaseProduct()
    {
       

        for (int i = 0; i < CreditShopManager.instance.m_ItemList.Count; i++)
        {
            if (CreditShopManager.instance.m_ItemList[i].InAppKey == buyBtn.productId)
            {
                CreditShopManager.instance.BuyPanelObj.SetActive(false);
                 InventoryManager.instance.SubmitSendCoinstoServer(int.Parse(CreditShopManager.instance.m_ItemList[i].ProductAmount.text));
                // PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins") + int.Parse(m_ItemList[i].ProductAmount.text));
                // UpdateCoinsAmount();
            }
        }    
    }
  */
}
