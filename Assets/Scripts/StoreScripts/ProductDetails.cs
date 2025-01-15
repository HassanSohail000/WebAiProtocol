using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Purchasing;

public class ProductDetails : MonoBehaviour
{
    public Text ProductAmount;
    public Text ProductPrice;
    public string InAppKey;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(ItemBtnClickedSelf);
    }

    public void ItemBtnClickedSelf()
    {
        CreditShopManager.instance.ItemBtnClicked(this);    
        if (PlayerPrefs.HasKey("PaymentMethod"))
        {
            CreditShopManager.instance.BuyPanelObj.SetActive(true);
            CreditShopManager.instance.BuyPanelObj.GetComponent<BuyPanel>().ProductAmountBuyPanel = ProductAmount.text;
            CreditShopManager.instance.BuyPanelObj.GetComponent<BuyPanel>().ProductPriceBuyPanel = ProductPrice.text;
            CreditShopManager.instance.BuyPanelObj.GetComponent<BuyPanel>().InAppKeyBuyPanel = InAppKey;
            CreditShopManager.instance.BuyPanelObj.GetComponent<BuyPanel>().AssignData();
            print(ProductAmount.text);
            print(ProductPrice.text);
            print(InAppKey);
        }

    }
    /*
    public void OnPurchaseComplete(Product _product)
    {
        CreditShopManager.instance.OnPurchaseComplete(_product);
     }
    public void OnPurchaseFailure(Product _product, PurchaseFailureReason reason)
    {
        CreditShopManager.instance.OnPurchaseFailure(_product, reason);
    }
    */
    // Update is called once per frame
    void Update()
    {

    }
}
