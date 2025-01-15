using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class AvatarDefaultClothes
{
    public Texture2D DefaultSkin, DefaultEyes, DefaultLips;
    public GameObject DefaultPent, DefaultShoes, DefaultShirt, DefaultHair;
}
public class DefaultClothDatabase : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();
    public static DefaultClothDatabase instance;
    SaveCharacterProperties saveCharacterObj;
    private bool RevertBool;

    public Texture2D DefaultSkin, DefaultEyes, DefaultLips;
    public GameObject DefaultPent, DefaultShoes, DefaultShirt, DefaultHair;

    public AvatarDefaultClothes maleAvatarDefaultCostume;
    public AvatarDefaultClothes femaleAvatarDefaultCostume;

    AddressableDownloader downloader;
    //public void itemlistresettodefault()
    //{
    //    Awake();
    //}
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        if (instance != this)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        SetPlayerDefault();
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1) // Account is opened
        {

        }

        downloader = AddressableDownloader.Instance;

        // StartCoroutine(WaitAndDownloadFromRevert(1.0f));

    }

    void SetPlayerDefault()
    {
        //naked
        if (this.gameObject != null)
            saveCharacterObj = this.gameObject.GetComponent<SaveCharacterProperties>();
        itemList.Add(new Item(0, "", "", "naked_legs", "Legs"));
        itemList.Add(new Item(1, "", "", "naked_chest", "Chest"));
        itemList.Add(new Item(2, "", "", "bald_head", "Hair"));
        itemList.Add(new Item(3, "", "", "no_beard", "Beard"));
        itemList.Add(new Item(4, "", "", "no_mustache", "Mustache"));
        itemList.Add(new Item(5, "", "", "empty_hand_r", "HandRight"));
        itemList.Add(new Item(6, "", "", "no_armor", "ChestArmor"));
        itemList.Add(new Item(7, "", "", "naked_slug", "Feet"));
        //Commenred By Talha
        //itemList.Add(new Item(50, "", "", "MDpants", "Legs", (GameObject)Resources.Load("Gear/MDpants")));
        //itemList.Add(new Item(51, "", "", "MDboots", "Feet", (GameObject)Resources.Load("Gear/MDboots")));
        //itemList.Add(new Item(52, "", "", "MDgambeson", "Chest", (GameObject)Resources.Load("Gear/MDgambeson")));
        ////------------------------------------------------------------------------------------------------------------------------------------

        /* Hassan
                itemList.Add(new Item(0, "", "", "naked_legs", "Legs", (GameObject)Resources.Load("Gear/MDpant"), "", "", "Bottom"));
                itemList.Add(new Item(0, "", "", "naked_slug", "Feet", (GameObject)Resources.Load("Gear/MDshoes"), "", "", "Shoes"));
                itemList.Add(new Item(0, "", "", "naked_chest", "Chest", (GameObject)Resources.Load("Gear/MDshirt"), "", "", "Outer"));
                itemList.Add(new Item(0, "", "", "bald_head", "Hair", (GameObject)Resources.Load("Gear/MDhairs"), "", "", "Hair"));
        Hassan*/

        //---------------------------------------------------------------------------------------------------------------------------------------
        ////weapons
        //itemList.Add(new Item(0, "", "", "halberd", "HandRight", (GameObject)Resources.Load("Gear/halberd"), "", "", ""));
        //hair and beard
        //itemList.Add(new Item(200, "", "", "CG01hair", "Hair", (GameObject)Resources.Load("Gear/CG01hair")));
        //itemList.Add(new Item(201, "", "", "CG02hair", "Hair", (GameObject)Resources.Load("Gear/CG02hair")));
        //itemList.Add(new Item(202, "", "", "CG03hair", "Hair", (GameObject)Resources.Load("Gear/CG03hair")));
        //itemList.Add(new Item(203, "", "", "CG04hair", "Hair", (GameObject)Resources.Load("Gear/CG04hair")));
        //itemList.Add(new Item(204, "", "", "CG05hair", "Hair", (GameObject)Resources.Load("Gear/CG05hair")));
        //itemList.Add(new Item(205, "", "", "FDhair", "Hair", (GameObject)Resources.Load("Gear/FDhair")));
        //itemList.Add(new Item(206, "", "", "MDhair", "Hair", (GameObject)Resources.Load("Gear/MDhair")));

        /* Hassan
        itemList.Add(new Item(0, "", "", "beard", "Beard", (GameObject)Resources.Load("Gear/beard"), "", "", ""));
        itemList.Add(new Item(0, "", "", "mustache", "Mustache", (GameObject)Resources.Load("Gear/mustache"), "", "", ""));
        Hassan*/
    }

    //public void AssignSavedCloths()
    //{
    //    print("Assign Saved Cloth FUnction");

    //    // itemList.Add(new Item(0, "", "", "MDpants", "Legs", (GameObject)Resources.Load("Gear/MDpants"), "", "Bottom"));
    //    InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes("naked_legs", "Legs", "MDpants", 0);
    //    // itemList.Add(new Item(0, "", "", "FDboots", "Feet", (GameObject)Resources.Load("Gear/FDboots"), "", "Shoes"));
    //    InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes("naked_slug", "Feet", "FDboots", 7);
    //    // itemList.Add(new Item(0, "", "", "MDgambeson", "Chest", (GameObject)Resources.Load("Gear/MDgambeson"), "", "Outer"));
    //    InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes("naked_chest", "Chest", "MDgambeson", 1);
    //    // itemList.Add(new Item(0, "", "", "FDhair", "Hair", (GameObject)Resources.Load("Gear/FDhair"), "", "Hair"));
    //    InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes("bald_head", "Hair", "FDhair", 2);

    //}
    public void RevertSavedCloths()
    {
        RevertBool = true;
        StartCoroutine(WaitAndDownloadFromRevert(.01f));
    }
    public void DownloadFromOtherWorld()
    {
        StartCoroutine(WaitAndDownloadFromRevert(0f));
    }

    public IEnumerator WaitAndDownloadFromRevert(float delay)
    {
        yield return new WaitForSeconds(delay);
        SaveCharacterProperties.instance.characterController.InitializeAvatar();

        #region Commented Section
        //SaveCharacterProperties.instance.SavePlayerPropertiesInClassObj();
        //        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        //        {
        //            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
        //            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
        //            for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
        //            {

        //                string currentlink = "";
        //#if UNITY_ANDROID
        //                if (!string.IsNullOrEmpty(_CharacterData.myItemObj[i].ItemLinkAndroid))
        //                    currentlink = _CharacterData.myItemObj[i].ItemLinkAndroid;
        //                else
        //                    currentlink = _CharacterData.myItemObj[i].ItemLinkIOS;
        //#else
        //        currentlink = _CharacterData.myItemObj[i].ItemLinkIOS;
        //#endif



        //                if ((_CharacterData.myItemObj[i].ItemID == 0 || _CharacterData.myItemObj[i].ItemID == -1) && RevertBool)
        //                {
        //                    BindDefaultItems(_CharacterData.myItemObj[i]);
        //                }
        //                else
        //                {
        //                    if (!string.IsNullOrEmpty(currentlink))   // if link is empty thn dont call it
        //                    {
        //                        //  Debug.Log("Downloading --- " + _CharacterData.myItemObj[i].ItemLink + " Link " + _CharacterData.myItemObj[i].ItemType);
        //                        string _temptype = _CharacterData.myItemObj[i].Slug;
        //                        ItemDetail itemobj = new ItemDetail();
        //                        itemobj.name = _CharacterData.myItemObj[i].ItemName.ToLower();
        //                        itemobj.id = _CharacterData.myItemObj[i].ItemID.ToString();
        //                        itemobj.assetLinkIos = _CharacterData.myItemObj[i].ItemLinkIOS;
        //                        itemobj.assetLinkAndroid = _CharacterData.myItemObj[i].ItemLinkAndroid;

        //                        //InventoryManager.instance._DownloadRigClothes.NeedToDownloadOrNot(itemobj, _CharacterData.myItemObj[i].ItemLinkAndroid, _CharacterData.myItemObj[i].ItemLinkIOS, _CharacterData.myItemObj[i].ItemType, _CharacterData.myItemObj[i].ItemName.ToLower(), _CharacterData.myItemObj[i].ItemID);
        //                        if (!_CharacterData.myItemObj[i].ItemName.Contains("md", System.StringComparison.CurrentCultureIgnoreCase))
        //                        {
        //                            downloader.StartCoroutine(downloader.DownloadAddressableObj(_CharacterData.myItemObj[i].ItemID,_CharacterData.myItemObj[i].ItemName, _CharacterData.myItemObj[i].ItemType, GameManager.Instance.mainCharacter.GetComponent<AvatarController>()));
        //                        }
        //                        else
        //                        {
        //                            GameManager.Instance.mainCharacter.GetComponent<AvatarController>().WearDefaultItem(_CharacterData.myItemObj[i].ItemType, GameManager.Instance.mainCharacter.gameObject);
        //                        }

        //                    }
        //                    //try
        //                    //{
        //                    //}
        //                    //catch (Exception e)
        //                    //{
        //                    //    Debug.Log(e.ToString());
        //                    //}
        //                }
        //                //  }  
        //            }

        //            yield return new WaitForSeconds(.05f);

        //        }
        //        else if (RevertBool)
        //        {
        //            //InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes("naked_legs", "Legs", "MDpant", 0);
        //            //InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes("naked_slug", "Feet", "MDshoes", 7);
        //            //InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes("naked_chest", "Chest", "MDshirt", 1);
        //            //InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes("bald_head", "Hair", "MDhairs", 2);

        //            // Commented By Talha Now use texture for Body
        //            //GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornChest.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
        //            // GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));
        //            //GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornLegs.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
        //            //GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));


        //            //-------------------------------------
        //            // GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[1].mainTexture = DefaultLips;
        //            //Change Eye Color Here For Default
        //            // GameManager.Instance.EyeballTexture1.material.mainTexture = DefaultEyes;
        //            // GameManager.Instance.EyeballTexture2.material.mainTexture = DefaultEyes;
        //            // GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[0].mainTexture = DefaultSkin;

        //            // Commented By Talha Now use texture for Body
        //            //for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
        //            //{
        //            //    if (GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
        //            //        GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.mainTexture = DefaultSkin;
        //            //}
        //        }
        // Commented By WaqasAhmad
        {
            //if (Screen.orientation == ScreenOrientation.LandscapeLeft && chkOrientation == false)

            //{
            //    chkOrientation = true;
            //    try
            //    {
            //        LoadingHandler.Instance.Loading_WhiteScreen.transform.GetChild(0).gameObject.SetActive(true);
            //    }
            //    catch (Exception e)
            //    {
            //        Debug.Log("Exception here............................");
            //    }

            //    LoadingHandler.Instance.Loading_WhiteScreen.SetActive(true);

            //    Invoke("loadingscreenOff", 0.7f);
            //}
        }

        //if (!ConstantsHolder.xanaConstants.JjWorldSceneChange && !ConstantsHolder.xanaConstants.orientationchanged)
        //{
        //    Debug.LogError("~~~~~ Waqas_ ItemDataBase ~~~~~~~~~~~");
        //    //Screen.orientation = ScreenOrientation.Portrait;
        //}
        //  }

        #endregion

        //if (LoadingHandler.Instance)
        //    LoadingHandler.Instance.HideLoading();

        yield return null;
    }
    bool chkOrientation;
    void loadingscreenOff()
    {
        // Commented By WaqasAhmad
        {
            //LoadingHandler.Instance.Loading_WhiteScreen.SetActive(false);
            //chkOrientation = false;
        }
    }
    //bool CompareWithEquipedItems(Item SavedItemObj)
    //{

    //    switch (SavedItemObj.ItemType)
    //    {
    //        // Commented By WaqasAhmad [All Cases] // Dont using eyelense from Equioment Script
    //        //case "Lip":
    //        //    if (GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.SubCategoryname == SavedItemObj.SubCategoryname)
    //        //    {
    //        //        if (GameManager.Instance.mainCharacter.GetComponent<Equipment>().LipsTexture.ItemID == SavedItemObj.ItemID)
    //        //        {
    //        //            return true;
    //        //        }
    //        //        else
    //        //        {
    //        //            return false;
    //        //        }
    //        //    }
    //        //    return false;

    //        //case "Eyes":
    //        //    if (GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.SubCategoryname == SavedItemObj.SubCategoryname)
    //        //    {
    //        //        if (GameManager.Instance.mainCharacter.GetComponent<Equipment>().EyesTexture.ItemID == SavedItemObj.ItemID)
    //        //        {
    //        //            return true;
    //        //        }
    //        //        else
    //        //        {
    //        //            return false;
    //        //        }
    //        //    }
    //        //    return false;

    //        //case "Skin":
    //        //    if (GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.SubCategoryname == SavedItemObj.SubCategoryname)
    //        //    {
    //        //        if (GameManager.Instance.mainCharacter.GetComponent<Equipment>().SkinToneTexture.ItemID == SavedItemObj.ItemID)
    //        //        {
    //        //            return true;
    //        //        }
    //        //        else
    //        //        {
    //        //            return false;
    //        //        }
    //        //    }
    //        //    return false;

    //        default:
    //            List<Item> EquipList = new List<Item>();
    //            EquipList = GameManager.Instance.mainCharacter.GetComponent<Equipment>().equippedItems;
    //            for (int i = 0; i < EquipList.Count; i++)
    //            {
    //                if (EquipList[i].SubCategoryname == SavedItemObj.SubCategoryname)
    //                {
    //                    if (EquipList[i].ItemID == SavedItemObj.ItemID)
    //                    {
    //                        return true;
    //                    }
    //                    else
    //                    {
    //                        return false;
    //                    }
    //                }
    //            }
    //            return false;
    //    }
    //}

    //    IEnumerator WaitAndDownload()
    //    {
    //        yield return new WaitForEndOfFrame();
    //        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
    //        {
    //            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
    //            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
    //            for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
    //            {
    //                string currentlink = "";
    //#if UNITY_ANDROID
    //                currentlink = _CharacterData.myItemObj[i].ItemLinkAndroid;
    //#else
    //currentlink = _CharacterData.myItemObj[i].ItemLinkIOS;
    //#endif


    //                if (_CharacterData.myItemObj[i].ItemID != 0)
    //                {
    //                    yield return new WaitForSeconds(.05f);

    //                    ItemDetail itemobj = new ItemDetail();
    //                    itemobj.name = _CharacterData.myItemObj[i].ItemName;
    //                    itemobj.id = _CharacterData.myItemObj[i].ItemID.ToString();
    //                    itemobj.assetLinkIos = _CharacterData.myItemObj[i].ItemLinkIOS;
    //                    itemobj.assetLinkAndroid = _CharacterData.myItemObj[i].ItemLinkAndroid;
    //                    InventoryManager.instance._DownloadRigClothes.NeedToDownloadOrNot(itemobj, _CharacterData.myItemObj[i].ItemLinkAndroid, _CharacterData.myItemObj[i].ItemLinkIOS, _CharacterData.myItemObj[i].SubCategoryname, _CharacterData.myItemObj[i].ItemName.ToLower(), _CharacterData.myItemObj[i].ItemID);

    //                }
    //            }
    //        }
    //        yield return null;
    //    }
    public void BindDefaultItems(Item _getItem)
    {

        switch (_getItem.SubCategoryname)
        {
            case "Bottom":
                {
                    itemList.Add(new Item(0, "", "", "MDpant", "Legs", (GameObject)Resources.Load("Gear/MDpant"), "", "", "Bottom"));

                    // Commented By Talha Now use texture for Body
                    //InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes((GameObject)Resources.Load("Gear/MDpant"), "naked_legs", "Legs", _getItem.Slug, 0);
                    //GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornLegs.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
                    //  GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));
                    break;
                }
            case "Outer":
                {
                    itemList.Add(new Item(0, "", "", "MDshirt", "Chest", (GameObject)Resources.Load("Gear/MDshirt"), "", "", "Outer"));
                   // InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes((GameObject)Resources.Load("Gear/MDshirt"), "naked_chest", "Chest", _getItem.Slug, 1);


                    // Commented By Talha Now use texture for Body
                    //GameManager.Instance.mainCharacter.GetComponent<Equipment>().wornChest.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,
                    //                               GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[2].GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0));
                    break;
                }

            case "Shoes":
                {
                    itemList.Add(new Item(0, "", "", "MDshoes", "Feet", (GameObject)Resources.Load("Gear/MDshoes"), "", "", "Shoes"));
                    //InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes((GameObject)Resources.Load("Gear/MDshoes"), "naked_slug", "Feet", _getItem.Slug, 7);

                    break;
                }
            case "Hair":
                {
                    itemList.Add(new Item(0, "", "", "MDhairs", "Hair", (GameObject)Resources.Load("Gear/MDhairs"), "", "", "Hair"));
                    //InventoryManager.instance._DownloadRigClothes.ui.AddOrRemoveClothes((GameObject)Resources.Load("Gear/MDhairs"), "bald_head", "Hair", _getItem.Slug, 2);
                    break;
                }

            case "Lip":
                {
                    //  GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[1].mainTexture = DefaultLips;
                    break;
                }

            case "Eyes":
                {
                    //Change Eye Color Here For Database
                    //GameManager.Instance.EyeballTexture1.material.mainTexture = DefaultEyes;
                    //GameManager.Instance.EyeballTexture2.material.mainTexture = DefaultEyes;
                    break;
                }

            case "Skin":
                {
                    GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[2].mainTexture = DefaultSkin;

                    // Commented By Talha Now use texture for Body
                    //for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
                    //{
                    //    if (GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
                    //        GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.mainTexture = DefaultSkin;
                    //}
                    break;
                }
        }
    }

    public Item FetchItemByID(int id)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].ItemID == id)
            {
                return itemList[i];
            }
        }
        return null;
    }
    public Item FetchItemBySlug(string slugName)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].Slug == slugName)
            {
                return itemList[i];
            }
        }
        return null;
    }

