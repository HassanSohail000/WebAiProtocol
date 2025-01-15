
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerSideUserDataHandler : MonoBehaviour
{
    public static ServerSideUserDataHandler Instance;

    //Event will be called when user loged In and new Avatar is saved by user. Event is created for multiple avatar saving.
    public event Action<int, int> loadAllAvatar;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void CreateUserOccupiedAsset(Action CallBack)
    {
        StartCoroutine(CreateUserData(CallBack));
    }
    public void GetDataFromServer()
    {
        StartCoroutine(GetUserData(ConstantsGod.AUTH_TOKEN));
    }

    IEnumerator GetUserData(string token)   // check if  data Exist
    {
       
        UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.OCCUPIDEASSETS + "1/1");
        www.SetRequestHeader("Authorization", token);
        www.SendWebRequest();
        while (!www.isDone)
        {
            yield return null;
        }
        string str = www.downloadHandler.text;
        Root getdata = new Root();
        getdata = JsonUtility.FromJson<Root>(str);
        if (www.result!=UnityWebRequest.Result.ConnectionError && www.result==UnityWebRequest.Result.Success)
        {
            if (getdata.success)
            {
                if (getdata.data.count == 0)
                {
                    //print("!!Not Data Found, New User");
                    SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
                    SubCatString.FaceBlendsShapes = new float[GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];
                    string jbody = GameManager.Instance.selectedPresetData != "" ? GameManager.Instance.selectedPresetData : JsonUtility.ToJson(SubCatString);
                    File.WriteAllText(GameManager.Instance.GetStringFolderPath(), jbody);
                    //if user does not have data then open preset panel
                    ConstantsHolder.xanaConstants.isFirstPanel = true;
                    //if (!ConstantsHolder.xanaConstants.SwitchXanaToXSummit)
                    //{
                    //    LoadingHandler.Instance.nftLoadingScreen.SetActive(false);
                    //}
                    //else
                    //{
                    //    LoadingHandler.Instance.LoadingScreenSummit.SetActive(false);
                    //}
                    //MainSceneEventHandler.OpenPresetPanel?.Invoke();
                }
                else
                {
                    string jsonbody = JsonUtility.ToJson(getdata.data.rows[0].json);
                    LoadPlayerAvatar.avatarId = getdata.data.rows[0].id.ToString();
                    LoadPlayerAvatar.avatarName = getdata.data.rows[0].name;
                    //Debug.Log("avatarName: " + jsonbody);
                    LoadPlayerAvatar.avatarThumbnailUrl = getdata.data.rows[0].thumbnail;
                    ConstantsHolder.userId = getdata.data.rows[0].createdBy.ToString();
                    File.WriteAllText(GetStringFolderPath(), jsonbody);
                    yield return new WaitForSeconds(0.1f);
                    if (!ConstantsHolder.xanaConstants.openLandingSceneDirectly && ConstantsHolder.xanaConstants.SwitchXanaToXSummit)
                    {
                        if (Screen.orientation == ScreenOrientation.LandscapeRight || Screen.orientation == ScreenOrientation.LandscapeLeft)
                        {
                            if (ConstantsHolder.xanaConstants.EnableSignInPanelByDefault)
                                Screen.orientation = ScreenOrientation.LandscapeLeft;
                            else
                                Screen.orientation = ScreenOrientation.Portrait;
                            //LoadingHandler.Instance.LoadingScreenSummit.SetActive(false);
                        }
                    }
                    if (ConstantsHolder.xanaConstants.openLandingSceneDirectly)
                    {
                        //GameManager.Instance.mainCharacter.GetComponent<CharacterOnScreenNameHandler>().SetNameOfPlayerAgain();
                        // assign gender to save character properties
                        // This gander is use for character initialization 
                        SaveCharacterProperties.instance.SaveItemList.gender = getdata.data.rows[0].json.gender;
                        if (!ConstantsHolder.xanaConstants.isJoiningXANADeeplink)
                        {
                           // Debug.Log("Open landing Call from Get User");
                            //MainSceneEventHandler.OpenLandingScene?.Invoke();
                        }
                        yield break;
                    }

                    if (!ConstantsHolder.xanaConstants.isSummitBtnPressed)
                    {
                        Screen.orientation = ScreenOrientation.Portrait;
                    }
                    loadprevious();

                    GameManager.Instance.mainCharacter.GetComponent<AvatarController>().InitializeAvatar();
                }
            }
        }

        www.Dispose();

        if (loadAllAvatar != null && InventoryManager.instance.MultipleSave)
        {
            loadAllAvatar?.Invoke(1, 20);
        }

        
        //LoadPlayerAvatar.instance_loadplayer.LoadPlayerAvatar_onAvatarSaved(1, 20);

    }



    public IEnumerator CreateUserData(Action callBack)   // send json data with user id but first check if user already exist or not   user ID
    {

        string url = ConstantsGod.API_BASEURL + ConstantsGod.CREATEOCCUPIDEUSER;
        //  string urlwithId = url + UserIDfromServer; //Adding user ID
        //Get data from file 
        // need to add check if file exists
        Json json = new Json();
        json = json.CreateFromJSON(File.ReadAllText(GetStringFolderPath()));
        //StartCoroutine(AddingEnteries(UserIDfromServer));
        SendUpdateData senddata = new SendUpdateData();
        senddata.name = LoadPlayerAvatar.avatarName;
        senddata.json = JsonUtility.ToJson(json);
        senddata.thumbnail = LoadPlayerAvatar.avatarThumbnailUrl;
        senddata.description = "None";
        string bodyJson = JsonUtility.ToJson(senddata);
        //print(bodyJson);
        UnityWebRequest www = new UnityWebRequest(url, "Post");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJson);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();
        while (!www.isDone)
        {
            yield return null;
        }
        //////Debug.Log(www.downloadHandler.text);
        string str = www.downloadHandler.text;
        Root getdata = new Root();
        getdata = JsonUtility.FromJson<Root>(str);
        //  print(getdata.success);
        if (!www.isHttpError && !www.isNetworkError)
        {
            if (getdata.success)
            {

                print("DataUpdated");
                GetResponseupdatedata _getdata = new GetResponseupdatedata();
                _getdata = JsonUtility.FromJson<GetResponseupdatedata>(www.downloadHandler.text);
                print(_getdata.msg);
                print("Data Sent : " + _getdata.success);
                print(JsonUtility.ToJson(_getdata.data.ToString()));
            }
        }

        callBack();

        if (loadAllAvatar != null)
            loadAllAvatar?.Invoke(1, 1);
        //LoadPlayerAvatar.instance_loadplayer.LoadPlayerAvatar_onAvatarSaved(1, 1);
    }

    public void UpdateUserOccupiedAsset(string avatarID)
    {
        StartCoroutine(UpdateExistingUserData(avatarID));
    }

    IEnumerator UpdateExistingUserData(string avatarID)
    {

        Json json = new Json();
        json = json.CreateFromJSON(File.ReadAllText(GetStringFolderPath()));

        SendUpdateData senddata = new SendUpdateData();
        senddata.name = LoadPlayerAvatar.avatarName;
        senddata.json = JsonUtility.ToJson(json);
        senddata.thumbnail = LoadPlayerAvatar.avatarThumbnailUrl;
        senddata.description = "avatar updated at :-" + DateTime.Now.ToString();
        string bodyJson = JsonUtility.ToJson(senddata);

        //WWWForm formData = new WWWForm();
        //formData.AddField("name", senddata.name);
        //formData.AddField("thumbnail", senddata.thumbnail);
        //formData.AddField("json", senddata.json);
        //formData.AddField("description", senddata.description);

        ////Debug.Log(LoadPlayerAvatar.avatarId + "--" + LoadPlayerAvatar.avatarName + "--" + LoadPlayerAvatar.avatarThumbnailUrl + "--" + senddata.json);

        //UnityWebRequest www =UnityWebRequest.Post(ConstantsGod.API_BASEURL + ConstantsGod.UPDATEOCCUPIDEUSER + avatarID,formData);

        UnityWebRequest www = new UnityWebRequest(ConstantsGod.API_BASEURL + ConstantsGod.UPDATEOCCUPIDEUSER + avatarID, "Post");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJson);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        ////Debug.Log(www.downloadHandler.text);
        string str = www.downloadHandler.text;
        Root getdata = new Root();
        getdata = JsonUtility.FromJson<Root>(str);

        if (!www.isHttpError && !www.isNetworkError)
        {
            if (getdata.success)
            {

                print("DataUpdated");
                //if (InventoryManager.instance.AvatarUpdated != null)
                //InventoryManager.instance.AvatarUpdated.SetActive(true);
                if (InventoryManager.instance.MultipleSave && LoadPlayerAvatar.instance_loadplayer != null)
                    LoadPlayerAvatar.instance_loadplayer.LoadPlayerAvatar_onAvatarSaved(1, 1);
                /*if (InventoryManager.instance.isSaveFromreturnHomePopUp)
                {
                    InventoryManager.instance.AvatarUpdated.SetActive(false);
                    InventoryManager.instance.isSaveFromreturnHomePopUp = false;
                    InventoryManager.instance.OnClickHomeButton();
                }*/
                //   GetResponseupdatedata getdata = new GetResponseupdatedata();
                // getdata = JsonUtility.FromJson<GetResponseupdatedata>(www.downloadHandler.text);
                //   print(getdata.msg);
                //   print( "Data Sent : "+getdata.success);
                //  print(JsonUtility.ToJson(getdata.data.json));
            }
        }

        //LoadPlayerAvatar.instance_loadplayer.LoadPlayerAvatar_onAvatarSaved(1, 1);
    }




    public void DeleteAvatarDataFromServer(string token, string UserId)
    {
        StartCoroutine(DeleteUserData(token, UserId));
    }


    IEnumerator DeleteUserData(string token, string userID)   // delete data if Exist
    {
        //  print("Token " + ConstantsGod.AUTH_TOKEN);
        UnityWebRequest www = UnityWebRequest.Delete(ConstantsGod.API_BASEURL + ConstantsGod.DELETEOCCUPIDEUSER + userID);
        www.SetRequestHeader("Authorization", token);
        yield return www.SendWebRequest();

        if (www.responseCode == 200)
        {
            ////Debug.Log("Occupied Asset Delete Successfully");
        }

        //string str = www..text;
        //Root db = new Root();
        //db = JsonUtility.FromJson<Root>(str);
        ////print(db.success);
        //// print(db.data);
        ////print(db.msg);
        //if (db.msg == "Occupied Asset Delete Successfully")
        //{
        //    print("data Deleted successfully");

        //}
        //else if (db.msg == "Occupied Asset get successfully")
        //{
        //    print("data Received Successfully");
        //}
    }
    [HideInInspector]
    public int UserIDfromServer;

    //For Preset account to get presets
    public void getPresetDataFromServer()
    {
        //  StartCoroutine(GetPresetData_Server());
    }
    IEnumerator GetPresetData_Server()   // check if  data Exist
    {
        UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.OCCUPIDEASSETS + "1/50");
        www.SetRequestHeader("Authorization", PlayerPrefs.GetString("LoginToken_Preset"));
        yield return www.SendWebRequest();
        ////Debug.Log(www.downloadHandler.text);
        string str = www.downloadHandler.text;
        Root getdata = new Root();
        getdata = JsonUtility.FromJson<Root>(str);

        //DefaultEnteriesforManican.instance.DefaultReset();
        if (!www.isHttpError && !www.isNetworkError)
        {
            if (getdata.success)
            {

                // its a new user so create file 
                if (getdata.data.count == 0)
                {
                    // do nothing
                }
                else
                {
                    // write latest json data to file
                    GameObject contentparent = InventoryManager.instance.ClothsPanel[4].GetComponent<ScrollRect>().content.gameObject;
                    GameObject contentStartUpPanel = InventoryManager.instance.PresetArrayContent;

                    for (int c = 0; c < getdata.data.count - 1; c++)
                    {
                        contentparent.transform.GetChild(c).GetComponent<PresetData_Jsons>().JsonDataPreset =
                         JsonUtility.ToJson(getdata.data.rows[c].json);
                        // Populating Panel For the FirstTimeOnly
                        //Kindly add a check to populate it once 
                        contentStartUpPanel.transform.GetChild(c).GetComponent<PresetData_Jsons>().JsonDataPreset =
                        JsonUtility.ToJson(getdata.data.rows[c].json);
                        //              string jsonbody = JsonUtility.ToJson(getdata.data.rows[0].json);
                    }
                    File.WriteAllText((Application.persistentDataPath + "/SavingReoPreset.json"), JsonUtility.ToJson(getdata.data.rows[0].json));
                    yield return new WaitForSeconds(0.1f);
                    //               loadprevious();
                }
            }
        }
        else
            Debug.Log("NetWorkissue");

    }


    public int localindexfilebuffer;

    public void loadprevious()
    {
        if (GameManager.Instance)
        {
            //GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
            ////Debug.Log("~~~ load pervoius call");
            //Invoke(nameof(WaitForFile), 10);
            //StartCoroutine(WaitForFile());
            SaveCharacterProperties.instance.LoadMorphsfromFile(); // loading morohs 
                                                                   // DefaultEnteriesforManican.instance.LastSaved_Reset();
        }

    }

    IEnumerator WaitForFile()
    {
        yield return new WaitForSeconds(5);
        //if (GameManager.Instance)
        //{
        //    GameManager.Instance.mainCharacter.GetComponent<Equipment>().SetItemDataFromFile();

        //}

    }

    public string GetStringFolderPath()
    {
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)  // loged from account)
        {

            return (Application.persistentDataPath + "/logIn.json");
        }
        else
        {

            return (Application.persistentDataPath + "/loginAsGuestClass.json");
        }
    }


    [Serializable]
    public class SendUpdateData
    {
        public string name;
        public string json;
        public string thumbnail;
        public string description;
    }
    [Serializable]
    public class GetResponseupdatedata
    {
        public string success;
        public Data data;
        public string msg;
    }

    [Serializable]
    public class ItemPrefab
    {
        public int instanceID;
    }

    [Serializable]
    public class ItemIcon
    {
        public int instanceID;
    }

    [Serializable]
    public class MyItemObj
    {
        public string Slug;
        public string ItemType;
        public ItemPrefab ItemPrefab;
        public int ItemID;
        public string ItemName;
        public string ItemDescription;
        //  public string ItemLink;
        public string ItemLinkAndroid;
        public string ItemLinkIOS;
        public string SubCategoryname;
        public bool Stackable;
        public ItemIcon ItemIcon;
    }

    public class MAnimCurve
    {
        public string serializedVersion;
        public List<object> m_Curve;
        public int m_PreInfinity;
        public int m_PostInfinity;
        public int m_RotationOrder;
    }

    [Serializable]
    public class EyePresets
    {
        public int _FaceMorphFeature;
        public int f_BlendShapeOne;
        public int f_BlendShapeTwo;
        public int m_BlendTime;
        public MAnimCurve m_AnimCurve;
    }

    [Serializable]
    public class NosePresets
    {
        public int _FaceMorphFeature;
        public int f_BlendShapeOne;
        public int f_BlendShapeTwo;
        public int m_BlendTime;
        public MAnimCurve m_AnimCurve;
    }

    [Serializable]
    public class LipsPresets
    {
        public int _FaceMorphFeature;
        public int f_BlendShapeOne;
        public int f_BlendShapeTwo;
        public int m_BlendTime;
        public MAnimCurve m_AnimCurve;
    }

    [Serializable]
    public class EyeBrowPresets
    {
        public int _FaceMorphFeature;
        public int f_BlendShapeOne;
        public int f_BlendShapeTwo;
        public int m_BlendTime;
        public MAnimCurve m_AnimCurve;
    }

    [Serializable]
    public class FacePresets
    {
        public int _FaceMorphFeature;
        public int f_BlendShapeOne;
        public int f_BlendShapeTwo;
        public int m_BlendTime;
        public MAnimCurve m_AnimCurve;
    }

    [Serializable]
    public class Json
    {
        public string id;
        public string name;
        public string thumbnail;
        public string gender;
        public List<Item> myItemObj;
        public string avatarType = "OldAvatar";

        public string ai_gender;
        public Color hair_color;
        public string skin_color;
        public Color lip_color;
        public string face_gender;
        public int faceItemData;
        public int lipItemData;
        public int noseItemData;
        public string hairItemData;
        public string eyeItemData;
        public int eyeShapeItemData;
        public bool charactertypeAi;

        public List<BoneDataContainer> SavedBones;
        public int SkinId;
        public Color Skin;
        public Color LipColor;
        public float SssIntensity;
        public Color SkinGerdientColor;
        public bool isSkinColorChanged;
        public bool isLipColorChanged;
        public int HairColorPaletteValue;
        public int BodyFat;
        public int FaceValue;
        public int NoseValue;
        public int EyeValue;
        public int EyesColorValue;
        public int EyesColorPaletteValue;
        public int EyeBrowValue;
        public int EyeBrowColorPaletteValue;
        public int EyeLashesValue;
        public int MakeupValue;
        public int LipsValue;
        public int LipsColorValue;
        public int LipsColorPaletteValue;
        public bool faceMorphed;
        public bool eyeBrowMorphed;
        public bool eyeMorphed;
        public bool noseMorphed;
        public bool lipMorphed;
        public string PresetValue;
        public string eyeTextureName;
        public string eyeLashesName;
        public string eyebrrowTexture;
        public string makeupName;
        public float[] FaceBlendsShapes;


        public Color HairColor;
        public Color EyebrowColor;
        public Color EyeColor;

        public Json CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Json>(jsonString);
        }
    }

    [Serializable]
    public class User
    {
        public int id;
        public string name;
        public string email;
        public string avatar;
    }

    [Serializable]
    public class Row
    {
        public int id;
        public string name;
        public string thumbnail;
        public Json json;
        public string description;
        public bool isDeleted;
        public int createdBy;
        public DateTime createdAt;
        public DateTime updatedAt;
        public User user;
    }
    [Serializable]
    public class Root
    {
        public bool success;
        public Data data;
        public string msg;
    }

    [Serializable]
    public class Data
    {
        public int count;
        public List<Row> rows;
    }
}


