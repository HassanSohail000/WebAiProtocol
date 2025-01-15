using UnityEngine;
using System;

public class BackButtonHandler : MonoBehaviour
{

    public screenTabs _screenTabs;
    public static BackButtonHandler instance;
    public GameObject exitPanel;
    public GameObject landscapeExit;

    public enum screenTabs
    {
        Hometab,
        Othertabs,
        Avatar,
        Post,
        FeedProfile,
        FriendsProfile,
        EditProfile,
        FaceCam,
        MainSetting,
        SubSetting,
        Gameplay
    }

    private void OnEnable()
    {
        //HomeFooterHandler.OnScreenTabStateChange += setScreenTabEnum;
        //UIHandler.OnScreenTabStateChange += setScreenTabEnum;
        InventoryManager.OnScreenTabStateChange += setScreenTabEnum;
       // FeedData.OnScreenTabStateChange += setScreenTabEnum;
       // FindFriendWithNameItem.OnScreenTabStateChange += setScreenTabEnum;
       // MyProfileDataManager.OnScreenTabStateChange += setScreenTabEnum;
        //UGCUIManager.OnScreenTabStateChange += setScreenTabEnum; // UGC Removed by Ahsan
       // WorldManager.OnScreenTabStateChange += setScreenTabEnum;
       // FollowingItemController.OnScreenTabStateChange += setScreenTabEnum;
       // SNSSettingController.OnScreenTabStateChange += setScreenTabEnum;
    }

    private void OnDisable()
    {
       // HomeFooterHandler.OnScreenTabStateChange -= setScreenTabEnum;
       // UIHandler.OnScreenTabStateChange -= setScreenTabEnum;
        InventoryManager.OnScreenTabStateChange -= setScreenTabEnum;
      //  FeedData.OnScreenTabStateChange -= setScreenTabEnum;
       // FindFriendWithNameItem.OnScreenTabStateChange -= setScreenTabEnum;
       // MyProfileDataManager.OnScreenTabStateChange -= setScreenTabEnum;
        //UGCUIManager.OnScreenTabStateChange -= setScreenTabEnum; // UGC Removed by Ahsan
       // WorldManager.OnScreenTabStateChange -= setScreenTabEnum;
//FollowingItemController.OnScreenTabStateChange -= setScreenTabEnum;
      //  SNSSettingController.OnScreenTabStateChange -= setScreenTabEnum;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackButtonPressed();
        }
    }

    private void HandleBackButtonPressed()
    {
        switch (_screenTabs)
        {
            case screenTabs.Hometab:
                PromptQuitGame();
                break;
            case screenTabs.Othertabs:
                GoToHomeScreen();
                break;
            case screenTabs.Avatar:
                ExitFromAvatarTab();
                break;
            case screenTabs.Post:
                ExitFromPostTab();
                break;
            case screenTabs.FeedProfile:
                ExitfromFeedOtherPlayerProfile();
                break;
            case screenTabs.FriendsProfile:
                ExitFromAddFriendsProfile();
                break;
            case screenTabs.EditProfile:
                ExitFromEditProfile();
                break;
            case screenTabs.FaceCam:
                ExitFromfaceCam();
                break;
            case screenTabs.MainSetting:
                ExitFromMainSetting();
                break;
            case screenTabs.SubSetting:
                ExitFromSubSetting();
                break;
        }
    }

    private void ExitFromEditProfile()
    {
       // MyProfileDataManager.Instance.editProfileScreen.SetActive(false);
       // FeedUIController.Instance.footerCan.SetActive(true);
        _screenTabs = screenTabs.Othertabs;
    }

    private void ExitFromMainSetting()
    {
       // FeedUIController.Instance.SNSSettingController.OnClickSettingClose();
    }

    private void ExitFromSubSetting()
    {
      //  FeedUIController.Instance.SNSSettingController.OnClickMyAccountBackButton();
    }

    private void ExitFromfaceCam()
    {
        // UGC Removed by Ahsan
        //UGCUIManager.instance.BackToHomeScreen();
    }

    private void ExitFromAvatarTab()
    {
        InventoryManager.instance.OnClickBackButton();
    }

    private void GoToHomeScreen()
    {
      //  GameManager.Instance.bottomTabManagerInstance.OnClickHomeButton();
    }

    private void ExitFromPostTab()
    {
        GameManager.Instance.UiManager.ResetPlayerToLastPostPosted();
        GameManager.Instance.UiManager.SwitchToPostScreen(false);

    }

    private void ExitfromFeedOtherPlayerProfile()
    {
      //  GameManager.Instance.bottomTabManagerInstance.OnClickFeedButton();
    }

    private void ExitFromAddFriendsProfile()
    {
       // GameManager.Instance.bottomTabManagerInstance.OnClickAddFriends();
    }

    private void PromptQuitGame()
    {
        if (Screen.orientation == ScreenOrientation.Portrait)
            exitPanel.SetActive(true);
        else
            landscapeExit.SetActive(true);
    }

    public void ConfirmQuitDialog()
    {
        Application.Quit();
    }


    public void setScreenTabEnum(screenTabs screenTabs)
    {
        _screenTabs = screenTabs;
    }

}
