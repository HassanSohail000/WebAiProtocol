using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;
using SuperStar.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DG.DemiLib;
//using WebSocketSharp;
//using HSVPicker;


public class InventoryManager : MonoBehaviour
{
    [Header("Holds Api response")]
    public ResponseHolder apiResponseHolder;

    //public DownloadandRigClothes _DownloadRigClothes;
    public static InventoryManager instance;
    [Header("Main Panels Store")]
    public GameObject StoreItemsPanel;
    //public GameObject CheckOutBuyItemPanel;
    public GameObject ShowSignUpPanel;
    //public GameObject LowCoinsPanel;
    //public GameObject ShopBuyCoinsPanel;
    public EnumClass.CategoryEnum CategoriesEnumVar;
    public Text textskin;

    public GameObject ItemsBtnPrefab;
    [FormerlySerializedAs("oval")] public GameObject GreyRibbonImage;
    public GameObject WhiteRibbonImage;
    public Color HighlightedColor;
    public Color NormalColor;
    [Header("Total Panels Cloths")]
    public GameObject MainPanelCloth;
    public GameObject[] ClothsPanel;
    public GameObject BtnsPanelCloth;
    public GameObject ClothBtnLine;
    public Text ClothBtnText;
    [Header("Total Panels Avatar")]
    public GameObject MainPanelAvatar;

    public GameObject[] AvatarPanel;
    public GameObject BtnsPanelAvatar;
    public GameObject AvatarBtnLine;
    public Text AvatarBtnText;

    [Header("Return Home Pop up")]
    public GameObject ReturnHomePopUp;
    [Header("Total Buying Btns")]
    public GameObject BuyStoreBtn;
    public GameObject SaveStoreBtn; // Store Save Button
    public GameObject saveButton;  // Popup Save Button
    [Header("Total Texts money Display")]
    public Text BuyCountertxt;
    public Text TotalGameCoins;

    public List<StoreItemHolder> AllCategoriesData;
    public ScrollRect[] ItemsScrollrect;
    private int _BottomApiPagaCount = 1;
    private int _OuterApiPagaCount = 1;
    private int _ShoesApiPagaCount = 1;
    private int _HairApiPagaCount = 1;

    public List<ItemDetail> TotalBtnlist;
    public List<ItemDetail> CategorieslistHeads;
    public List<ItemDetail> CategorieslistFace;
    public List<ItemDetail> CategorieslistInner;
    public List<ItemDetail> CategorieslistOuter;
    public List<ItemDetail> CategorieslistAccesary;
    public List<ItemDetail> CategorieslistBottom;
    public List<ItemDetail> CategorieslistSocks;
    public List<ItemDetail> CategorieslistShoes;
    public List<ItemDetail> CategorieslistEyesColor;
    public List<ItemDetail> CategorieslistLipsColor;
    public List<ItemDetail> CategorieslistSkinToneColor;
    public List<ItemDetail> CategorieslistHairs;
    public List<ItemDetail> CategorieslistHairsColors;
    public GameObject SceneLoader;

    private int
        headsDownlaodedCount,
        faceDownlaodedCount,
        innerDownlaodedCount, innerDownlaodedCountFemale,
        outerDownlaodedCount, outerDownlaodedCountFemale,
        accesaryDownlaodedCount,
        bottomDownlaodedCount, bottomDownlaodedCountFemale,
        socksDownlaodedCount,
        shoesDownlaodedCount, shoesDownlaodedCountFemale,
        hairDwonloadedCount, hairDwonloadedCountFemale,
        LipsColorDwonloadedCount,
        EyesColorDwonloadedCount,
        EyeBrowColorDwonloadedCount,
        HairColorDwonloadedCount,
        skinColorDwonloadedCount,
        eyeBrowDwonloadedCount,
        eyeBrowColorDwonloadedCount,
        eyeLashesDwonloadedCount,
        eyesDwonloadedCount,
        lipsDwonloadedCount;

    [Space(10f)]
    public GameObject colorCustomizationPrefabBtn;

    //[Header("Buy Panel")]
    //public GameObject BuyItemPrefab;
    //public Transform BuyPanelParentOfBtns;
    //public List<GameObject> TotalObjectsInBuyPanel;
    //public List<GameObject> TotalSelectedInBuyPanel;
    //public Text TotalPriceBuyPanelTxt;
    //public Text TotalItemsBuyPanelTxt;
    //public GameObject BuyBtnCheckOut;
    //public string[] ArrayofBuyItems;
    //private int TotalItemPriceCheckOut;

    [Header("Color Customizations")]
    public bool colorMode = false;
    public GameObject colorBtn;
    public BodyColorCustomization bodyColorCustomization;
    //public CustomFakeStore fakeStore;
    public SliderColorPicker skinColorSlider;
    // Get Data FromJsonFiles
    [HideInInspector]
    public GetAllInfo JsonDataObj;

    // New APIS Integration //
    // APIS
    public string GetAllCategoriesAPI;
    public string GetAllSubCategoriesAPI;
    public string GetAllItems;

    private bool Clothdatabool;
    private int IndexofPanel;
    private int PreviousSelectionCount;


    // Containers
    GetAllInfoMainCategories ObjofMainCategory;
    string[] ArrayofMainCategories;
    public List<ItemsofSubCategories> SubCategoriesList;
    private bool CheckAPILoaded;
    List<TotaItemsClass> dataListOfItems = new List<TotaItemsClass>();

    public GameObject[] faceAvatarButton, eyeAvatarButton,
                  lipAvatarButton, eyeBrowsAvatarButton,
                   noseAvatarButton;

    public bool checkforSavebutton;  // its just for to check if item is downloaded or not
    public GameObject UndoBtn, RedoBtn, AvatarSaved, AvatarSavedGuest, AvatarUpdated;
    public GameObject Defaultreset, LastSavedreset, PanelResetDefault;
    // public GameObject ButtonFor_Preset;
    public GameObject StartPanel_PresetParentPanel, PresetArrayContent, selfiePanel, loaderPanel;
    public GameObject StartPanel_PresetParentPanelSummit;
    // public GameObject backbutton_preset;
    public Transform contentList;

    public GameObject faceTapButton;
    public GameObject eyeBrowTapButton;
    public GameObject eyeTapButton;
    public GameObject noseTapButton;
    public GameObject lipTapButton;

    public Button hairColorButton;
    public Button eyeBrowsColorButton;

    public int panelIndex = 0;
    int buttonIndex = -1;
    public bool saveButtonPressed = false;
    [HideInInspector]
    public bool UndoClicked = false;
    [HideInInspector]
    public bool RedoClicked = false;

    //private Image saveStoreBtnImage;
    //public Button saveStoreBtnButton;
    public GameObject load;
    public GameObject loaderForItems;

    [Header("My Avatar Panel Prefab Refrence")]
    public Button myAvatarButton;

    //public ColorPicker skinColorPicker;
    public bool MultipleSave; // to enable/ disable multiple save 
    private GameObject childObject;
    public Button newAvatarPresetBtn;
    public CanvasScaler _CanvasScaler;
    public Action storeOpen;
    //public UGCItemsData ugcItemsData;
    //public UGCItemData itemData;
    public CharacterBodyParts characterBodyParts;
    public Sprite defaultPngForSkinIcon;
    AvatarController _avatarController;
    public static event Action<BackButtonHandler.screenTabs> OnScreenTabStateChange;

    [Header("Comming Soon")] // Used for Vtuber Avatar
    public GameObject clothBottom_ComingSoon;
    public GameObject clothOuter_ComingSoon;
    public GameObject AvatarHair_ComingSoon, AvatarFace_ComingSoon, AvatarEyeBrow_ComingSoon, AvatarEye_ComingSoon, AvatarNose_ComingSoon, AvatarLip_ComingSoon;


    private void Awake()
    {

        instance = this;
        checkforSavebutton = false;

        DisableColorPanels();
        //characterBodyParts = GameManager.Instance.mainCharacter.GetComponent<CharacterBodyParts>();
        //for (int i = 0; i < 20; i++) { itemButtonsPool.Add( Instantiate(ItemsBtnPrefab)); }
    }
    [SerializeField]
    List<GridLayoutGroup> panelsLayoutGroups;

    public static Action upateAssetOnGenderChanged;

    public void TurnOffLoader(int time) {
        StartCoroutine(offLoader(time));
    }

    IEnumerator offLoader(float _time) {
        yield return new WaitForSeconds(_time);
        SceneLoader.SetActive(false);
    }

    void Start()
    {
        load = LoadPlayerAvatar.instance_loadplayer.loader;
        saveButton = LoadPlayerAvatar.instance_loadplayer.saveButton.gameObject;
        //saveStoreBtnImage = SaveStoreBtn.GetComponent<Image>();
        //saveStoreBtnButton = SaveStoreBtn.GetComponent<Button>();
        CheckAPILoaded = false;
       // Debug.Log("##################################################%%%%%%%%%%%%%%%%%%%");
        if (PlayerPrefs.GetInt("WalletLogin") != 1)
        {
            GetAllMainCategories();
            Clothdatabool = false;
            dataListOfItems = new List<TotaItemsClass>();
            PreviousSelectionCount = -1;
            StartCoroutine(WaitForInstance());
            if (AvatarSaved)
                AvatarSaved.SetActive(false);
            SetPresetValue();
        }

        if (ConstantsHolder.xanaConstants.screenType == ConstantsHolder.ScreenType.TabScreen)
        {
            for (int i = 0; i < panelsLayoutGroups.Count; i++)
            {
                panelsLayoutGroups[i].gameObject.AddComponent<AdjustGridLayoutCellSize>();
                panelsLayoutGroups[i].padding.left = 25;
                panelsLayoutGroups[i].padding.right = 25;
            }
            panelsLayoutGroups[panelsLayoutGroups.Count - 1].constraintCount = 4;
        }

        if (Defaultreset)
            Defaultreset.GetComponent<Button>().onClick.AddListener(delegate { Character_DefaultReset(true); });

        if (UndoBtn)
        {
            UndoBtn.GetComponent<Button>().onClick.AddListener(UndoStepBtn);
            UndoBtn.GetComponent<Button>().interactable = true;
        }
        if (RedoBtn)
        {
            RedoBtn.GetComponent<Button>().interactable = true;
            RedoBtn.GetComponent<Button>().onClick.AddListener(RedoStepBtn);
        }
        if (LastSavedreset)
        {
            LastSavedreset.GetComponent<Button>().onClick.AddListener(Character_ResettoLastSaved);
        }

    }

    public void SetDefaultValues() // This is called When comming back from Worlds
    {
        CheckAPILoaded = true;
        if (ArrayofMainCategories == null || ArrayofMainCategories.Length == 0)
            GetAllMainCategories();

        if (SubCategoriesList.Count == 0)
            GetAllSubCategories();

        // Update Save Btn Methods
        CheckWhenUserLogin();
    }


    private void OnEnable()
    {
       // MainSceneEventHandler.OnSucessFullLogin += CheckWhenUserLogin;
        upateAssetOnGenderChanged += ChangeItemsOnGenderChange;
    }
    private void OnDisable()
    {
      //  MainSceneEventHandler.OnSucessFullLogin -= CheckWhenUserLogin;
        upateAssetOnGenderChanged -= ChangeItemsOnGenderChange;
    }

    void ChangeItemsOnGenderChange()
    {
        CharacterHandler _charHandler = CharacterHandler.instance;
        for (int i = 0; i < AllCategoriesData.Count; i++)
        {
            if (AllCategoriesData[i].subItems.Count > 0)
            {
                for (int j = 0; j < AllCategoriesData[i].subItems.Count; j++)
                {
                    if ((_charHandler.activePlayerGender == AvatarGender.Male && AllCategoriesData[i].subItems[j].gender.Equals("0")) ||
                         (_charHandler.activePlayerGender == AvatarGender.Female && AllCategoriesData[i].subItems[j].gender.Equals("1")))
                    {
                        AllCategoriesData[i].subItems[j].obj.SetActive(true);
                    }
                    else
                    {
                        AllCategoriesData[i].subItems[j].obj.SetActive(false);
                    }
                }
            }
        }

        // Update Character Reference On Gender Change
        AvatarCustomizationManager.Instance.m_MainCharacter = GameManager.Instance.mainCharacter;
        AvatarCustomizationManager.Instance.f_MainCharacter = GameManager.Instance.mainCharacter;

        //ResetDownloadCount();
    }
    public void skipAvatarSelection()
    {
       // UserLoginSignupManager.instance.enterNamePanel.SetActive(true);
        _CanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
    }

    public void WalletLoggedinCall()
    {
        GetAllMainCategories();
        Clothdatabool = false;
        dataListOfItems = new List<TotaItemsClass>();
        PreviousSelectionCount = -1;
        CheckWhenUserLogin();
        if (AvatarSaved)
            AvatarSaved.SetActive(false);
        SetPresetValue();
    }
    // Using accessory panel as preset jsons
    void SetPresetValue()
    {
        if (PlayerPrefs.GetString("PresetValue") != "")
            ConstantsHolder.xanaConstants.PresetValueString = PlayerPrefs.GetString("PresetValue");
    }

    private void Update()
    {
        // Quick fix AKA ElFY
        SaveBtn();
    }
    public void SaveBtn()
    {
        Image image = SaveStoreBtn.GetComponent<Image>();
        Button button = SaveStoreBtn.GetComponent<Button>();

        if (image.color == Color.white)
            button.interactable = false;
        else
            button.interactable = true;

        //if (saveStoreBtnImage.color == Color.white)
        //    saveStoreBtnButton.interactable = false;
        //else
        //    saveStoreBtnButton.interactable = true;
    }
    public void CheckWhenUserLogin()
    {
        Button _storeSaveBtn = SaveStoreBtn.GetComponent<Button>();
        Button _panelSaveBtn = saveButton.GetComponent<Button>();

        _storeSaveBtn.onClick.RemoveAllListeners();
        _panelSaveBtn.onClick.RemoveAllListeners();
        newAvatarPresetBtn.onClick.RemoveAllListeners();
        //saveButton.GetComponent<Button>().onClick.RemoveAllListeners();
        //if (PlayerPrefs.GetInt("IsLoggedIn") == 1) // As Guest Functionality Removed, No need for this check anymore 
        //{

        //Debug.Log("################################### Assigning ");
        if (MultipleSave)
        {
            if (AvatarSelfie.instance != null)
            {
                _storeSaveBtn.onClick.AddListener(() => AvatarSelfie.instance.TakeScreenShootAndSaveData((IsSucess) => { }));
            }

            //Debug.Log("################################### Checking ");
            if (LoadPlayerAvatar.instance_loadplayer != null)
            {
                //Debug.Log("################################### Found ");
                _storeSaveBtn.onClick.AddListener(() => LoadPlayerAvatar.instance_loadplayer.OpenPlayerNamePanel());
            }

            if (AvatarSelfie.instance != null)
                newAvatarPresetBtn.onClick.AddListener(() => AvatarSelfie.instance.TakeScreenShootAndSaveData((IsSucess) => { }));
            if (LoadPlayerAvatar.instance_loadplayer != null)
                newAvatarPresetBtn.onClick.AddListener(() => LoadPlayerAvatar.instance_loadplayer.OpenPlayerNamePanel());

            _panelSaveBtn.GetComponent<Button>().onClick.AddListener(OnSaveBtnClicked);

        }
        else
        {
            _storeSaveBtn.onClick.AddListener(OnSaveBtnClicked);
        }
        GameManager.Instance.isTabSwitched = false;
        //}
        //else
        //{
        //    saveStoreBtnButton.onClick.AddListener(OnSaveBtnClicked);
        //}
    }


    IEnumerator WaitForInstance()
    {
        Button _storeSaveBtn = SaveStoreBtn.GetComponent<Button>();
        _storeSaveBtn.onClick.RemoveAllListeners();
        //Debug.Log("################################### Removed ");
        saveButton.GetComponent<Button>().onClick.RemoveAllListeners();
        yield return new WaitForSeconds(.1f);
        //SaveStoreBtn.GetComponent<Button>().onClick.AddListener(OnSaveBtnClicked);
        if (/*PlayerPrefs.GetInt("IsLoggedIn") == 1 &&*/ MultipleSave)
        {
            if (AvatarSelfie.instance != null)
            {
                _storeSaveBtn.onClick.AddListener(() => AvatarSelfie.instance.TakeScreenShootAndSaveData((IsSucess) => { }));
            }

            if (LoadPlayerAvatar.instance_loadplayer != null)
            {
                //Debug.Log("################################### Added ");
                _storeSaveBtn.onClick.AddListener(() => LoadPlayerAvatar.instance_loadplayer.OpenPlayerNamePanel());
            }
            saveButton.GetComponent<Button>().onClick.AddListener(OnSaveBtnClicked);
        }
        else
        {
            _storeSaveBtn.onClick.AddListener(OnSaveBtnClicked);
        }

        //if (Defaultreset)
        //    Defaultreset.GetComponent<Button>().onClick.AddListener(delegate { Character_DefaultReset(true); });

        //if (UndoBtn)
        //{
        //    UndoBtn.GetComponent<Button>().onClick.AddListener(UndoStepBtn);
        //    UndoBtn.GetComponent<Button>().interactable = true;
        //}
        //if (RedoBtn)
        //{
        //    RedoBtn.GetComponent<Button>().interactable = true;
        //    RedoBtn.GetComponent<Button>().onClick.AddListener(RedoStepBtn);
        //}
        //if (LastSavedreset)
        //{
        //    LastSavedreset.GetComponent<Button>().onClick.AddListener(Character_ResettoLastSaved);
        //}
        // backbutton_preset.GetComponent<Button>().onClick.AddListener(BackTrackPreset);
    }

    void Character_DefaultReset(bool clearData = true)
    {
        if (PlayerPrefs.GetInt("presetPanel") == 1)
            PlayerPrefs.SetInt("presetPanel", 0);

        PlayerPrefs.SetInt("Loaded", 0);
        //DefaultEnteriesforManican.instance.DefaultReset();

        ////Deleting Player data
        //LoadPlayerAvatar.instance_loadplayer.DeleteAvatarDataFromServer(ConstantsGod.AUTH_TOKEN, LoadPlayerAvatar.avatarId);
        ClearingLists(0);
        ClearingLists(1);
        ClearingLists(2);
        ClearingLists(7);
        if (clearData)
            ResetSaveFile();
        if (GameManager.Instance.mainCharacter.GetComponent<AvatarController>().wornEyeWearable != null)
        {
            GameManager.Instance.mainCharacter.GetComponent<AvatarController>().UnStichItem("EyeWearable");
        }
        UndoSelection();
        ConstantsHolder.xanaConstants._lastClickedBtn = null;
        ConstantsHolder.xanaConstants._curretClickedBtn = null;
        if (GameManager.Instance) // reseting body type
        {
            GameManager.Instance.mainCharacter.GetComponent<AvatarController>().ResizeClothToBodyFat(GameManager.Instance.mainCharacter.gameObject, 0);
        }

        GameManager.Instance.mainCharacter.GetComponent<CharacterBodyParts>().DefaultTexture();
        GameManager.Instance.mainCharacter.GetComponent<AvatarController>().UnStichItem("Feet");
        GameManager.Instance.mainCharacter.GetComponent<AvatarController>().InitializeAvatar();

        //GameManager.Instance.mainCharacter.GetComponent<Equipment>().SaveDefaultValues();
        //GameManager.Instance.mainCharacter.GetComponent<Equipment>().UpdateStoreList();
        //Comented By Talha For Default cloth showing
        Default_LastSaved_PanelDisabler();
        //PlayerPrefs.SetString("PresetValue", "");
        ConstantsHolder.xanaConstants.PresetValueString = null;
        PresetData_Jsons.clickname = "";
        UpdateXanaConstants();
        //DefaultClothDatabase.instance.GetComponent<SaveCharacterProperties>().SavePlayerProperties();
        UpdateStoreSelection(ConstantsHolder.xanaConstants.currentButtonIndex);
        //ConstantsHolder.xanaConstants._lastClickedBtn = null;

       // if (GameManager.Instance.eyesBlinking != null)          // Added by Ali Hamza 
           // GameManager.Instance.eyesBlinking.StoreBlendShapeValues();

        UnselectAllSelectedItemOnReset();
    }
    void Character_ResettoLastSaved()
    {
        UndoSelection();
        // PlayerPrefs.SetInt("Loaded", 0); 
        // apply last saved values from local

        if (PlayerPrefs.GetInt("presetPanel") == 1)
            PlayerPrefs.SetInt("presetPanel", 0);
        //GameManager.Instance.mainCharacter.GetComponent<AvatarController>().ResetForLastSaved();
        // DefaultEnteriesforManican.instance.ResetForPresets();
        //GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
        GameManager.Instance.mainCharacter.GetComponent<CharacterBodyParts>().DefaultTexture();
        GameManager.Instance.mainCharacter.GetComponent<AvatarController>().InitializeAvatar();

        //On merging from Release getting this error
        //GameManager.Instance.mainCharacter.GetComponent<DefaultEnteriesforManican>().DefaultReset_HAck();
        //SaveCharacterProperties.instance.LoadMorphsfromFile();
        Default_LastSaved_PanelDisabler();


        GreyRibbonImage.SetActive(true);
        WhiteRibbonImage.SetActive(false);
        SaveStoreBtn.GetComponent<Image>().color = Color.white;

        PresetData_Jsons test;
        if (FindObjectOfType<PresetData_Jsons>())
        {
            test = FindObjectOfType<PresetData_Jsons>();
            test.callit(); // = ""; // null;
        }// null;
         // DefaultEnteriesforManican.instance.LastSaved_Reset();
         // Default_LastSaved_PanelDisabler();
        ConstantsHolder.xanaConstants._lastClickedBtn = null;

        if (TempEnumVar == EnumClass.CategoryEnum.EyeBrowAvatar)
        {
            for (int i = 1; i < AllCategoriesData[10].parentObj.transform.childCount; i++) // AvatarEyeBrow
            {
                if (AllCategoriesData[10].parentObj.transform.GetChild(i).GetComponent<ItemDetail>().id.ParseToInt() == SaveCharacterProperties.instance.characterController.eyeBrowId)
                    AllCategoriesData[10].parentObj.transform.GetChild(i).GetComponent<Image>().enabled = true;
                else
                    AllCategoriesData[10].parentObj.transform.GetChild(i).GetComponent<Image>().enabled = false;
            }
        }
        else if (TempEnumVar == EnumClass.CategoryEnum.EyeLashesAvatar)
        {
            for (int i = 0; i < AllCategoriesData[11].parentObj.transform.childCount; i++) // EyeLashes
            {
                if (AllCategoriesData[11].parentObj.transform.GetChild(i).GetComponent<ItemDetail>().id.ParseToInt() == SaveCharacterProperties.instance.characterController.eyeLashesId)
                    AllCategoriesData[11].parentObj.transform.GetChild(i).GetComponent<Image>().enabled = true;
                else
                    AllCategoriesData[11].parentObj.transform.GetChild(i).GetComponent<Image>().enabled = false;
            }
        }

        UpdateXanaConstants();
        // 22 Custom Eyes -- 24 Custom Lips -- 26 Custom Skin
        if (AllCategoriesData[22].parentObj.activeInHierarchy || AllCategoriesData[24].parentObj.activeInHierarchy || AllCategoriesData[26].parentObj.gameObject.activeInHierarchy)
            UpdateColor(ConstantsHolder.xanaConstants.currentButtonIndex);
        else
            UpdateStoreSelection(ConstantsHolder.xanaConstants.currentButtonIndex);

       // if (GameManager.Instance.eyesBlinking != null)          // Added by Ali Hamza 
        //    GameManager.Instance.eyesBlinking.StoreBlendShapeValues();
    }


