using Newtonsoft.Json.Linq;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ConstantsHolder : MonoBehaviour
{
//#if UNITY_WEBGL && !UNITY_EDITOR
//    [DllImport("__Internal")]
//    private static extern void LoadAuth(int score,string textupdate);
//#endif


    public static ConstantsHolder xanaConstants;
    public delegate void UserNameToggleDeligate(int userNameToggleConstant);
    public static event UserNameToggleDeligate userNameToggleDelegate;
    public static Action<GameObject> ontriggteredplayerEntered;
    public static Action<GameObject> ontriggteredplayerExit;
    public bool pushToTalk;
    public bool openLandingSceneDirectly;
    public bool XSummitBg = false;
    public static bool DomeHeaderInfo=true;
    public bool SwitchXanaToXSummit=true;
    public static bool IsXSummitApp;
    public bool OpenSpaceScreen_fromSummit = false;
    public bool chatFlagBtnStatus = false;
    public bool TelegramLoginBtnStatus = false;
    public static int DomeImageCompression = 350;

    //Login Info
    public static bool isAdmin;
    //public static bool isGuestLogin;
    public static bool loggedIn;
    public static bool isWalletLogin;
    public static string xanaliaToken;
    public static string xanaToken;
    public static string userId;
    public static string userName;
    public static string uniqueUserName;

    public static bool isAddressableCatalogDownload;
    public bool registerFirstTime = false;
    public bool isHoldCharacterNFT;
    public bool isNFTEquiped;
    public bool LoginasGustprofile = false; // for gust profile
    public bool LoggedInAsGuest = false; // to check is user logged in as guest
    public bool isFirstPanel = false; // User has Not data Open First Panel
    public string NFTUrl;
    public string clothJson;
    public string defaultFightingName;

    [Header("TapToKnow")]
    public bool tapToKnow_organization = false;
    public bool tapToKnow_profession = false;
    public bool tapToKnow_industry = false;
    public bool tapToKnow_email = false;
    public bool tapToKnow_contact = false;
    public bool tapToKnow_join = false;
    public bool tapToKnow_hobbies = false;
    public bool tapToKnow_status = false;

    public int mic;
    public bool UserMicEnable;
    public int minimap;
    public int userNameVisibilty;
    public bool profileImageModifedByUser = false;
    public string userProfileLink;
    public string CurrentSceneName;
    public string EnviornmentName;
    public static int userLimit;
    public string summitSubDomeType;
    public string summitSubDomeCatagory;
    public bool haveSubDomeEnabled;
    // public string museumDownloadLink;// = "https://angeluim-metaverse.s3.ap-southeast-1.amazonaws.com/unitydata/environments/Museums/Aurora_Art_Museum/auroramuseum.android";
    public GameObject buttonClicked;
    public GameObject _lastClickedBtn;
    public GameObject _lastAvatarClickedBtn;
    public GameObject _curretClickedBtn;
    public bool IsMuseum = false;
    public bool IsDeemoNFT = false;
    public string hair = "";
    public string hairColoPalette = "";
    public int faceIndex = 0;
    public bool isFaceMorphed = false;
    public int eyeBrowIndex = 0;
    public int eyeBrowColorPaletteIndex = 0;
    public int eyeLashesIndex = 0;
    public int makeupIndex = 0;
    public bool isEyebrowMorphed = false;
    public int eyeIndex = 0;
    public string eyeColor = "";
    public string eyeColorPalette = "";
    public bool isEyeMorphed = false;
    public int noseIndex = 0;
    public bool isNoseMorphed = false;
    public int lipIndex = 0;
    public string lipColor = "";
    public string lipColorPalette = "";
    public bool isLipMorphed = false;
    public int bodyNumber = -1;
    public string skinColor = "";
    public string shirt = "";
    public string shoes = "";
    public string pants = "";
    public string eyeWearable = "";
    public int currentButtonIndex = -1;
    public string PresetValueString;
    public GameObject[] avatarStoreSelection;
    public GameObject[] wearableStoreSelection;
    public GameObject[] colorSelection;
    public bool setIdolVillaPosition = true;
    public GameObject lastSelectedButton;

    public bool orientationchanged = false;
    public bool SelfiMovement = true;
    public GameObject ConnectionPopUpPanel;
    public int presetItemsApplied = 0;
    public bool isSkinApplied = false;
    public bool isPresetHairColor = false;
    public bool isCameraMan;
    public bool isCameraManInRoom = false;
    public bool isBackfromSns = false;

    public bool isBackFromWorld = false;
    public String MuseumID;
    public string domeCreatorName;
    public bool isBuilderGame = false;
    public bool isSummitBtnPressed = false;

    public bool isSummitDeepLink = false;
    public bool isJoiningXANADeeplink = false;
    
    //For Metabuzz Environments
    public enum ComingFrom
    {
        None,
        Dune,
        Daisen
    }
    public ComingFrom comingFrom = ComingFrom.None;

    public bool IsMetabuzzEnvironment
    {
        get
        {
            if (!string.IsNullOrEmpty(EnviornmentName))
                if (EnviornmentName.Contains("DUNE") || EnviornmentName == "TOTTORI METAVERSE" || EnviornmentName.Contains("Daisen"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            return false;
        }
    }

    //for world transition from JJworldChanger a world
    public bool hasWorldTransitionedInternally;
    // Is in Store
    public bool isStoreActive = false;

    // For Analatics 
    public int worldIdFromApi;
    public string playerSocketID;
    public int customWorldId;

    // For Firebase
    public bool isFirebaseInit = false;

    //public JJMussuemEntry mussuemEntry = JJMussuemEntry.Null;
    public string JjWorldTeleportSceneName;
    public string NFTBoxerJson = "/BoxerNFTData.json";

    //for Create Room Scene Avatar
    [Header("SNS Variables")]
    public bool r_isSNSComingSoonActive = true;
    public GameObject r_MainSceneAvatar;

    public enum ScreenType { MobileScreen, TabScreen }
    public ScreenType screenType;
    /// <summary>
    /// variables for builder scene integration 
    /// </summary>
    /// 
    public bool isWalletLoadingbool = false;
    public bool isBuilderScene;
    public int builderMapID;
    public bool JjWorldSceneChange = false;
    public bool isFromXanaLobby = false;
    public bool isFromHomeTab = false;
    public bool isFromTottoriWorld = false;

    [HideInInspector]
    public bool needToClearMemory = true;
    // Tutorials
    public bool isTutorialLoaded = false;
    public bool isLobbyTutorialLoaded = false;

    //Toyota Home Aichi
    public enum MeetingStatus { End, Inprogress, HouseFull }
    [SerializeField]
    public MeetingStatus meetingStatus;
    public bool IsShowChatToAll = true;
    public bool IsChatUseByOther = false;

    //XANA Summit 
    public static bool isFromXANASummit = false;
    public static bool IsSummitDomeWorld = false;
    public static bool MultiSectionPhoton = false;
    public static bool TempDiasableMultiPartPhoton = false;
    public static bool DiasableMultiPartPhoton = false;
    public static bool DisableFppRotation = false;
    public static int domeId;
    public static int visitorCount;
    public static string DomeType;
    public static string DomeCategory;
    public static bool isPenguin;
    public static bool isFixedHumanoid;
    public static int AvatarIndex;
    public static bool HaveSubWorlds;
    public static string Thumbnail;
    public static string description;
    public static bool isTeleporting = false;

    public List<string> presetClothsJsonList =new List<string>();

    //Daily reward
    public bool isGoingForHomeScene = false;
    public bool hasToShowDailyPopup = false;


    #region XANA PARTY WORLD
    public bool PenpenzBuild = false;
    public bool isXanaPartyWorld = false;
    public bool isJoinigXanaPartyGame = false;
    public int XanaPartyGameId;
    public string XanaPartyGameName;
    public bool isMasterOfGame = false;
    public static int XanaPartyMaxPlayers = 25;
    public bool EnableSignInPanelByDefault = false;
    public bool GameIsFinished = false;
    public string LastLobbyName;
  //  public bool isBuilderGame = false;
#endregion


    public string r_EmoteStoragePersistentPath
    {
        get
        {
            return Application.persistentDataPath + "/EmoteAnimationBundle";
        }
    }

    public string[] labels = { "boyc11hair", "staffshirt", "girlc24pants", "shoesc43" }; // labels of each group item for preload
    public UnityEvent<float> ProgressEvent;
    public UnityEvent<bool> CompletionEvent;
    private AsyncOperationHandle downloadHandle;



    public bool IsProfileVisit = false; // bool to check is player in profile section.
    public int SnsProfileID = 0; // Id of user profile when the user visit the profile section.
    public bool IsOtherProfileVisit = false; // to Check is other profile player visit
    public string r_EmoteReactionPersistentPath
    {
        get
        {
            return Application.persistentDataPath + "/EmoteReaction";
        }
    }


    public void Awake()
    {
        if (xanaConstants)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            xanaConstants = this;
            if (PlayerPrefs.HasKey("micSound"))
            {
                mic = PlayerPrefs.GetInt("micSound");
            }
            else
            {
                //PlayerPrefs.SetInt("micSound", 1); By default mic will be off
                mic = PlayerPrefs.GetInt("micSound");
            }
            if (PlayerPrefs.HasKey("minimap"))
            {
                minimap = PlayerPrefs.GetInt("minimap");
            }
            else
            {
                PlayerPrefs.SetInt("minimap", 0); // Bydefault Off
                minimap = PlayerPrefs.GetInt("minimap");
            }

            //if (PlayerPrefs.HasKey("userName"))
            //{
            //    userName = PlayerPrefs.GetInt("userName");
            //}
            //else
            //{
            //    PlayerPrefs.SetInt("userName", 1);
            //    userName = PlayerPrefs.GetInt("userName");
            //}



            DontDestroyOnLoad(this.gameObject);
        }

        userNameVisibilty = 1;
        avatarStoreSelection = new GameObject[11];
        wearableStoreSelection = new GameObject[8];
        colorSelection = new GameObject[6];
#if !UNITY_EDITOR
        var aspectRatio = Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
        if (DeviceDiagonalSizeInInches() > 6.5f && aspectRatio < 2f)
        {
            screenType = ScreenType.TabScreen;
        }
        else
        {
            screenType = ScreenType.MobileScreen;
        }
#endif
    }
    public float DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
        Debug.Log("Getting device inches: " + diagonalInches);

        return diagonalInches;
    }
    private void Start()
    {
        //  StartCoroutine(LoadAddressableDependenceies());
    }

    public void SetPlayerProperties(string cloths = "")
    {
       // Debug.Log("SetPlayerProperties");
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        if (string.IsNullOrEmpty(cloths))
        {
            playerProperties.Add("ClothJson", GetJsonFolderData());
        }
        else
        {
            playerProperties.Add("ClothJson", cloths);
        }
        playerProperties.Add("NFTEquiped", isNFTEquiped);
        PhotonNetwork.LocalPlayer.CustomProperties = playerProperties;
    }

    public string GetRandomPresetClothJson()
    {
        return presetClothsJsonList[UnityEngine.Random.Range(0, presetClothsJsonList.Count)];
    }
    public string GetJsonFolderData()
    {
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)  // loged from account)
        {
            if (ConstantsHolder.xanaConstants.isNFTEquiped)
            {
                return File.ReadAllText(Application.persistentDataPath + ConstantsHolder.xanaConstants.NFTBoxerJson);
            }
            else
            {
                return File.ReadAllText(Application.persistentDataPath + "/logIn.json");
            }
        }
        else
        {
            return File.ReadAllText(Application.persistentDataPath + "/loginAsGuestClass.json");
        }
    }
    public void StopMic()
    {
        PlayerPrefs.SetInt("micSound", 0);
        mic = PlayerPrefs.GetInt("micSound");
    }

    public void PlayMic()
    {
        PlayerPrefs.SetInt("micSound", 1);
        mic = PlayerPrefs.GetInt("micSound");
    }

    /// <summary>
    /// To preload addressable dependenceies
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadAddressableDependenceies()
    {
        //yield return Addressables.DownloadDependenciesAsync("boyc11hair", true);

        // Check the download size
        for (int i = 0; i < labels.Length; i++)
        {
            downloadHandle = Addressables.DownloadDependenciesAsync(labels[i], false);
            float progress = 0;

            while (downloadHandle.Status == AsyncOperationStatus.None)
            {
                float percentageComplete = downloadHandle.GetDownloadStatus().Percent;
                if (percentageComplete > progress * 1.1) // Report at most every 10% or so
                {
                    progress = percentageComplete; // More accurate %
                    ProgressEvent.Invoke(progress);
                }
                yield return null;
            }

            CompletionEvent.Invoke(downloadHandle.Status == AsyncOperationStatus.Succeeded);
            Addressables.Release(downloadHandle); //Release the operation handle
        }
    }

    //////constant string variables 
    public const string collectibleMsg = "Item Collected...";
    public static void OnInvokeUsername(int userNameToggle)
    {
        userNameToggleDelegate?.Invoke(userNameToggle);
    }


    public IEnumerator JavaScriptToken(string objects)
    {
        // GameManager.currentLanguage = "en";

        yield return new WaitForSeconds(.5f);
        try
        {
            JObject objectnew = JObject.Parse(objects);
            //Debug.Log("Disable inpute===" + InputDisableHome);
            Debug.Log("React Side Get Response===" + objectnew);
            ConstantsGod.AUTH_TOKEN = objectnew.GetValue("UserToken").ToString();
            
            // InputDisableHome = (bool)objectnew.GetValue("disableInput");
            // isLogin = ApiCallToken;
            // isLogin = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MTYyLCJpYXQiOjE3MzUwMjc2NDQsImV4cCI6MTczNTExNDA0NH0.-8JpuAdPa4JeO-LZe6Wn8nQQXUMZzcsReU6q9r1wMlo";
            //            XanaConstants.xanaConstants.EnviornmentName = objectnew.GetValue("EnvName").ToString();
            //            //  XanaConstants.xanaConstants.EnviornmentName = "PMYRoomC";
            //            PlayerPrefs.SetString("ChatLobby", XanaConstants.xanaConstants.EnviornmentName);
            //            XanaConstants.xanaConstants.ChatLobby = XanaConstants.xanaConstants.EnviornmentName;
            //            XanaConstants.xanaConstants.isBuilderScene = (bool)objectnew.GetValue("BuilderCheck");
            //            XanaConstants.xanaConstants.builderMapID = (int)objectnew.GetValue("IsBuilderMapCode");
            //            isGuest = (bool)objectnew.GetValue("IsGuest");
            //            isWebGLEvent = (bool)objectnew.GetValue("join_webgl_event");
            //            WebGLEventId = (int)objectnew.GetValue("join_webgl_event_id");
            //            webglEventYoutubeurl = objectnew.GetValue("webgl_event_youtubeurl").ToString();
            //            //  webglEventVideourl = objectnew.GetValue("webgl_event_offlineurl").ToString();
            //            XanaConstants.xanaConstants.Language = objectnew.GetValue("lang").ToString();
            //            GameManager.currentLanguage = XanaConstants.xanaConstants.Language;
            //            AvatarJson = objectnew.GetValue("avatarjson").ToString();
            //            if (string.IsNullOrEmpty(AvatarJson))
            //            {
            //                if (!string.IsNullOrEmpty(PlayerPrefs.GetString("AvatarJson")))
            //                {
            //                    AvatarJson = PlayerPrefs.GetString("AvatarJson");
            //                }

            //            }
            //            AvatarName = objectnew.GetValue("avatarname").ToString();
            //            if (string.IsNullOrEmpty(AvatarName))
            //            {
            //                if (!string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")))
            //                {
            //                    AvatarName = PlayerPrefs.GetString("PlayerName");

            //                }

            //            }
            //            isVoice = (bool)objectnew.GetValue("IsVoice");
            //            isMessage = (bool)objectnew.GetValue("IsMessage");
            //            isAnimation = (bool)objectnew.GetValue("IsAnimation");
            //            WebGLMuseumId = (int)objectnew.GetValue("museumId");



            //            // XanaConstants.xanaConstants.EnviornmentName = "D    Infinity Labo"
            //            if (XanaConstants.xanaConstants.EnviornmentName.Contains("GOZ"))
            //            {
            //                XanaConstants.xanaConstants.EnviornmentName = "GOZANIMATOR HARUNA GOUZU GALLERY 2021";
            //            }
            //            if (XanaConstants.xanaConstants.EnviornmentName.Contains("D   Infinity Labo")
            //             || XanaConstants.xanaConstants.EnviornmentName.Contains("D  Infinity Labo")
            //             || XanaConstants.xanaConstants.EnviornmentName.Contains("D    Infinity Labo"))


            //            {
            //                XanaConstants.xanaConstants.EnviornmentName = "D Infinity Labo";
            //            }


            //            //if (XanaConstants.xanaConstants.EnviornmentName.Contains("XANA Lobby"))
            //            //{
            //            //    XanaConstants.xanaConstants.EnviornmentName = "XANA Lobby";
            //            //}
            //            // StartCoroutine(GameManager.Instance.FontLoadWait(5f));

            //            StartCoroutine(EmoteAnimationPlay.Instance.getAllAnimations());
            //            if (XanaConstants.xanaConstants.isBuilderScene)
            //            {
            //                StartCoroutine(AddressableDownloader.Instance.HoldLoadObject());
            //            }

            //            if (PlayerPrefs.GetInt("pmyloadfirst") == 1)
            //            {
            //                Debug.Log("PMY code call" + PlayerPrefs.GetInt("pmyloadfirst"));
            //                StartCoroutine(GameManager.Instance.GetClassCodeFromServer());
            //            }
            //            else
            //            {
            //                Debug.Log("PMY code call 1" + !XanaConstants.xanaConstants.pmy_isClassAvailable);
            //                if (!XanaConstants.xanaConstants.pmy_isClassAvailable)
            //                {
            //                    if (XanaConstants.xanaConstants.EnviornmentName.Equals("PMY ACADEMY"))
            //                    {
            //                        Debug.Log("PMY code call 2" + !XanaConstants.xanaConstants.pmy_isClassAvailable);
            //                        StartCoroutine(GameManager.Instance.GetClassCodeFromServer());

            //                    }
            //                }
            //            }


            //            //else
            //            //{
            //            //    GameManager.currentLanguage ="en";
            //            //}
            //        }


            //        catch (Exception e)
            //        {

            //        }

            //        //  yield return StartCoroutine(AddressableDownloader.Instance.HoldLoadObject());


            //        if (!string.IsNullOrEmpty(isLogin) && !string.IsNullOrEmpty(XanaConstants.xanaConstants.EnviornmentName) ||
            //            !string.IsNullOrEmpty(isLogin) && XanaConstants.xanaConstants.isBuilderScene)
            //        {
            //            if (!XanaConstants.xanaConstants.EnviornmentName.Contains("PMY ACADEMY"))
            //            {

            //                SetParameter();
            //            }
            //            else
            //            {
            //                Debug.Log("PMY code call 3" + XanaConstants.xanaConstants.pmy_isClassAvailable);
            //                if (PlayerPrefs.GetInt(ConstantsGod.XANA_BACK) != 2 && PlayerPrefs.GetInt("PMY") != 1)
            //                {
            //                    PlayerPrefs.SetInt("PMY", 1);
            //                    Debug.Log("PMY Acadamy value==" + PlayerPrefs.GetInt(ConstantsGod.XANA_BACK));
            //                    PlayerPrefs.SetFloat("pmyvol", -1);
            //                }

            //                if (PlayerPrefs.GetInt("pmyloadfirst") == 1)
            //                {
            //                    Debug.Log("PMY code call 4" + XanaConstants.xanaConstants.pmy_isClassAvailable);
            //                    SetParameter();
            //                    //                    foreach (var item in XanaConstants.xanaConstants.pmy_ClassCode)
            //                    //                    {
            //                    //                        // Debug.Log("code value 3---" + item.codeText);



            //                    //                        if (item.codeText.ToString().Trim().Equals(PlayerPrefs.GetString("PMY code").Trim().ToString()))
            //                    //                        {
            //                    //                            Debug.Log("code value 2---" + item.codeText);

            //                    //                            //Debug.Log("code value 1---" + classCodeInputField);
            //                    //                            XanaConstants.xanaConstants.pmySchooldDataID = item.id;

            //                    //                            Debug.Log("<color=green> PMY -- Class Available  </color>");
            //                    //                            XanaConstants.xanaConstants.pmy_isClassAvailable = true;
            //                    //                            PlayerPrefs.SetInt("pmyloadfirst", 1);
            //                    //                            XanaConstants.xanaConstants.pmy_joinedClassCode = classCodeInputField_text.text;
            //                    //                            //XanaConstants.xanaConstants.EnviornmentName = "PMY ACADEMY";
            //                    //                            // LoadingHandler.Instance.ShowLoading();
            //                    //                            Debug.Log("Class call====");
            //                    //#if UNITY_WEBGL && !UNITY_EDITOR
            //                    //        WebGLInput.captureAllKeyboardInput = true;
            //                    //#endif
            //                    //#if UNITY_WEBGL == true && UNITY_EDITOR == false
            //                    //    PMYLoad(true);

            //                    //#endif
            //                    //                            // enterClassCodePanel.SetActive(false);

            //                    //                            //LoadingHandler.Instance.UpdateLoadingSlider(1 * .2f);
            //                    //                            //LoadingHandler.Instance.UpdateLoadingStatusText("Loading World");

            //                    //                         //   CodeValid = true;
            //                    //                            //    WorldManager.instance.PlayWorld();
            //                    //                            break;
            //                    //                        }

            //                    //                    }

            //                }
            //                else
            //                {
            //                    if (!XanaConstants.xanaConstants.pmy_isClassAvailable)
            //                    {

            //#if UNITY_WEBGL == true && UNITY_EDITOR == false
            //    HidePreset(true);
            //#endif
            //                        LoadingHandler.Instance.HideLoading();
            //#if UNITY_WEBGL == true && UNITY_EDITOR == false
            //    Load (100,"Complete...");
            //#endif
            //#if UNITY_WEBGL && !UNITY_EDITOR
            //        WebGLInput.captureAllKeyboardInput = false;
            //#endif
            //                        enterClassCodePanel.SetActive(true);
            //                    }
            //                }

        }
        catch (Exception e) { 
            
        }

    }
}
