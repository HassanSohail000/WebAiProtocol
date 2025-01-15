using SuperStar.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static InventoryManager;
using System;
using static StoreUndoRedo;
using System.Globalization;

public class ItemDetail : MonoBehaviour
{
    public Color NormalColor;
    public Color HighlightedColor;
    public string id;
    public string assetLinkAndroid;
    public string assetLinkIos;
    public string assetLinkWindows;
    public string iconLink;
    public string categoryId;
    public string subCategory;
    public string name;
    public string isPaid;
    public string price;
    public string isPurchased;
    public string isFavourite;
    public string isOccupied;
    public bool isDeleted;
    public string createdBy;
    public string createdAt;
    public string updatedAt;
    public string[] itemTags;
    public Image SelectImg;
    public Text PriceTxt;
    public int MyIndex;
    public bool SelectedBool;
    public Image _iconImg;
    private string _clothetype;
    private string DefaultTempString;
    [HideInInspector]
    public string _downloadableURL;
    public EnumClass.CategoryEnum CategoriesEnumVar;
    public Coroutine runningCoroutine;
    public bool enableUpdate = false;
    public bool completedCoroutine = false, runOnce = false, AddInUndoRedoOnce = false;
    public GameObject loadingSpriteImage;
    public bool firstTimeEnable = false;

    bool itemAlreadySaved = false;
    private bool isHairItem = false;
    int saveIndex = -1;
    //bool isAdded = true;
    private AddressableDownloader downloader;
    CharacterBodyParts characterBodyParts;
    InventoryManager store;
   
    
    
    private void Start()
    {
        store = InventoryManager.instance;
        characterBodyParts = GameManager.Instance.mainCharacter.GetComponent<CharacterBodyParts>();

        if (CategoriesEnumVar.Equals(EnumClass.CategoryEnum.HairAvatar) && this.id == ConstantsHolder.xanaConstants.hair)
        {
            isHairItem = true;
            // //Debug.Log("IsStartItem=true");
        }
        CheckDeemoNft();
    }


    private void OnEnable()
    {
        downloader = AddressableDownloader.Instance;
        if (enableUpdate)
            StartRun();

        Invoke("Delay", 0.1f);    // AR changes
    }
    private void OnDisable()
    {
        if (this.name == "ColorButton")
            return;
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }

        //AssetCache.Instance.RemoveFromMemory(iconLink, true);       // AR changes
        //Destroy(this.gameObject);                                   // AR changes


        //this.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }


    public void CheckDeemoNft()
    {
        if (!ConstantsHolder.xanaConstants.IsDeemoNFT)
        {
            if (name.Contains("deemotshirt"))
            {
                //Debug.Log("yES dEEMO dEEMO");
                this.gameObject.SetActive(false);
            }

        }
    }
    void Delay()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            string CurrentString = "";
            CurrentString = CategoriesEnumVar.ToString();