    public void DeletePreviousItems()
    {
        //Resources.UnloadUnusedAssets();
        //   Caching.ClearCache();

        if (AllCategoriesData[8].parentObj.transform.childCount >= 1) // Avatar - hair
        {
            //print("~~~~~~~ ParentOfBtnsAvatarHairs" + ParentOfBtnsAvatarHairs.childCount);
            for (int i = AllCategoriesData[8].parentObj.transform.childCount - 1; i >= 1; i--)
            {
                AssetCache.Instance.RemoveFromMemory(AllCategoriesData[8].parentObj.transform.GetChild(i).GetComponent<ItemDetail>().iconLink, true);
                Destroy(AllCategoriesData[8].parentObj.transform.GetChild(i).gameObject);
                //Resources.UnloadUnusedAssets();
            }
        }
        // Eyebrow has Customization Icon Donot delect index 0
        if (AllCategoriesData[10].parentObj.transform.childCount >= 2) // Avatar - Eyebrow
        {
            //print("~~~~~~~ ParentOfBtnsAvatarEyeBrows" + ParentOfBtnsAvatarEyeBrows.childCount);
            for (int i = AllCategoriesData[10].parentObj.transform.childCount - 1; i >= 2; i--)
            {
                AssetCache.Instance.RemoveFromMemory(AllCategoriesData[10].parentObj.transform.GetChild(i).GetComponent<ItemDetail>().iconLink, true);
                Destroy(AllCategoriesData[10].parentObj.transform.GetChild(i).gameObject);
                //Resources.UnloadUnusedAssets();
            }

        }
        if (AllCategoriesData[11].parentObj.transform.childCount >= 1) // Avatar EyeLashes
        {
            //print("~~~~~~~ ParentOfBtnsAvatarEyeLashes" + ParentOfBtnsAvatarEyeLashes.childCount);
            for (int i = AllCategoriesData[11].parentObj.transform.childCount - 1; i >= 0; i--)
            {
                AssetCache.Instance.RemoveFromMemory(AllCategoriesData[11].parentObj.transform.GetChild(i).GetComponent<ItemDetail>().iconLink, true);
                Destroy(AllCategoriesData[11].parentObj.transform.GetChild(i).gameObject);
                //Resources.UnloadUnusedAssets();
            }
        }

        //if (ParentOfBtnsAvatarEyes.childCount >= 1) // eyes
        //{
        //    print("~~~~~~~ ParentOfBtnsAvatarEyes" + ParentOfBtnsAvatarEyes.childCount);
        //    for (int i = ParentOfBtnsAvatarEyes.childCount - 1; i >= 0; i--)
        //    {
        //        Destroy(ParentOfBtnsAvatarEyes.GetChild(i).gameObject);
        //    }
        //}

        if (AllCategoriesData[16].parentObj.transform.childCount >= 1) // Avatar Skin
        {
            //print("~~~~~~~ ParentOfBtnsAvatarSkin" + ParentOfBtnsAvatarSkin.childCount);
            for (int i = AllCategoriesData[16].parentObj.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(AllCategoriesData[16].parentObj.transform.GetChild(i).gameObject);
                //Resources.UnloadUnusedAssets();
            }
        }
        if (AllCategoriesData[5].parentObj.transform.childCount >= 1) // bottom
        {
            //print("~~~~~~~ ParentOfBtnsForBottom" + ParentOfBtnsForBottom.childCount);
            for (int i = AllCategoriesData[5].parentObj.transform.childCount - 1; i >= 0; i--)
            {
                AssetCache.Instance.RemoveFromMemory(AllCategoriesData[5].parentObj.transform.GetChild(i).GetComponent<ItemDetail>().iconLink, true);
                Destroy(AllCategoriesData[5].parentObj.transform.GetChild(i).gameObject);
                //Resources.UnloadUnusedAssets();
            }
        }
        if (AllCategoriesData[7].parentObj.transform.childCount >= 1) // Shoes
        {
            //print("~~~~~~~ ParentOfBtnsForShoes" + ParentOfBtnsForShoes.childCount);
            for (int i = AllCategoriesData[7].parentObj.transform.childCount - 1; i >= 0; i--)
            {
                AssetCache.Instance.RemoveFromMemory(AllCategoriesData[7].parentObj.transform.GetChild(i).GetComponent<ItemDetail>().iconLink, true);
                Destroy(AllCategoriesData[7].parentObj.transform.GetChild(i).gameObject);
                //Resources.UnloadUnusedAssets();
            }
        }
        if (AllCategoriesData[3].parentObj.transform.childCount >= 1) // Outer
        {
            //print("~~~~~~~ ParentOfBtnsForOuter" + ParentOfBtnsForOuter.childCount);
            for (int i = AllCategoriesData[3].parentObj.transform.childCount - 1; i >= 0; i--)
            {
                AssetCache.Instance.RemoveFromMemory(AllCategoriesData[3].parentObj.transform.GetChild(i).GetComponent<ItemDetail>().iconLink, true);
                Destroy(AllCategoriesData[3].parentObj.transform.GetChild(i).gameObject);
                //Resources.UnloadUnusedAssets();
            }
        }

        // Cleared Stored References
        for (int i = 0; i < AllCategoriesData.Count; i++)
        {
            AllCategoriesData[i].subItems.Clear();
        }

        ResetDownloadCount();
        ResetPageIndex();

       // if (LoadingHandler.Instance)
         //   LoadingHandler.Instance.storeLoadingScreen.SetActive(false);

    }

    void ResetDownloadCount()
    {
        headsDownlaodedCount = 0;
        faceDownlaodedCount = 0;

        innerDownlaodedCount = 0;
        innerDownlaodedCountFemale = 0;

        outerDownlaodedCount = 0;
        outerDownlaodedCountFemale = 0;

        accesaryDownlaodedCount = 0;

        bottomDownlaodedCount = 0;
        bottomDownlaodedCountFemale = 0;

        socksDownlaodedCount = 0;

        shoesDownlaodedCount = 0;
        shoesDownlaodedCountFemale = 0;

        hairDwonloadedCount = 0;
        hairDwonloadedCountFemale = 0;

        LipsColorDwonloadedCount = 0;
        EyesColorDwonloadedCount = 0;
        EyeBrowColorDwonloadedCount = 0;
        HairColorDwonloadedCount = 0;
        skinColorDwonloadedCount = 0;
        eyeBrowDwonloadedCount = 0;
        eyeBrowColorDwonloadedCount = 0;
        eyeLashesDwonloadedCount = 0;
        eyesDwonloadedCount = 0;
        lipsDwonloadedCount = 0;
    }
    void ClearingLists(int index)
    {
        //AssetBundle.UnloadAllAssetBundles(false);
        //Resources.UnloadUnusedAssets();

        DefaultClothDatabase itemsData = DefaultClothDatabase.instance;
        //Equipment equipment = GameManager.Instance.mainCharacter.GetComponent<Equipment>();
        itemsData.itemList[index].ItemName = "";
        //itemsData.itemList[index].ItemID =0;
        //itemsData.itemList[index].ItemLinkAndroid= "";
        //itemsData.itemList[index].ItemLinkIOS = "";

        //equipment.equippedItems[index].ItemName = "";
        //equipment.equippedItems[index].ItemID = 0;
        //equipment.equippedItems[index].ItemLinkAndroid = "";
        //equipment.equippedItems[index].ItemLinkIOS = "";
    }

    IEnumerator StoreSelection()
    {
        //AssetBundle.UnloadAllAssetBundles(false);
        //Resources.UnloadUnusedAssets();

        yield return new WaitForSeconds(0.5f);

        UpdateStoreSelection(ConstantsHolder.xanaConstants.currentButtonIndex);
    }
    void Default_LastSaved_PanelDisabler()
    {
        PanelResetDefault.SetActive(false);
    }

    public void OnSaveBtnClicked()
    {
        //AssetBundle.UnloadAllAssetBundles(false);
        //Resources.UnloadUnusedAssets();

       // Debug.LogError("##############Save Btn CLicked");
        if (DefaultClothDatabase.instance.gameObject != null)
        {
            //  print("ppp+");
            if (PlayerPrefs.GetInt("presetPanel") == 1)
            {   // preset panel is enable so saving preset to account 
                PlayerPrefs.SetInt("presetPanel", 0);
            }
            //if (PresetData_Jsons.lastSelectedPreset)
            //{
            //    PlayerPrefs.SetString("PresetValue", PresetData_Jsons.lastSelectedPreset.name);
            //    ConstantsHolder.xanaConstants.PresetValueString = PlayerPrefs.GetString("PresetValue");
            //}
            PlayerPrefs.SetInt("Loaded", 1);
            if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
            {
                if (InventoryManager.instance.MultipleSave)
                {
                    AvatarSaved.SetActive(true);
                }
                else
                {
                    AvatarSaved.SetActive(false);
                    AvatarSavedGuest.SetActive(true);
                }
            }
            else
            {
                if (!isSaveFromreturnHomePopUp)
                {
                    AvatarSavedGuest.SetActive(true);
                }
            }
            DefaultClothDatabase.instance.GetComponent<SaveCharacterProperties>().SavePlayerProperties();
            //GameManager.Instance.mainCharacter.GetComponent<Equipment>().UpdateStoreList();
            saveButtonPressed = true;
            ResetMorphBooleanValues();

        }
    }
    /// <New APIS>
    IEnumerator WaitForAPICallCompleted(int m_GetIndex)
    {
        //print("wait Until");
        yield return new WaitUntil(() => CheckAPILoaded == true);
        if (CheckAPILoaded)
        {
            //print("wait Until completed");
            if (SubCategoriesList.Count > 0)
            {
                SubmitAllItemswithSpecificSubCategory(SubCategoriesList[m_GetIndex].id, false);
            }
        }
    }
    // **************************** Get Items by Sub categories ******************************//
    private string StringIndexofSubcategories(int _index)
    {
        var result = string.Join(",", _index.ToString());
        result = "[" + result + "]";
        return result;
    }
    [System.Serializable]
    public class ConvertSubCategoriesToJsonObj
    {
        public string subCategories;
        public int pageNumber;
        public int pageSize;
        public string order;
        public string sort;
        public int gender;
        public ConvertSubCategoriesToJsonObj CreateTOJSON(string jsonString, int _pageNumber, int _PageSize)
        {
            ConvertSubCategoriesToJsonObj myObj = new ConvertSubCategoriesToJsonObj();
            myObj.subCategories = jsonString;
            myObj.pageNumber = _pageNumber;
            myObj.pageSize = _PageSize;
            return myObj;
        }
        public ConvertSubCategoriesToJsonObj CreateTOJSON(string jsonString, int _pageNumber, int _PageSize, string _order)
        {
            ConvertSubCategoriesToJsonObj myObj = new ConvertSubCategoriesToJsonObj();
            myObj.subCategories = jsonString;
            myObj.pageNumber = _pageNumber;
            myObj.pageSize = _PageSize;
            myObj.order = _order;
            return myObj;
        }
        public ConvertSubCategoriesToJsonObj CreateTOJSON(string jsonString, int _pageNumber, int _PageSize, string _order, string sortingType)
        {
            ConvertSubCategoriesToJsonObj myObj = new ConvertSubCategoriesToJsonObj();
            myObj.subCategories = jsonString;
            myObj.pageNumber = _pageNumber;
            myObj.pageSize = _PageSize;
            myObj.order = _order;
            myObj.sort = sortingType;

            if (CharacterHandler.instance.activePlayerGender == AvatarGender.Male)
                myObj.gender = 0;
            else
                myObj.gender = 1;
            return myObj;
        }
    }
    public void SubmitAllItemswithSpecificSubCategory(int GetCategoryIndex, bool check)
    {
        bool Once;
        Once = check;
        if (PreviousSelectionCount != IndexofPanel)
        {
            PreviousSelectionCount = IndexofPanel;
            Once = true;
        }
        else
        {
            //Debug.Log("<color=red> Same Button Clicking </color>");
        }
        if (Once || loadingItems)
        {
            string result = StringIndexofSubcategories(GetCategoryIndex);
            int _pagenumber = GetActivePanelPageIndex();
            ConvertSubCategoriesToJsonObj SubCatString = new ConvertSubCategoriesToJsonObj();
            string bodyJson = JsonUtility.ToJson(SubCatString.CreateTOJSON(result, _pagenumber, 40, "asc", "name")); // API Update New Parameter added for sorting
            if (hitAllItemAPICorountine != null)
                StopCoroutine(hitAllItemAPICorountine);
            hitAllItemAPICorountine = StartCoroutine(HitALLItemsAPI(ConstantsGod.API_BASEURL + ConstantsGod.GETALLSTOREITEMS, bodyJson));
        }
    }
    public bool loadingItems = false;
    bool _ResettingAssetList = false;

