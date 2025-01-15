using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Purchasing;

public class BuyPanel : MonoBehaviour
{
    public Text BuyDetailsTxt;
    public GameObject CryptoIcon;
    public GameObject CreditCardIcon;
    public GameObject BuyBtnObj;
    public string ProductAmountBuyPanel;
    public string ProductPriceBuyPanel;
    public string InAppKeyBuyPanel;
    public Text MoneyText;
    public Text PaymentTypeText;
    public GameObject ChangepaymentMethodPanel;
    // Start is called before the first frame update
    void Start()
    {
          
    }  
    private void OnEnable()
    {
        //  StartCoroutine(waitandActive());
        //  BuyBtnObj.GetComponent<IAPButton>().enabled = true;
    }
    public void AssignData()
    {   
        print("Key is " + InAppKeyBuyPanel);
     //   BuyBtnObj.gameObject.GetComponent<IAPButton>().productId = InAppKeyBuyPanel;
        BuyDetailsTxt.text = "Buy " + ProductAmountBuyPanel + " Coins On " + ProductPriceBuyPanel;
        MoneyText.text = ProductPriceBuyPanel;
        if (PlayerPrefs.GetString("PaymentMethod") == "CreditCard")
        {
            CryptoIcon.SetActive(false);
            CreditCardIcon.SetActive(true);
            PaymentTypeText.text = "Credit Card";
        }
        else   
        {
            CryptoIcon.SetActive(true);
            CreditCardIcon.SetActive(false);
            PaymentTypeText.text = "Crypto";
             
        }  
    }
    public void OpenChangePaymentMethod()
    {
        ChangepaymentMethodPanel.SetActive(true);
    }
    void CloseChangePaymentMethod()
    {
        ChangepaymentMethodPanel.SetActive(false);
    }
    public void ChangePaymentMethodFtn(string TakeMethod)
    {
        PlayerPrefs.SetString("PaymentMethod", TakeMethod);
        CloseChangePaymentMethod();
        PaymentMethodChanged();
    }
    void PaymentMethodChanged()
    {
        if (PlayerPrefs.GetString("PaymentMethod") == "CreditCard")
        {
            CryptoIcon.SetActive(false);
            CreditCardIcon.SetActive(true);
            PaymentTypeText.text = "Credit Card";
        }
        else
        {
            CryptoIcon.SetActive(true);
            CreditCardIcon.SetActive(false);
            PaymentTypeText.text = "Crypto";
        }
    }    

    IEnumerator waitandActive()
    {
        yield return new WaitForSeconds(1);
    }
    /*
    public void OnPurchaseComplete(Product _product)
    {
        CreditShopManager.instance.OnPurchaseComplete(_product);
        StartCoroutine(waitandActive());
        //  CloseBtnBuyPanel();
    }
    public void OnPurchaseFailure(Product _product, PurchaseFailureReason reason)
    {
        CreditShopManager.instance.OnPurchaseFailure(_product, reason);
        // CloseBtnBuyPanel();
    }
    */
    public void CloseBtnBuyPanel()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
