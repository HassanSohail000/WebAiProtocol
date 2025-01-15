//using AdvancedInputFieldPlugin;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;
//using SimpleJSON;
using UnityEngine.SceneManagement;
//using UnityEditor.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public GameObject LoginRegisterScreen, SignUpScreen, HomePage, Canvas, HomeWorldScreen;
    public CanvasGroup Loadinghandler_CanvasRef;
    public GameObject _SplashScreen;
    public GameObject SplashScreenXana;
    public GameObject SplashScreenSummit;

    public Transform _postScreen, _postCamera, _postScreenBG;
    public bool IsSplashActive = true;
    /*public Transform SecondSliderScrollView;*/

    [Header("Footer Reference")]
    public GameObject _footerCan;
    public GameObject faceMorphPanel;
    //[Space(5)]
    //[Header("New World Layout References")]
    //public Transform SearchHomeHolder;
    public Transform /*SearchWorldHolder,*/
        SearchWorldScreenHolder;
    //AvatarWindowHolder,

    //WorldWorldTabsHolder, 
    /*,*/
    //LobbyTabHolder,
    /*AdvanceSearchInputField*/

    //public GameObject worldHolder;
    public GameObject searchWorldHolder;
    //public XSummitBgChange XSummitBgChange;

    public bool isAvatarSelectionBtnClicked = false;
    [SerializeField] Color postButtonColor;

    //public static event Action<BackButtonHandler.screenTabs> OnScreenTabStateChange;

    private void Awake()
    {
        StartCoroutine(FetchFeatures());
        Canvas.GetComponent<CanvasGroup>().alpha = 0;
        Canvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
        Canvas.GetComponent<CanvasGroup>().interactable = false;
        _footerCan.GetComponent<CanvasGroup>().alpha = 0.0f;
        _footerCan.GetComponent<CanvasGroup>().interactable = false;
        _footerCan.GetComponent<CanvasGroup>().blocksRaycasts = false;
        //_SplashScreen.SetActive(false);
        if (SaveCharacterProperties.NeedToShowSplash == 1)
        {
            print("showing splash from Start UI Handler");
            _SplashScreen.SetActive(true);
        }
        else
        {
            _SplashScreen.SetActive(false);
            SplashMemoryFree();
            _footerCan.GetComponent<CanvasGroup>().alpha = 1.0f;
            _footerCan.GetComponent<CanvasGroup>().interactable = true;
            _footerCan.GetComponent<CanvasGroup>().blocksRaycasts = true;
            Canvas.GetComponent<CanvasGroup>().alpha = 1.0f;
            Canvas.GetComponent<CanvasGroup>().interactable = true;
            Canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
    bool a = false;


    public void AvatarSelectionBtnClicked()
    {
        if (!GameManager.Instance.isAllSceneLoaded)
            return;

        if (!isAvatarSelectionBtnClicked)
            isAvatarSelectionBtnClicked = true;
        GameManager.Instance.HomeCameraInputHandler(false);

    }

    public void SwitchToPostScreen(bool flag)
    {

        if (!GameManager.Instance.isAllSceneLoaded)
            return;

        if ((PlayerPrefs.GetInt("IsLoggedIn") == 0))
        {
            // SNSNotificationHandler.Instance.ShowNotificationMsg("Need To Login");
        }
        else
        {
            //CharacterHandler.instance.playerNameCanvas.SetActive(!flag);
            GameManager.Instance.HomeCameraInputHandler(!flag);

            _postScreen.gameObject.SetActive(flag);
            _postScreenBG.gameObject.SetActive(flag);
            HomePage.gameObject.SetActive(!flag);
            _postCamera.gameObject.SetActive(flag);
            ShowFooter(!flag);
           // GameManager.Instance.ActorManager.IdlePlayerAvatorForPostMenu(flag);
            //GameManager.Instance.userAnimationPostFeature.GetComponent<UserPostFeature>().ActivatePostButtbleHome(!flag);
           // GameManager.Instance.userAnimationPostFeature.postButton.interactable = false;
           // GameManager.Instance.userAnimationPostFeature.postButtonText.color = postButtonColor;
        }

        //OnScreenTabStateChange?.Invoke(BackButtonHandler.screenTabs.Post);
    }
    public void ResetPlayerToLastPostPosted()
    {
        //GameManager.Instance.userAnimationPostFeature.transform.GetComponent<UserPostFeature>().SetLastPostToPlayer();
        //GameManager.Instance.HomeCamera.GetComponent<HomeCameraController>().CenterAlignCam();
        //OnScreenTabStateChange?.Invoke(BackButtonHandler.screenTabs.Hometab);
    }
    public void AvaterButtonCustomPushed()
    {
        //WorldDescriptionPopupPreview.m_WorldIsClicked = false;
        //WorldDescriptionPopupPreview.m_MuseumIsClicked = false;
    }
    public void IsWorldClicked()
    {
        //if (WorldDescriptionPopupPreview.m_WorldIsClicked || WorldDescriptionPopupPreview.m_MuseumIsClicked || ConstantsHolder.loggedIn)
        //    WorldManager.instance.PlayWorld();
    }
    public void ShowFooter(bool _state)
    {
        _footerCan.SetActive(_state);
    }
    private void Start()
    {
        if (SaveCharacterProperties.NeedToShowSplash == 1)
        {
            
            if (!ConstantsHolder.xanaConstants.SwitchXanaToXSummit)
            {
                SplashScreenXana.SetActive(true);
                SplashScreenSummit.SetActive(false);
                if (Screen.orientation == ScreenOrientation.LandscapeRight || Screen.orientation == ScreenOrientation.LandscapeLeft)
                {
                    Screen.orientation = ScreenOrientation.Portrait;
                }

            }
            else
            {
                SplashScreenXana.SetActive(false);
                if (ConstantsHolder.xanaConstants.openLandingSceneDirectly)
                {
                    SplashScreenSummit.SetActive(true);
                    Screen.orientation = ScreenOrientation.LandscapeLeft; // this shouldn't run if user is reconnecting
                }
            }
        }
        else
        {
            _SplashScreen.SetActive(false);
            if (!ConstantsHolder.xanaConstants.isFromXanaLobby)
            {
                Screen.orientation = ScreenOrientation.Portrait;
            }
            _footerCan.GetComponent<CanvasGroup>().alpha = 1.0f;
            _footerCan.GetComponent<CanvasGroup>().interactable = true;
            _footerCan.GetComponent<CanvasGroup>().blocksRaycasts = true;
            Canvas.GetComponent<CanvasGroup>().alpha = 1.0f;
            Canvas.GetComponent<CanvasGroup>().interactable = true;
            Canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
            //ReleaseSplashFromMemory();
            //SplashScreenXana.SetActive(false);
            //SplashScreenSummit.SetActive(false);
            //StartCoroutine(IsSplashEnable(false, 0f));
        }

        if (SaveCharacterProperties.NeedToShowSplash == 1 && !ConstantsHolder.xanaConstants.isJoiningXANADeeplink)
        {
            if (PlayerPrefs.HasKey("TermsConditionAgreement"))
            {
                IsSplashActive = false;
                print("call from TermsConditionAgreement");
                StartCoroutine(IsSplashEnable(false, 10f));
                if (!ConstantsHolder.xanaConstants.SwitchXanaToXSummit)
                {
                    if (Screen.orientation == ScreenOrientation.LandscapeRight || Screen.orientation == ScreenOrientation.LandscapeLeft)
                    {
                        Screen.orientation = ScreenOrientation.Portrait;
                    }

                }
                else
                {
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                }
            }
        }
        else
        {
            print("call from else neet to show splash");
            // StartCoroutine(IsSplashEnable(false, 0f));
            _SplashScreen.SetActive(false);
            if (!ConstantsHolder.xanaConstants.isFromXanaLobby)
            {
                Screen.orientation = ScreenOrientation.Portrait;
            }
            _footerCan.GetComponent<CanvasGroup>().alpha = 1.0f;
            _footerCan.GetComponent<CanvasGroup>().interactable = true;
            _footerCan.GetComponent<CanvasGroup>().blocksRaycasts = true;
            Canvas.GetComponent<CanvasGroup>().alpha = 1.0f;
            Canvas.GetComponent<CanvasGroup>().interactable = true;
            Canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
            // ReleaseSplashFromMemory();
            //StartCoroutine(LoadingHandler.Instance.ShowLoadingForCharacterUpdation(5));
        }
    }
    public IEnumerator IsSplashEnable(bool _state, float _time)
    {
        SaveCharacterProperties.NeedToShowSplash = 2;
        Canvas.GetComponent<CanvasGroup>().alpha = 0;
        //LoadingHandler.Instance.worldLoadingScreen.GetComponent<CanvasGroup>().alpha = 0.0f;
        _footerCan.GetComponent<CanvasGroup>().alpha = 0.0f;
        Canvas.GetComponent<CanvasGroup>().interactable = false;
        Canvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _footerCan.GetComponent<CanvasGroup>().interactable = false;
        _footerCan.GetComponent<CanvasGroup>().blocksRaycasts = false;
        yield return new WaitForSeconds(_time);
        print("IsSplashEnable call with  state : "+ _state);
        _SplashScreen.SetActive(_state);
        Canvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        Canvas.GetComponent<CanvasGroup>().interactable = true;
        Canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
        _footerCan.GetComponent<CanvasGroup>().interactable = true;
        _footerCan.GetComponent<CanvasGroup>().blocksRaycasts = true;
        //LoadingHandler.Instance.worldLoadingScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
        _footerCan.GetComponent<CanvasGroup>().alpha = 1.0f;
        if (Loadinghandler_CanvasRef != null)
            Loadinghandler_CanvasRef.alpha = 1.0f;
        ShowFooter(!_state);
        if (!_state)
        {
            SplashMemoryFree();
        }
    }

    public void SplashMemoryFree()
    {
        StartCoroutine(ReleaseSplashFromMemory());
    }

    IEnumerator ReleaseSplashFromMemory()
    {
        yield return new WaitForSeconds(2);
        Destroy(_SplashScreen);
    }


    public int PreviousScreen;
    public void SwitchToScreen(int Screen)
    {
        switch (Screen)
        {
            case 0:
                {
                    PreviousScreen = 0;
                    SearchWorldScreenHolder.gameObject.SetActive(false);
                    //SearchHomeHolder.gameObject.SetActive(true);
                    //SearchWorldHolder.gameObject.SetActive(false);
                    //AvatarWindowHolder.gameObject.SetActive(false);
                    /*LobbyTabHolder.gameObject.SetActive(LobbyTabHolder.GetComponent<LobbyWorldViewFlagHandler>().ActivityInApp());*/
                    //  HomeWorldTabsHolder.gameObject.SetActive(true);
                    //WorldWorldTabsHolder.gameObject.SetActive(false);
                    //WorldManager.instance.WorldPageStateHandler(false);
                    //WorldManager.instance.WorldScrollReset();
                    //worldHolder.SetActive(true);
                    searchWorldHolder.SetActive(false);
                    /*SecondSliderScrollView.GetComponent<Mask>().enabled = false;*/
                    break;
                }
            case 1:
                {
                    PreviousScreen = 1;
                    SearchWorldScreenHolder.gameObject.SetActive(false);
                    //SearchHomeHolder.gameObject.SetActive(false);
                    //SearchWorldHolder.gameObject.SetActive(true);
                    //AvatarWindowHolder.gameObject.SetActive(false);
                    /*LobbyTabHolder.gameObject.SetActive(false);*/
                    //  HomeWorldTabsHolder.gameObject.SetActive(false);
                    //WorldWorldTabsHolder.gameObject.SetActive(true);
                    //WorldManager.instance.WorldPageStateHandler(true);
                    //WorldManager.instance.WorldScrollReset();
                    /*SecondSliderScrollView.GetComponent<Mask>().enabled = true;*/
                    break;
                }
            case 2:
                {
                    //AdvanceSearchInputField.GetComponent<AdvancedInputField>().Clear();
                    //WorldManager.instance.previousSearchKey = "";
                    //WorldManager.instance.SearchKey = "";
                    //SearchWorldScreenHolder.gameObject.SetActive(true);
                    ////SearchHomeHolder.gameObject.SetActive(false);
                    ////SearchWorldHolder.gameObject.SetActive(false);
                    ////AvatarWindowHolder.gameObject.SetActive(false);
                    ///*LobbyTabHolder.gameObject.SetActive(false);*/
                    ////worldHolder.SetActive(false);
                    //WorldManager.instance.worldSearchManager.searchWorldInput.Clear();
                    //searchWorldHolder.SetActive(true);
                    ////  HomeWorldTabsHolder.gameObject.SetActive(false);
                    ////WorldWorldTabsHolder.gameObject.SetActive(false);
                    ////WorldManager.instance.WorldPageStateHandler(true);
                    //WorldManager.instance.WorldScrollReset();
                    //WorldManager.instance.WorldLoadingText(APIURL.Temp);
                    ///* SecondSliderScrollView.GetComponent<Mask>().enabled = true;*/
                    ShowFooter(true);
                    break;
                }
        }
    }
    IEnumerator FetchFeatures()
    {
        //UnityWebRequest request = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.FeaturesListApi + "?app_name=" + Application.productName + "&version=" + Application.version);
        //request.SetRequestHeader("Content-Type", "application/json");
        //request.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
        //request.SendWebRequest();
        //while (!request.isDone)
        //    yield return null;
        //if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        //{
        //    Debug.LogError("Error fetching features: " + request.error);
        //}
        //else
        //{
        //    string jsonResponse = request.downloadHandler.text;
        //    ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);
        //    if (apiResponse.success && apiResponse.data != null)
        //        for (int i = 0; i < apiResponse.data.Count; i++)
        //        {
        //            if (apiResponse.data[i].feature_list.TryGetValue("DomeHeaderInfo", out bool status))
        //            {
        //                ConstantsHolder.DomeHeaderInfo = status;
        //            }
        //            if (apiResponse.data[i].feature_list.TryGetValue("SummitApp", out bool status1))
        //            {
        //                ConstantsHolder.xanaConstants.SwitchXanaToXSummit = status1;
        //            }
        //            if (apiResponse.data[i].feature_list.TryGetValue("Xsummitbg", out bool status2))
        //            {
        //                ConstantsHolder.xanaConstants.XSummitBg = status2;

        //                XSummitBgChange.ChangeSummitBG(); // Call ChangeBackground after 2 seconds

        //                if (SceneManager.GetSceneByName("XSummitLoginSignupScene").isLoaded)
        //                {
        //                    UserLoginSignupManager.instance.XSummitBgChange.ChangeSummitBG();
        //                }


        //            }
        //            if (apiResponse.data[i].feature_list.TryGetValue("XanaChatFlag", out bool status3))
        //            {
        //                ConstantsHolder.xanaConstants.chatFlagBtnStatus = status3;
        //            }
        //            if (apiResponse.data[i].feature_list.TryGetValue("TelegramLogin", out bool status4))
        //            {
        //                ConstantsHolder.xanaConstants.TelegramLoginBtnStatus = status4;
        //            }

        //            #region Tap To Know Variables

        //            if (apiResponse.data[i].feature_list.TryGetValue("tapToKnow_organization", out bool status5))
        //            {
        //                ConstantsHolder.xanaConstants.tapToKnow_organization = status5;
        //            }
        //            if (apiResponse.data[i].feature_list.TryGetValue("tapToKnow_profession", out bool status6))
        //            {
        //                ConstantsHolder.xanaConstants.tapToKnow_profession = status6;
        //            }
        //            if (apiResponse.data[i].feature_list.TryGetValue("tapToKnow_industry", out bool status7))
        //            {
        //                ConstantsHolder.xanaConstants.tapToKnow_industry = status7;
        //            }
        //            if (apiResponse.data[i].feature_list.TryGetValue("tapToKnow_email", out bool status8))
        //            {
        //                ConstantsHolder.xanaConstants.tapToKnow_email  = status8;
        //            }
        //            if (apiResponse.data[i].feature_list.TryGetValue("tapToKnow_contact", out bool status9))
        //            {
        //                ConstantsHolder.xanaConstants.tapToKnow_contact = status9;
        //            }
        //            if (apiResponse.data[i].feature_list.TryGetValue("tapToKnow_join", out bool status10))
        //            {
        //                ConstantsHolder.xanaConstants.tapToKnow_join = status10;
        //            }
        //            if (apiResponse.data[i].feature_list.TryGetValue("tapToKnow_hobbies", out bool status11))
        //            {
        //                ConstantsHolder.xanaConstants.tapToKnow_hobbies = status11;
        //            }
        //            if (apiResponse.data[i].feature_list.TryGetValue("tapToKnow_status", out bool status12))
        //            {
        //                ConstantsHolder.xanaConstants.tapToKnow_status = status12;
        //            }

        //            #endregion

        //            //if (apiResponse.data[i].feature_name == "SummitApp")
        //            //{
        //            //    ConstantsHolder.xanaConstants.SwitchXanaToXSummit = apiResponse.data[i].feature_status;
        //            //}
        //            //if (apiResponse.data[i].feature_name == "Xsummitbg")
        //            //{
        //            //    ConstantsHolder.xanaConstants.XSummitBg = apiResponse.data[i].feature_status;
        //            //    if (SceneManager.GetSceneByName("XSummitLoginSignupScene").isLoaded)
        //            //    {
        //            //        UserLoginSignupManager.instance.XSummitBgChange.ChangeBG();
        //            //    }
        //            //}

        //            //if (apiResponse.data[i].feature_name == "XanaChatFlag")
        //            //{
        //            //    ConstantsHolder.xanaConstants.chatFlagBtnStatus = apiResponse.data[i].feature_status;
        //            //}
        //        }
        //    Invoke("ChangeBackground", 5f);
        //}
        //request.Dispose();
        return null;
    }
    void ChangeBackground()
    {
        //if (UserLoginSignupManager.instance)
        //{
        //    if (PlayerPrefs.HasKey("TermsConditionAgreement"))
        //    {
        //        UserLoginSignupManager.instance.CheckForAutoLogin();

        //    }
        //    else
        //    {
        //        UserLoginSignupManager.instance.termsConditionPanel.SetActive(true);
        //    }
        //}
    }
}
[System.Serializable]
public class AppData
{
    public int id;
    public string app_name;
    public string version;
    public bool is_active;
    public Dictionary<string, bool> feature_list;
}
[System.Serializable]
public class ApiResponse
{
    public bool success;
    public List<AppData> data;
    public string msg;
}