    Coroutine itemLoading, hitAllItemAPICorountine, _generateCoroutin;
    IEnumerator HitALLItemsAPI(string url, string Jsondata)
    {
        if (apiResponseHolder.CheckResponse(url + Jsondata))
        {
            GetItemInfoNewAPI JsonDataObj1 = new GetItemInfoNewAPI();
            JsonDataObj1 = JsonUtility.FromJson<GetItemInfoNewAPI>(apiResponseHolder.GetResponse(url + Jsondata));
            dataListOfItems.Clear();

            _ResettingAssetList = true;
            yield return new WaitForSeconds(0.2f);

            dataListOfItems = JsonDataObj1.data[0].items;
            PutDataInOurAPPNewAPI();
            CheckAPILoaded = true; // Already Have the response not calling the API -- Resetting the value
            yield break;
        }
        //Debug.Log("<color=red> HitALLItemsAPI </color>");
        //if (LoadingHandler.Instance)
          //  LoadingHandler.Instance.storeLoadingScreen.SetActive(true);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        if (ConstantsHolder.loggedIn)
        {
            request.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
        }
        else
        {
            request.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
        }
        request.SendWebRequest();
        while (!request.isDone)
        {
            yield return null;
        }
        GetItemInfoNewAPI JsonDataObj = new GetItemInfoNewAPI();
        JsonDataObj = JsonUtility.FromJson<GetItemInfoNewAPI>(request.downloadHandler.text);

        //AssetBundle.UnloadAllAssetBundles(false);
        //Resources.UnloadUnusedAssets();
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                if (JsonDataObj.success == true)
                {
                    dataListOfItems.Clear();

                    _ResettingAssetList = true;
                    yield return new WaitForSeconds(0.2f);

                    dataListOfItems = JsonDataObj.data[0].items;

                    if (dataListOfItems.Count == 0)
                    {
                        UpdateActivePanelPageIndex(false);
                        yield break;
                    }

                    PutDataInOurAPPNewAPI();
                    apiResponseHolder.AddReponse(url + Jsondata, request.downloadHandler.text);
                  //  if (LoadingHandler.Instance)
                       // LoadingHandler.Instance.storeLoadingScreen.SetActive(false);
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                print("Network Error");
            }
            else
            {
                if (request.error != null)
                {
                    if (JsonDataObj.success == false)
                    {
                        //print("Hey success false " + JsonDataObj.msg);
                    }
                }
            }
            CheckAPILoaded = true;
        }
        request.Dispose();
    }



    public void CheckPageNumberForAssets()
    {
        ScrollRect myScroller = GetActiveRect();
        //Debug.LogError("Scroller Value : " + myScroller.verticalNormalizedPosition);
        if (loadingItems)
            return;

       // Debug.Log("Scrol GameoBJECT = " + myScroller.gameObject.name + "   content count" + myScroller.content.childCount + "Current Pannel " + GetActivePanelPageIndex());
        if (myScroller.verticalNormalizedPosition <= 0.1f && (myScroller.content.childCount == GetActivePanelPageIndex() * 40 || myScroller.content.childCount == ((GetActivePanelPageIndex() * 40 ) + 1 ))) // Color Object is also added in the list
        {
            loadingItems = true;
            int pageIndex = UpdateActivePanelPageIndex();
            int _downloadedAssetCount = UpdateActivePanelPageIndex(false, true);
            int _ActivePanelIndex = GetActivePanelIndex();

            // From API we get 40 Assets per page
            // This check is write to avoid multiple call to API while loading previous once
            if (_downloadedAssetCount % 40 != 0)
            {
                //Debug.Log("<color=red>Items Are Downloading</color>");
                return;
            }

            if (_downloadedAssetCount == -1)
            {
                loadingItems = false;
                return;
            }

            if (pageIndex < 0)
                return;

            SubmitAllItemswithSpecificSubCategory(SubCategoriesList[_ActivePanelIndex].id, false);
        }
        else
        {

        }
    }

    private ScrollRect GetActiveRect()
    {
        int _selectedPanel = ConstantsHolder.xanaConstants.currentButtonIndex;

        if (Clothdatabool)
        {
            if (_selectedPanel == 3) return ItemsScrollrect[1];  // Outer
            if (_selectedPanel == 5) return ItemsScrollrect[2]; // Bottom
            if (_selectedPanel == 7) return ItemsScrollrect[3]; // Shoes
        }
        else
        {
            return ItemsScrollrect[0]; // Hair Scroll Rect
        }

        return GetComponent<ScrollRect>();
    }

    private int GetActivePanelIndex()
    {
        int _selectedPanel = ConstantsHolder.xanaConstants.currentButtonIndex;
        if (Clothdatabool)
        {
            if (_selectedPanel == 3) return 3;  // Outer
            if (_selectedPanel == 5) return 5; // Bottom
            if (_selectedPanel == 7) return 7; // Shoes
            else
                return -1;
        }
        else
        {
            return 8; // Hair
        }

    }
    private int GetActivePanelPageIndex()
    {
        int _selectedPanel = ConstantsHolder.xanaConstants.currentButtonIndex;
        //Debug.Log("Cloths Bool " + Clothdatabool + _selectedPanel);
        if (Clothdatabool)
        {
            if (_selectedPanel == 3) return _OuterApiPagaCount;  // Outer
            if (_selectedPanel == 5) return _BottomApiPagaCount; // Bottom
            if (_selectedPanel == 7) return _ShoesApiPagaCount; // Shoes

            return 1;
        }
        else
        {
            return _HairApiPagaCount;
        }
    }
    private int UpdateActivePanelPageIndex(bool addValue = true, bool downloadCount = false)
    {
        int _selectedPanel = ConstantsHolder.xanaConstants.currentButtonIndex;


        //Debug.LogError("Selected Panel Index: " + _selectedPanel);

        if (downloadCount)
        {
            if (Clothdatabool)
            {
                if (_selectedPanel == 3) { return GetDownloadedNumber(EnumClass.CategoryEnum.Outer); } // Outer
                if (_selectedPanel == 5) { return GetDownloadedNumber(EnumClass.CategoryEnum.Bottom); } // Bottom
                if (_selectedPanel == 7) { return GetDownloadedNumber(EnumClass.CategoryEnum.Shoes); }// Shoes

                return -1; ;
            }
            else if (_selectedPanel == 0)
            {
                return GetDownloadedNumber(EnumClass.CategoryEnum.HairAvatar);
            }
            else
            {
                return -1;
            }
        }
        else if (addValue)
        {
            if (Clothdatabool)
            {
                if (_selectedPanel == 3) { _OuterApiPagaCount += 1; return _OuterApiPagaCount; } // Outer
                if (_selectedPanel == 5) { _BottomApiPagaCount += 1; return _BottomApiPagaCount; } // Bottom
                if (_selectedPanel == 7) { _ShoesApiPagaCount += 1; return _ShoesApiPagaCount; }// Shoes

                return -1;
            }
            else if (_selectedPanel == 0)
            {
                _HairApiPagaCount += 1;
                return _HairApiPagaCount; ;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            if (Clothdatabool)
            {
                if (_selectedPanel == 3) { _OuterApiPagaCount -= 1; return _selectedPanel; } // Outer
                if (_selectedPanel == 5) { _BottomApiPagaCount -= 1; return _selectedPanel; } // Bottom
                if (_selectedPanel == 7) { _ShoesApiPagaCount -= 1; return _selectedPanel; }// Shoes

                return -1; ;
            }
            else if (_selectedPanel == 0)
            {
                _HairApiPagaCount -= 1;
                return 8;
            }
            else
            {
                return -1;
            }
        }
    }
    private void ResetPageIndex()
    {
        //print("Reset Index....");
        _BottomApiPagaCount = 1;
        _OuterApiPagaCount = 1;
        _ShoesApiPagaCount = 1;
        _HairApiPagaCount = 1;
    }




    [System.Serializable]
    public class GetItemInfoNewAPI
    {
        public bool success;
        public List<DataList> data;
        public string msg;
    }
    [System.Serializable]
    public class DataList
    {
        public int id;
        public int categoryId;
        public string name;
        public string createdAt;
        public string updatedAt;
        public List<TotaItemsClass> items;
    }

    [System.Serializable]
    public class TotaItemsClass
    {
        public int id;
        public string assetLinkAndroid;
        public string assetLinkIos;
        public string assetLinkWindows; //assetGender
        public string assetGender; //
        public string iconLink;
        public int categoryId;
        public int subCategoryId;
        public string name;
        public bool isPaid;
        public string price;
        public bool isPurchased;
        public bool isFavourite;
        public bool isOccupied;
        public bool isDeleted;
        public string createdBy;
        public string createdAt;
        public string updatedAt;
        public string[] itemTags;
    }
    // **************************** Get Items by Sub categories ENDSSSSS ******************************//

    // **************************************************************************************************//
    ////////////////////////// <MAIN Category Started Here> ///////////////////////////////////////////////
    public void GetAllMainCategories()
    {
        StartCoroutine(HitAllMainCategoriesAPI(ConstantsGod.API_BASEURL + ConstantsGod.GETALLSTOREITEMCATEGORY, ""));
    }
    IEnumerator HitAllMainCategoriesAPI(string url, string Jsondata)
    {
        while (ConstantsGod.AUTH_TOKEN.Equals("AUTH_TOKEN"))
        {
            yield return new WaitForSecondsRealtime(1f);
        }

        if (apiResponseHolder.CheckResponse(url + Jsondata))
        {
            string res = apiResponseHolder.GetResponse(url + Jsondata);
            ObjofMainCategory = GetAllDataNewAPI(res);
            SaveAllMainCategoriesToArray();
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
            request.SendWebRequest();
            while (!request.isDone)
            {
                yield return null;
            }
            ObjofMainCategory = GetAllDataNewAPI(request.downloadHandler.text);
            //Debug.LogError(request.downloadHandler.text);
            if (!request.isHttpError && !request.isNetworkError)
            {
                if (request.error == null)
                {
                    if (ObjofMainCategory.success == true)
                    {
                        SaveAllMainCategoriesToArray();
                        apiResponseHolder.AddReponse(url + Jsondata, request.downloadHandler.text);
                        //Debug.LogError(request.downloadHandler.text);
                    }
                }
            }
            else
            {
                if (request.isNetworkError)
                {
                    print("Network Error");
                }
                else
                {
                    if (request.error != null)
                    {
                        if (ObjofMainCategory.success == false)
                        {
                            //print("Hey success false " + ObjofMainCategory.msg);
                        }
                    }
                }
            }
            request.Dispose();
        }
    }
    public void SaveAllMainCategoriesToArray()
    {
        List<string> purchaseItemsIDs = new List<string>();
        for (int i = 0; i < ObjofMainCategory.data.Count; i++)
        {
            purchaseItemsIDs.Add(ObjofMainCategory.data[i].id);
        }
        ArrayofMainCategories = purchaseItemsIDs.ToArray();
        //AssetBundle.UnloadAllAssetBundles(false);
        //Resources.UnloadUnusedAssets();

        GetAllSubCategories();
    }

    public GetAllInfoMainCategories GetAllDataNewAPI(string m_JsonData)
    {
        GetAllInfoMainCategories JsonDataObj = new GetAllInfoMainCategories();
        JsonDataObj = JsonUtility.FromJson<GetAllInfoMainCategories>(m_JsonData);
        return JsonDataObj;
    }
    [System.Serializable]
    public class GetAllInfoMainCategories
    {
        public bool success;
        public List<ItemsParentsNewAPI> data;
        public string msg;
    }
    [System.Serializable]
    public class ItemsParentsNewAPI
    {
        public string id;
        public string name;
        public string createdAt;
        public string updatedAt;
    }
    ////////////////////////// <MAIN Category ENDS here> ///////////////////////////////////////////////

    ///////////////////////// ************************** //////////////////////////////////////////////
    ////////////////////////// <SUB Category STARTS here> ///////////////////////////////////////////////
    private string AccessIndexOfSpecificCategory()
    {
        if (ArrayofMainCategories == null || ArrayofMainCategories.Length == 0)
            return "";

        var result = string.Join(",", ArrayofMainCategories);
        result = "[" + result + "]";
        return result;
    }
    [System.Serializable]
    public class ConvertMainCat_Index_ToJson
    {
        public string categories;
        public ConvertMainCat_Index_ToJson CreateTOJSON(string jsonString)
        {
            ConvertMainCat_Index_ToJson myObj = new ConvertMainCat_Index_ToJson();
            myObj.categories = jsonString;
            return myObj;
        }
    }
    public void GetAllSubCategories()
    {
        string result = AccessIndexOfSpecificCategory();

        if (string.IsNullOrEmpty(result))
        {
            //Debug.Log("<color=red> ArrayofMainCategories is null or empty </color>");
            return;
        }
        ConvertMainCat_Index_ToJson MainCatString = new ConvertMainCat_Index_ToJson();
        string bodyJson = JsonUtility.ToJson(MainCatString.CreateTOJSON(result));
        StartCoroutine(HitSUBCategoriesAPI(ConstantsGod.API_BASEURL + ConstantsGod.GETALLSTOREITEMSUBCATEGORY, bodyJson));
    }

    IEnumerator HitSUBCategoriesAPI(string url, string Jsondata)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
        request.SendWebRequest();
        while (!request.isDone)
        {
            yield return null;
        }
        GetAllInfoSUBOFCategories JsonDataObj = new GetAllInfoSUBOFCategories();
        JsonDataObj = GetDataofSUBCategories(request.downloadHandler.text);
        //AssetBundle.UnloadAllAssetBundles(false);
        //Resources.UnloadUnusedAssets();
        //Debug.LogError("all sub categories :- " + request.downloadHandler.text);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                if (JsonDataObj.success == true)
                {
                    SubCategoriesList = JsonDataObj.data;
                    CheckAPILoaded = true;
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                CheckAPILoaded = true;
            }
            else
            {
                if (request.error != null)
                {
                    if (JsonDataObj.success == false)
                    {
                        CheckAPILoaded = true;
                    }
                }
            }
        }
        request.Dispose();
    }
    public GetAllInfoSUBOFCategories GetDataofSUBCategories(string m_JsonData)
    {
        GetAllInfoSUBOFCategories JsonDataObj = new GetAllInfoSUBOFCategories();
        JsonDataObj = JsonUtility.FromJson<GetAllInfoSUBOFCategories>(m_JsonData);
        return JsonDataObj;
    }

    [System.Serializable]
    public class GetAllInfoSUBOFCategories
    {
        public bool success;
        public List<ItemsofSubCategories> data;
        public string msg;
    }
    [System.Serializable]
    public class ItemsofSubCategories
    {
        public int id;
        public int categoryId;
        public string name;
        public string createdAt;
        public string updatedAt;
    }
    //***************************** Get All Sub Categories END here **************************//

    /// </END APIS>

    public void GetDataofGuestUser()
    {
        BtnsPanelAvatar.GetComponent<SubBottons>().ClothBool = false;
        BtnsPanelAvatar.GetComponent<SubBottons>().AvatarBool = true;
        // When Comming form home then set last panel to -1
        PreviousSelectionCount = -1;
        ConstantsHolder.xanaConstants.currentButtonIndex = 0;
        BtnsPanelAvatar.GetComponent<SubBottons>().ClickBtnFtn(0);
        ////Debug.Log("Store hair data call====");
        SelectPanel(1);
        PlayerPrefs.SetInt("TotalCoins", 0);
        //UpdateUserCoins();
        if (!GameManager.Instance.OnceGuestBool)
        {
            RefreshDefault();
            GameManager.Instance.OnceGuestBool = true;
        }
        BuyCountertxt.text = "0";
    }
    public void GetDataAfterLogin()
    {
        // When Comming form home then set last panel to -1
        PreviousSelectionCount = -1;
        ConstantsHolder.xanaConstants.currentButtonIndex = 0;

        if (!GameManager.Instance.BottomAvatarButtonBool)
        {
            //BtnsPanelCloth.GetComponent<SubBottons>().ClickBtnFtn(0);
            //BtnsPanelAvatar.GetComponent<SubBottons>().ClickBtnFtn(0);
            SelectPanel(1);
        }
        else
        {
            //BtnsPanelCloth.GetComponent<SubBottons>().ClickBtnFtn(3);
            SelectPanel(0);
        }
        SubmitUserDetailAPI();
        if (!GameManager.Instance.OnceLoginBool)
        {
            RefreshDefault();
            GameManager.Instance.OnceLoginBool = true;
        }
        BuyCountertxt.text = "0";
    }
    public void SignUpAndLoginPanel(int TakeString)
    {
        //Debug.Log("<color=red> OpenPanelIndex:::" + TakeString + "</color>");
        switch (TakeString)
        {
            case 0:
                {
                    StoreItemsPanel.SetActive(false);
                    ShowSignUpPanel.SetActive(false);
                    GameManager.Instance.BGPlane.SetActive(false);
                    GameManager.Instance.UiManager.HomePage.SetActive(true);
                   // UserLoginSignupManager.instance.OpenUIPanel(6);
                    break;
                }
            case 1:
                {
                    StoreItemsPanel.SetActive(false);
                    ShowSignUpPanel.SetActive(false);
                    GameManager.Instance.BGPlane.SetActive(false);
                    GameManager.Instance.UiManager.HomePage.SetActive(true);
                   // UserLoginSignupManager.instance.OpenUIPanel(1);
                    break;
                }
            case 2:
                {
                    OpenMainPanel("StoreItemsPanel");
                    UndoSelection();
                    GetDataofGuestUser();
                    GameManager.Instance.ShadowPlane.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, 0.3137f));
                    break;
                }
            case 3:
                {
                    OpenMainPanel("StoreItemsPanel");

                    GetDataAfterLogin();
                    if (PlayerPrefs.GetInt("IsLoggedIn") == 1 && MultipleSave)
                    {
                        myAvatarButton.gameObject.SetActive(true);
                    }
                    GameManager.Instance.BottomAvatarButtonBool = false;
                    break;
                }
        }
    }
    public void OpenMainPanel(string TakePanel)
    {
        StoreItemsPanel.SetActive(false);
        //CheckOutBuyItemPanel.SetActive(false);
        ShowSignUpPanel.SetActive(false);
        //LowCoinsPanel.SetActive(false);
        //ShopBuyCoinsPanel.SetActive(false);
        switch (TakePanel)
        {
            case "StoreItemsPanel":
                {
                    StoreItemsPanel.SetActive(true);

                    break;
                }
            case "CheckOutBuyItemPanel":
                {
                    //CheckOutBuyItemPanel.SetActive(true);
                    break;
                }
            case "ShowSignUpPanel":
                {
                    StoreItemsPanel.SetActive(true);
                    ShowSignUpPanel.SetActive(true);
                    break;
                }
            case "LowCoinsPanel":
                {
                    //CheckOutBuyItemPanel.SetActive(true);
                    //LowCoinsPanel.SetActive(true);
                    break;
                }
            case "ShopBuyCoinsPanel":
                {
                    //ShopBuyCoinsPanel.SetActive(true);
                    break;
                }
        }

        Invoke("DelayAction", 0.2f);
    }

    void DelayAction()
    {
        if (storeOpen != null)                      // AR changes store open event
            storeOpen.Invoke();                     // call store open event
    }

    //private void OnDisable()
    //{
    //    Resources.UnloadUnusedAssets();
    //}

    //private void OnDestroy()
    //{
    //    Resources.UnloadUnusedAssets();
    //}
    public void BackToHomeFromCharCustomization()
    {

        // if user is getting back fromaccessory panel/preset panel
        if (PlayerPrefs.GetInt("presetPanel") == 1)
        {

            //  if (GameManager.Instance.UserStatus_)
            PlayerPrefs.SetInt("presetPanel", 0);  // was loggedin as account 
                                                   //  else
                                                   //     PlayerPrefs.SetInt("IsLoggedIn", 0);  // was as a guest
                                                   //SaveCharacterProperties.instance.LoadMorphsfromFile();

        }
        //PresetData_Jsons.lastSelectedPresetName = null;
        //ConstantsHolder.xanaConstants.PresetValueString = PlayerPrefs.GetString("PresetValue");

        GameManager.Instance.mainCharacter.GetComponent<Animator>().SetBool("Customization", false);

       // PatchForStore.isCustomizationPanelOpen = false;
        GreyRibbonImage.SetActive(true);
        WhiteRibbonImage.SetActive(false);
        SaveStoreBtn.GetComponent<Image>().color = Color.white;
        GameManager.Instance.mainCharacter.GetComponent<AvatarController>().InitializeAvatar();
        saveButtonPressed = true;
        AvatarCustomizationUIHandler.Instance.LoadMyClothCustomizationPanel();
        GameManager.Instance.ShadowPlane.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, 0.7843f));

        //SaveCharacterProperties.instance.LoadMorphsfromFile();
        if (DefaultClothDatabase.instance != null)
        {
            DefaultClothDatabase.instance.RevertSavedCloths();

            UpdateXanaConstants();
            SaveCharacterProperties.instance.AssignCustomSlidersData();
            SaveCharacterProperties.instance.AssignSavedPresets();
            //GameManager.Instance.BlendShapeManager.DismissPoints();

            GameManager.Instance.BackFromStoreofCharacterCustom();
            MainPanelCloth.SetActive(false);
            StoreItemsPanel.SetActive(false);
            UndoSelection();
            //UndoRedo.undoRedo.undoRedoList.DestroyActionWithParameters(UndoRedo.undoRedo.undoRedoList);
            StoreUndoRedo.obj.DestroyList();
            DeletePreviousItems();

        }
        BackToMain();
        ResetPageIndex();
        //Transform parentEyeBrowAvatar = ParentOfBtnsAvatarEyeBrows;                 // AH Working
        //if (parentEyeBrowAvatar.childCount > 1)
        //{
        //    for (int i = 1; i < parentEyeBrowAvatar.childCount; i++)
        //        parentEyeBrowAvatar.GetChild(i).GetComponent<Image>().enabled = false;
        //}
        //Transform parentEyelashesAvatar = ParentOfBtnsAvatarEyeLashes;                 // AH Working
        //if (parentEyelashesAvatar.childCount > 1)
        //{
        //    for (int i = 0; i < parentEyelashesAvatar.childCount; i++)
        //        parentEyelashesAvatar.GetChild(i).GetComponent<Image>().enabled = false;
        //}

        //DeletePreviousItems();

    }

    public void OnClickRetunHomePopUpBackButton()
    {
        ReturnHomePopUp.SetActive(false);
    }
    public void OnClickBackButton()
    {
        //GameManager.Instance.mainCharacter.GetComponent<FaceIK>().ikActive= true;
        // GameManager.Instance.ActorManager.IdlePlayerAvatorForMenu(false);
        //  GameManager.Instance.userAnimationPostFeature.GetComponent<UserPostFeature>().ActivatePostButtbleHome(true);

        eyeBrowsColorButton.gameObject.SetActive(false);
        hairColorButton.gameObject.SetActive(false);
        if(GameManager.Instance.UiManager)
        GameManager.Instance.UiManager.ShowFooter(true);
        if (SaveStoreBtn.GetComponent<Button>().interactable == true)
            ReturnHomePopUp.SetActive(true);
        else
            OnClickHomeButton();

        OnScreenTabStateChange?.Invoke(BackButtonHandler.screenTabs.Hometab);
    }

    public void OnClickHomeButton()
    {
        //  GameManager.Instance.mainCharacter.GetComponent<AvatarControllerHome>().UpdateState(false);
        ConstantsHolder.xanaConstants.isStoreActive = false;
        ConstantsHolder.xanaConstants._lastClickedBtn = null;
        PresetData_Jsons.clickname = "";
        isSaveFromreturnHomePopUp = false;
        ReturnHomePopUp.SetActive(false);
        AvatarUpdated.SetActive(false);
        BackToHomeFromCharCustomization();

    }
    public bool isSaveFromreturnHomePopUp;
    public void OnClickSaveAvatarButton()
    {
        isSaveFromreturnHomePopUp = true;
        ReturnHomeLoader returnHomeLoader = ReturnHomePopUp.GetComponent<ReturnHomeLoader>();
        returnHomeLoader.saveloader.SetActive(true);
        returnHomeLoader.saveButton.enabled = false;
        returnHomeLoader.closeButton.enabled = false;
        returnHomeLoader.homeButton.enabled = false;
        if (AvatarSelfie.instance != null) //this will take screenshot of character and automatically save avatar to server.
            AvatarSelfie.instance.TakeScreenShootAndSaveData((IsSucess) =>
            {
                if (IsSucess)
                    OnSaveBtnClicked();
                else
                    OnSaveBtnClicked(); //save avatar without thumbnail images this is for guest user.
                ReturnHomePopUp.GetComponent<ReturnHomeLoader>().saveloader.SetActive(false);
                ReturnHomePopUp.GetComponent<ReturnHomeLoader>().saveButton.enabled = true;
                ReturnHomePopUp.GetComponent<ReturnHomeLoader>().closeButton.enabled = true;
                ReturnHomePopUp.GetComponent<ReturnHomeLoader>().homeButton.enabled = true;
                ReturnHomePopUp.SetActive(false);
            });
        //commented invoke to shorten save flow as per nakamoto suggestion 
        //saveStoreBtnButton.onClick.Invoke();
        //LoadPlayerAvatar.instance_loadplayer.UpdateExistingUserData();

    }

    public void SelectPanel(int TakeIndex)
    {
        ////Debug.Log("<color=red> Panel Index:" + TakeIndex + "</color>");
        panelIndex = TakeIndex;
        ResetPageIndex();
        //  InventoryManager.instance.DeletePreviousItems();
        //Resources.UnloadUnusedAssets();



        if (TakeIndex == 0)
        {
            ////Debug.LogError("<color=red> Panel Index:" + TakeIndex + "</color>");
            //Resources.UnloadUnusedAssets();
            // CLoth

            if (GameManager.Instance.mainCharacter.name.Contains("Vtuber_Avatar"))
            {
                buttonIndex = 4;
                BtnsPanelCloth.GetComponent<SubBottons>().ClickBtnFtn(4);
            }
            else
            {
                buttonIndex = 3;
                BtnsPanelCloth.GetComponent<SubBottons>().ClickBtnFtn(3);
            }

            ConstantsHolder.xanaConstants.currentButtonIndex = buttonIndex;
            
            MainPanelCloth.SetActive(true);
            MainPanelAvatar.SetActive(false);
            //OpenClothContainerPanel(0);
            //BtnsPanelCloth.GetComponent<SubBottons>().ClickBtnFtn(3);
            ClothBtnLine.SetActive(true);
            AvatarBtnLine.SetActive(false);
            ClothBtnText.color = HighlightedColor;
            AvatarBtnText.color = NormalColor;
            UpdateStoreSelection(3);
        }
        else
        {
            ////Debug.LogError("<color=red> Panel Index:" + TakeIndex + "</color>");
            MainPanelCloth.SetActive(false);
            MainPanelAvatar.SetActive(true);

            if (GameManager.Instance.mainCharacter.name.Contains("Vtuber_Avatar"))
            {
                buttonIndex = 10;
                BtnsPanelAvatar.GetComponent<SubBottons>().ClickBtnFtn(10);
            }
            else
            {
                buttonIndex = 0;
                BtnsPanelAvatar.GetComponent<SubBottons>().ClickBtnFtn(0);
            }

            ConstantsHolder.xanaConstants.currentButtonIndex = buttonIndex;

            // Header Buttons
            ClothBtnLine.SetActive(false);
            AvatarBtnLine.SetActive(true);
            ClothBtnText.color = NormalColor;
            AvatarBtnText.color = HighlightedColor;

            //OpenAvatarContainerPanel(0);
            ////Debug.Log("Undo Redo Call the btn functionality");

            UpdateStoreSelection(0);
        }

        if (PlayerPrefs.GetInt("presetPanel") == 1)
        {
            PlayerPrefs.SetInt("presetPanel", 0);  // was loggedin as account 

            InventoryManager.instance.GreyRibbonImage.SetActive(true);
            InventoryManager.instance.WhiteRibbonImage.SetActive(false);
            InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;

            SaveCharacterProperties.instance.LoadMorphsfromFile();
        }

        DisableColorPanels();

    }


    //public void UpdateUserCoins()
    //{
    //    string totalCoins = PlayerPrefs.GetInt("TotalCoins").ToString();
    //    double coins = Double.Parse(totalCoins);
    //    totalCoins = String.Format("{0:n0}", coins);
    //    //TotalGameCoins.text = totalCoins;
    //    CreditShopManager.instance.TotalCoins.text = totalCoins;
    //}
    // Update is called once per frame

    public void OpenClothContainerPanel(int m_GetIndex)
    {
        buttonIndex = m_GetIndex;
        Clothdatabool = true;
        IndexofPanel = m_GetIndex;
        for (int i = 0; i < ClothsPanel.Length; i++)
        {
            if (m_GetIndex != i)
                ClothsPanel[i].SetActive(false);
        }
        ClothsPanel[m_GetIndex].SetActive(true);

        if (GameManager.Instance.mainCharacter.name.Contains("Vtuber_Avatar"))
        {
            CloseCommingSoonPanel(true);
            return;
        }
        else
        {
            CloseCommingSoonPanel(false);
        }

        if (m_GetIndex == 0 || m_GetIndex == 1 || m_GetIndex == 2 /*|| m_GetIndex == 3*/ || m_GetIndex == 4 /*|| m_GetIndex == 5*/ || m_GetIndex == 6 /*|| m_GetIndex == 9*/) //its a preset do nothing
        {
            // When Preset click than update the panel index
            PreviousSelectionCount = IndexofPanel;
            return;
        }
        if (CheckAPILoaded)
        {
            if (SubCategoriesList.Count > 0)
            {
                SubmitAllItemswithSpecificSubCategory(SubCategoriesList[m_GetIndex].id, false);
            }
        }
        else
        {
            StartCoroutine(WaitForAPICallCompleted(m_GetIndex));
        }
    }

    void CloseCommingSoonPanel(bool status)
    {
        clothBottom_ComingSoon.SetActive(status);
        clothOuter_ComingSoon.SetActive(status);

        AvatarHair_ComingSoon.SetActive(status);
        AvatarFace_ComingSoon.SetActive(status); 
        AvatarEyeBrow_ComingSoon.SetActive(status);
        AvatarEye_ComingSoon.SetActive(status);
        AvatarNose_ComingSoon.SetActive(status);
        AvatarLip_ComingSoon.SetActive(status);
    }

    public void OpenAvatarContainerPanel(int m_GetIndex)
    {

        buttonIndex = m_GetIndex;
        Clothdatabool = false;
        IndexofPanel = m_GetIndex + 8; //16


        for (int i = 0; i < AvatarPanel.Length; i++)
        {
            AvatarPanel[i].SetActive(false);
        }
        AvatarPanel[m_GetIndex].SetActive(true);
        //CheckColorProperty(m_GetIndex);    // Temperarlily Disble Color Panel

        if (GameManager.Instance.mainCharacter.name.Contains("Vtuber_Avatar"))
        {
            CloseCommingSoonPanel(true);
            return;
        }
        else
        {
            CloseCommingSoonPanel(false);
        }



        if (m_GetIndex != 0) //Dont need to call API for otherthan Hair
        {
            // When Preset click than update the panel index
            PreviousSelectionCount = IndexofPanel;
            return;
        }
        if (CheckAPILoaded)
        {
            if (SubCategoriesList.Count > 0)
            {
                // Debug.LogError("second time :- " + m_GetIndex);
                //print(SubCategoriesList[m_GetIndex + 8].id);
                SubmitAllItemswithSpecificSubCategory(SubCategoriesList[m_GetIndex + 8].id, false);
            }

        }
        else
        {
            // Debug.LogError("second time :- " + m_GetIndex);
            StartCoroutine(WaitForAPICallCompleted(m_GetIndex));
        }
    }

    public bool CheckColorPanelEnabled(int num)             // AR custom function check color panel enable or not
    {
        switch (num)
        {
            case 0:
                return AllCategoriesData[19].parentObj.activeSelf ? true : false; // Custom Hairs
                break;
            case 2:
                return AllCategoriesData[20].parentObj.activeSelf ? true : false; // Custom EyeBrows
                break;
            case 3:
                return AllCategoriesData[23].parentObj.gameObject.activeSelf ? true : false; // Custom Eyes Palette
                break;
            case 5:
                return AllCategoriesData[25].parentObj.gameObject.activeSelf ? true : false; // Custom Lips Palette
                break;
            default:
                return false;
                break;
        }
    }

    public void OpenColorPanel(int index)
    {
        //Debug.Log("<color=blue> Open Color Panel Index: " + index + "</color>");
        if (index == 0)
        {
            AllCategoriesData[19].parentObj.SetActive(true);
            AllCategoriesData[8].parentObj.SetActive(false);
            SetContentOnScroll(AvatarPanel[0], (RectTransform)AllCategoriesData[19].parentObj.transform);
        }
        else if (index == 2)
        {
            AllCategoriesData[20].parentObj.SetActive(true);
            AllCategoriesData[10].parentObj.SetActive(false);
            SetContentOnScroll(AvatarPanel[2], (RectTransform)AllCategoriesData[20].parentObj.transform);
        }
        else if (index == 3)
        {
            AllCategoriesData[23].parentObj.SetActive(true);
            AllCategoriesData[12].parentObj.SetActive(false);
            SetContentOnScroll(AvatarPanel[3], (RectTransform)AllCategoriesData[23].parentObj.transform);
        }
        else if (index == 5)
        {
            //Debug.Log("Open color palette");
            AllCategoriesData[25].parentObj.SetActive(true);
            AllCategoriesData[14].parentObj.SetActive(false);
            SetContentOnScroll(AvatarPanel[5], (RectTransform)AllCategoriesData[25].parentObj.transform);
        }
    }

    bool tempBool;
    public bool CloseColorPanel(int index)
    {
        if (index == 0)
        {
            //if (ParentOfBtnsCustomHair.gameObject.activeInHierarchy)
            //    tempBool = true;
            AllCategoriesData[19].parentObj.SetActive(false);
            AllCategoriesData[8].parentObj.SetActive(true);
            SetContentOnScroll(AvatarPanel[0], (RectTransform)AllCategoriesData[8].parentObj.transform);
            return tempBool;
        }
        else if (index == 2)
        {
            //if (ParentOfBtnsCustomEyeBrows.gameObject.activeInHierarchy)
            //    tempBool = true;
            AllCategoriesData[20].parentObj.SetActive(false);
            AllCategoriesData[10].parentObj.SetActive(true);
            SetContentOnScroll(AvatarPanel[2], (RectTransform)AllCategoriesData[10].parentObj.transform);
            return tempBool;
        }
        else if (index == 3)
        {
            //if (ParentOfBtnsCustomEyesPalette.gameObject.activeInHierarchy)
            //    tempBool = true;
            AllCategoriesData[23].parentObj.gameObject.SetActive(false);
            AllCategoriesData[22].parentObj.gameObject.SetActive(false);
            AllCategoriesData[12].parentObj.gameObject.SetActive(true);
            SetContentOnScroll(AvatarPanel[3], (RectTransform)AllCategoriesData[12].parentObj.transform);
            return tempBool;
        }
        else if (index == 5)
        {
            //if (ParentOfBtnsCustomLipsPalette.gameObject.activeInHierarchy)
            //    tempBool = true;
            AllCategoriesData[25].parentObj.SetActive(false);
            AllCategoriesData[24].parentObj.SetActive(false);
            AllCategoriesData[14].parentObj.gameObject.SetActive(true);
            SetContentOnScroll(AvatarPanel[5], (RectTransform)AllCategoriesData[14].parentObj.transform);
            return tempBool;
        }
        return tempBool;
    }
    void CheckColorProperty(int _index)
    {
        if (_index == 3 || _index == 5 /*|| _index == 8*/)
        {
            SwitchColorMode(_index);
            //colorBtn.SetActive(true);
            colorBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            colorBtn.GetComponent<Button>().onClick.AddListener(() => OnColorButtonClicked(_index));

        }
        else if (_index == 0 || _index == 2)
        {
            SwitchColorMode(_index);
        }
        else
        {
            colorBtn.SetActive(false);
        }
    }

    void SwitchColorMode(int index)
    {
        //AssetBundle.UnloadAllAssetBundles(false);
        //Resources.UnloadUnusedAssets();

        ////Debug.Log("ColorBtn : " + index);
        StoreStackHandler.obj.UpdatePanelStatus(index, false);    // AR changes
        textskin.enabled = false;
        colorBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        colorBtn.GetComponent<Button>().onClick.AddListener(() => OnColorButtonClicked(index));

        switch (index)
        {
            case 0:
                AllCategoriesData[8].parentObj.SetActive(true);
                AllCategoriesData[19].parentObj.SetActive(false);

                SetContentOnScroll(AvatarPanel[0], (RectTransform)AllCategoriesData[8].parentObj.transform);
                break;
            case 1:
                AllCategoriesData[9].parentObj.SetActive(true);
                AllCategoriesData[21].parentObj.SetActive(false);

                SetContentOnScroll(AvatarPanel[1], (RectTransform)AllCategoriesData[9].parentObj.transform);
                break;
            case 2:
                AllCategoriesData[10].parentObj.SetActive(true);
                AllCategoriesData[20].parentObj.SetActive(false);

                SetContentOnScroll(AvatarPanel[2], (RectTransform)AllCategoriesData[10].parentObj.transform);
                break;
            case 3:
                AllCategoriesData[12].parentObj.SetActive(true);
                AllCategoriesData[22].parentObj.SetActive(false);
                AllCategoriesData[23].parentObj.SetActive(false);
                SetContentOnScroll(AvatarPanel[3], (RectTransform)AllCategoriesData[12].parentObj.transform);
                break;
            case 5:
                AllCategoriesData[14].parentObj.SetActive(true);
                AllCategoriesData[24].parentObj.SetActive(false);
                AllCategoriesData[25].parentObj.SetActive(false);
                SetContentOnScroll(AvatarPanel[5], (RectTransform)AllCategoriesData[14].parentObj.transform);
                break;
            case 6:
                AllCategoriesData[26].parentObj.SetActive(true);
                AllCategoriesData[16].parentObj.SetActive(false);

                SetContentOnScroll(AvatarPanel[8], (RectTransform)AllCategoriesData[26].parentObj.transform);
                break;
        }
        //AssetBundle.UnloadAllAssetBundles(false);
        //Resources.UnloadUnusedAssets();

        UpdateStoreSelection(index);
    }
    public void OnColorButtonClicked(int _index)
    {
        //AssetBundle.UnloadAllAssetBundles(false);
        //Resources.UnloadUnusedAssets();

        colorBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        colorBtn.GetComponent<Button>().onClick.AddListener(() => SwitchColorMode(_index));

        switch (_index)
        {
            case 1:
                AllCategoriesData[9].parentObj.SetActive(false);
                AllCategoriesData[21].parentObj.SetActive(true);

                SetContentOnScroll(AvatarPanel[1], (RectTransform)AllCategoriesData[21].parentObj.transform);
                break;
            case 3:
                AllCategoriesData[12].parentObj.SetActive(false);
                AllCategoriesData[23].parentObj.SetActive(false);
                AllCategoriesData[22].parentObj.SetActive(true);

                UpdateColor(_index);

                SetContentOnScroll(AvatarPanel[3], (RectTransform)AllCategoriesData[22].parentObj.transform);
                if (AllCategoriesData[24].parentObj.transform.childCount == 0)
                {
                    SubmitAllItemswithSpecificSubCategory(SubCategoriesList[_index + 8].id, true);
                }

                break;
            case 5:
                AllCategoriesData[14].parentObj.SetActive(false);
                AllCategoriesData[25].parentObj.SetActive(false);
                AllCategoriesData[24].parentObj.SetActive(true);

                UpdateColor(_index);

                SetContentOnScroll(AvatarPanel[5], (RectTransform)AllCategoriesData[24].parentObj.transform);
                if (AllCategoriesData[24].parentObj.transform.childCount == 0)
                {
                    SubmitAllItemswithSpecificSubCategory(SubCategoriesList[_index + 8].id, true);
                }

                break;
            case 7:
                AllCategoriesData[16].parentObj.SetActive(false); // Avatar - Skin
                AllCategoriesData[26].parentObj.SetActive(true); // Color - Skin


                UpdateColor(_index);

                SetContentOnScroll(AvatarPanel[7], (RectTransform)AllCategoriesData[18].parentObj.transform);
                break;
        }
    }

    // update color call when open color selection
    public void UpdateColor(int _index)
    {
        ////Debug.Log("<color=red>Update Color Index</color>" + _index);
        textskin.enabled = true;
        switch (_index)
        {
            case 0:
                if (ConstantsHolder.xanaConstants.hairColoPalette != "" && AllCategoriesData[19].parentObj.transform.childCount != 0)
                {
                    for (int i = 0; i < AllCategoriesData[19].parentObj.transform.childCount; i++)
                    {
                        childObject = AllCategoriesData[19].parentObj.transform.GetChild(i).gameObject;
                        if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.hairColoPalette)
                        {
                            //Debug.Log("ID = " + childObject.GetComponent<ItemDetail>().id);

                            childObject.GetComponent<Image>().enabled = true;
                            ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                            //Debug.Log("<color=red>InventoryManager AssignLastClickedBtnHere</color>");
                            ConstantsHolder.xanaConstants.colorSelection[2] = childObject;

                            CheckForItemDetail(ConstantsHolder.xanaConstants.eyeColorPalette, 2);

                            break;
                        }
                    }
                }
                break;
            case 2:
                if (ConstantsHolder.xanaConstants.eyeBrowColorPaletteIndex != -1 && AllCategoriesData[20].parentObj.transform.childCount != 0)
                {
                    for (int i = 0; i < AllCategoriesData[20].parentObj.transform.childCount; i++)
                    {
                        childObject = AllCategoriesData[20].parentObj.transform.GetChild(i).gameObject;
                        if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.eyeBrowColorPaletteIndex.ToString())
                        {
                            childObject.GetComponent<Image>().enabled = true;
                            ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                            //Debug.Log("<color=red>InventoryManager AssignLastClickedBtnHere</color>");
                            ConstantsHolder.xanaConstants.colorSelection[3] = childObject;

                            CheckForItemDetail(ConstantsHolder.xanaConstants.eyeColorPalette, 3);

                            break;
                        }
                    }
                }
                break;
            case 3:
                if (ConstantsHolder.xanaConstants.eyeColor != "")
                {
                    for (int i = 0; i < AllCategoriesData[22].parentObj.transform.childCount; i++)
                    {
                        childObject = AllCategoriesData[22].parentObj.transform.GetChild(i).gameObject;
                        if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.eyeColor)
                        {
                            childObject.GetComponent<Image>().enabled = true;
                            ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                            //Debug.Log("<color=red>InventoryManager AssignLastClickedBtnHere</color>");
                            ConstantsHolder.xanaConstants.colorSelection[0] = childObject;

                            CheckForItemDetail(ConstantsHolder.xanaConstants.eyeColor, 4);

                            break;
                        }
                    }
                }
                if (ConstantsHolder.xanaConstants.eyeColorPalette != "" && AllCategoriesData[23].parentObj.transform.childCount != 0)
                {
                    for (int i = 0; i < AllCategoriesData[23].parentObj.transform.childCount; i++)
                    {
                        childObject = AllCategoriesData[23].parentObj.transform.GetChild(i).gameObject;
                        if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.eyeColorPalette)
                        {
                            childObject.GetComponent<Image>().enabled = true;
                            ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                            //Debug.Log("<color=red>InventoryManager AssignLastClickedBtnHere</color>");
                            ConstantsHolder.xanaConstants.colorSelection[4] = childObject;

                            CheckForItemDetail(ConstantsHolder.xanaConstants.eyeColorPalette, 4);

                            break;
                        }
                    }
                }
                break;

            case 5:
                if (ConstantsHolder.xanaConstants.lipColor != "")
                {
                    for (int i = 0; i < AllCategoriesData[24].parentObj.transform.childCount; i++)
                    {
                        childObject = AllCategoriesData[24].parentObj.transform.GetChild(i).gameObject;
                        if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.lipColor)
                        {
                            childObject.GetComponent<Image>().enabled = true;
                            ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                            //Debug.Log("<color=red>InventoryManager AssignLastClickedBtnHere</color>");
                            ConstantsHolder.xanaConstants.colorSelection[1] = childObject;

                            CheckForItemDetail(ConstantsHolder.xanaConstants.lipColor, 5);

                            break;
                        }
                    }
                }
                if (ConstantsHolder.xanaConstants.lipColorPalette != "" && AllCategoriesData[25].parentObj.transform.childCount != 0)
                {
                    for (int i = 0; i < AllCategoriesData[25].parentObj.transform.childCount; i++)
                    {
                        childObject = AllCategoriesData[25].parentObj.transform.GetChild(i).gameObject;
                        if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.lipColorPalette)
                        {
                            childObject.GetComponent<Image>().enabled = true;
                            ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                            //Debug.Log("<color=red>InventoryManager AssignLastClickedBtnHere</color>");
                            ConstantsHolder.xanaConstants.colorSelection[5] = childObject;

                            CheckForItemDetail(ConstantsHolder.xanaConstants.lipColorPalette, 5);

                            break;
                        }
                    }
                }
                break;

            case 7:
                if (ConstantsHolder.xanaConstants.skinColor != "")
                {
                    for (int i = 0; i < AllCategoriesData[26].parentObj.transform.childCount; i++)
                    {
                        childObject = AllCategoriesData[26].parentObj.transform.GetChild(i).gameObject;
                        if (childObject.GetComponent<ItemDetail>().MyIndex.ToString() == ConstantsHolder.xanaConstants.skinColor)
                        {
                            childObject.GetComponent<Image>().enabled = true;
                            ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                            //Debug.Log("<color=red>InventoryManager AssignLastClickedBtnHere</color>");
                            ConstantsHolder.xanaConstants.avatarStoreSelection[7] = childObject;

                            CheckForItemDetail(ConstantsHolder.xanaConstants.skinColor, 6);

                            break;
                        }
                    }
                }
                break;


        }
    }

    public void SetContentOnScroll(GameObject _scrollView, RectTransform _content)
    {
        _scrollView.GetComponent<ScrollRect>().content = _content;
    }

    public void ClearBuyItems()
    {
        for (int i = 0; i < TotalBtnlist.Count; i++)
        {
            TotalBtnlist[i].SelectedBool = false;
            TotalBtnlist[i].gameObject.GetComponent<Image>().color = TotalBtnlist[i].NormalColor;
        }
        TotalBtnlist.Clear();

    }
    public void GetSelectedBtn(int getIndex, EnumClass.CategoryEnum selectedEnum)
    {

        switch (selectedEnum)
        {
            case EnumClass.CategoryEnum.Head:
                {
                    //AssetBundle.UnloadAllAssetBundles(false);
                    // Resources.UnloadUnusedAssets();

                    for (int i = 0; i < CategorieslistHeads.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistHeads[i].SelectedBool = true;
                            CategorieslistHeads[i].gameObject.GetComponent<Image>().color = CategorieslistHeads[i].HighlightedColor;

                            TotalBtnlist.Add(CategorieslistHeads[i]);
                        }
                        else
                        {
                            CategorieslistHeads[i].SelectedBool = false;
                            CategorieslistHeads[i].gameObject.GetComponent<Image>().color = CategorieslistHeads[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistHeads[i]);
                        }
                    }
                    break;
                }
            case EnumClass.CategoryEnum.Face:
                {
                    //AssetBundle.UnloadAllAssetBundles(false);
                    //Resources.UnloadUnusedAssets();

                    for (int i = 0; i < CategorieslistFace.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistFace[i].SelectedBool = true;
                            CategorieslistFace[i].GetComponent<Image>().color = CategorieslistFace[i].HighlightedColor;

                            TotalBtnlist.Add(CategorieslistFace[i]);
                        }
                        else
                        {
                            CategorieslistFace[i].SelectedBool = false;
                            CategorieslistFace[i].GetComponent<Image>().color = CategorieslistFace[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistFace[i]);
                        }
                    }
                    break;
                }
            case EnumClass.CategoryEnum.Inner:
                {
                    //AssetBundle.UnloadAllAssetBundles(false);
                    //Resources.UnloadUnusedAssets();

                    for (int i = 0; i < CategorieslistInner.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistInner[i].SelectedBool = true;
                            CategorieslistInner[i].GetComponent<Image>().color = CategorieslistInner[i].HighlightedColor;

                            TotalBtnlist.Add(CategorieslistInner[i]);

                        }
                        else
                        {
                            CategorieslistInner[i].SelectedBool = false;
                            CategorieslistInner[i].GetComponent<Image>().color = CategorieslistInner[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistInner[i]);
                        }
                    }
                    break;
                }
            case EnumClass.CategoryEnum.Outer:
                {
                    //AssetBundle.UnloadAllAssetBundles(false);
                    //Resources.UnloadUnusedAssets();

                    for (int i = 0; i < CategorieslistOuter.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistOuter[i].SelectedBool = true;
                            CategorieslistOuter[i].GetComponent<Image>().color = CategorieslistOuter[i].HighlightedColor;
                            TotalBtnlist.Add(CategorieslistOuter[i]);
                        }
                        else
                        {
                            CategorieslistOuter[i].SelectedBool = false;
                            CategorieslistOuter[i].GetComponent<Image>().color = CategorieslistOuter[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistOuter[i]);
                        }
                    }
                    break;
                }

            case EnumClass.CategoryEnum.Accesary:
                {
                    //AssetBundle.UnloadAllAssetBundles(false);
                    //Resources.UnloadUnusedAssets();

                    for (int i = 0; i < CategorieslistAccesary.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistAccesary[i].SelectedBool = true;
                            CategorieslistAccesary[i].GetComponent<Image>().color = CategorieslistAccesary[i].HighlightedColor;

                            TotalBtnlist.Add(CategorieslistAccesary[i]);
                        }
                        else
                        {
                            CategorieslistAccesary[i].SelectedBool = false;
                            CategorieslistAccesary[i].GetComponent<Image>().color = CategorieslistAccesary[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistAccesary[i]);
                        }
                    }
                    break;
                }

            case EnumClass.CategoryEnum.Bottom:
                {
                    //AssetBundle.UnloadAllAssetBundles(false);
                    //Resources.UnloadUnusedAssets();

                    for (int i = 0; i < CategorieslistBottom.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistBottom[i].SelectedBool = true;
                            CategorieslistBottom[i].GetComponent<Image>().color = CategorieslistBottom[i].HighlightedColor;

                            TotalBtnlist.Add(CategorieslistBottom[i]);
                        }
                        else
                        {
                            CategorieslistBottom[i].SelectedBool = false;
                            CategorieslistBottom[i].GetComponent<Image>().color = CategorieslistBottom[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistBottom[i]);
                        }
                    }
                    break;
                }
            case EnumClass.CategoryEnum.Socks:
                {
                    //AssetBundle.UnloadAllAssetBundles(false);
                    //Resources.UnloadUnusedAssets();

                    for (int i = 0; i < CategorieslistSocks.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistSocks[i].SelectedBool = true;
                            CategorieslistSocks[i].GetComponent<Image>().color = CategorieslistSocks[i].HighlightedColor;
                            TotalBtnlist.Add(CategorieslistSocks[i]);
                        }
                        else
                        {
                            CategorieslistSocks[i].SelectedBool = false;
                            CategorieslistSocks[i].GetComponent<Image>().color = CategorieslistSocks[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistSocks[i]);
                        }
                    }
                    break;
                }
            // CategorieslistShoes
            case EnumClass.CategoryEnum.Shoes:
                {
                    //AssetBundle.UnloadAllAssetBundles(false);
                    //Resources.UnloadUnusedAssets();

                    for (int i = 0; i < CategorieslistShoes.Count; i++)
                    {
                        if (i == getIndex)
                        {
                            CategorieslistShoes[i].SelectedBool = true;
                            CategorieslistShoes[i].GetComponent<Image>().color = CategorieslistShoes[i].HighlightedColor;
                            TotalBtnlist.Add(CategorieslistShoes[i]);
                        }
                        else
                        {
                            CategorieslistShoes[i].SelectedBool = false;
                            CategorieslistShoes[i].GetComponent<Image>().color = CategorieslistShoes[i].NormalColor;
                            TotalBtnlist.Remove(CategorieslistShoes[i]);
                        }
                    }
                    break;
                }
        }
        SelectBtnObjs();
    }

    public void SelectBtnObjs()
    {
        //AssetBundle.UnloadAllAssetBundles(false);
        //Resources.UnloadUnusedAssets();

        int TotalPrice = 0;
        for (int i = 0; i < TotalBtnlist.Count; i++)
        {
            if (TotalBtnlist[i].SelectedBool)
            {
                TotalPrice += int.Parse(TotalBtnlist[i].PriceTxt.text);
            }
        }
        BuyCountertxt.text = TotalBtnlist.Count.ToString();
    }
    //public void ClearParentFromObjs()
    //{
    //    if (BuyPanelParentOfBtns.childCount > 0)
    //    {
    //        for (int i = 0; i < BuyPanelParentOfBtns.childCount; i++)
    //        {
    //            Destroy(BuyPanelParentOfBtns.GetChild(i).gameObject);
    //        }
    //    }
    //}



    //public void GoToCheckOut()
    //{
    //    AssetBundle.UnloadAllAssetBundles(false);
    //    Resources.UnloadUnusedAssets();

    //    int Counter = 0;
    //    int TotalPrice = 0;
    //    TotalObjectsInBuyPanel.Clear();

    //    if (BuyPanelParentOfBtns.childCount > 0)
    //    {
    //        ClearParentFromObjs();
    //    }
    //    for (int i = 0; i < TotalBtnlist.Count; i++)
    //    {
    //        if (TotalBtnlist[i].SelectedBool)
    //        {
    //            Counter += 1;
    //            TotalPrice += int.Parse(TotalBtnlist[i].PriceTxt.text);
    //            print(Counter);
    //            TotalObjectsInBuyPanel.Add(TotalBtnlist[i].gameObject);
    //        }
    //    }
    //    if (Counter > 0)
    //    {
    //        if (!UserRegisterationManager.instance.LoggedIn)
    //        {
    //            OpenMainPanel("ShowSignUpPanel");
    //        }
    //        else
    //        {
    //            TotalItemPriceCheckOut = 0;
    //            TotalSelectedInBuyPanel.Clear();

    //            StoreItemsPanel.SetActive(false);
    //            CheckOutBuyItemPanel.SetActive(true);
    //            for (int i = 0; i < TotalObjectsInBuyPanel.Count; i++)
    //            {
    //                GameObject L_ItemBtnObj = Instantiate(BuyItemPrefab, BuyPanelParentOfBtns.transform);
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().PriceTxt.text = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().PriceTxt.text;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().CategoryTxt.text = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().CategoriesEnumVar.ToString();

    //                // Add All Value from One Button to Buy Checkout Btn
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().assetLinkAndroid = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().assetLinkAndroid;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().assetLinkIos = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().assetLinkIos;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().assetLinkWindows = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().assetLinkWindows;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().createdAt = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().createdAt;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().createdBy = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().createdBy;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().iconLink = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().iconLink;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().id = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().id;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().isFavourite = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().isFavourite;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().isOccupied = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().isOccupied;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().isPaid = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().isPaid;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().isPurchased = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().isPurchased;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().name = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().name;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().price = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().price;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().categoryId = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().categoryId;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().subCategory = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().subCategory;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().updatedAt = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().updatedAt;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().itemTags = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().itemTags;
    //                L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().CategoriesEnumVar = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().CategoriesEnumVar;
    //                TotalSelectedInBuyPanel.Add(L_ItemBtnObj.gameObject);
    //                TotalItemPriceCheckOut += int.Parse(L_ItemBtnObj.GetComponent<ItemDetailBuyItem>().PriceTxt.text);
    //            }
    //            TotalPriceBuyPanelTxt.text = TotalItemPriceCheckOut.ToString();
    //            TotalItemsBuyPanelTxt.text = TotalObjectsInBuyPanel.Count.ToString();
    //        }
    //    }
    //}
    //public void BuyItems()
    //{
    //    if (PlayerPrefs.GetInt("TotalCoins") < TotalItemPriceCheckOut)
    //    {
    //        print("Price is less than Selected Items");
    //    }
    //    else if (PlayerPrefs.GetInt("TotalCoins") >= TotalItemPriceCheckOut)
    //    {
    //        if (TotalObjectsInBuyPanel.Count > 0)
    //        {
    //            for (int i = 0; i < TotalObjectsInBuyPanel.Count; i++)
    //            {
    //                EnumClass.CategoryEnum selectedEnum = TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().CategoriesEnumVar;
    //                switch (selectedEnum)
    //                {
    //                    case EnumClass.CategoryEnum.Head:
    //                        {
    //                            if (CategorieslistHeads.Contains(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>()))
    //                            {
    //                                int Getindex = CategorieslistHeads.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
    //                                CategorieslistHeads[Getindex].price = "0";
    //                                CategorieslistHeads[Getindex].isPaid = "true";
    //                                CategorieslistHeads[Getindex].isPurchased = "true";
    //                            }
    //                            break;
    //                        }
    //                    case EnumClass.CategoryEnum.Face:
    //                        {
    //                            int Getindex = CategorieslistFace.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
    //                            CategorieslistFace[Getindex].price = "0";
    //                            CategorieslistFace[Getindex].isPaid = "true";
    //                            CategorieslistFace[Getindex].isPurchased = "true";


    //                            break;
    //                        }
    //                    case EnumClass.CategoryEnum.Inner:
    //                        {
    //                            int Getindex = CategorieslistInner.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
    //                            CategorieslistInner[Getindex].price = "0";
    //                            CategorieslistInner[Getindex].isPaid = "true";
    //                            CategorieslistInner[Getindex].isPurchased = "true";
    //                            break;
    //                        }
    //                    case EnumClass.CategoryEnum.Outer:
    //                        {
    //                            int Getindex = CategorieslistOuter.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
    //                            CategorieslistOuter[Getindex].price = "0";
    //                            CategorieslistOuter[Getindex].isPaid = "true";
    //                            CategorieslistOuter[Getindex].isPurchased = "true";
    //                            break;
    //                        }
    //                    case EnumClass.CategoryEnum.Accesary:
    //                        {
    //                            int Getindex = CategorieslistAccesary.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
    //                            CategorieslistAccesary[Getindex].price = "0";
    //                            CategorieslistAccesary[Getindex].isPaid = "true";
    //                            CategorieslistAccesary[Getindex].isPurchased = "true";
    //                            break;
    //                        }
    //                    case EnumClass.CategoryEnum.Bottom:
    //                        {
    //                            int Getindex = CategorieslistBottom.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
    //                            CategorieslistBottom[Getindex].price = "0";
    //                            CategorieslistBottom[Getindex].isPaid = "true";
    //                            CategorieslistBottom[Getindex].isPurchased = "true";
    //                            break;
    //                        }
    //                    case EnumClass.CategoryEnum.Socks:
    //                        {
    //                            int Getindex = CategorieslistSocks.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
    //                            CategorieslistSocks[Getindex].price = "0";
    //                            CategorieslistSocks[Getindex].isPaid = "true";
    //                            CategorieslistSocks[Getindex].isPurchased = "true";
    //                            break;
    //                        }
    //                    case EnumClass.CategoryEnum.Shoes:
    //                        {
    //                            int Getindex = CategorieslistShoes.IndexOf(TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>());
    //                            CategorieslistShoes[Getindex].price = "0";
    //                            CategorieslistShoes[Getindex].isPaid = "true";
    //                            CategorieslistShoes[Getindex].isPurchased = "true";
    //                            break;
    //                        }
    //                }
    //            }
    //            //PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins") - TotalItemPriceCheckOut);
    //        }
    //        UpdateItemsDetails();
    //        //CloseCheckOutPanel();
    //    }
    //}
    void UpdateItemsDetails()
    {
        //UpdateUserCoins();
        //for (int i = 0; i < TotalObjectsInBuyPanel.Count; i++)
        //{
        //    TotalObjectsInBuyPanel[i].GetComponent<ItemDetail>().DeSelectAfterBuying();
        //}
        //TotalObjectsInBuyPanel.Clear();

        // ShirtsSelection
        for (int i = 0; i < CategorieslistHeads.Count; i++)
        {
            CategorieslistHeads[i].UpdateValues();
        }
        // PantsSelection
        for (int i = 0; i < CategorieslistFace.Count; i++)
        {
            CategorieslistFace[i].UpdateValues();
        }
        // CategorieslistShoes
        for (int i = 0; i < CategorieslistInner.Count; i++)
        {
            CategorieslistInner[i].UpdateValues();
        }
        // CategorieslistHairs
        for (int i = 0; i < CategorieslistOuter.Count; i++)
        {
            CategorieslistOuter[i].UpdateValues();
        }
        // CategorieslistGlasses
        for (int i = 0; i < CategorieslistAccesary.Count; i++)
        {
            CategorieslistAccesary[i].UpdateValues();
        }
        // CategorieslistHats
        for (int i = 0; i < CategorieslistBottom.Count; i++)
        {
            CategorieslistBottom[i].UpdateValues();
        }
        //CategorieslistBags
        for (int i = 0; i < CategorieslistSocks.Count; i++)
        {
            CategorieslistSocks[i].UpdateValues();
        }
        //CategorieslistBags
        for (int i = 0; i < CategorieslistShoes.Count; i++)
        {
            CategorieslistShoes[i].UpdateValues();
        }
    }
    //public void RemoveItemsFromCheckOut(GameObject itemDetailsBtn)
    //{
    //    if (TotalSelectedInBuyPanel.Contains(itemDetailsBtn))
    //    {
    //        TotalSelectedInBuyPanel.Remove(itemDetailsBtn);
    //        UpdateCheckOutCash();
    //    }
    //    // TotalObjectsInBuyPanel
    //}
    //public void AddItemsFromCheckOut(GameObject itemDetailsBtn)
    //{
    //    if (!TotalSelectedInBuyPanel.Contains(itemDetailsBtn))
    //    {
    //        TotalSelectedInBuyPanel.Add(itemDetailsBtn);
    //        UpdateCheckOutCash();
    //    }
    //    // TotalObjectsInBuyPanel
    //}
    //public void BuyItemsbyPurchaseAPI()
    //{
    //    List<string> purchaseItemsIDs = new List<string>();
    //    if (PlayerPrefs.GetInt("TotalCoins") < TotalItemPriceCheckOut)
    //    {
    //        OpenMainPanel("LowCoinsPanel");
    //    }
    //    else if (PlayerPrefs.GetInt("TotalCoins") >= TotalItemPriceCheckOut)
    //    {
    //        if (TotalSelectedInBuyPanel.Count > 0)
    //        {
    //            for (int i = 0; i < TotalSelectedInBuyPanel.Count; i++)
    //            {
    //                purchaseItemsIDs.Add(TotalSelectedInBuyPanel[i].GetComponent<ItemDetailBuyItem>().id.ToString());
    //            }
    //        }
    //        ArrayofBuyItems = purchaseItemsIDs.ToArray();
    //        SubmitPurchaseAPI(ArrayofBuyItems);
    //    }
    //}
    //public void UpdateCheckOutCash()
    //{
    //    TotalItemPriceCheckOut = 0;
    //    if (TotalSelectedInBuyPanel.Count == 0)
    //    {
    //        TotalItemPriceCheckOut = 0;
    //        BuyBtnCheckOut.GetComponent<Button>().interactable = false;
    //    }
    //    else
    //    {
    //        BuyBtnCheckOut.GetComponent<Button>().interactable = true;
    //        for (int i = 0; i < TotalSelectedInBuyPanel.Count; i++)
    //        {
    //            TotalItemPriceCheckOut += int.Parse(TotalSelectedInBuyPanel[i].GetComponent<ItemDetailBuyItem>().PriceTxt.text);
    //        }
    //    }
    //    TotalPriceBuyPanelTxt.text = TotalItemPriceCheckOut.ToString();
    //    TotalItemsBuyPanelTxt.text = TotalSelectedInBuyPanel.Count.ToString();
    //}
    //public void CloseCheckOutPanel()
    //{
    //    CheckOutBuyItemPanel.SetActive(false);
    //}
    public class EnumClass : MonoBehaviour
    {
        public enum CategoryEnum
        {
            Head,
            Face,
            Inner,
            Outer,
            Accesary,
            Bottom,
            Socks,
            Shoes,
            HairAvatar,
            HairAvatarColor,
            LipsAvatar,
            LipsAvatarColor,
            EyesAvatar,
            EyesAvatarColor,
            SkinToneAvatar,
            Presets,
            EyeBrowAvatar,
            EyeBrowAvatarColor,
            EyeLashesAvatar,
            Nose,
            Body,
            Makeup,
            Avatar,
            Wearable,
            SliderColor,
            AvatarBtns,
            WearableBtns,
            None
        }
    }

    // Purchase Item Starts Here
    private void SubmitPurchaseAPI(string[] TakeArrayofBuyItems)
    {
        var result = string.Join(",", TakeArrayofBuyItems);
        result = "[" + result + "]";
        ClassforPurchaseAPI purchaseCLassObj = new ClassforPurchaseAPI();
        string bodyJson = JsonUtility.ToJson(purchaseCLassObj.CreateTOJSON(result)); ;
        StartCoroutine(HitPurchaseAPI(ConstantsGod.API_BASEURL + ConstantsGod.PurchasedAPI, bodyJson));
    }
    IEnumerator HitPurchaseAPI(string url, string Jsondata)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
        request.SendWebRequest();
        while (request.isDone)
        {
            yield return null;
        }
        ClassforPurchaseDataExtract PurchaseDataExtractObj = new ClassforPurchaseDataExtract();
        PurchaseDataExtractObj = PurchaseDataExtractObj.CreateFromJSON(request.downloadHandler.text);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                if (PurchaseDataExtractObj.success == true)
                {
                    RefreshDefault();
                    SubmitUserDetailAPI();
                    SubmitDefaultAPI();
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                print("Error Accured " + request.error.ToUpper());
            }
            else
            {
                if (request.error != null)
                {
                    if (PurchaseDataExtractObj.success == false)
                    {
                        print("Hey success false " + PurchaseDataExtractObj.msg);
                    }
                }
            }
        }
        request.Dispose();

    }
    void RefreshDefault()
    {
        CategorieslistHeads.Clear();
        CategorieslistFace.Clear();
        CategorieslistInner.Clear();
        CategorieslistOuter.Clear();
        CategorieslistAccesary.Clear();
        CategorieslistBottom.Clear();
        CategorieslistSocks.Clear();
        CategorieslistShoes.Clear();
        CategorieslistHairs.Clear();
        CategorieslistHairsColors.Clear();
        CategorieslistLipsColor.Clear();
        CategorieslistSkinToneColor.Clear();
        CategorieslistEyesColor.Clear();
        TotalBtnlist.Clear();

        BuyCountertxt.text = "0";
    }
    [System.Serializable]
    public class ClassforPurchaseAPI
    {
        public string ItemIds;
        public ClassforPurchaseAPI CreateTOJSON(string jsonString)
        {
            ClassforPurchaseAPI myObj = new ClassforPurchaseAPI();
            myObj.ItemIds = jsonString;
            return myObj;
        }
    }
    [System.Serializable]
    public class ClassforPurchaseDataExtract
    {
        public bool success;
        public string data;
        public string msg;
        public ClassforPurchaseDataExtract CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ClassforPurchaseDataExtract>(jsonString);
        }
    }
    // Purchase Item End Here


    //  UserDetails Starts here ************************************************************************
    //private string TestNetXenyTokenAPI = "https://backend.xanalia.com/sale-nft/get-xeny-tokens-by-user";
    //private string MainNetXenyTokenAPI = ""; // Mainnet Api here



    public void UpdateUserXeny()
    {
        if (!ConstantsHolder.xanaConstants.LoggedInAsGuest)
        {
            StartCoroutine(RequestUserXenyDataRoutine());
        }
    }

    private IEnumerator RequestUserXenyDataRoutine()
    {
        XenyRequestedData xenyRequestData = new()
        {
            userAddress = PlayerPrefs.GetString("publicID")     //For Testing Xent coins address= "0xA4eFBae8755fE223eB4288B278BEb410F8c6e27E";
        };
        string jsonData = JsonConvert.SerializeObject(xenyRequestData);
        // Convert the JSON data to a byte array
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = UnityWebRequest.Post(ConstantsGod.GetUserXenyCoinsApi, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SendWebRequest();

        while (!request.isDone)
        {
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }

        if ((request.result == UnityWebRequest.Result.ConnectionError) || (request.result == UnityWebRequest.Result.ProtocolError))
        {
            Debug.Log("<color=red> Get XENY Api Error: " + request.error + "</color>");
        }
        else
        {
            TotalGameCoins.text = request.downloadHandler.text;
        }
        request.Dispose();
        StopCoroutine(RequestUserXenyDataRoutine());

    }

    public void SubmitUserDetailAPI()
    {
        //UpdateUserXeny();
        //string localAPI = "";
        //if (!APIBasepointManager.instance.IsXanaLive)
        //{
        //    localAPI = TestNetXenyTokenAPI;
        //}
        //else
        //{
        //    // Mainnet Api
        //    localAPI = MainNetXenyTokenAPI;
        //}
        //StartCoroutine(XenyTokenUserAddrerss(localAPI));
    }

    //private XenyRequestedData xenyRequestData;
    //IEnumerator XenyTokenUserAddrerss(string url)
    //{

    //    xenyRequestData = new XenyRequestedData();
    //    xenyRequestData.userAddress = PlayerPrefs.GetString("publicID");     //For Testing Xent coins address= "0xA4eFBae8755fE223eB4288B278BEb410F8c6e27E";
    //    Debug.Log("User Address is = " + xenyRequestData.userAddress);
    //    string jsonData = JsonConvert.SerializeObject(xenyRequestData);
    //    // Convert the JSON data to a byte array
    //    byte[] postData = Encoding.UTF8.GetBytes(jsonData);
    //    UnityWebRequest request = UnityWebRequest.Post(url, "POST");
    //    request.uploadHandler = new UploadHandlerRaw(postData);
    //    request.downloadHandler = new DownloadHandlerBuffer();
    //    request.SetRequestHeader("Content-Type", "application/json");
    //    request.SendWebRequest();
    //    while (!request.isDone)
    //    {
    //        yield return null;
    //    }
    //    //Debug.Log("hamara data v" + request.downloadHandler.text);

    //    if (!request.isHttpError && !request.isNetworkError)
    //    {
    //        if (request.error == null)
    //        {
    //            JObject json = JObject.Parse(request.downloadHandler.text);
    //            string token = json["userXenyTokens"].ToString();
    //            TotalGameCoins.text = token;
    //            print("xeny coins are = " + token);
    //        }
    //    }
    //    else
    //    {
    //        if (request.isNetworkError)
    //        {
    //            print("Error Occured " + request.error.ToUpper());
    //        }
    //    }
    //    request.Dispose();
    //}
    // Submit GetUser Details        
    //IEnumerator HitGetUserDetails(string url, string Jsondata)
    //{
    //    using (UnityWebRequest request = UnityWebRequest.Get(url))
    //    {
    //        request.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);

    //        request.SendWebRequest();
    //        while (!request.isDone)
    //        {
    //            yield return null;
    //        }
    //        ClassforUserDetails myObjectOfUserDetail = new ClassforUserDetails();
    //        myObjectOfUserDetail = myObjectOfUserDetail.CreateFromJSON(request.downloadHandler.text);

    //        if (!request.isHttpError && !request.isNetworkError)
    //        {
    //            if (request.error == null)
    //            {
    //                if (PlayerPrefs.GetInt("IsChanged") == 0)
    //                {
    //                    PlayerPrefs.SetInt("IsChanged", 1);
    //                    UndoSelection();
    //                    StartCoroutine(CharacterChange());
    //                }

    //                if (myObjectOfUserDetail.success == true)
    //                {
    //                    decimal CoinsInDecimal = decimal.Parse(myObjectOfUserDetail.data.coins);
    //                    int Coinsint = (int)CoinsInDecimal;
    //                    PlayerPrefs.SetInt("TotalCoins", Coinsint);
    //                    //UpdateUserCoins();
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (request.isNetworkError)
    //            {
    //                print(request.error.ToUpper());
    //            }
    //            else
    //            {
    //                if (request.error != null)
    //                {
    //                    if (myObjectOfUserDetail.success == false)
    //                    {
    //                        print("Hey success false " + myObjectOfUserDetail.msg);
    //                    }
    //                }
    //            }
    //        }
    //        request.Dispose();
    //    }
    //}

    [System.Serializable]
    public class ClassforUserDetails
    {
        public bool success;
        public JsondataOfUserDetails data;
        public string msg;
        public ClassforUserDetails CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ClassforUserDetails>(jsonString);
        }
    }

    [System.Serializable]
    public class JsondataOfUserDetails
    {
        public int id;
        public string name;
        public string dob;
        public string phoneNumber;
        public string email;
        public string avatar;
        public int role;
        public string coins;
        public bool isVerified;
        public bool isRegister;
        public bool isDeleted;
        public string createdAt;
        public string updatedAt;
        public UserProfileForUserDetails userProfile;
        public static JsondataOfUserDetails CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<JsondataOfUserDetails>(jsonString);
        }
    }
    [System.Serializable]
    public class UserProfileForUserDetails
    {
        public string id;
        public string userId;
        public string gender;
        public string job;
        public string country;
        public string bio;
        public string isDeleted;
        public string createdAt;
        public string updatedAt;
        public static JsondataOfUserDetails CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<JsondataOfUserDetails>(jsonString);
        }
    }
    //  UserDetails End here -------------------------------------------------------------

    //********** Get Guest Issues ******

    public void SubmitDefaultAPIForGuest()
    {
        StartCoroutine(HitDefaultAPIforGuest(ConstantsGod.API_BASEURL + ConstantsGod.GetDefaultAPI, ""));
    }
    IEnumerator HitDefaultAPIforGuest(string url, string Jsondata)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
            request.SendWebRequest();
            while (!request.isDone)
            {
                yield return null;
            }
            JsonDataObj = GetAllData(request.downloadHandler.text);
            if (!request.isHttpError && !request.isNetworkError)
            {
                if (request.error == null)
                {
                    //Debug.Log(request.downloadHandler.text);
                    if (JsonDataObj.success == true)
                    {
                        print("Success True All Default Data Fetched for Guest");
                    }
                }
            }
            else
            {
                if (request.isNetworkError)
                {
                    print("Network Error");
                }
                else
                {
                    if (request.error != null)
                    {
                        if (JsonDataObj.success == false)
                        {
                            print("Hey success false " + JsonDataObj.msg);
                        }
                    }
                }
            }
            request.Dispose();
        }
    }

    //  ******************************************** Get Default Starts Here ********************************************
    public void SubmitDefaultAPI()
    {
        StartCoroutine(HitDefaultAPI(ConstantsGod.API_BASEURL + ConstantsGod.GetDefaultAPI, ""));
    }
    IEnumerator HitDefaultAPI(string url, string Jsondata)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
            request.SendWebRequest();
            while (!request.isDone)
            {
                yield return null;
            }
            JsonDataObj = GetAllData(request.downloadHandler.text);
            if (!request.isHttpError && !request.isNetworkError)
            {
                if (request.error == null)
                {
                    //Debug.Log(request.downloadHandler.text);
                    if (JsonDataObj.success == true)
                    {
                        print("Success True All Default Data Fetched");
                    }
                }
            }
            else
            {
                if (request.isNetworkError)
                {
                    print("Network Error");
                }
                else
                {
                    if (request.error != null)
                    {
                        if (JsonDataObj.success == false)
                        {
                            print("Hey success false " + JsonDataObj.msg);
                        }
                    }
                }
            }
            request.Dispose();
        }
    }

    public GetAllInfo GetAllData(string m_JsonData)
    {
        JsonDataObj = JsonUtility.FromJson<GetAllInfo>(m_JsonData);
        return JsonDataObj;
    }
    [System.Serializable]
    public class GetAllInfo
    {
        public bool success;
        public ItemClass data;
        public string msg;
    }
    [System.Serializable]
    public class ItemClass
    {
        public CategoryClass Items;
    }
    [System.Serializable]
    public class CategoryClass
    {
        public ToalClothItemsClass Cloth;
        public ToalAvatarItemsClass Avatar;
    }

    [System.Serializable]
    public class ToalClothItemsClass
    {
        public List<Head> Head;
        public List<Face> Face;
        public List<Inner> Inner;
        public List<Outer> Outer;
        public List<Accesary> Accesary;
        public List<Bottom> Bottom;
        public List<Socks> Socks;
        public List<Shoes> Shoes;
    }
    [System.Serializable]
    public class ToalAvatarItemsClass
    {
        public List<Hair> HairSelection;
        public List<FaceAvatar> FaceAvatarSelection;
        public List<EyeBrow> EyeBrowSelection;
        public List<Eyes> EyesSelection;
        public List<Nose> NoseSelection;
        public List<Lip> LipSelection;
        public List<Body> BodySelection;
        public List<Skin> SkinSelection;
    }
    // Cloth Customization

    // PantsSelection
    [System.Serializable]
    public class Head : ItemsParents
    {

    }
    //ShirtsSelection  
    [System.Serializable]
    public class Face : ItemsParents
    {

    }
    //HairsSelection 
    [System.Serializable]
    public class Inner : ItemsParents
    {

    }
    //ShoesSelection
    [System.Serializable]
    public class Outer : ItemsParents
    {


    }
    //Glasses
    [System.Serializable]
    public class Accesary : ItemsParents
    {
    }
    //Hats
    [System.Serializable]
    public class Bottom : ItemsParents
    {


    }
    //Bags
    [System.Serializable]
    public class Socks : ItemsParents
    {

    }
    //CategoriesDetails
    [System.Serializable]
    public class Shoes : ItemsParents
    {

    }

    // Avatar Customization

    // HairSelection
    [System.Serializable]
    public class Hair : ItemsParents
    {

    }
    // FaceAvatarSelection  
    [System.Serializable]
    public class FaceAvatar : ItemsParents
    {

    }
    // EyeBrowSelection 
    [System.Serializable]
    public class EyeBrow : ItemsParents
    {

    }
    //  EyesSelection
    [System.Serializable]
    public class Eyes : ItemsParents
    {


    }
    // NoseSelection
    [System.Serializable]
    public class Nose : ItemsParents
    {
    }
    // LipSelection
    [System.Serializable]
    public class Lip : ItemsParents
    {

    }
    //  BodySelection
    [System.Serializable]
    public class Body : ItemsParents
    {

    }
    // SkinSelection
    [System.Serializable]
    public class Skin : ItemsParents
    {

    }


    public class ItemsParents
    {
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
    }
    // ----------------------------------------- Get Default ENDS Here -----------------------------------------
    //  *************************************** Start Send Coins to Server ********************************************
    public void SubmitSendCoinstoServer(int getCoinsAfterInApp)
    {
        decimal CoinsInDecimal = Convert.ToDecimal(getCoinsAfterInApp);
        CoinsInDecimal = CoinsInDecimal + 0.00m;
        ClassofSendCoins sendcoinsObj = new ClassofSendCoins();
        string bodyJson = JsonUtility.ToJson(sendcoinsObj.CreateTOJSON(CoinsInDecimal.ToString()));
        StartCoroutine(HitSendCoinsAPI(ConstantsGod.API_BASEURL + ConstantsGod.SendCoinsAPI, bodyJson));
    }
    IEnumerator HitSendCoinsAPI(string url, string Jsondata)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(Jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);

        request.SendWebRequest();
        while (!request.isDone)
        {
            yield return null;
        }
        ClassforSendCoinsExtraction SendCoinsDataExtractObj = new ClassforSendCoinsExtraction();
        SendCoinsDataExtractObj = SendCoinsDataExtractObj.CreateFromJSON(request.downloadHandler.text);
        if (!request.isHttpError && !request.isNetworkError)
        {
            if (request.error == null)
            {
                if (SendCoinsDataExtractObj.success == true)
                {
                    SubmitUserDetailAPI();
                }
            }
        }
        else
        {
            if (request.isNetworkError)
            {
                print("Error Accured " + request.error.ToUpper());
            }
            else
            {
                if (request.error != null)
                {
                    if (SendCoinsDataExtractObj.success == false)
                    {
                        //   print("Hey success false " + SendCoinsDataExtractObj.msg);
                    }
                }
            }
        }
        request.Dispose();
    }

    [System.Serializable]
    public class ClassofSendCoins
    {
        public string coins;
        public ClassofSendCoins CreateTOJSON(string jsonString)
        {
            ClassofSendCoins myObj = new ClassofSendCoins();
            myObj.coins = jsonString;
            return myObj;
        }
    }
    [System.Serializable]
    public class ClassforSendCoinsExtraction
    {
        public bool success;
        public string data;
        public string msg;
        public ClassforSendCoinsExtraction CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ClassforSendCoinsExtraction>(jsonString);
        }
    }

    //  *************************************** End Coins to Server ********************************************
    public EnumClass.CategoryEnum TempEnumVar;

    public void PutDataInOurAPPNewAPI()
    {
        _ResettingAssetList = false;
        if (itemLoading != null)
            StopCoroutine(itemLoading);
        TempSubcategoryParent = null;
        StopCoroutine(PutDataInOurAPPNewAPICoroutine());
        itemLoading = StartCoroutine(PutDataInOurAPPNewAPICoroutine());

    }

    int myIndexInList;

    Transform TempSubcategoryParent = null;
    public IEnumerator PutDataInOurAPPNewAPICoroutine()
    {
        print("~~ Coroutine call");
        if (_avatarController == null)
        {
            _avatarController = GameManager.Instance.mainCharacter.GetComponent<AvatarController>();
        }

        if (!colorMode)
            yield return null;
        RefreshDefault();
        List<ItemDetail> TempitemDetail;
        TempitemDetail = new List<ItemDetail>();

        //    //Debug.Log("<color=red>Planel Index: " + IndexofPanel + "</color>");
        switch (IndexofPanel)
        {
            case 0: //TempSubcategoryParent = ParentOfBtnsForHeads;  //TempEnumVar = EnumClass.CategoryEnum.Head;  // CategorieslistHeads = TempitemDetail;
            case 1: //TempSubcategoryParent = ParentOfBtnsForFace;   //TempEnumVar = EnumClass.CategoryEnum.Face;  // CategorieslistFace = TempitemDetail;
            case 2: //TempSubcategoryParent = ParentOfBtnsForInner;  //TempEnumVar = EnumClass.CategoryEnum.Inner; // CategorieslistInner = TempitemDetail;
            case 6: //TempSubcategoryParent = ParentOfBtnsForSocks;  //TempEnumVar = EnumClass.CategoryEnum.Socks; // CategorieslistSocks = TempitemDetail;
                break;

            case 3: // Outer - Shirts
                {
                    myIndexInList = IndexofPanel;
                    TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                    CategorieslistOuter = TempitemDetail;
                    Debug.Log("lIST cOUNT " + CategorieslistOuter.Count);
                    TempEnumVar = EnumClass.CategoryEnum.Outer;
                    ActivateGenerateItemsCoroutine(TempSubcategoryParent.transform, CategorieslistOuter);
                    break;
                }
            case 4: // Presets
                {
                    //TempSubcategoryParent = ParentOfBtnsForAccesary;
                    myIndexInList = IndexofPanel;
                    TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                    TempEnumVar = EnumClass.CategoryEnum.Accesary;
                    break;
                }
            case 5: // Pants - Bottom
                {
                    myIndexInList = IndexofPanel;
                    TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                    TempEnumVar = EnumClass.CategoryEnum.Bottom;
                    ActivateGenerateItemsCoroutine(TempSubcategoryParent.transform, TempitemDetail);
                    break;
                }
            case 7: // Shoes
                {
                    myIndexInList = IndexofPanel;
                    TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                    TempEnumVar = EnumClass.CategoryEnum.Shoes;
                    ActivateGenerateItemsCoroutine(TempSubcategoryParent.transform, TempitemDetail);
                    break;
                }
            case 8: // Hairs
                {
                    if (!colorMode)
                    {
                        myIndexInList = IndexofPanel;
                        TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                        TempEnumVar = EnumClass.CategoryEnum.HairAvatar;
                        ActivateGenerateItemsCoroutine(TempSubcategoryParent.transform, TempitemDetail);
                    }
                    else
                    {
                        //TempSubcategoryParent = ParentOfBtnsCustomHair;
                        myIndexInList = 19;
                        TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                        TempEnumVar = EnumClass.CategoryEnum.HairAvatarColor;

                        ActivateGenerateItemsCoroutine(TempSubcategoryParent.transform, TempitemDetail);

                        //int loopStart = GetDownloadedNumber(TempEnumVar);
                        //for (int i = loopStart; i < characterBodyParts.hairColor.Count; i++)
                        //{
                        //    yield return new WaitForEndOfFrame();
                        //    InstantiateStoreItems(TempSubcategoryParent.transform, i, characterBodyParts.hairColor[i].ToString(), TempitemDetail);
                        //}
                    }
                    break;
                }

            case 10: // EyeBrow_Case - Added By WaqasAhmad
                {
                    if (!colorMode)
                    {
                        //TempSubcategoryParent = ParentOfBtnsAvatarEyeBrows;
                        myIndexInList = panelIndex;
                        TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                        TempEnumVar = EnumClass.CategoryEnum.EyeBrowAvatar;
                        eyeBrowTapButton.SetActive(true);

                        break; // currently there is no eyebrow available

                        for (int i = 1; i < AllCategoriesData[20].parentObj.transform.childCount; i++)
                        {
                            if (AllCategoriesData[20].parentObj.transform.GetChild(i).GetComponent<ItemDetail>().id.ParseToInt() == SaveCharacterProperties.instance.characterController.eyeBrowId)
                                AllCategoriesData[20].parentObj.transform.GetChild(i).GetComponent<Image>().enabled = true;
                            else
                                AllCategoriesData[20].parentObj.transform.GetChild(i).GetComponent<Image>().enabled = false;
                        }
                        ActivateGenerateItemsCoroutine(TempSubcategoryParent.transform, TempitemDetail);
                    }
                    else
                    {
                        //TempSubcategoryParent = ParentOfBtnsCustomEyeBrows;
                        myIndexInList = 20;
                        TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                        TempEnumVar = EnumClass.CategoryEnum.EyeBrowAvatarColor;

                        int loopStart = GetDownloadedNumber(TempEnumVar);
                        for (int i = loopStart; i < characterBodyParts.eyeBrowsColor.Count; i++)
                        {
                            yield return new WaitForEndOfFrame();
                            InstantiateStoreItems(TempSubcategoryParent.transform, i, characterBodyParts.eyeBrowsColor[i].ToString(), TempitemDetail);
                        }
                    }
                    break;
                }
            case 11: // Eyes
                {
                    if (!colorMode)
                    {
                        //TempSubcategoryParent = ParentOfBtnsCustomEyes;
                        myIndexInList = 22;
                        TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                        TempEnumVar = EnumClass.CategoryEnum.EyesAvatar;
                    }
                    else
                    {
                        //TempSubcategoryParent = ParentOfBtnsCustomEyesPalette;
                        myIndexInList = 23;
                        TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                        TempEnumVar = EnumClass.CategoryEnum.EyesAvatarColor;

                        int loopStart = GetDownloadedNumber(TempEnumVar);
                        for (int i = loopStart; i < characterBodyParts.eyeColor.Count; i++)
                        {
                            yield return new WaitForEndOfFrame();
                            InstantiateStoreItems(TempSubcategoryParent.transform, i, characterBodyParts.eyeColor[i].ToString(), TempitemDetail);
                        }
                    }
                    break;
                }
            case 13: // Lips
                {
                    if (!colorMode)
                    {
                        //TempSubcategoryParent = ParentOfBtnsCustomLips;
                        myIndexInList = 24;
                        TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                        TempEnumVar = EnumClass.CategoryEnum.LipsAvatar;
                    }
                    else
                    {
                        //TempSubcategoryParent = ParentOfBtnsCustomLipsPalette;
                        myIndexInList = 25;
                        TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                        TempEnumVar = EnumClass.CategoryEnum.LipsAvatarColor;

                        int loopStart = GetDownloadedNumber(TempEnumVar);
                        for (int i = loopStart; i < characterBodyParts.lipColorPalette.Count; i++)
                        {
                            yield return new WaitForEndOfFrame();
                            InstantiateStoreItems(TempSubcategoryParent.transform, i, characterBodyParts.lipColorPalette[i].ToString(), TempitemDetail);
                        }
                    }
                    break;
                }
            case 15: // SkinTone
                {
                    //TempSubcategoryParent = ParentOfBtnsCustomSkin;
                    myIndexInList = 26;
                    TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                    TempEnumVar = EnumClass.CategoryEnum.SkinToneAvatar;

                    int loopStart = GetDownloadedNumber(TempEnumVar);
                    for (int i = loopStart; i < characterBodyParts.skinColor.Count; i++)
                    {
                        yield return new WaitForEndOfFrame();
                        InstantiateStoreItems(TempSubcategoryParent.transform, i, characterBodyParts.skinColor[i].ToString(), TempitemDetail);
                    }

                    break;
                }
            case 16: // EyeLashes
                {// EyeBrowPoints

                    //TempSubcategoryParent = ParentOfBtnsAvatarEyeLashes;
                    myIndexInList = 11;
                    TempSubcategoryParent = AllCategoriesData[myIndexInList].parentObj.transform;
                    TempEnumVar = EnumClass.CategoryEnum.EyeLashesAvatar;

                    for (int i = 1; i < AllCategoriesData[11].parentObj.transform.childCount; i++)
                    {
                        if (AllCategoriesData[11].parentObj.transform.GetChild(i).GetComponent<ItemDetail>().id.ParseToInt() == SaveCharacterProperties.instance.characterController.eyeLashesId)
                            AllCategoriesData[11].parentObj.transform.GetChild(i).GetComponent<Image>().enabled = true;
                        else
                            AllCategoriesData[11].parentObj.transform.GetChild(i).GetComponent<Image>().enabled = false;
                    }
                    ActivateGenerateItemsCoroutine(TempSubcategoryParent.transform, TempitemDetail);
                    break;
                }
        }


        if (TempSubcategoryParent != null && TempitemDetail.Count > 0)
        {
            UpdateColor(buttonIndex);
            if (buttonIndex != -1)
            {
                UpdateStoreSelection(buttonIndex);
            }
        }
    }

    void ActivateGenerateItemsCoroutine(Transform parentObj, List<ItemDetail> TempitemDetail)
    {
        if (_generateCoroutin != null)
            StopCoroutine(_generateCoroutin);
        _generateCoroutin = StartCoroutine(GenerateItemsBtn(parentObj, TempitemDetail));
    }

    private IEnumerator GenerateItemsBtn(Transform parentObj, List<ItemDetail> TempitemDetail)
    {
        int _LoopStart = GetDownloadedNumber(TempEnumVar);
        const int _ItemsPerPage = 40; // Getting 40 items from the server in a single page
        int _CurrentPageIndex = GetActivePanelPageIndex() - 1;
        int _MaxItems = (_CurrentPageIndex * _ItemsPerPage) + dataListOfItems.Count;

        // Calculate the starting index for the data list
        int _StartFromIndex = _MaxItems - dataListOfItems.Count;
        _ResettingAssetList = false;
        Debug.Log(" issue could be here current inde x = " + _CurrentPageIndex + " start items " + _StartFromIndex);
        for (int i = _LoopStart; i < _MaxItems; i++)
        {
            int _DataIndex = i - _StartFromIndex;

            if (_DataIndex < 0)
                break;

            // Wait until the end of the frame to instantiate the item
            yield return new WaitForEndOfFrame();
            if (_ResettingAssetList == false && parentObj == TempSubcategoryParent)
            {
                InstantiateStoreItems(parentObj, _DataIndex, string.Empty, TempitemDetail, false);
            }

        }

        // Indicate that all data has been loaded
        loadingItems = false;
    }

    CharacterHandler _charHandler;
    int localcount = 0;
    void InstantiateStoreItems(Transform parentObj, int objId, string objName, List<ItemDetail> TempitemDetail, bool useDefaultValue = true)
    {
        localcount++;
        if (string.IsNullOrEmpty(dataListOfItems[objId].iconLink) && parentObj != TempSubcategoryParent && _ResettingAssetList == false)
        {
            Debug.Log("Icon Link is Empty or Parent Object is not same");
            return;
        }

        _charHandler = CharacterHandler.instance;
        GameObject L_ItemBtnObj = Instantiate(ItemsBtnPrefab, parentObj);
        L_ItemBtnObj.transform.parent = parentObj;
        L_ItemBtnObj.transform.localScale = new Vector3(1, 1, 1);
        ItemDetail abc = L_ItemBtnObj.GetComponent<ItemDetail>();

        // Implement Image Kit URL
        string ImageKitUrl = dataListOfItems[objId].iconLink;
        ImageKitUrl = ImageKitUrl.Replace("https://cdn.xana.net/xanaprod/Defaults/", "https://aydvewoyxq.cloudimg.io/_xanaprod_/xanaprod/Defaults/");
        ImageKitUrl += "?width=128&height=128";


        abc.iconLink = useDefaultValue ? "" : ImageKitUrl;
        abc.id = useDefaultValue ? (objId + 1).ToString() : dataListOfItems[objId].id.ToString();
        abc.isFavourite = useDefaultValue ? "False" : dataListOfItems[objId].isFavourite.ToString();
        abc.isOccupied = useDefaultValue ? "False" : dataListOfItems[objId].isOccupied.ToString();
        abc.isPaid = useDefaultValue ? "False" : dataListOfItems[objId].isPaid.ToString();
        abc.isPurchased = useDefaultValue ? "true" : dataListOfItems[objId].isPurchased.ToString();
        abc.name = useDefaultValue ? objName : dataListOfItems[objId].name.ToString(); ;
        abc.price = useDefaultValue ? "0" : dataListOfItems[objId].price;
        abc.categoryId = useDefaultValue ? "0" : dataListOfItems[objId].categoryId.ToString();
        abc.subCategory = useDefaultValue ? "0" : dataListOfItems[objId].subCategoryId.ToString();
        abc.MyIndex = objId;
        abc.CategoriesEnumVar = TempEnumVar;

        UpdateCategoryDownloadedInt(TempEnumVar);
        L_ItemBtnObj.SetActive(true);
        if (abc.transform.parent.gameObject.activeSelf)
        {
            abc.StartRun();
            abc.enableUpdate = true;
        }
        else
        {
            abc.enableUpdate = true;
        }

        TempitemDetail.Add(abc);

        // Save Items References
        SubitemData subitemData = new SubitemData();
        subitemData.gender = dataListOfItems[objId].assetGender;
        subitemData.obj = L_ItemBtnObj;

        if (AllCategoriesData[myIndexInList].subItems == null)
            AllCategoriesData[myIndexInList].subItems = new List<SubitemData>();

        AllCategoriesData[myIndexInList].subItems.Add(subitemData);
        {
            // Initilize Item Specific for Gender
            // Getting int values from Server for assets Gender 0 for male and 1 for female
            // In Xana we are uisng string for geneder specificaton

            if ((_charHandler.activePlayerGender == AvatarGender.Male && !dataListOfItems[objId].assetGender.Equals("0")) ||
                (_charHandler.activePlayerGender == AvatarGender.Female && !dataListOfItems[objId].assetGender.Equals("1")))
            {
                Debug.Log("Gender not Matched With Asset");
                L_ItemBtnObj.SetActive(false);
            }
        }

    }


    int GetDownloadedNumber(EnumClass.CategoryEnum categoryEnum)
    {
        string _MyGender = CharacterHandler.instance.activePlayerGender.ToString();
        switch (TempEnumVar)
        {
            case EnumClass.CategoryEnum.Head:
                return headsDownlaodedCount;
            case EnumClass.CategoryEnum.Face:
                return faceDownlaodedCount;
            case EnumClass.CategoryEnum.Inner:
                if (_MyGender == "Female")
                    return innerDownlaodedCountFemale;
                else
                    return innerDownlaodedCount;
            case EnumClass.CategoryEnum.Outer:
                if (_MyGender == "Female")
                    return outerDownlaodedCountFemale;
                else
                    return outerDownlaodedCount;
            case EnumClass.CategoryEnum.Accesary:
                return accesaryDownlaodedCount;
            case EnumClass.CategoryEnum.Bottom:
                if (_MyGender == "Female")
                    return bottomDownlaodedCountFemale;
                else
                    return bottomDownlaodedCount;
            case EnumClass.CategoryEnum.Socks:
                return socksDownlaodedCount;
            case EnumClass.CategoryEnum.Shoes:
                if (_MyGender == "Female")
                    return shoesDownlaodedCountFemale;
                else
                    return shoesDownlaodedCount;
            case EnumClass.CategoryEnum.HairAvatar:
                if (_MyGender == "Female")
                    return hairDwonloadedCountFemale;
                else
                    return hairDwonloadedCount;
            case EnumClass.CategoryEnum.HairAvatarColor:
                return HairColorDwonloadedCount;
            case EnumClass.CategoryEnum.EyesAvatarColor:
                return EyesColorDwonloadedCount;
            case EnumClass.CategoryEnum.LipsAvatarColor:
                return LipsColorDwonloadedCount;
            case EnumClass.CategoryEnum.SkinToneAvatar:
                return skinColorDwonloadedCount;
            case EnumClass.CategoryEnum.EyeBrowAvatar:
                return eyeBrowDwonloadedCount;
            case EnumClass.CategoryEnum.EyeBrowAvatarColor:
                return eyeBrowColorDwonloadedCount;
            case EnumClass.CategoryEnum.EyeLashesAvatar:
                return eyeLashesDwonloadedCount;
            case EnumClass.CategoryEnum.EyesAvatar:
                return eyesDwonloadedCount;
            case EnumClass.CategoryEnum.LipsAvatar:
                return lipsDwonloadedCount;

        }
        return 0;
    }

    void UpdateCategoryDownloadedInt(EnumClass.CategoryEnum TempEnumVar)
    {
        string _MyGender = CharacterHandler.instance.activePlayerGender.ToString();

        switch (TempEnumVar)
        {
            case EnumClass.CategoryEnum.Head:
                {
                    headsDownlaodedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.Face:
                {
                    faceDownlaodedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.Inner:
                {
                    if (_MyGender == "Female")
                        innerDownlaodedCountFemale++;
                    else
                        innerDownlaodedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.Outer:
                {
                    if (_MyGender == "Female")
                        outerDownlaodedCountFemale++;
                    else
                        outerDownlaodedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.Accesary:
                {
                    accesaryDownlaodedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.Bottom:
                {
                    if (_MyGender == "Female")
                        bottomDownlaodedCountFemale++;
                    else
                        bottomDownlaodedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.Socks:
                {
                    socksDownlaodedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.Shoes:
                {
                    if (_MyGender == "Female")
                        shoesDownlaodedCountFemale++;
                    else
                        shoesDownlaodedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.HairAvatar:
                {
                    if (_MyGender == "Female")
                        hairDwonloadedCountFemale++;
                    else
                        hairDwonloadedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.HairAvatarColor:
                {
                    HairColorDwonloadedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.EyesAvatarColor:
                {
                    EyesColorDwonloadedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.LipsAvatarColor:
                {
                    LipsColorDwonloadedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.SkinToneAvatar:
                {
                    skinColorDwonloadedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.EyeBrowAvatar:
                {
                    eyeBrowDwonloadedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.EyeBrowAvatarColor:
                {
                    eyeBrowColorDwonloadedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.EyeLashesAvatar:
                {
                    eyeLashesDwonloadedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.EyesAvatar:
                {
                    eyesDwonloadedCount++;
                    break;
                }
            case EnumClass.CategoryEnum.LipsAvatar:
                {
                    lipsDwonloadedCount++;
                    break;
                }

        }
    }

    public void LoadLocalItems()
    {

    }
    //UNDO REDO FUNCTIONALITY------------------

    public List<UndoRedoDataClass> UndoRedoList = new List<UndoRedoDataClass>();
    public int CurrentIndex = -1;
    [Serializable]
    public class UndoRedoDataClass
    {
        public Item ClothTex_Item = new Item();
    }

    IEnumerator CharacterChange()
    {
        yield return new WaitForSeconds(3.5f);

        UpdateStoreSelection(0);
    }



    //public void UndoFunc()
    //{
    //    //UndoSelection();
    //    //RedoBtn.GetComponent<Button>().interactable = true;
    //    //InventoryManager.instance.SaveStoreBtn.SetActive(true);
    //    //InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
    //    //InventoryManager.instance.GreyRibbonImage.SetActive(false);
    //    //InventoryManager.instance.WhiteRibbonImage.SetActive(true);

    //    //if (CurrentIndex != 0)
    //    //{
    //    //    if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType != UndoRedoList[CurrentIndex - 1].ClothTex_Item.ItemType)
    //    //        CurrentIndex--;
    //    //    else
    //    //        CurrentIndex--;
    //    //}
    //    //else
    //    //{
    //    //    CurrentIndex--;
    //    //}

    //    //if (CurrentIndex < 0)
    //    //    CurrentIndex = 0;


    //    //if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Lip" || UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Eyes" || UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Skin")
    //    //{
    //    //    _DownloadRigClothes.BindExistingClothes(UndoRedoList[CurrentIndex].ClothTex_Item.ItemType, UndoRedoList[CurrentIndex].ClothTex_Item.ItemName);

    //    //    if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Lip")
    //    //    {
    //    //        ConstantsHolder.xanaConstants.lipColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
    //    //    }

    //    //    else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Eyes")
    //    //    {
    //    //        ConstantsHolder.xanaConstants.eyeColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
    //    //    }

    //    //    else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Skin")
    //    //    {
    //    //        ConstantsHolder.xanaConstants.skinColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
    //    //    }
    //    //}
    //    //else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "BodyFat")
    //    //{

    //    //    AvatarCustomizationManager.Instance.UpdateChBodyShape(UndoRedoList[CurrentIndex].ClothTex_Item.ItemID);
    //    //    ConstantsHolder.xanaConstants.bodyNumber = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID;
    //    //}
    //    //else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Preset")
    //    //{
    //    //    if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemName == "Zero")
    //    //    {
    //    //        GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne, 0);
    //    //    }
    //    //    else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemName == "One")
    //    //    {
    //    //        GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemID, 100);
    //    //        GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne, 0);
    //    //    }
    //    //    else
    //    //    {
    //    //        UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().CustomizationTriggerTwo();
    //    //    }
    //    //    GameObject tmp = UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab;

    //    //    if (tmp)
    //    //    {
    //    //        if (tmp.transform.IsChildOf(ParentOfBtnsAvatarFace))
    //    //        {
    //    //            if (tmp.GetComponent<BodyCustomizationTrigger>())
    //    //                ConstantsHolder.xanaConstants.faceIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
    //    //            else
    //    //                ConstantsHolder.xanaConstants.faceIndex = 0;
    //    //        }

    //    //        else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarEyeBrows))
    //    //        {
    //    //            if (tmp.GetComponent<BodyCustomizationTrigger>())
    //    //                ConstantsHolder.xanaConstants.eyeBrowIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
    //    //            else
    //    //                ConstantsHolder.xanaConstants.eyeBrowIndex = 0;
    //    //        }

    //    //        else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarEyes))
    //    //        {
    //    //            if (tmp.GetComponent<BodyCustomizationTrigger>())
    //    //                ConstantsHolder.xanaConstants.eyeIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
    //    //            else
    //    //                ConstantsHolder.xanaConstants.eyeIndex = 0;
    //    //        }

    //    //        else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarNose))
    //    //        {
    //    //            if (tmp.GetComponent<BodyCustomizationTrigger>())
    //    //                ConstantsHolder.xanaConstants.noseIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
    //    //            else
    //    //                ConstantsHolder.xanaConstants.noseIndex = 0;
    //    //        }

    //    //        else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarLips))
    //    //        {
    //    //            if (tmp.GetComponent<BodyCustomizationTrigger>())
    //    //                ConstantsHolder.xanaConstants.lipIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
    //    //            else
    //    //                ConstantsHolder.xanaConstants.lipIndex = 0;
    //    //        }
    //    //    }

    //    //}
    //    //else
    //    //{
    //    //    GameManager.Instance.EquipUiObj.ChangeCostume(UndoRedoList[CurrentIndex].ClothTex_Item.ItemName.ToLower());
    //    //}

    //    //if (CurrentIndex == 0)
    //    //{
    //    //    UndoBtn.GetComponent<Button>().interactable = false;
    //    //}

    //    //if (!ParentOfBtnsCustomEyes.gameObject.activeSelf && !ParentOfBtnsCustomLips.gameObject.activeSelf && !ParentOfBtnsCustomSkin.gameObject.activeSelf)
    //    //    UpdateStoreSelection(ConstantsHolder.xanaConstants.currentButtonIndex);

    //    //else
    //    //    UpdateColor(ConstantsHolder.xanaConstants.currentButtonIndex);
    //}
    //public void RedoFunc()
    //{
    //    //UndoSelection();
    //    //InventoryManager.instance.SaveStoreBtn.SetActive(true);
    //    //InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
    //    //InventoryManager.instance.GreyRibbonImage.SetActive(false);
    //    //InventoryManager.instance.WhiteRibbonImage.SetActive(true);

    //    //if (CurrentIndex < UndoRedoList.Count - 1)
    //    //{
    //    //    if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType != UndoRedoList[CurrentIndex + 1].ClothTex_Item.ItemType)
    //    //        CurrentIndex++;
    //    //    else
    //    //        CurrentIndex++;
    //    //}

    //    //else
    //    //{
    //    //    CurrentIndex++;
    //    //}

    //    //if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Lip" || UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Eyes" || UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Skin")
    //    //{
    //    //    _DownloadRigClothes.BindExistingClothes(UndoRedoList[CurrentIndex].ClothTex_Item.ItemType, UndoRedoList[CurrentIndex].ClothTex_Item.ItemName);

    //    //    if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Lip")
    //    //    {
    //    //        ConstantsHolder.xanaConstants.lipColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
    //    //    }

    //    //    else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Eyes")
    //    //    {
    //    //        ConstantsHolder.xanaConstants.eyeColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
    //    //    }

    //    //    else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Skin")
    //    //    {
    //    //        ConstantsHolder.xanaConstants.skinColor = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID.ToString();
    //    //    }
    //    //}
    //    //else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "BodyFat")
    //    //{
    //    //    AvatarCustomizationManager.Instance.UpdateChBodyShape(UndoRedoList[CurrentIndex].ClothTex_Item.ItemID);
    //    //    ConstantsHolder.xanaConstants.bodyNumber = UndoRedoList[CurrentIndex].ClothTex_Item.ItemID;
    //    //}
    //    //else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemType == "Preset")
    //    //{
    //    //    if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemName == "Zero")
    //    //    {
    //    //        GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne, 0);
    //    //    }
    //    //    else if (UndoRedoList[CurrentIndex].ClothTex_Item.ItemName == "One")
    //    //    {
    //    //        GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemID, 100);
    //    //        GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne, 0);
    //    //    }
    //    //    else
    //    //    {
    //    //        UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab.GetComponent<BodyCustomizationTrigger>().CustomizationTriggerTwo();
    //    //    }

    //    //    GameObject tmp = UndoRedoList[CurrentIndex].ClothTex_Item.ItemPrefab;

    //    //    if (tmp)
    //    //    {
    //    //        if (tmp.transform.IsChildOf(ParentOfBtnsAvatarFace))
    //    //        {
    //    //            if (tmp.GetComponent<BodyCustomizationTrigger>())
    //    //                ConstantsHolder.xanaConstants.faceIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
    //    //            else
    //    //                ConstantsHolder.xanaConstants.faceIndex = 0;
    //    //        }

    //    //        else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarEyeBrows))
    //    //        {
    //    //            if (tmp.GetComponent<BodyCustomizationTrigger>())
    //    //                ConstantsHolder.xanaConstants.eyeBrowIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
    //    //            else
    //    //                ConstantsHolder.xanaConstants.eyeBrowIndex = 0;
    //    //        }

    //    //        else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarEyes))
    //    //        {
    //    //            if (tmp.GetComponent<BodyCustomizationTrigger>())
    //    //                ConstantsHolder.xanaConstants.eyeIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
    //    //            else
    //    //                ConstantsHolder.xanaConstants.eyeIndex = 0;
    //    //        }

    //    //        else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarNose))
    //    //        {
    //    //            if (tmp.GetComponent<BodyCustomizationTrigger>())
    //    //                ConstantsHolder.xanaConstants.noseIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
    //    //            else
    //    //                ConstantsHolder.xanaConstants.noseIndex = 0;
    //    //        }

    //    //        else if (tmp.transform.IsChildOf(ParentOfBtnsAvatarLips))
    //    //        {
    //    //            if (tmp.GetComponent<BodyCustomizationTrigger>())
    //    //                ConstantsHolder.xanaConstants.lipIndex = tmp.GetComponent<BodyCustomizationTrigger>().f_BlendShapeOne;
    //    //            else
    //    //                ConstantsHolder.xanaConstants.lipIndex = 0;
    //    //        }
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    GameManager.Instance.EquipUiObj.ChangeCostume(UndoRedoList[CurrentIndex].ClothTex_Item.ItemName.ToLower());
    //    //}

    //    //if (CurrentIndex == UndoRedoList.Count - 1)
    //    //{
    //    //    RedoBtn.GetComponent<Button>().interactable = false;
    //    //}

    //    //UndoBtn.GetComponent<Button>().interactable = true;

    //    //if (!ParentOfBtnsCustomEyes.gameObject.activeSelf && !ParentOfBtnsCustomLips.gameObject.activeSelf && !ParentOfBtnsCustomSkin.gameObject.activeSelf)
    //    //    UpdateStoreSelection(ConstantsHolder.xanaConstants.currentButtonIndex);

    //    //else
    //    //    UpdateColor(ConstantsHolder.xanaConstants.currentButtonIndex);
    //}
    private void OnApplicationQuit()
    {
        //PresetData_Jsons.lastSelectedPresetName = null;
    }

    // AR change start
    public void ForcellySetLastClickedBtnOfHair()
    {
        for (int i = 0; i < AllCategoriesData[8].parentObj.transform.childCount; i++)
        {
            childObject = AllCategoriesData[8].parentObj.transform.GetChild(i).gameObject;
            if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.hair)
            {
                childObject.GetComponent<Image>().enabled = true;
                ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                //Debug.Log("<color=red>InventoryManager AssignLastClickedBtnHere</color>");
                ConstantsHolder.xanaConstants.avatarStoreSelection[0] = childObject;

                CheckForItemDetail(ConstantsHolder.xanaConstants.hair, 3);
            }
        }
    }
    // AR changes end

    public void UpdateStoreSelection(int index)
    {
        ////Debug.Log("<color=blue>Update Store Selection: "+index+"</color>");
        switch (index)
        {
            case 0:
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(1);
                if (ConstantsHolder.xanaConstants.hair != "")
                {
                    if (GameManager.Instance.mainCharacter.GetComponent<AvatarController>().wornHair.name == "MDhairs")
                    {
                        // //Debug.Log("Hairs list------"+ ParentOfBtnsAvatarHairs.transform.childCount);
                        ////Debug.Log("<color=blue>Store Selection if</color>");
                        for (int i = 0; i < AllCategoriesData[8].parentObj.transform.childCount; i++)
                        {
                            childObject = AllCategoriesData[8].parentObj.transform.GetChild(i).gameObject;
                            if (childObject.GetComponent<Image>().enabled)
                                childObject.GetComponent<Image>().enabled = false;
                        }



                    }
                    else if (!AllCategoriesData[19].parentObj.activeSelf)
                    {
                        // //Debug.Log("<color=blue>Store Selection else</color>");
                        ////Debug.Log("Hairs list 2------" + ParentOfBtnsAvatarHairs.transform.childCount);
                        for (int i = 0; i < AllCategoriesData[8].parentObj.transform.childCount; i++)
                        {
                            childObject = AllCategoriesData[8].parentObj.transform.GetChild(i).gameObject;
                            if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.hair)
                            {
                                //  //Debug.Log("<color=blue>Enabled Selection</color>");
                                childObject.GetComponent<Image>().enabled = true;
                                ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                                // //Debug.Log("<color=red>InventoryManager AssignLastClickedBtnHere</color>");
                                ConstantsHolder.xanaConstants.avatarStoreSelection[0] = childObject;

                                CheckForItemDetail(ConstantsHolder.xanaConstants.hair, 3);

                                break;
                            }
                        }
                    }



                }

                break;

            case 1:
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(1);
                if (!ConstantsHolder.xanaConstants.isFaceMorphed)
                {
                    if (ConstantsHolder.xanaConstants.faceIndex != -1)
                    {
                        for (int i = 0; i < faceAvatarButton.Length; i++)
                        {
                            if (faceAvatarButton[i].GetComponent<AvatarBtn>().AvatarBtnId == ConstantsHolder.xanaConstants.faceIndex)
                            {
                                faceAvatarButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                ConstantsHolder.xanaConstants._lastAvatarClickedBtn = faceAvatarButton[i];
                                ConstantsHolder.xanaConstants.avatarStoreSelection[1] = faceAvatarButton[i];

                                CheckForAvatarBtn(ConstantsHolder.xanaConstants.faceIndex, "face");
                                break;
                            }
                        }
                    }

                    //else
                    //{

                    //    int childNumber = ParentOfBtnsAvatarFace.transform.childCount - 1;

                    //    ParentOfBtnsAvatarFace.transform.GetChild(childNumber).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    //    ConstantsHolder.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarFace.transform.GetChild(childNumber).gameObject;
                    //    ConstantsHolder.xanaConstants.avatarStoreSelection[1] = ParentOfBtnsAvatarFace.transform.GetChild(childNumber).gameObject;

                    //    CheckForAvatarBtn(0, "face");
                    //}
                }

                else
                {
                    faceTapButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                    if (ConstantsHolder.xanaConstants._lastClickedBtn)
                    {
                        if (ConstantsHolder.xanaConstants._lastClickedBtn.name == faceTapButton.name)
                        {
                            ConstantsHolder.xanaConstants._lastClickedBtn = null;
                            ConstantsHolder.xanaConstants.avatarStoreSelection[1] = null;
                        }
                    }

                    saveButtonPressed = false;
                }

                break;

            case 2:
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(1);
                if (!ConstantsHolder.xanaConstants.isEyebrowMorphed)
                {
                    if (ConstantsHolder.xanaConstants.eyeBrowIndex != -1)
                    {
                        for (int i = 0; i < eyeBrowsAvatarButton.Length; i++)
                        {
                            if (eyeBrowsAvatarButton[i].GetComponent<AvatarBtn>().AvatarBtnId == ConstantsHolder.xanaConstants.eyeBrowIndex)
                            {
                                eyeBrowsAvatarButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                ConstantsHolder.xanaConstants._lastAvatarClickedBtn = eyeBrowsAvatarButton[i];
                                ConstantsHolder.xanaConstants.avatarStoreSelection[2] = eyeBrowsAvatarButton[i];

                                CheckForAvatarBtn(ConstantsHolder.xanaConstants.eyeBrowIndex, "eyeBrow");

                                break;
                            }
                        }
                        for (int i = 1; i < AllCategoriesData[10].parentObj.transform.childCount; i++)
                        {
                            childObject = AllCategoriesData[10].parentObj.transform.GetChild(i).gameObject;
                            if (childObject.GetComponent<ItemDetail>().id.ParseToInt() == SaveCharacterProperties.instance.characterController.eyeBrowId)
                            {
                                childObject.GetComponent<Image>().enabled = true;
                                ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                            }
                            else
                                childObject.GetComponent<Image>().enabled = false;
                        }

                        // Activate Eyebrow Customization Btn
                        if (AllCategoriesData[10].parentObj.transform.childCount > 2)
                            eyeBrowTapButton.SetActive(true);
                    }

                    //else
                    //{

                    //    int childNumber = ParentOfBtnsAvatarEyeBrows.transform.childCount - 1;
                    //    print("child number is = " + childNumber);
                    //    ParentOfBtnsAvatarEyeBrows.transform.GetChild(childNumber).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    //    ConstantsHolder.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarEyeBrows.transform.GetChild(childNumber).gameObject;
                    //    ConstantsHolder.xanaConstants.avatarStoreSelection[2] = ParentOfBtnsAvatarEyeBrows.transform.GetChild(childNumber).gameObject;

                    //    CheckForAvatarBtn(0, "eyeBrow");
                    //}
                }

                else
                {
                    eyeBrowTapButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                    if (ConstantsHolder.xanaConstants._lastClickedBtn)
                    {
                        if (ConstantsHolder.xanaConstants._lastClickedBtn.name == eyeBrowTapButton.name)
                        {
                            ConstantsHolder.xanaConstants._lastClickedBtn = null;
                            ConstantsHolder.xanaConstants.avatarStoreSelection[2] = null;
                        }
                    }

                    saveButtonPressed = false;
                }

                break;

            case 3:
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(1);
                if (panelIndex == 1)
                {
                    if (!ConstantsHolder.xanaConstants.isEyeMorphed)
                    {
                        if (ConstantsHolder.xanaConstants.eyeIndex != -1)
                        {
                            for (int i = 0; i < eyeAvatarButton.Length; i++)
                            {
                                if (eyeAvatarButton[i].GetComponent<AvatarBtn>().AvatarBtnId == ConstantsHolder.xanaConstants.eyeIndex)
                                {
                                    eyeAvatarButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                    ConstantsHolder.xanaConstants._lastAvatarClickedBtn = eyeAvatarButton[i];
                                    ConstantsHolder.xanaConstants.avatarStoreSelection[3] = eyeAvatarButton[i];

                                    CheckForAvatarBtn(ConstantsHolder.xanaConstants.eyeIndex, "eye");

                                    break;
                                }
                            }
                        }

                        //else
                        //{

                        //    int childNumber = ParentOfBtnsAvatarEyes.transform.childCount - 1;

                        //    ParentOfBtnsAvatarEyes.transform.GetChild(childNumber).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                        //    ConstantsHolder.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarEyes.transform.GetChild(childNumber).gameObject;
                        //    ConstantsHolder.xanaConstants.avatarStoreSelection[3] = ParentOfBtnsAvatarEyes.transform.GetChild(childNumber).gameObject;

                        //    CheckForAvatarBtn(0, "eye");
                        //}
                    }

                    else
                    {
                        eyeTapButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                        if (ConstantsHolder.xanaConstants._lastClickedBtn)
                        {
                            if (ConstantsHolder.xanaConstants._lastClickedBtn.name == eyeTapButton.name)
                            {
                                ConstantsHolder.xanaConstants._lastClickedBtn = null;
                                ConstantsHolder.xanaConstants.avatarStoreSelection[3] = null;
                            }
                        }

                        saveButtonPressed = false;
                    }
                }

                else
                {
                    if (ConstantsHolder.xanaConstants.shirt != "")
                    {
                        if (GameManager.Instance.mainCharacter.GetComponent<AvatarController>().wornShirt.name == "MDshirt")
                        {
                            for (int i = 0; i < AllCategoriesData[3].parentObj.transform.childCount; i++)
                            {
                                childObject = AllCategoriesData[3].parentObj.transform.GetChild(i).gameObject;
                                if (childObject.GetComponent<Image>().enabled)
                                    childObject.GetComponent<Image>().enabled = false;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < AllCategoriesData[3].parentObj.transform.childCount; i++)
                            {
                                childObject = AllCategoriesData[3].parentObj.transform.GetChild(i).gameObject;
                                if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.shirt)
                                {
                                    childObject.GetComponent<Image>().enabled = true;
                                    ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                                    ConstantsHolder.xanaConstants.wearableStoreSelection[0] = childObject;

                                    CheckForItemDetail(ConstantsHolder.xanaConstants.shirt, 1);

                                    break;
                                }
                            }
                        }
                    }
                }

                break;

            case 4:
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(1);
                if (panelIndex == 1)
                {
                    if (!ConstantsHolder.xanaConstants.isNoseMorphed)
                    {
                        if (ConstantsHolder.xanaConstants.noseIndex != -1)
                        {
                            for (int i = 0; i < noseAvatarButton.Length; i++)
                            {
                                if (noseAvatarButton[i].GetComponent<AvatarBtn>().AvatarBtnId == ConstantsHolder.xanaConstants.noseIndex)
                                {
                                    noseAvatarButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                    ConstantsHolder.xanaConstants._lastAvatarClickedBtn = noseAvatarButton[i];
                                    ConstantsHolder.xanaConstants.avatarStoreSelection[4] = noseAvatarButton[i];

                                    CheckForAvatarBtn(ConstantsHolder.xanaConstants.noseIndex, "nose");

                                    break;
                                }
                            }
                        }

                        //else
                        //{

                        //    int childNumber = ParentOfBtnsAvatarNose.transform.childCount - 1;

                        //    ParentOfBtnsAvatarNose.transform.GetChild(childNumber).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                        //    ConstantsHolder.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarNose.transform.GetChild(childNumber).gameObject;
                        //    ConstantsHolder.xanaConstants.avatarStoreSelection[4] = ParentOfBtnsAvatarNose.transform.GetChild(childNumber).gameObject;

                        //    CheckForAvatarBtn(0, "nose");
                        //}
                    }

                    else
                    {
                        noseTapButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                        if (ConstantsHolder.xanaConstants._lastClickedBtn)
                        {
                            if (ConstantsHolder.xanaConstants._lastClickedBtn.name == noseTapButton.name)
                            {
                                ConstantsHolder.xanaConstants._lastClickedBtn = null;
                                ConstantsHolder.xanaConstants.avatarStoreSelection[4] = null;
                            }
                        }

                        saveButtonPressed = false;
                    }
                }
                else
                {
                    if (ConstantsHolder.xanaConstants.PresetValueString != "")
                    {
                        for (int i = 0; i < AllCategoriesData[4].parentObj.transform.childCount; i++)
                        {
                            childObject = AllCategoriesData[4].parentObj.transform.GetChild(i).gameObject;
                            childObject.transform.GetChild(0).gameObject.SetActive(false);
                            if (childObject.name == ConstantsHolder.xanaConstants.PresetValueString)
                            {
                                ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                                childObject.transform.GetChild(0).gameObject.SetActive(true);
                                ConstantsHolder.xanaConstants.wearableStoreSelection[3] = childObject;
                            }
                        }
                    }
                }
                break;

            case 5:
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(1);
                if (panelIndex == 1)
                {
                    if (!ConstantsHolder.xanaConstants.isLipMorphed)
                    {
                        if (ConstantsHolder.xanaConstants.lipIndex != -1)
                        {
                            for (int i = 0; i < lipAvatarButton.Length; i++)
                            {
                                if (lipAvatarButton[i].GetComponent<AvatarBtn>().AvatarBtnId == ConstantsHolder.xanaConstants.lipIndex)
                                {
                                    lipAvatarButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                    ConstantsHolder.xanaConstants._lastAvatarClickedBtn = lipAvatarButton[i];
                                    ConstantsHolder.xanaConstants.avatarStoreSelection[5] = lipAvatarButton[i];

                                    CheckForAvatarBtn(ConstantsHolder.xanaConstants.lipIndex, "lip");

                                    break;
                                }
                            }
                        }

                        //else
                        //{

                        //    int childNumber = ParentOfBtnsAvatarLips.transform.childCount - 1;

                        //    ParentOfBtnsAvatarLips.transform.GetChild(childNumber).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                        //    ConstantsHolder.xanaConstants._lastClickedBtn = ParentOfBtnsAvatarLips.transform.GetChild(childNumber).gameObject;
                        //    ConstantsHolder.xanaConstants.avatarStoreSelection[5] = ParentOfBtnsAvatarLips.transform.GetChild(childNumber).gameObject;

                        //    CheckForAvatarBtn(0, "lip");
                        //}
                    }

                    else
                    {
                        lipTapButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                        if (ConstantsHolder.xanaConstants._lastClickedBtn)
                        {
                            if (ConstantsHolder.xanaConstants._lastClickedBtn.name == lipTapButton.name)
                            {
                                ConstantsHolder.xanaConstants._lastClickedBtn = null;
                                ConstantsHolder.xanaConstants.avatarStoreSelection[5] = null;
                            }
                        }

                        saveButtonPressed = false;
                    }
                }

                else
                {
                    if (ConstantsHolder.xanaConstants.pants != "")
                    {
                        ////Debug.Log(ParentOfBtnsForOuter.transform.childCount);
                        if (GameManager.Instance.mainCharacter.GetComponent<AvatarController>().wornPant != null)
                        {
                            if (GameManager.Instance.mainCharacter.GetComponent<AvatarController>().wornPant.name == "MDpant")
                            {
                                for (int i = 0; i < AllCategoriesData[5].parentObj.transform.childCount; i++)
                                {
                                    childObject = AllCategoriesData[5].parentObj.transform.GetChild(i).gameObject;
                                    if (childObject.GetComponent<Image>().enabled)
                                        childObject.GetComponent<Image>().enabled = false;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < AllCategoriesData[5].parentObj.transform.childCount; i++)
                                {
                                    childObject = AllCategoriesData[5].parentObj.transform.GetChild(i).gameObject;
                                    if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.pants)
                                    {
                                        childObject.GetComponent<Image>().enabled = true;
                                        ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                                        ConstantsHolder.xanaConstants.wearableStoreSelection[1] = childObject;

                                        CheckForItemDetail(ConstantsHolder.xanaConstants.pants, 0);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                break;

            case 6:
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(2);
                if (ConstantsHolder.xanaConstants.bodyNumber != -1)
                {
                    for (int i = 0; i < AllCategoriesData[15].parentObj.transform.childCount; i++)
                    {
                        childObject = AllCategoriesData[15].parentObj.transform.GetChild(i).gameObject;
                        if (childObject.GetComponent<AvatarBtn>()._Bodyint == ConstantsHolder.xanaConstants.bodyNumber)
                        {
                            childObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                            ConstantsHolder.xanaConstants._lastAvatarClickedBtn = childObject;
                            ConstantsHolder.xanaConstants.avatarStoreSelection[6] = childObject;
                            break;
                        }
                    }
                }

                break;

            case 7:
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(2);
                if (panelIndex == 0)
                {
                    if (ConstantsHolder.xanaConstants.shoes != "")
                    {
                        GameObject currentShoes = GameManager.Instance.mainCharacter.GetComponent<AvatarController>().wornShoes;
                        if (currentShoes && GameManager.Instance.mainCharacter.GetComponent<AvatarController>().wornShoes.name == "MDshoes")
                        {
                            for (int i = 0; i < AllCategoriesData[7].parentObj.transform.childCount; i++)
                            {
                                childObject = AllCategoriesData[7].parentObj.transform.GetChild(i).gameObject;
                                if (childObject.GetComponent<Image>().enabled)
                                    childObject.GetComponent<Image>().enabled = false;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < AllCategoriesData[7].parentObj.transform.childCount; i++)
                            {
                                childObject = AllCategoriesData[7].parentObj.transform.GetChild(i).gameObject;
                                if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.shoes)
                                {
                                    childObject.GetComponent<Image>().enabled = true;
                                    ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                                    ConstantsHolder.xanaConstants.wearableStoreSelection[2] = childObject;

                                    CheckForItemDetail(ConstantsHolder.xanaConstants.shoes, 2);

                                    break;
                                }
                            }
                        }
                    }
                }

                break;
            case 8:
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(1);
                if (ConstantsHolder.xanaConstants.eyeLashesIndex != -1)
                {
                    for (int i = 0; i < AllCategoriesData[11].parentObj.transform.childCount; i++)
                    {
                        childObject = AllCategoriesData[11].parentObj.transform.GetChild(i).gameObject;
                        // Commented By Ahsan
                        //if (childObject.GetComponent<ItemDetail>().id.ParseToInt() == SaveCharacterProperties.instance.characterController.eyeLashesId)
                        //    childObject.GetComponent<Image>().enabled = true;
                        //else
                        //    childObject.GetComponent<Image>().enabled = false;

                        //if (childObject.GetComponent<AvatarBtn>().AvatarBtnId == ConstantsHolder.xanaConstants.eyeLashesIndex)
                        if (childObject.GetComponent<ItemDetail>().id == ConstantsHolder.xanaConstants.eyeLashesIndex.ToString())
                        {
                            childObject.GetComponent<Image>().enabled = true;
                            ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                            ConstantsHolder.xanaConstants.avatarStoreSelection[8] = childObject;
                            break;
                        }
                    }
                }
                break;
            case 9:
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(1);
                if (ConstantsHolder.xanaConstants.makeupIndex != -1)
                {
                    for (int i = 0; i < AllCategoriesData[17].parentObj.transform.childCount; i++)
                    {
                        childObject = AllCategoriesData[17].parentObj.transform.GetChild(i).gameObject;
                        if (childObject.GetComponent<AvatarBtn>().AvatarBtnId == ConstantsHolder.xanaConstants.makeupIndex)
                        {
                            childObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                            ConstantsHolder.xanaConstants._lastAvatarClickedBtn = childObject;
                            ConstantsHolder.xanaConstants.avatarStoreSelection[9] = childObject;
                            break;
                        }
                    }
                }
                break;
            case 10:
                GameManager.Instance.mainCharacter.GetComponent<FaceIK>().SetLookPos(2);
                if (ConstantsHolder.xanaConstants.PresetValueString != "")
                {

                    for (int i = 0; i < AllCategoriesData[18].parentObj.transform.childCount; i++)
                    {
                        childObject = AllCategoriesData[18].parentObj.transform.GetChild(i).gameObject;
                        childObject.transform.GetChild(0).gameObject.SetActive(false);
                        if (childObject.name == ConstantsHolder.xanaConstants.PresetValueString)
                        {
                            ConstantsHolder.xanaConstants._lastClickedBtn = childObject;
                            childObject.transform.GetChild(0).gameObject.SetActive(true);
                            ConstantsHolder.xanaConstants.avatarStoreSelection[ConstantsHolder.xanaConstants.currentButtonIndex] = childObject;
                        }
                    }
                }
                break;
        }
    }
    public void UndoSelection()
    {
        for (int i = 0; i < ConstantsHolder.xanaConstants.avatarStoreSelection.Length; i++)
        {
            if (ConstantsHolder.xanaConstants.avatarStoreSelection[i])
            {
                if (ConstantsHolder.xanaConstants.avatarStoreSelection[i].GetComponent<ItemDetail>())
                {
                    ConstantsHolder.xanaConstants.avatarStoreSelection[i].GetComponent<Image>().enabled = false;
                }
                else if (ConstantsHolder.xanaConstants.avatarStoreSelection[i].GetComponent<PresetData_Jsons>())
                {
                    ConstantsHolder.xanaConstants.avatarStoreSelection[i].transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    ConstantsHolder.xanaConstants.avatarStoreSelection[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                }

                ConstantsHolder.xanaConstants.avatarStoreSelection[i] = null;
            }
        }

        for (int i = 0; i < ConstantsHolder.xanaConstants.wearableStoreSelection.Length; i++)
        {
            if (ConstantsHolder.xanaConstants.wearableStoreSelection[i])
            {
                if (ConstantsHolder.xanaConstants.wearableStoreSelection[i].GetComponent<ItemDetail>())
                {
                    ConstantsHolder.xanaConstants.wearableStoreSelection[i].GetComponent<Image>().enabled = false;
                }
                else if (ConstantsHolder.xanaConstants.wearableStoreSelection[i].GetComponent<PresetData_Jsons>())
                {
                    ConstantsHolder.xanaConstants.wearableStoreSelection[i].transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    ConstantsHolder.xanaConstants.wearableStoreSelection[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                }

                ConstantsHolder.xanaConstants.wearableStoreSelection[i] = null;
            }
        }

        for (int i = 0; i < ConstantsHolder.xanaConstants.colorSelection.Length; i++)
        {
            if (ConstantsHolder.xanaConstants.colorSelection[i])
            {
                if (ConstantsHolder.xanaConstants.colorSelection[i].GetComponent<ItemDetail>())
                {
                    ConstantsHolder.xanaConstants.colorSelection[i].GetComponent<Image>().enabled = false;
                }

                else
                {
                    ConstantsHolder.xanaConstants.colorSelection[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                }

                ConstantsHolder.xanaConstants.colorSelection[i] = null;
            }
        }
    }

    void UnselectAllSelectedItemOnReset()
    {
        //foreach (var categoryData in AllCategoriesData)
        //{
        //    var parentTransform = categoryData.parentObj.transform;
        //    for (int j = 0; j < parentTransform.childCount; j++)
        //    {
        //        var childTransform = parentTransform.GetChild(j);
        //        int chldCount = childTransform.GetComponent<PresetData_Jsons>() ? 0 : 1;

        //        if (childTransform.childCount > (chldCount + 1))
        //        {
        //            var image = childTransform.GetChild(chldCount).GetComponent<Image>();
        //            if (image != null)
        //            {
        //                image.enabled = false;
        //            }
        //        }
        //    }
        //}
    }

    public void ResetMorphBooleanValues()
    {
        ConstantsHolder.xanaConstants.isFaceMorphed = SaveCharacterProperties.instance.SaveItemList.faceMorphed;
        ConstantsHolder.xanaConstants.isEyebrowMorphed = SaveCharacterProperties.instance.SaveItemList.eyeBrowMorphed;
        ConstantsHolder.xanaConstants.isEyeMorphed = SaveCharacterProperties.instance.SaveItemList.eyeMorphed;
        ConstantsHolder.xanaConstants.isNoseMorphed = SaveCharacterProperties.instance.SaveItemList.noseMorphed;
        ConstantsHolder.xanaConstants.isLipMorphed = SaveCharacterProperties.instance.SaveItemList.lipMorphed;

        if (ConstantsHolder.xanaConstants._lastClickedBtn)
        {
            if (ConstantsHolder.xanaConstants._lastClickedBtn.GetComponent<AvatarBtn>())
                ConstantsHolder.xanaConstants._lastClickedBtn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

            ConstantsHolder.xanaConstants._lastClickedBtn = null;

            if (ConstantsHolder.xanaConstants.currentButtonIndex >= 0 && ConstantsHolder.xanaConstants.currentButtonIndex < ConstantsHolder.xanaConstants.avatarStoreSelection.Length)
                ConstantsHolder.xanaConstants.avatarStoreSelection[ConstantsHolder.xanaConstants.currentButtonIndex] = null;
        }

        if (AllCategoriesData[22].parentObj.activeInHierarchy)
            OnColorButtonClicked(ConstantsHolder.xanaConstants.currentButtonIndex);
        else if (AllCategoriesData[24].parentObj.activeInHierarchy)
            OnColorButtonClicked(ConstantsHolder.xanaConstants.currentButtonIndex);
        else if (AllCategoriesData[26].parentObj.activeInHierarchy)
            OnColorButtonClicked(ConstantsHolder.xanaConstants.currentButtonIndex);
        else
            UpdateStoreSelection(ConstantsHolder.xanaConstants.currentButtonIndex);

        ////Debug.Log("IsLoggedIn " + PlayerPrefs.GetInt("IsLoggedIn"));
        if (PlayerPrefs.GetInt("IsLoggedIn") == 0)
        {
            if (isSaveFromreturnHomePopUp)
            {
                OnClickHomeButton();
            }
        }
    }

    public void DisableColorPanels()
    {
        AllCategoriesData[22].parentObj.SetActive(false);
        AllCategoriesData[24].parentObj.SetActive(false);
        AllCategoriesData[26].parentObj.SetActive(true);
    }


    // Enable Save Button if the character record is changed. 
    public void CheckForItemDetail(string currentId, int idIndex)
    {
        if (!saveButtonPressed)
        {
            if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
            {
                SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

                _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                //if (_CharacterData.myItemObj.Count != 0)
                if (_CharacterData.myItemObj.Count > idIndex)
                {
                    if (currentId == _CharacterData.myItemObj[idIndex].ItemID.ToString())
                    {
                        ActivateSaveButton(false);
                    }

                    else
                    {
                        ActivateSaveButton(true);
                    }
                }
            }
        }

        else
        {
            saveButtonPressed = false;
        }
    }
    public void CheckForAvatarBtn(int currentId, string itemType)
    {
        if (!saveButtonPressed)
        {
            if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
            {
                SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

                _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

                bool itemIsSaved = false;

                switch (itemType)
                {
                    case "face":
                        if (currentId == _CharacterData.FaceValue)
                        {
                            itemIsSaved = true;
                        }
                        break;

                    case "eyeBrow":
                        if (currentId == _CharacterData.EyeBrowValue)
                        {
                            itemIsSaved = true;
                        }
                        break;

                    case "eye":
                        if (currentId == _CharacterData.EyeValue)
                        {
                            itemIsSaved = true;
                        }
                        break;

                    case "nose":
                        if (currentId == _CharacterData.NoseValue)
                        {
                            itemIsSaved = true;
                        }
                        break;

                    case "lip":
                        if (currentId == _CharacterData.LipsValue)
                        {
                            itemIsSaved = true;
                        }
                        break;
                }

                if (itemIsSaved)
                {
                    ActivateSaveButton(false);
                }

                else
                {
                    ActivateSaveButton(true);
                }
            }
        }

        else
        {
            saveButtonPressed = false;
        }
    }

    public void ResetSaveFile()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            CharacterBodyParts bodyParts = GameManager.Instance.mainCharacter.GetComponent<CharacterBodyParts>();
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
            _CharacterData.myItemObj.Clear();

            if (_CharacterData.FaceBlendsShapes != null)
                for (int i = 0; i < _CharacterData.FaceBlendsShapes.Length; i++)
                {
                    _CharacterData.FaceBlendsShapes[i] = 0;
                }
            _CharacterData.gender = SaveCharacterProperties.instance.SaveItemList.gender;
            _CharacterData.SavedBones.Clear();
            _CharacterData.eyeTextureName = "";
            _CharacterData.Skin = bodyParts.DefaultSkinColor;
            _CharacterData.LipColor = bodyParts.DefaultLipColor;
            _CharacterData.EyebrowColor = bodyParts.DefaultEyebrowColor;
            _CharacterData.EyebrowColor = Color.white;
            _CharacterData.HairColor = Color.black; // bodyParts.DefaultHairColor; 
            _CharacterData.HairColorPaletteValue = 0;
            _CharacterData.MakeupValue = 0;
            _CharacterData.EyeLashesValue = 0;
            _CharacterData.FaceValue = 0;
            _CharacterData.EyeBrowValue = 0;
            _CharacterData.EyeBrowColorPaletteValue = 0;
            _CharacterData.EyeValue = 0;
            _CharacterData.EyesColorValue = 0;
            _CharacterData.EyesColorPaletteValue = 0;
            _CharacterData.LipsValue = 0;
            _CharacterData.LipsColorValue = 0;
            _CharacterData.LipsColorPaletteValue = 0;
            _CharacterData.NoseValue = 0;
            _CharacterData.BodyFat = 0;
            _CharacterData.SkinId = -1;
            _CharacterData.PresetValue = "";
            if (bodyParts.defaultMakeup)
                _CharacterData.makeupName = bodyParts.defaultMakeup.name;
            _CharacterData.eyeLashesName = bodyParts.defaultEyelashes.name;
            //_CharacterData.eyebrrowTexture = "";
            _CharacterData.eyebrrowTexture = bodyParts.defaultEyebrow.name;
            //_CharacterData.eyeBrowName = bodyParts.defaultEyebrow.name;

            SaveCharacterProperties.instance.SaveItemList = _CharacterData;
            string bodyJson = JsonUtility.ToJson(_CharacterData);
            File.WriteAllText(GameManager.Instance.GetStringFolderPath(), bodyJson);
            if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
                ServerSideUserDataHandler.Instance.CreateUserOccupiedAsset(() => { });
        }

        //print("in delete");
        //File.Delete(GameManager.Instance.GetStringFolderPath());
    }

    public void ActivateSaveButton(bool activate)
    {
        //if (!activate)
        //{
        //    SaveStoreBtn.SetActive(true);
        //    saveStoreBtnButton.interactable = false;
        //    saveStoreBtnImage.color = Color.white;
        //    GreyRibbonImage.SetActive(true);
        //    WhiteRibbonImage.SetActive(false);
        //}

        //else
        //{
        //    saveStoreBtnButton.interactable = true;
        //    SaveStoreBtn.SetActive(true);
        //    saveStoreBtnImage.color = new Color(0f, 0.5f, 1f, 0.8f);
        //    GreyRibbonImage.SetActive(false);
        //    WhiteRibbonImage.SetActive(true);
        //}
    }



    /// <summary>
    /// Undo Selection
    /// </summary>
    public void UndoStepBtn()
    {
        //print("undo call");
        UndoClicked = true;
    //    if (GameManager.Instance.eyesBlinking != null)          // Added by Ali Hamza 
       //     GameManager.Instance.eyesBlinking.StoreBlendShapeValues();
    }

    /// <summary>
    /// Redo Selection
    /// </summary>
    public void RedoStepBtn()
    {
        //print("redo call");
        RedoClicked = true;
     //   if (GameManager.Instance.eyesBlinking != null)          // Added by Ali Hamza 
        //    GameManager.Instance.eyesBlinking.StoreBlendShapeValues();
    }
    public void UpdateXanaConstants()
    {
        return;
        if (SaveCharacterProperties.instance.SaveItemList.myItemObj.Count == 0)
        {
            return;
        }
        ConstantsHolder.xanaConstants.hair = SaveCharacterProperties.instance.SaveItemList.myItemObj[2].ItemID.ToString();
        ConstantsHolder.xanaConstants.hairColoPalette = SaveCharacterProperties.instance.SaveItemList.HairColorPaletteValue.ToString();
        ConstantsHolder.xanaConstants.shirt = SaveCharacterProperties.instance.SaveItemList.myItemObj[1].ItemID.ToString();
        ConstantsHolder.xanaConstants.pants = SaveCharacterProperties.instance.SaveItemList.myItemObj[0].ItemID.ToString();
        ConstantsHolder.xanaConstants.shoes = SaveCharacterProperties.instance.SaveItemList.myItemObj[3].ItemID.ToString();
        ConstantsHolder.xanaConstants.eyeWearable = SaveCharacterProperties.instance.SaveItemList.EyeValue.ToString();

        ConstantsHolder.xanaConstants.PresetValueString = SaveCharacterProperties.instance.SaveItemList.PresetValue;
        ConstantsHolder.xanaConstants.skinColor = SaveCharacterProperties.instance.SaveItemList.SkinId.ToString();
        ConstantsHolder.xanaConstants.faceIndex = SaveCharacterProperties.instance.SaveItemList.FaceValue;
        ConstantsHolder.xanaConstants.eyeBrowIndex = SaveCharacterProperties.instance.SaveItemList.EyeBrowValue;
        ConstantsHolder.xanaConstants.eyeBrowColorPaletteIndex = SaveCharacterProperties.instance.SaveItemList.EyeBrowColorPaletteValue;
        ConstantsHolder.xanaConstants.eyeLashesIndex = SaveCharacterProperties.instance.SaveItemList.EyeLashesValue;
        ConstantsHolder.xanaConstants.eyeIndex = SaveCharacterProperties.instance.SaveItemList.EyeValue;
        ConstantsHolder.xanaConstants.eyeColor = SaveCharacterProperties.instance.SaveItemList.EyesColorValue.ToString();
        ConstantsHolder.xanaConstants.eyeColorPalette = SaveCharacterProperties.instance.SaveItemList.EyesColorPaletteValue.ToString();
        ConstantsHolder.xanaConstants.noseIndex = SaveCharacterProperties.instance.SaveItemList.NoseValue;
        ConstantsHolder.xanaConstants.lipIndex = SaveCharacterProperties.instance.SaveItemList.LipsValue;
        ConstantsHolder.xanaConstants.lipColor = SaveCharacterProperties.instance.SaveItemList.LipsColorValue.ToString();
        ConstantsHolder.xanaConstants.lipColorPalette = SaveCharacterProperties.instance.SaveItemList.LipsColorPaletteValue.ToString();
        ConstantsHolder.xanaConstants.bodyNumber = SaveCharacterProperties.instance.SaveItemList.BodyFat;
        ConstantsHolder.xanaConstants.makeupIndex = SaveCharacterProperties.instance.SaveItemList.MakeupValue;
        //}
    }
    // UGC Removed By Ahsan
    //public void ApplyUGCValueOnCharacter(string _gender)
    //{
    //    CharacterBodyParts _charcterBodyParts = GameManager.Instance.mainCharacter.GetComponent<CharacterBodyParts>();
    //    // _charcterBodyParts.head.materials[2].SetColor("_BaseColor", itemData.skin_color);
    //    _charcterBodyParts.head.materials[2].SetColor("_Lips_Color", itemData.lips_color);
    //    // _charcterBodyParts.body.materials[0].SetColor("_BaseColor", itemData.skin_color);
    //    for (int i = 0; i < _charcterBodyParts.head.sharedMesh.blendShapeCount - 1; i++)
    //    {
    //        _charcterBodyParts.head.SetBlendShapeWeight(i, 0);
    //    }
    //    if (itemData.faceItemData != 0 && _charcterBodyParts != null)
    //    {
    //        _charcterBodyParts.head.SetBlendShapeWeight(itemData.faceItemData, 100);
    //    }
    //    if (itemData.noseItemData != 0)
    //        _charcterBodyParts.head.SetBlendShapeWeight(itemData.noseItemData, 100);
    //    if (itemData.lipItemData != 0)
    //        _charcterBodyParts.head.SetBlendShapeWeight(itemData.lipItemData, 100);
    //    if (itemData.eyeShapeItemData != 0)
    //        _charcterBodyParts.head.SetBlendShapeWeight(itemData.eyeShapeItemData, 100);
    //    if (itemData._hairItemData != null)
    //    {
    //        if (!itemData._hairItemData.Contains("No hair"))
    //            StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(-1, itemData._hairItemData, "Hair", _gender, GameManager.Instance.mainCharacter.GetComponent<AvatarController>(), itemData.hair_color, true));
    //    }
    //    if (itemData._eyeItemData != null)
    //    {
    //        StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(itemData._eyeItemData, GameManager.Instance.mainCharacter.GetComponent<AvatarController>().gameObject, CurrentTextureType.EyeLense));
    //    }
    //    if (itemData.skin_color != null)
    //    {
    //        //char[] charsToTrim = { '#' };
    //        //string cleanString = itemData.skin_color.TrimStart(charsToTrim);
    //        if (itemData.gender == "male")
    //        {//itemData.skin_color
    //            StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTextureByName("Assets/Store Items Addressables/1k_Boy_Face_Texture", itemData.skin_color, GameManager.Instance.mainCharacter.GetComponent<AvatarController>().gameObject, CurrentTextureType.Face));
    //            StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTextureByName("Assets/Store Items Addressables/1k_Boy_Body_Texture", itemData.skin_color, GameManager.Instance.mainCharacter.GetComponent<AvatarController>().gameObject, CurrentTextureType.Skin));
    //        }
    //        else
    //        {
    //            StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTextureByName("Assets/Store Items Addressables/1k_Girl_Face_Textures", itemData.skin_color, GameManager.Instance.mainCharacter.GetComponent<AvatarController>().gameObject, CurrentTextureType.Face));
    //            StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTextureByName("Assets/Store Items Addressables/1k_Girl_Body_Texture", itemData.skin_color, GameManager.Instance.mainCharacter.GetComponent<AvatarController>().gameObject, CurrentTextureType.Skin));
    //        }
    //    }
    //}
    //public void ApplyDefaultValueOnCharacter(string _gender)
    //{
    //    CharacterBodyParts _charcterBodyParts = GameManager.Instance.mainCharacter.GetComponent<CharacterBodyParts>();
    //    if (_gender == AvatarGender.Male.ToString())
    //    {
    //        _charcterBodyParts.head.materials[2].SetTexture("_Base_Texture", itemData.default_male_face_color);
    //        _charcterBodyParts.head.materials[2].SetColor("_Lips_Color", itemData.default_male_lips_color);
    //        _charcterBodyParts.body.materials[0].SetTexture("_Base_Texture", itemData.default_male_body_color);
    //        _charcterBodyParts.ApplyEyeLenTexture(CharacterHandler.instance.maleAvatarData.DEye_texture, _charcterBodyParts.gameObject);
    //    }
    //    else
    //    {
    //        _charcterBodyParts.head.materials[2].SetTexture("_Base_Texture", itemData.default_female_face_color);
    //        _charcterBodyParts.head.materials[2].SetColor("_Lips_Color", itemData.default_female_lips_color);
    //        _charcterBodyParts.body.materials[0].SetTexture("_Base_Texture", itemData.default_female_body_color);
    //        _charcterBodyParts.ApplyEyeLenTexture(CharacterHandler.instance.femaleAvatarData.DEye_texture, _charcterBodyParts.gameObject);
    //    }
    //    for (int i = 0; i < _charcterBodyParts.head.sharedMesh.blendShapeCount - 1; i++)
    //    {
    //        _charcterBodyParts.head.SetBlendShapeWeight(i, 0);
    //    }

    //}
    public void BackToLoginSelectionPanel()
    {
        if (ConstantsHolder.xanaConstants.LoggedInAsGuest && ConstantsHolder.xanaConstants.SwitchXanaToXSummit)
        {
           // UserLoginSignupManager.instance.signUpOrloginSelectionPanel.SetActive(true);
            StartPanel_PresetParentPanelSummit.SetActive(false);
        }

    }
    public void BackToMain()
    {
        GameManager.Instance.HomeCameraInputHandler(true);
       // GameManager.Instance.HomeCamera.GetComponent<HomeCameraController>().CenterAlignCam();
    }

    public void OnClickPresetSlctBackBtn()
    {
      //  UserLoginSignupManager.instance.signUpOrloginSelectionPanel.SetActive(true);
        StartPanel_PresetParentPanel.SetActive(false);
    }

}
public class XenyRequestedData
{
    public string userAddress;
}
public class XenyTokenData
{
    public double xenyToken;
    public string address;
}


[System.Serializable]
public class StoreItemHolder
{
    public string itemName;
    public GameObject parentObj;
    public List<SubitemData> subItems;
}

[System.Serializable]
public class SubitemData
{
    public string gender;
    public GameObject obj;
}public static class StringExtensions
{
    public static int ParseToInt(this string value)
    {
        return int.Parse(value);
    }
}
