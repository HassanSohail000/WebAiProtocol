using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEngine.Purchasing;
using UnityEngine.UI;
public class CreditShopManager : MonoBehaviour
{
    public static CreditShopManager instance;
    [Header("Zem/Coin Bundles")]
    public Bundle[] m_Bundles;
    [Header("Payment Method Panel")]
    public GameObject m_PaymentMethodPanel;
    public Text TotalCoins;
    [Header("Store Panel")]
    public GameObject m_StoreContainer;
    public GameObject m_ItemlistPrefab;
    [HideInInspector]
    public List<ProductDetails> m_ItemList;
    public GameObject BuyPanelObj;  
     //** Selected Bundle Index
    [SerializeField]
    int m_SelectedBundleIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //void Start()
    //{
    //    // PlayerPrefs.DeleteAll();
    //    m_ItemList.Clear();
    //    for (int i = 0; i < m_Bundles.Length; i++)
    //    {
    //        GameObject L_ItemBtnObj = Instantiate(m_ItemlistPrefab, m_StoreContainer.transform);
    //        L_ItemBtnObj.GetComponent<ProductDetails>().ProductAmount.text = m_Bundles[i].m_Amount.ToString();
    //        L_ItemBtnObj.GetComponent<ProductDetails>().ProductPrice.text = m_Bundles[i].m_Price.ToString() + "$";
    //        L_ItemBtnObj.GetComponent<ProductDetails>().InAppKey = m_Bundles[i].InAppKey.ToString();
    //   //   L_ItemBtnObj.gameObject.GetComponent<IAPButton>().enabled = false;
    //        m_ItemList.Add(L_ItemBtnObj.GetComponent<ProductDetails>());
    //    }   
    //    if (PlayerPrefs.GetString("PaymentMethod") == "CreditCard")   
    //    {
    //        EnableAllInAppBtns();
    //    }
    //    UpdateCoinsAmount();
    //}

    void EnableAllInAppBtns()
    {
        for (int i = 0; i < m_ItemList.Count; i++)
        {
        //    m_ItemList[i].gameObject.GetComponent<IAPButton>().enabled = true;
          //  m_ItemList[i].gameObject.GetComponent<IAPButton>().productId = m_ItemList[i].InAppKey;
        }
    }
    public void UpdateCoinsAmount()
    {
        string totalCoins = PlayerPrefs.GetInt("TotalCoins").ToString();
        double coins = Double.Parse(totalCoins);
        totalCoins = String.Format("{0:n0}", coins);
        TotalCoins.text = totalCoins;

    }

    // Assign PlayerPrefs = CreditCard or Crypto
    public void ItemBtnClicked(ProductDetails p_details)
    {
        if (!PlayerPrefs.HasKey("PaymentMethod"))
        {
            m_PaymentMethodPanel.SetActive(true);
        }
        else
        {
            if (PlayerPrefs.GetString("PaymentMethod") == "Crypto")
            {
                print("This is Crypto Method");
                print("price  " + p_details.ProductPrice.text);
                print("Amount  " + p_details.ProductAmount.text);
            }
        }
    }
    public void CloseBtnPaymentMethod()
    {
        m_PaymentMethodPanel.SetActive(false);
    }
    public void SelectPaymentMethod(String paymentText)
    {
        PlayerPrefs.SetString("PaymentMethod", paymentText);
        StartCoroutine(WaitandClose());
        if (PlayerPrefs.GetString("PaymentMethod") == "CreditCard")
        {
            //  EnableAllInAppBtns();
        }
        CloseBtnPaymentMethod();
    }
    IEnumerator WaitandClose()
    {
        yield return new WaitForSeconds(.2f);
        BuyPanelObj.GetComponent<BuyPanel>().CloseBtnBuyPanel();
    }  
    /*
    public void OnPurchaseComplete(Product _product)
    {
        print("Product ID is " + _product.definition.id);
        print("Product transection id is " + _product.transactionID);
        print("Product Receipt is " + _product.receipt);
        for (int i = 0; i < m_ItemList.Count; i++)
        {
            if (m_ItemList[i].InAppKey == _product.definition.id)
            {
 
                InventoryManager.instance.SubmitSendCoinstoServer(int.Parse(m_ItemList[i].ProductAmount.text));   
                // PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins") + int.Parse(m_ItemList[i].ProductAmount.text));
                // UpdateCoinsAmount();
            }
        }    
        StartCoroutine(WaitandClose());
    }
    public void OnPurchaseFailure(Product _product, PurchaseFailureReason reason)
    {
        print(reason);
    }   
    */
}

[Serializable]
public class Bundle
{
    // public enum BundleCategory {Zem,Coin}
    //  public BundleCategory m_BundleCategory;
    public int m_Amount;
    public int m_Price;
    public string InAppKey;

}