//    public IEnumerator WaitAndDownloadFromRevertLOCAL(string json)
//    {
//        yield return new WaitForSeconds(0.05f);


//        //if (File.Exists(Application.persistentDataPath + "/User_" + userID)) 
//        //{


//        SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
//        _CharacterData = _CharacterData.CreateFromJSON(json);

//        for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
//        {
//            string currentlink = "";
//#if UNITY_ANDROID
//            currentlink = _CharacterData.myItemObj[i].ItemLinkAndroid;
//#else
//currentlink = _CharacterData.myItemObj[i].ItemLinkIOS;
//#endif

//            if (!CompareWithEquipedItems(_CharacterData.myItemObj[i]))
//            {
//                if (_CharacterData.myItemObj[i].ItemID == 0 && RevertBool)
//                {
//                    BindDefaultItems(_CharacterData.myItemObj[i]);
//                }
//                else
//                {
//                    string _temptype = _CharacterData.myItemObj[i].Slug;

//                    ItemDetail itemobj = new ItemDetail();
//                    itemobj.name = _CharacterData.myItemObj[i].ItemName.ToLower();
//                    itemobj.id = _CharacterData.myItemObj[i].ItemID.ToString();
//                    itemobj.assetLinkIos = _CharacterData.myItemObj[i].ItemLinkIOS;
//                    itemobj.assetLinkAndroid = _CharacterData.myItemObj[i].ItemLinkAndroid;
//                    // InventoryManager.instance._DownloadRigClothes.NeedToDownloadOrNot(itemobj, _CharacterData.myItemObj[i].ItemLinkAndroid, _CharacterData.myItemObj[i].ItemLinkIOS, _CharacterData.myItemObj[i].SubCategoryname, _CharacterData.myItemObj[i].ItemName.ToLower(), _CharacterData.myItemObj[i].ItemID);
//                    GameObject item = null;
//                    downloader.StartCoroutine(downloader.DownloadAddressableObj(_CharacterData.eyeTextureName, (GameObject callback) => {
//                        item = callback;
//                    }));
//                    if (item != null)
//                    {
//                        StichItem(item, _CharacterData.myItemObj[i].ItemType, GameManager.);
//                    }
//                    else
//                    {
//                        WearDefaultItem(type, this.gameObject);
//                    }
//                }
//            }
//            yield return new WaitForSeconds(.05f);
//        }
//    }
}