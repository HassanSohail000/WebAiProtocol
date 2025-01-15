using UnityEngine;

[System.Serializable]
public class Item
{
    public string Slug;
    public string ItemType;
    public GameObject ItemPrefab;
    public int ItemID;
    public string ItemName;
    public string ItemDescription;
    public string ItemLinkAndroid;
    public string ItemLinkIOS;
    public string SubCategoryname;
    //public string ClothingType; //work, combat, social, naked 
    public bool Stackable;
    public Sprite ItemIcon;
     //constructor for facial hair, hair, weapons, and armor
    public Item(int itemID, string itemName, string itemDescription, string slug, string itemType, GameObject itemPrefab, string _ItemLinkAndroid, string _ItemLinkIOS, string _SubCategoryname)
    {
        this.ItemID = itemID;
        this.ItemName = itemName;
        this.ItemDescription = itemDescription;
        this.Slug = slug;
        this.ItemType = itemType;
        this.ItemPrefab = itemPrefab;
        this.ItemLinkAndroid = _ItemLinkAndroid;
        this.ItemLinkIOS = _ItemLinkIOS;
        this.SubCategoryname = _SubCategoryname;
    }
    //constructor for no equipment
    public Item(int itemID, string itemName, string itemDescription, string slug, string itemType)
    {
        this.ItemID = itemID;
        this.ItemName = itemName;
        this.ItemDescription = itemDescription;
        this.Slug = slug;
        this.ItemType = itemType;
    }

    public Item(int itemId,string itemName,  string itemType)
    {
        this.ItemID = itemId;
        this.ItemName = itemName;
        this.ItemType = itemType;
    }
    public Item(int itemId, GameObject itemPrefab, string itemType)
    {
        this.ItemID = itemId;
        this.ItemPrefab = itemPrefab;
        this.ItemType = itemType;
    }
    //empty constructor  
    public Item()
    {
        this.ItemID = -1;
    }

}