            switch (CurrentString)
            {
                case "HairAvatar":
                    {
                        ////Debug.Log(this.id + " xanaConstantsHairs: " + ConstantsHolder.xanaConstants.hair);
                        ////Debug.Log("wornHairId: " + SaveCharacterProperties.instance.characterController.wornHairId.ToString());
                        if ((isHairItem && this.id == ConstantsHolder.xanaConstants.hair) ||
                            (StoreStackHandler.obj.IsCallByBtn() && this.id == ConstantsHolder.xanaConstants.hair))
                        {
                            isHairItem = false;
                            if (!StoreUndoRedo.obj.addToList)
                                StoreUndoRedo.obj.addToList = true;
                            else
                            {
                                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ItemBtnClicked", StoreUndoRedo.ActionType.ChangeItem, Color.white, EnumClass.CategoryEnum.HairAvatar);
                                //Debug.Log("<color=red> Set Default Hair </color>");
                            }
                        }
                        break;
                    }
                case "EyeBrowAvatar":
                    {
                        ////Debug.Log(id.ParseToInt() + " EyeBrowValue: " + _CharacterData.EyeBrowValue);
                        if (StoreStackHandler.obj.IsCallByBtn() && id.ParseToInt() == _CharacterData.EyeBrowValue)
                        {
                            if (!StoreUndoRedo.obj.addToList)
                                StoreUndoRedo.obj.addToList = true;
                            else
                            {
                                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ItemBtnClicked", StoreUndoRedo.ActionType.ChangeItem, Color.white, EnumClass.CategoryEnum.EyeBrowAvatar);
                                //Debug.Log("<color=red> Set Default EyeBrow </color>");
                            }
                        }
                        break;
                    }
                case "SkinToneAvatar":
                    {
                        ////Debug.Log(MyIndex + " SkinValue: " + _CharacterData.SkinId);
                        if (StoreStackHandler.obj.IsCallByBtn() && MyIndex == _CharacterData.SkinId)
                        {
                            if (!StoreUndoRedo.obj.addToList)
                                StoreUndoRedo.obj.addToList = true;
                            else
                            {
                                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ColorBtnClicked", StoreUndoRedo.ActionType.ChangeColor, _iconImg.color, EnumClass.CategoryEnum.SkinToneAvatar);
                                //Debug.Log("<color=red> Set Default Skin Color </color>");
                            }
                        }
                    }
                    break;
                case "HairAvatarColor":
                    {
                        ////Debug.Log(id + " XanaHairColoPalette: " + ConstantsHolder.xanaConstants.hairColoPalette);
                        if (StoreStackHandler.obj.IsCallByBtn() && this.id == ConstantsHolder.xanaConstants.hairColoPalette)
                        {
                            if (!StoreUndoRedo.obj.addToList)
                                StoreUndoRedo.obj.addToList = true;
                            else
                            {
                                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ColorBtnClicked", StoreUndoRedo.ActionType.ChangeColor, _iconImg.color, EnumClass.CategoryEnum.HairAvatarColor);
                                //Debug.Log("<color=red> Set Default HairColor:::: </color>");
                            }
                        }
                        break;
                    }
                case "EyeBrowAvatarColor":
                    {
                        //Debug.Log("XanaEyeBrowColorPaletteIndex: " + ConstantsHolder.xanaConstants.eyeBrowColorPaletteIndex);
                        if (StoreStackHandler.obj.IsCallByBtn() && id.ParseToInt() == ConstantsHolder.xanaConstants.eyeBrowColorPaletteIndex)
                        {
                            if (!StoreUndoRedo.obj.addToList)
                                StoreUndoRedo.obj.addToList = true;
                            else
                            {
                                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ColorBtnClicked", StoreUndoRedo.ActionType.ChangeColor, _iconImg.color, EnumClass.CategoryEnum.EyeBrowAvatarColor);
                                //Debug.Log("<color=red> Set Default EyeBrowColorPalette::::" + this.gameObject.name + " </color>");
                            }
                        }
                        break;
                    }
                case "EyesAvatarColor":
                    {
                        //Debug.Log("XanaEyeColorPalette: " + ConstantsHolder.xanaConstants.eyeColorPalette);
                        if (StoreStackHandler.obj.IsCallByBtn() && this.id == ConstantsHolder.xanaConstants.eyeColorPalette)
                        {
                            if (!StoreUndoRedo.obj.addToList)
                                StoreUndoRedo.obj.addToList = true;
                            else
                            {
                                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ColorBtnClicked", StoreUndoRedo.ActionType.ChangeColor, _iconImg.color, EnumClass.CategoryEnum.EyesAvatarColor);
                                //Debug.Log("<color=red> Set Default EyeColorPalette:::: </color>");
                            }
                        }
                        break;
                    }
                case "LipsAvatarColor":
                    {
                        //Debug.Log("XanaLipColorPalette: " + ConstantsHolder.xanaConstants.lipColorPalette);
                        if (StoreStackHandler.obj.IsCallByBtn() && this.id == ConstantsHolder.xanaConstants.lipColorPalette)
                        {
                            if (!StoreUndoRedo.obj.addToList)
                                StoreUndoRedo.obj.addToList = true;
                            else
                            {
                                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ColorBtnClicked", StoreUndoRedo.ActionType.ChangeColor, _iconImg.color, EnumClass.CategoryEnum.LipsAvatarColor);
                                //Debug.Log("<color=red> Set Default LipsColorPalette:::: </color>");
                            }
                        }
                        break;
                    }
                case "EyeLashesAvatar":
                    {
                        ////Debug.Log(id.ParseToInt() + " EyeLashesValue: " + ConstantsHolder.xanaConstants.eyeLashesIndex);
                        if (StoreStackHandler.obj.IsCallByBtn() && id.ParseToInt() == ConstantsHolder.xanaConstants.eyeLashesIndex)
                        {
                            if (!StoreUndoRedo.obj.addToList)
                                StoreUndoRedo.obj.addToList = true;
                            else
                            {
                                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ItemBtnClicked", StoreUndoRedo.ActionType.ChangeItem, Color.white, EnumClass.CategoryEnum.EyeLashesAvatar);
                                //Debug.Log("<color=red> Set Default EyeBrowPoints </color>");
                            }
                        }
                        break;
                    }
                case "Outer":
                    {
                        ////Debug.Log("Enter In outer: " + ConstantsHolder.xanaConstants.shirt);
                        if (id == ConstantsHolder.xanaConstants.shirt)  // StoreStackHandler.obj.IsCallByBtn() && 
                        {
                            if (!StoreUndoRedo.obj.addToList)
                                StoreUndoRedo.obj.addToList = true;
                            else
                            {
                                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ItemBtnClicked", StoreUndoRedo.ActionType.ChangeItem, Color.white, EnumClass.CategoryEnum.Outer);
                                //Debug.Log("<color=red> Set Default Shirts </color>");
                            }
                        }
                        break;
                    }
                case "Shoes":
                    {
                        ////Debug.Log("Enter In Shose: " + ConstantsHolder.xanaConstants.shoes);
                        if (id == ConstantsHolder.xanaConstants.shoes)  // StoreStackHandler.obj.IsCallByBtn() && 
                        {
                            if (!StoreUndoRedo.obj.addToList)
                                StoreUndoRedo.obj.addToList = true;
                            else
                            {
                                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ItemBtnClicked", StoreUndoRedo.ActionType.ChangeItem, Color.white, EnumClass.CategoryEnum.Shoes);
                                //Debug.Log("<color=red> Set Default Shoes </color>");
                            }
                        }
                        break;
                    }
                case "Bottom":
                    {
                        ////Debug.Log("Enter In Shose: " + ConstantsHolder.xanaConstants.pants);
                        if (id == ConstantsHolder.xanaConstants.pants)  // StoreStackHandler.obj.IsCallByBtn() && 
                        {
                            if (!StoreUndoRedo.obj.addToList)
                                StoreUndoRedo.obj.addToList = true;
                            else
                            {
                                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ItemBtnClicked", StoreUndoRedo.ActionType.ChangeItem, Color.white, EnumClass.CategoryEnum.Bottom);
                                //Debug.Log("<color=red> Set Default Pents </color>");
                            }
                        }
                        break;
                    }
            }
        }
    }

    public void StartRun()
    {
        if (!runOnce)
        {

            runOnce = true;
            //if (runningCoroutine != null)
            //{
            //    StopCoroutine(runningCoroutine);
            //}

            if (CategoriesEnumVar == EnumClass.CategoryEnum.EyeBrowAvatar ||
                CategoriesEnumVar == EnumClass.CategoryEnum.EyeBrowAvatarColor ||
                CategoriesEnumVar == EnumClass.CategoryEnum.EyeLashesAvatar ||
                CategoriesEnumVar == EnumClass.CategoryEnum.LipsAvatar ||
                 CategoriesEnumVar == EnumClass.CategoryEnum.LipsAvatarColor ||
                CategoriesEnumVar == EnumClass.CategoryEnum.HairAvatarColor ||
                CategoriesEnumVar == EnumClass.CategoryEnum.EyesAvatar ||
                CategoriesEnumVar == EnumClass.CategoryEnum.EyesAvatarColor ||
                CategoriesEnumVar == EnumClass.CategoryEnum.SkinToneAvatar)
            {

                // Added By Waqas Ahmad
                // Eyebrow Icon resizing require
                // Eyebrow button method is itemBtnClicked
                //
                if (CategoriesEnumVar == EnumClass.CategoryEnum.EyeBrowAvatar || CategoriesEnumVar == EnumClass.CategoryEnum.EyeLashesAvatar)
                {
                    this.gameObject.GetComponent<Button>().onClick.AddListener(ItemBtnClicked);
                    this.gameObject.GetComponent<Button>().onClick.AddListener(ResetButtonState);
                }
                //
                else
                    this.gameObject.GetComponent<Button>().onClick.AddListener(ColorBtnClicked);


                PriceTxt.gameObject.SetActive(false);
                _iconImg.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                _iconImg.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

            }
            else
            {
                this.gameObject.GetComponent<Button>().onClick.AddListener(ItemBtnClicked);
                this.gameObject.GetComponent<Button>().onClick.AddListener(ResetButtonState);
            }

            if (string.IsNullOrEmpty(price))
            {
                price = "0";
            }

            decimal PriceInDecimal = decimal.Parse(price,CultureInfo.InvariantCulture);
            int priceint = (int)PriceInDecimal;
            PriceTxt.text = priceint.ToString();
            switch (CategoriesEnumVar)
            {
                case EnumClass.CategoryEnum.HairAvatar:
                    {
                        _clothetype = "Hair";
                        DefaultTempString = "/MDhairs.";
                    }
                    break;
                case EnumClass.CategoryEnum.Shoes:
                    {
                        _clothetype = "Feet";
                        DefaultTempString = "/MDshoes.";
                    }
                    break;
                case EnumClass.CategoryEnum.Outer:
                    {
                        _clothetype = "Chest";
                        DefaultTempString = "/MDshirt.";
                    }
                    break;
                case EnumClass.CategoryEnum.Bottom:
                    {
                        _clothetype = "Legs";
                        DefaultTempString = "/MDpant.";
                    }
                    break;
                case EnumClass.CategoryEnum.LipsAvatar:
                    {
                        _clothetype = "Lip";
                    }
                    break;
                case EnumClass.CategoryEnum.EyesAvatar:
                    {
                        _clothetype = "Eyes";
                    }
                    break;
                case EnumClass.CategoryEnum.SkinToneAvatar:
                    {
                        _clothetype = "Skin";
                    }
                    break;
            }
            isPurchased = "true";

            if (isPaid == "true")
            {
                PriceTxt.text = "Coming Soon";
                this.gameObject.GetComponent<Button>().interactable = false;
            }
        }
        if (CategoriesEnumVar == EnumClass.CategoryEnum.SkinToneAvatar)
        {
            _iconImg.sprite = store.defaultPngForSkinIcon;
            _iconImg.color = characterBodyParts.skinColor[MyIndex];
            loadingSpriteImage.SetActive(false);
            completedCoroutine = true;
            enableUpdate = false;
        }
        else if (CategoriesEnumVar == EnumClass.CategoryEnum.HairAvatarColor)
        {
            _iconImg.sprite = store.defaultPngForSkinIcon;
            _iconImg.color = characterBodyParts.hairColor[MyIndex];
            loadingSpriteImage.SetActive(false);
            completedCoroutine = true;
            enableUpdate = false;
        }
        else if (CategoriesEnumVar == EnumClass.CategoryEnum.EyeBrowAvatarColor)
        {
            _iconImg.sprite = store.defaultPngForSkinIcon;
            _iconImg.color = characterBodyParts.eyeBrowsColor[MyIndex];
            loadingSpriteImage.SetActive(false);
            completedCoroutine = true;
            enableUpdate = false;
        }
        else if (CategoriesEnumVar == EnumClass.CategoryEnum.EyesAvatarColor)
        {
            _iconImg.sprite = store.defaultPngForSkinIcon;
            _iconImg.color = characterBodyParts.eyeColor[MyIndex];
            loadingSpriteImage.SetActive(false);
            completedCoroutine = true;
            enableUpdate = false;
        }
        else if (CategoriesEnumVar == EnumClass.CategoryEnum.LipsAvatarColor)
        {
            _iconImg.sprite = store.defaultPngForSkinIcon;
            _iconImg.color = characterBodyParts.lipColorPalette[MyIndex];
            loadingSpriteImage.SetActive(false);
            completedCoroutine = true;
            enableUpdate = false;
        }
        else //if (iconLink != null && CategoriesEnumVar != EnumClass.CategoryEnum.SkinToneAvatar && CategoriesEnumVar != EnumClass.CategoryEnum.HairAvatarColor)
        {
            if (!completedCoroutine)
            {
                //Debug.Log("Downloading-Icon-Link: " + iconLink);
                AssetCache.Instance.EnqueueOneResAndWait(iconLink, iconLink, (success) =>
                {
                    if (success)
                    {
                        AssetCache.Instance.LoadSpriteIntoImage(_iconImg, iconLink, changeAspectRatio: true);
                        // CheckAndSetResolutionOfImage(imgFeed.sprite);
                        //  isImageSuccessDownloadAndSave = true;
                        if (_iconImg != null)
                        {
                            // While Downloading User change the tab destroing objects make its null
                            _iconImg.gameObject.SetActive(true);
                            loadingSpriteImage.SetActive(false);
                        }
                        completedCoroutine = true;
                        enableUpdate = false;
                    }
                    else
                    {
                        ////Debug.LogError("Download Failed");
                    }
                });


                //runningCoroutine = StartCoroutine(addsprite(_iconImg, iconLink));
            }
            else
            {
                _iconImg.gameObject.SetActive(true);
            }
        }

    }

    public void UpdateValues()
    {
        decimal PriceInDecimal = decimal.Parse(price);
        int priceint = (int)PriceInDecimal;
        PriceTxt.text = priceint.ToString();
    }

    public IEnumerator addsprite(Image data, string rewview)
    {
        // WHEN IMAGES ARE IN PNG FORMAT
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(rewview))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                ////Debug.Log(uwr.error);
            }
            else
            {
                Texture2D tex = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                // tex.Compress(true);
                if (data)   // Check if data is null or not
                    data.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                _iconImg.gameObject.SetActive(true);
                loadingSpriteImage.SetActive(false);
                completedCoroutine = true;
                enableUpdate = false;
            }
            uwr.Dispose();
        }
        yield return null;

    }
    public void DeSelectBtns()
    {
        //  SelectImg.enabled = false;
        this.gameObject.GetComponent<Image>().color = NormalColor;
    }

    public void DeSelectAfterBuying()
    {
        SelectedBool = !SelectedBool;
        if (SelectedBool)
        {
            // SelectImg.enabled = true;
            this.gameObject.GetComponent<Image>().color = HighlightedColor;
        }
        else
        {
           //  SelectImg.enabled = false;
            this.gameObject.GetComponent<Image>().color = NormalColor;
        }
        if (!SelectedBool)
        {
            InventoryManager.instance.GetSelectedBtn(-1, CategoriesEnumVar);
        }
        else
        {
            InventoryManager.instance.GetSelectedBtn(MyIndex, CategoriesEnumVar);
        }
    }

    public void ItemBtnClicked()
    {
        Debug.Log("on hua yaha par");
        if (GameManager.Instance.isStoreAssetDownloading || GetComponent<Image>().enabled is true)
            return;

        string CurrentString = "";
        CurrentString = CategoriesEnumVar.ToString();
        GameManager.Instance.ResetSelectedItems();
       

        switch (CurrentString)
        {
            case "Shoes":
                {
                    CurrentString = "Shoes";
                    break;
                }
            case "Outer":
                {
                    CurrentString = "Shirts/Outer";
                    break;
                }
            case "HairAvatar":
                {
                    CurrentString = "Hairs";
                    break;
                }
            // Added by Ahsan
            case "HairAvatarColor":
                {
                    CurrentString = "HairColor";
                    break;
                }
            case "Bottom":
                {
                    CurrentString = "Pents /Bottom";
                    break;
                }
            // Added By WaqasAhmad
            case "EyeBrowAvatar":
                {
                    CurrentString = "Eye Brow";
                    break;
                }
            // Added by Ahsan
            case "EyeBrowAvatarColor":
                {
                    CurrentString = "EyeBrowColor";
                    break;
                }
            // Added By WaqasAhmad
            case "EyeLashesAvatar":
                {
                    CurrentString = "Eye Lashes";
                    break;
                }
            case "EyesAvatarColor":
                {
                    CurrentString = "EyesColor";
                    break;
                }
            case "LipsAvatarColor":
                {
                    CurrentString = "LipsColor";
                    break;
                }
        }

        if (!UserPassManager.Instance.CheckSpecificItem(CurrentString))
        {
            UserPassManager.Instance.PremiumUserUI.SetActive(true);

            //print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            itemAlreadySaved = false;
            switch (CurrentString)
            {
                case "Shoes":
                    {
                        ConstantsHolder.xanaConstants.shoes = id;
                        ConstantsHolder.xanaConstants.wearableStoreSelection[ConstantsHolder.xanaConstants.currentButtonIndex] = gameObject;
                        saveIndex = 3;

                        break;
                    }
                case "Shirts/Outer":
                    {
                        ConstantsHolder.xanaConstants.shirt = id;
                        ConstantsHolder.xanaConstants.wearableStoreSelection[ConstantsHolder.xanaConstants.currentButtonIndex] = gameObject;
                        saveIndex = 1;
                        break;
                    }
                case "Hairs":
                    {
                        ConstantsHolder.xanaConstants.hair = id;
                        ConstantsHolder.xanaConstants.avatarStoreSelection[ConstantsHolder.xanaConstants.currentButtonIndex] = gameObject;
                        saveIndex = 2;
                        //ConstantsHolder.xanaConstants.isPresetHairColor = true; presetHairColor;


                        ////Debug.Log("ConstantsHolder Hairs: " + ConstantsHolder.xanaConstants.hair);
                        ////Debug.Log("wornHairId: " + SaveCharacterProperties.instance.characterController.wornHairId.ToString());
                        break;
                    }
                case "HairColor":
                    {
                        //if (!InventoryManager.instance.CheckColorPanelEnabled(ConstantsHolder.xanaConstants.currentButtonIndex))
                        //{
                        //Debug.Log("<color=blue> Open Hair Color Panel: </color>");
                        InventoryManager.instance.OpenColorPanel(ConstantsHolder.xanaConstants.currentButtonIndex);
                        InventoryManager.instance.colorMode = true;
                        InventoryManager.instance.PutDataInOurAPPNewAPI();
                        InventoryManager.instance.colorMode = false;
                        //}
                        //else   
                        //{
                        //    int enumNum = Array.IndexOf(Enum.GetValues(typeof(EnumClass.CategoryEnum)), EnumClass.CategoryEnum.HairAvatar);
                        //    StoreUndoRedo.obj.AvatarBtnPressedForcally(enumNum);
                        //}

                        // AR changes start
                        if (!StoreUndoRedo.obj.addToList)
                            StoreUndoRedo.obj.addToList = true;
                        else
                        {
                            StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ItemBtnClicked", StoreUndoRedo.ActionType.ChangePanel, Color.white, EnumClass.CategoryEnum.HairAvatar);
                            //Debug.Log("<color=red> Set Hair color btn into list:::: </color>");
                        }
                        // AR changes end
                        if (this.gameObject.name == "Color Button")
                            return;
                        break;
                    }
                case "EyesColor":
                    {
                        InventoryManager.instance.OpenColorPanel(ConstantsHolder.xanaConstants.currentButtonIndex);
                        InventoryManager.instance.colorMode = true;
                        InventoryManager.instance.PutDataInOurAPPNewAPI();
                        InventoryManager.instance.colorMode = false;

                        if (!StoreUndoRedo.obj.addToList)
                            StoreUndoRedo.obj.addToList = true;
                        else
                        {
                            StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ItemBtnClicked", StoreUndoRedo.ActionType.ChangePanel, Color.white, EnumClass.CategoryEnum.EyesAvatar);
                            //Debug.Log("<color=red> Set eye color btn into list:::: </color>");
                        }
                        if (this.gameObject.name == "Color Button")
                            return;
                        break;
                    }
                case "LipsColor":
                    {
                        InventoryManager.instance.OpenColorPanel(ConstantsHolder.xanaConstants.currentButtonIndex);
                        InventoryManager.instance.colorMode = true;
                        InventoryManager.instance.PutDataInOurAPPNewAPI();
                        InventoryManager.instance.colorMode = false;

                        if (!StoreUndoRedo.obj.addToList)
                            StoreUndoRedo.obj.addToList = true;
                        else
                        {
                            StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ItemBtnClicked", StoreUndoRedo.ActionType.ChangePanel, Color.white, EnumClass.CategoryEnum.LipsAvatar);
                            //Debug.Log("<color=red> Set lips color btn into list:::: </color>");
                        }

                        if (this.gameObject.name == "Color Button")
                            return;
                        break;
                    }
                case "EyeBrowColor":
                    {
                        InventoryManager.instance.OpenColorPanel(ConstantsHolder.xanaConstants.currentButtonIndex);
                        InventoryManager.instance.colorMode = true;
                        InventoryManager.instance.PutDataInOurAPPNewAPI();
                        InventoryManager.instance.colorMode = false;

                        if (!StoreUndoRedo.obj.addToList)
                            StoreUndoRedo.obj.addToList = true;
                        else
                        {
                            StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ItemBtnClicked", StoreUndoRedo.ActionType.ChangePanel, Color.white, EnumClass.CategoryEnum.EyeBrowAvatar);
                            //Debug.Log("<color=red> Set eyeBrow color btn into list:::: </color>");
                        }
                        if (this.gameObject.name == "Color Button")
                            return;
                        break;
                    }
                case "Eye Brow":
                    {
                        ConstantsHolder.xanaConstants.eyeBrowIndex = id.ParseToInt();
                        //Debug.Log("Eye brow eyeBrowIndex: " + ConstantsHolder.xanaConstants.eyeBrowIndex);
                        SaveCharacterProperties.instance.characterController.eyeBrowId = id.ParseToInt();
                        //Debug.Log("Eye brow ID: " + SaveCharacterProperties.instance.characterController.eyeBrowId);
                        //Commented By Ahsan
                        //Transform ParentAvatarofEyeBrows = InventoryManager.instance.ParentOfBtnsAvatarEyeBrows;

                        //for (int i = 1; i < ParentAvatarofEyeBrows.childCount; i++)
                        //{
                        //    ParentAvatarofEyeBrows.GetChild(i).GetComponent<Image>().enabled = false;
                        //}

                        //ParentAvatarofEyeBrows.GetChild(MyIndex + 1).GetComponent<Image>().enabled = true;
                        break;
                    }
                case "Eye Lashes":
                    {
                        ConstantsHolder.xanaConstants.eyeLashesIndex = id.ParseToInt();
                        SaveCharacterProperties.instance.characterController.eyeLashesId = id.ParseToInt();
                        //Transform ParentAvatarofEyeLashes = InventoryManager.instance.ParentOfBtnsAvatarEyeLashes;
                        //for (int i = 0; i < ParentAvatarofEyeLashes.childCount; i++)
                        //{
                        //    ParentAvatarofEyeLashes.GetChild(i).GetComponent<Image>().enabled = false;
                        //}
                        //ParentAvatarofEyeLashes.GetChild(MyIndex).GetComponent<Image>().enabled = true;
                    }
                    break;
                case "Pents /Bottom":
                    {
                        ConstantsHolder.xanaConstants.pants = id;
                        ConstantsHolder.xanaConstants.wearableStoreSelection[ConstantsHolder.xanaConstants.currentButtonIndex] = gameObject;
                        saveIndex = 0;

                        break;
                    }
            }
            ConstantsHolder.xanaConstants._curretClickedBtn = this.gameObject;

            //Debug.Log("Undo Redo call in Purchase check: add to list= " + StoreUndoRedo.obj.addToList);
            if (!StoreUndoRedo.obj.addToList)
                StoreUndoRedo.obj.addToList = true;
            else
            {
                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ItemBtnClicked", StoreUndoRedo.ActionType.ChangeItem, Color.white, CategoriesEnumVar);
                //Debug.Log("<color=red> Set On Btn clicked " + CurrentString + ":::: </color>");
            }

            if (ConstantsHolder.xanaConstants._lastClickedBtn && ConstantsHolder.xanaConstants._curretClickedBtn == ConstantsHolder.xanaConstants._lastClickedBtn)
                return;

            ////Debug.Log("<color=red>" + ConstantsHolder.xanaConstants._curretClickedBtn.GetComponent<ItemDetail>().id + "</color>");
            ConstantsHolder.xanaConstants._curretClickedBtn.GetComponent<ItemDetail>().SelectImg.enabled = true;

            if (ConstantsHolder.xanaConstants._lastClickedBtn)
            {
                if (ConstantsHolder.xanaConstants._lastClickedBtn.GetComponent<ItemDetail>())
                    ConstantsHolder.xanaConstants._lastClickedBtn.GetComponent<ItemDetail>().SelectImg.enabled=false;
            }
            //Debug.Log("<color=red>ItemDetail AssignLastClickedBtnHere</color>");
            ConstantsHolder.xanaConstants._lastClickedBtn = this.gameObject;

            if (!completedCoroutine)
                return;


            if (isPurchased == "true")
            {
                if (!GameManager.Instance.isStoreAssetDownloading)
                {
                    GameManager.Instance.isStoreAssetDownloading = true;
                    if (!name.Contains("md", System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (name.Contains("eyebrow"))
                            downloader.StartCoroutine(downloader.DownloadAddressableTexture(name, GameManager.Instance.mainCharacter, CurrentTextureType.EyeBrows));
                        else if (name.Contains("eyelash"))
                            downloader.StartCoroutine(downloader.DownloadAddressableTexture(name, GameManager.Instance.mainCharacter, CurrentTextureType.EyeLashes));
                        else
                            downloader.StartCoroutine(downloader.DownloadAddressableObj(int.Parse(id), name, _clothetype, "Male", GameManager.Instance.mainCharacter.GetComponent<AvatarController>(), Color.clear, true));
                    }
                    else
                    {
                        GameManager.Instance.mainCharacter.GetComponent<AvatarController>().WearDefaultItem(_clothetype, GameManager.Instance.mainCharacter, "Male");
                    }
                    //InventoryManager.instance._DownloadRigClothes.NeedToDownloadOrNot(this, assetLinkAndroid, assetLinkIos, _clothetype, name.ToLower(), int.Parse(id));
                }
                this.GetComponent<Image>().enabled = true;

                if (InventoryManager.instance.UndoBtn)
                    InventoryManager.instance.UndoBtn.GetComponent<Button>().interactable = true;

                // to updated data in equipment and to change saveindex which is for save file
                switch (CategoriesEnumVar)
                {
                    case EnumClass.CategoryEnum.Head:
                        break;
                    case EnumClass.CategoryEnum.Face:
                        break;
                    case EnumClass.CategoryEnum.Inner:
                        break;
                    case EnumClass.CategoryEnum.Outer:
                        //CharacterController.instance.equipmentScript.SetItemIdName(int.Parse(id), name, assetLinkAndroid, assetLinkIos, 1);
                        ConstantsHolder.xanaConstants.shirt = id;
                        saveIndex = 1;
                        break;
                    case EnumClass.CategoryEnum.Accesary:
                        break;
                    case EnumClass.CategoryEnum.Bottom:
                        //CharacterController.instance.equipmentScript.SetItemIdName(int.Parse(id), name, assetLinkAndroid, assetLinkIos, 0);
                        ConstantsHolder.xanaConstants.pants = id;
                        saveIndex = 0;
                        break;
                    case EnumClass.CategoryEnum.Socks:
                        break;
                    case EnumClass.CategoryEnum.Shoes:
                        //CharacterController.instance.equipmentScript.SetItemIdName(int.Parse(id), name, assetLinkAndroid, assetLinkIos, 7);
                        ConstantsHolder.xanaConstants.shoes = id;
                        saveIndex = 3;
                        break;
                    case EnumClass.CategoryEnum.HairAvatar:
                        //CharacterController.instance.equipmentScript.SetItemIdName(int.Parse(id), name, assetLinkAndroid, assetLinkIos, 2);
                        ConstantsHolder.xanaConstants.hair = id;
                        saveIndex = 2;
                        break;
                    case EnumClass.CategoryEnum.LipsAvatar:
                        break;
                    case EnumClass.CategoryEnum.EyesAvatar:
                        break;
                    case EnumClass.CategoryEnum.SkinToneAvatar:
                        break;
                    case EnumClass.CategoryEnum.Presets:
                        break;
                    default:
                        break;
                }

                if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                {
                    SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

                    _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
                    if (saveIndex >= 0 && _CharacterData.myItemObj.Count != 0)
                    {
                        if (id == _CharacterData.myItemObj[saveIndex].ItemID.ToString())
                        {
                            itemAlreadySaved = true;
                        }
                    }

                    if (CategoriesEnumVar == EnumClass.CategoryEnum.EyeBrowAvatar)
                    {
                        if (id.ParseToInt() == _CharacterData.EyeBrowValue)
                            itemAlreadySaved = true;
                    }
                    else if (CategoriesEnumVar == EnumClass.CategoryEnum.EyeLashesAvatar)
                    {
                        if (id.ParseToInt() == _CharacterData.EyeLashesValue)
                            itemAlreadySaved = true;
                    }
                }

                if (!itemAlreadySaved)
                {
                    InventoryManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = true;
                    SavedButtonClickedBlue();
                }

                else
                {
                    InventoryManager.instance.SaveStoreBtn.SetActive(true);
                    InventoryManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = false;
                    InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
                    InventoryManager.instance.GreyRibbonImage.SetActive(true);
                    InventoryManager.instance.WhiteRibbonImage.SetActive(false);
                }

            }
        }
    }

    void ResetButtonState()
    {
        obj.currentButtonState = ButtonState.none;
        obj.calledOneTime = false;
    }
    // open color pallete panel by press color btn
    public void ColorBtnClicked()
    {
        SelectImg.enabled = true;
        //this.gameObject.GetComponent<Image>().color = HighlightedColor;
        //return;
        if (GetComponent<Image>().enabled is true)
            return;

        string CurrentString = "";

        if (CategoriesEnumVar.ToString() == "SkinToneAvatar")
        {
            CurrentString = "Skin";
        }
        else if (CategoriesEnumVar.ToString() == "EyesAvatar")
        {
            CurrentString = "eyes";
        }
        else if (CategoriesEnumVar.ToString() == "EyesAvatarColor")
        {
            CurrentString = "EyesColor";
        }
        else if (CategoriesEnumVar.ToString() == "LipsAvatar")
        {
            CurrentString = "Lip";
        }
        else if (CategoriesEnumVar.ToString() == "LipsAvatarColor")
        {
            CurrentString = "LipsColor";
        }
        else if (CategoriesEnumVar.ToString() == "HairAvatarColor")
        {
            CurrentString = "HairColor";
        }
        else if (CategoriesEnumVar.ToString() == "EyeBrowAvatarColor")
        {
            CurrentString = "EyeBrowColor";
        }

        //Debug.Log("Current String is: " + CurrentString);

        if (!UserPassManager.Instance.CheckSpecificItem(CurrentString))
        {
            UserPassManager.Instance.PremiumUserUI.SetActive(true);

            //print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            itemAlreadySaved = false;

            if (CategoriesEnumVar.ToString() == "SkinToneAvatar")
            {
                //print("in skin tone ");
                ConstantsHolder.xanaConstants.skinColor = MyIndex.ToString();
                ConstantsHolder.xanaConstants.avatarStoreSelection[ConstantsHolder.xanaConstants.currentButtonIndex] = gameObject;

                saveIndex = 6;
            }
            else if (CategoriesEnumVar.ToString() == "EyesAvatar")
            {
                ConstantsHolder.xanaConstants.eyeColor = id;
                ConstantsHolder.xanaConstants.colorSelection[0] = gameObject;

                saveIndex = 4;
            }
            else if (CategoriesEnumVar.ToString() == "LipsAvatar")
            {
                ConstantsHolder.xanaConstants.lipColor = id;
                ConstantsHolder.xanaConstants.colorSelection[1] = gameObject;

                saveIndex = 5;
            }
            else if (CategoriesEnumVar.ToString() == "HairAvatarColor")
            {
                ////Debug.Log("Assign ID to by default color selection: " + id);
                // //Debug.Log("Before: " + ConstantsHolder.xanaConstants.hairColoPalette);

                ConstantsHolder.xanaConstants.hairColoPalette = id;
                ConstantsHolder.xanaConstants.colorSelection[2] = gameObject;
                ////Debug.Log("After: " + ConstantsHolder.xanaConstants.hairColoPalette);
                //saveIndex = 4;
            }
            else if (CategoriesEnumVar.ToString() == "EyeBrowAvatarColor")
            {
                ConstantsHolder.xanaConstants.eyeBrowColorPaletteIndex = int.Parse(id);
                ConstantsHolder.xanaConstants.colorSelection[3] = gameObject;

                //saveIndex = 4;
            }
            else if (CategoriesEnumVar.ToString() == "EyesAvatarColor")
            {
                ConstantsHolder.xanaConstants.eyeColorPalette = id;
                ConstantsHolder.xanaConstants.colorSelection[4] = gameObject;

                //saveIndex = 4;
            }
            else if (CategoriesEnumVar.ToString() == "LipsAvatarColor")
            {
                ConstantsHolder.xanaConstants.lipColorPalette = id;
                ConstantsHolder.xanaConstants.colorSelection[5] = gameObject;

                //saveIndex = 4;
            }

            //Debug.Log("Undo Redo call in Purchase check: add to list= " + StoreUndoRedo.obj.addToList);
            if (!StoreUndoRedo.obj.addToList)
                StoreUndoRedo.obj.addToList = true;
            else
            {
                StoreUndoRedo.obj.ActionWithParametersAdd(this.gameObject, -1, "ColorBtnClicked", StoreUndoRedo.ActionType.ChangeColor, _iconImg.color, CategoriesEnumVar);
                //Debug.Log("<color=red> Set On color Btn clicked " + CurrentString + ":::: </color>");
            }

            ////Debug.Log("Item Detail Btn Clicked Obj Name: " + this.gameObject);
            ConstantsHolder.xanaConstants._curretClickedBtn = this.gameObject;

            if (ConstantsHolder.xanaConstants._lastClickedBtn && ConstantsHolder.xanaConstants._curretClickedBtn == ConstantsHolder.xanaConstants._lastClickedBtn)
                return;

            ConstantsHolder.xanaConstants._curretClickedBtn.GetComponent<Image>().enabled = true;

            if (ConstantsHolder.xanaConstants._lastClickedBtn)
            {
                // //Debug.Log("_lastClickedBtnId: " + ConstantsHolder.xanaConstants._lastClickedBtn.GetComponent<ItemDetail>().id);
                if (ConstantsHolder.xanaConstants._lastClickedBtn.GetComponent<ItemDetail>())
                {
                    ConstantsHolder.xanaConstants._lastClickedBtn.GetComponent<Image>().enabled = false;
                    ////Debug.Log("Disabled Image Here");
                }
            }
            InventoryManager.instance.ClearBuyItems();

            if (CategoriesEnumVar == EnumClass.CategoryEnum.SkinToneAvatar || CategoriesEnumVar == EnumClass.CategoryEnum.LipsAvatar)
            {
                applytexture(null, assetLinkIos);
            }
            else if (CategoriesEnumVar == EnumClass.CategoryEnum.EyesAvatar)
            {
                if (name != "")
                {
                    downloader.StartCoroutine(downloader.DownloadAddressableTexture(name, GameManager.Instance.mainCharacter, CurrentTextureType.EyeLense));
                    SaveCharacterProperties.instance.characterController.eyesColorId = int.Parse(id);
                }
                if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                {
                    SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

                    _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
                    //if (_CharacterData.myItemObj.Count != 0)
                    if (id == _CharacterData.EyesColorValue.ToString())
                    {
                        itemAlreadySaved = true;
                    }
                }
                //InventoryManager.instance._DownloadRigClothes.StartCoroutine(InventoryManager.instance._DownloadRigClothes.DownloadAndApplyEyeLenTexture(name, GameManager.Instance.mainCharacter));
            }
            else if (CategoriesEnumVar.ToString() == "HairAvatarColor")
            {
                characterBodyParts.ChangeHairColor(MyIndex);
                SaveCharacterProperties.instance.characterController.hairColorPaletteId = int.Parse(id);
            }
            else if (CategoriesEnumVar.ToString() == "EyeBrowAvatarColor")
            {
                characterBodyParts.ChangeEyebrowColor(MyIndex);
                SaveCharacterProperties.instance.characterController.eyeBrowColorPaletteId = int.Parse(id);
            }
            else if (CategoriesEnumVar.ToString() == "EyesAvatarColor")
            {
                characterBodyParts.ChangeEyeColor(MyIndex);
                SaveCharacterProperties.instance.characterController.eyesColorPaletteId = int.Parse(id);
            }
            else if (CategoriesEnumVar.ToString() == "LipsAvatarColor")
            {
                characterBodyParts.ChangeLipColorForPalette(MyIndex);
                SaveCharacterProperties.instance.characterController.lipsColorPaletteId = int.Parse(id);
            }
            else if (!File.Exists(Application.persistentDataPath + "/" + name))
            {
                InventoryManager.instance.load.SetActive(true);
                StartCoroutine(DownloadTextureFile(assetLinkIos));
            }
            else
            {
                applytexture(ReadTextureFromFile(Application.persistentDataPath + "/" + name), assetLinkIos);
            }

            if (!itemAlreadySaved)
            {
                InventoryManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = true;
                SavedButtonClickedBlue();
            }
            else
            {
                InventoryManager.instance.SaveStoreBtn.SetActive(true);
                InventoryManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = false;
                InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
                InventoryManager.instance.GreyRibbonImage.SetActive(true);
                InventoryManager.instance.WhiteRibbonImage.SetActive(false);
            }
            ////Debug.Log("<color=red>ItemDetail AssignLastClickedBtnHere</color>");
            ConstantsHolder.xanaConstants._lastClickedBtn = this.gameObject;
        }
    }

    IEnumerator DownloadTextureFile(string TextureURL)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(TextureURL))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                ////Debug.Log(uwr.error);
            }
            else
            {
                Texture2D tex = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                // tex.Compress(true);
                byte[] fileData = tex.EncodeToPNG();
                string filePath = Application.persistentDataPath + "/" + name;
                File.WriteAllBytes(filePath, fileData);
            }
            applytexture(ReadTextureFromFile(Application.persistentDataPath + "/" + name), assetLinkIos);
            uwr.Dispose();
        }
        yield return null;
    }

    public Texture2D ReadTextureFromFile(string path)
    {

        //print("Not downloading,available");
        byte[] _bytes;
        Texture2D mytexture;

        _bytes = File.ReadAllBytes(path);
        if (_bytes == null)
            return null;
        mytexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
        mytexture.LoadImage(_bytes);
        mytexture.Compress(true);
        return mytexture;
    }


    public void applytexture(Texture2D tex, string TextureURL)
    {
        //print("apply texture call");
        switch (_clothetype)
        {
            case "Lip":

                characterBodyParts.ChangeLipColor(MyIndex);
                SaveCharacterProperties.instance.characterController.lipsColorId = int.Parse(id);

                if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                {
                    SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

                    _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
                    //if (_CharacterData.myItemObj.Count != 0)
                    if (id == _CharacterData.LipsColorValue.ToString())
                    {
                        itemAlreadySaved = true;
                    }
                }
                break;

            case "Skin":
                // Waqas Ahmad
                characterBodyParts.ChangeSkinColor(MyIndex);
                //Debug.Log("Skin color slider");
                characterBodyParts.ChangeSkinColorSlider(MyIndex);
                SaveCharacterProperties.instance.characterController.skinId = MyIndex;

                if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
                {
                    SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

                    _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
                    //if (_CharacterData.myItemObj.Count != 0)
                    if (MyIndex == _CharacterData.SkinId)
                    {
                        itemAlreadySaved = true;
                    }
                }
                break;
        }

        if (InventoryManager.instance.UndoBtn)
            InventoryManager.instance.UndoBtn.GetComponent<Button>().interactable = true;

        if (!itemAlreadySaved)
        {
            InventoryManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = true;
            SavedButtonClickedBlue();
        }
        else
        {
            InventoryManager.instance.SaveStoreBtn.SetActive(true);
            InventoryManager.instance.SaveStoreBtn.GetComponent<Button>().interactable = false;
            InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
            InventoryManager.instance.GreyRibbonImage.SetActive(true);
            InventoryManager.instance.WhiteRibbonImage.SetActive(false);
        }
        InventoryManager.instance.load.SetActive(false);
    }

    void SavedButtonClickedBlue()
    {
        InventoryManager.instance.SaveStoreBtn.SetActive(true);
        InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
        InventoryManager.instance.GreyRibbonImage.SetActive(false);
        InventoryManager.instance.WhiteRibbonImage.SetActive(true);
    }
}