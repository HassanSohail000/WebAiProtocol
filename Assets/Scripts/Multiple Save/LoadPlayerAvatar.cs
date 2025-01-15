using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class LoadPlayerAvatar : ServerSideUserDataHandler
{
    public GameObject mainPanel;
    public ScrollRect avatarScrollRect;
    public GameObject avatarPrefab;
    public GameObject contentParent;

    public Button avatarButton;
    public Button continueButton;
    public Button deleteButton;

    public GameObject playerNamePanel;
    public GameObject deleteAvatarPanel;
    public TMP_InputField playerNameInputField;
    public Button PlayerPanelSaveButton;
    public Button updateExistingAvatar;

    public GameObject screenShotloader;
    [Header("Refrences for Store Prefab")]
    public GameObject loader;
    public Button saveButton;

    public static string avatarId = null;
    public static string avatarName = "Test";
    public static string avatarThumbnailUrl = "";
    public static string avatarJson = null;
    public static GameObject currentSelected = null;

    public static LoadPlayerAvatar instance_loadplayer;

    private int currentpageNum = 1;
    private int pageSize = 15;
    [SerializeField] GameObject NewAvatarBtn;


    private string compressionSuffix = "?width=512&height=512";
    private void Awake()
    {
        instance_loadplayer = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        avatarButton = InventoryManager.instance.myAvatarButton;
        loader.SetActive(false);
        if (PlayerPrefs.GetInt("IsLoggedIn") == 0)
        {
            if (InventoryManager.instance.MultipleSave)
                avatarButton.gameObject.SetActive(false);
        }
        else
        {
            if (InventoryManager.instance.MultipleSave)
                avatarButton.gameObject.SetActive(true);
        }

        avatarButton.onClick.AddListener(OpenAvatarPanel);

        Invoke(nameof(waitCall), 5f);
    }


    void waitCall()
    {
        if (InventoryManager.instance.MultipleSave)
            loadAllAvatar += (pageNo, NoOfRecords) => { LoadPlayerAvatar_onAvatarSaved(pageNo, NoOfRecords); };
    }
    //Event will be called when user loged In and new Avatar is saved by user.
    public void LoadPlayerAvatar_onAvatarSaved(int pageSize, int noOfRecords)
    {

        //disable the button for current release enable this feature later.
        if (avatarButton == null)
        {
            avatarButton = InventoryManager.instance.myAvatarButton;
        }
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        {
            if (InventoryManager.instance.MultipleSave)
            {
                avatarButton.gameObject.SetActive(true);
            }
            else
            {
                avatarButton.gameObject.SetActive(false);
            }
        }
        StartCoroutine(GetAvatarData_Server(pageSize, noOfRecords));
    }

    private void OnEnable()
    {
        if (currentSelected == null)
        {
            continueButton.interactable = false;
            deleteButton.interactable = false;
        }

    }

    public void OpenAvatarPanel()
    {
        if (/*!UserPassManager.Instance.CheckSpecificItem("MyAvatar")*/ false)
        {
            print("Please Upgrade to Premium account");
            return;
        }
        else
        {
            print("Horayyy you have Access");
        }
        if (contentParent.transform.childCount >= 1)
        {
            EmptyAvatarContainer();
            StartCoroutine(GetAvatarData_Server(1, 20));
        }
        mainPanel.SetActive(true);
    }

    public void CloseAvatarPanel()
    {
        mainPanel.SetActive(false);
        callAPIOneMoreTime = true;
    }

    public void OpenPlayerNamePanel()
    {
        if (avatarId == null && contentParent.transform.childCount > 1)
        {
            updateExistingAvatar.interactable = false;
        }
        else
            updateExistingAvatar.interactable = true;
        playerNameInputField.text = string.Empty;
        playerNamePanel.SetActive(true);
    }

    public void ClosePlayerNamePanel()
    {
        playerNamePanel.SetActive(false);
        InventoryManager.instance.isSaveFromreturnHomePopUp = false;
    }

    public void CloseDeleteAvatarPanel()
    {
        deleteAvatarPanel.SetActive(false);
    }

    public void OnEditingEnd()
    {
        avatarName = playerNameInputField.text;

        //if (string.IsNullOrEmpty(playerNameInputField.text))
        //{
        //    PlayerPanelSaveButton.interactable = false;
        //}
        //else
        //{
        //    PlayerPanelSaveButton.interactable = true;
        //}

    }

    public void HighLightSelected(GameObject clickObject)
    {
        continueButton.interactable = true;
        deleteButton.interactable = true;
        for (int i = 0; i < contentParent.transform.childCount; i++)
        {
            if (contentParent.transform.GetChild(i).GetComponent<SavedPlayerDataJson>())
                contentParent.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
        //enable prefab hightlight image
        clickObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    bool callAPIOneMoreTime = true;

    IEnumerator GetAvatarData_Server(int pageNo, int noOfRecords)   // check if  data Exist
    {
        //UnityWebRequest www = UnityWebRequest.Get("https://app-api.xana.net/item/get-user-occupied-asset/1/50");
        UnityWebRequest www = UnityWebRequest.Get(ConstantsGod.API_BASEURL + ConstantsGod.OCCUPIDEASSETS + pageNo + "/" + noOfRecords);
        www.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
        www.SendWebRequest();
        loader.SetActive(true);
        while (!www.isDone)
        {
            yield return null;
        }
        Debug.Log("Get all Avatar :- " + www.downloadHandler.text);
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
                    loader.SetActive(false);
                    // do nothing
                }
                else
                {
                    if(getdata.data.rows.Count > 0)
                    {
                        // write latest json data to file
                        for (int c = 0; c < getdata.data.rows.Count; c++)
                        {
                            //Debug.Log(getdata.data.rows[c].id.ToString()+"-------"+ getdata.data.rows[c].thumbnail);
                            if (!string.IsNullOrEmpty(getdata.data.rows[c].id.ToString()) && !string.IsNullOrEmpty(getdata.data.rows[c].thumbnail))
                            {
                                GameObject avatarInstance = Instantiate(avatarPrefab);
                                avatarInstance.transform.SetParent(contentParent.transform);
                                avatarInstance.transform.localPosition = Vector3.zero;
                                avatarInstance.transform.localScale = Vector3.one;
                                avatarInstance.transform.localRotation = Quaternion.identity;

                                if (pageNo == 1 && noOfRecords == 1)
                                {
                                    avatarInstance.transform.SetAsFirstSibling();
                                    avatarId = getdata.data.rows[c].id.ToString();
                                }

                                avatarInstance.GetComponent<SavedPlayerDataJson>().id = getdata.data.rows[c].id.ToString();
                                avatarInstance.GetComponent<SavedPlayerDataJson>().name = getdata.data.rows[c].name;
                                avatarInstance.GetComponent<SavedPlayerDataJson>().playerName.text = getdata.data.rows[c].name;
                                avatarInstance.GetComponent<SavedPlayerDataJson>().avatarJson = JsonUtility.ToJson(getdata.data.rows[c].json, true);
                                string thumbnailLink = getdata.data.rows[c].thumbnail;
                                avatarInstance.GetComponent<SavedPlayerDataJson>().avatarThumbnailLink = thumbnailLink;

                                GameObject imageObject = avatarInstance.GetComponent<SavedPlayerDataJson>().ImageObject;
                                GameObject loader = avatarInstance.GetComponent<SavedPlayerDataJson>().ImageDownloadingLoader;
                                loader.SetActive(true);
                                if (!string.IsNullOrEmpty(thumbnailLink) && thumbnailLink.Contains("http"))
                                    StartCoroutine(DownloadThumbnail(thumbnailLink, imageObject, loader));

                                avatarInstance.GetComponent<Button>().onClick.AddListener(() => HighLightSelected(avatarInstance));
                                if (pageNo == 1 && noOfRecords == 1)
                                {
                                    HighLightSelected(avatarInstance);
                                }
                                avatarInstance.gameObject.name = getdata.data.rows[c].id.ToString();
                            }
                        }

                        //File.WriteAllText((Application.persistentDataPath + "/SavingReoPreset.json"), JsonUtility.ToJson(getdata.data.rows[0].json));
                        yield return new WaitForSeconds(0.1f);
                        currentpageNum++;
                        loadNewPage = true;
                    }else
                    {
                        if (callAPIOneMoreTime)
                        {
                            callAPIOneMoreTime = false;
                            loadNewPage = true;
                            currentpageNum = 1; pageSize = 50;
                            print("Waqas Here And API is called");
                        }
                    }
                    
                    yield return StartCoroutine(offLoader());
                    www.Dispose();
                    NewAvatarBtn.transform.SetSiblingIndex(0);// to set new avatar button always on top
                    //if (InventoryManager.instance.isSaveFromreturnHomePopUp)
                    //{
                    //    InventoryManager.instance.OnClickHomeButton();
                    //}
                }
            }
            else
            {
                loader.SetActive(false);
            }

        }
        else
        {
           Debug.Log("NetWorkissue");
            loader.SetActive(false);
        }


    }


    IEnumerator offLoader()
    {
        yield return new WaitForSeconds(0.2f);
        loader.SetActive(false);
    }
    /// <summary>
    /// to Download Skin Texture from asset bundels
    /// </summary>
    /// <param name="TextureURL"> texture url to download</param>
    /// <returns></returns>
    public IEnumerator DownloadSkinTextureFile(string TextureURL)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(TextureURL))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Texture2D tex = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                // tex.Compress(true);
                byte[] fileData = tex.EncodeToPNG();
                string filePath = Application.persistentDataPath + "/" + name;
                File.WriteAllBytes(filePath, fileData);


            }
            applySkinTexture(ReadTextureFromFile(Application.persistentDataPath + "/" + name));
            uwr.Dispose();
        }
        yield return null;
    }

    IEnumerator DownloadThumbnail(string ImageUrl, GameObject thumbnail, GameObject downloadingLoader)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
        }
        else
        {
            if (ImageUrl.Equals(""))
            {
                downloadingLoader.SetActive(false);
                yield return null;
            }
            else
            {
                if (thumbnail != null)
                {
                    //print("~~~~~~~~~ " + ImageUrl+compressionSuffix);
                    ImageUrl = ImageUrl + compressionSuffix;
                    UnityWebRequest www = UnityWebRequestTexture.GetTexture(ImageUrl);
                    www.SendWebRequest();
                    //WWW www = new WWW(ImageUrl);
                    //www.SendWebRequest();
                    while (!www.isDone)
                    {
                        yield return null;
                    }
                    yield return www;
                    //Debug.Log(ImageUrl+"------"+www.downloadHandler.text);
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    texture.Compress(true);
                    thumbnail.GetComponent<RawImage>().texture = texture;
                    www.Dispose();
                    downloadingLoader.SetActive(false);
                    loader.SetActive(false);
                }
                //if (Application.internetReachability == NetworkReachability.NotReachable)
                //{

                //}
                //else
                //{
                //    //Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                //    if (thumbnail != null)
                //    {
                //        thumbnail.GetComponent<Image>().sprite = sprite;
                //        //Loader.SetActive(false);
                //    }
                //    else
                //    {
                //        // Loader.SetActive(false);
                //    }
                //}
            }
        }
    }

    public Texture2D ReadTextureFromFile(string path)
    {
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

    /// <summary>
    /// Apply Downloaded skin texture
    /// </summary>
    /// <param name="tex"> texture to apply </param>
    public void applySkinTexture(Texture2D tex)
    {
       Debug.Log("Waqas Eye : " + tex.name);
        GameManager.Instance.m_ChHead.GetComponent<Renderer>().materials[2].SetTexture("_BaseMap", tex); ;


        // Commented By Talha Now use texture for Body
        //for (int i = 0; i < GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts.Count; i++)
        //{
        //    if (GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>())
        //        GameManager.Instance.mainCharacter.GetComponent<CharcterBodyParts>().m_BodyParts[i].GetComponent<Renderer>().material.SetTexture("_BaseMap", tex);
        //}
    }

    bool isAlreadyRunning = true;
    public void LoadAvatarOnContinue()
    {
        //try
        //{
        if (currentSelected != null && isAlreadyRunning)
        {
            isAlreadyRunning = false;

            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = JsonUtility.FromJson<SavingCharacterDataClass>(avatarJson);
            //Lips Default
            //_CharacterData.myItemObj[5].ItemLinkAndroid = "";
            //_CharacterData.myItemObj[5].ItemName = "";
            //_CharacterData.myItemObj[5].ItemID = 0;
            //Lips
            //_CharacterData.BodyFat = 0;
            File.WriteAllText((Application.persistentDataPath + "/logIn.json"), JsonUtility.ToJson(_CharacterData));

            //DefaultEnteriesforManican.instance.ResetForPresets();
            //DownloadPlayerAssets();
            //GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
            SaveCharacterProperties.instance.LoadMorphsfromFile();
            loadprevious();
            StartCoroutine(DefaultClothDatabase.instance.WaitAndDownloadFromRevert(0));
            GameManager.Instance.mainCharacter.GetComponent<AvatarController>().InitializeAvatar();
            //InventoryManager.instance.UndoSelection();

            isAlreadyRunning = true;
            OnUpdateExistingRemoveOld(avatarId);
            ServerSideUserDataHandler.Instance.UpdateUserOccupiedAsset(avatarId);
            //Enable save button
            //if (InventoryManager.instance.StartPanel_PresetParentPanel.activeSelf)
            //{

            //    if (PlayerPrefs.GetInt("iSignup") == 1)
            //    {

            //        Invoke("abcd", 2.0f);

            //        InventoryManager.instance.StartPanel_PresetParentPanel.SetActive(false);
            //    }
            //    else                // as a guest
            //    {


            //        InventoryManager.instance.StartPanel_PresetParentPanel.SetActive(false);
            //        UserRegisterationManager.instance.usernamePanal.SetActive(true);
            //        // enable check so that it will know that index is comming from start of the game
            //        UserRegisterationManager.instance.checkbool_preser_start = false;
            //    }
            //}
            //else
            //{
            //    InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
            //    InventoryManager.instance.GreyRibbonImage.SetActive(false);
            //    InventoryManager.instance.WhiteRibbonImage.SetActive(true);
            //}
        }
        //        }
        //        catch (Exception e)
        //        {
        //            isAlreadyRunning = true;
        //#if UNITY_EDITOR
        //           Debug.Log(e.ToString());
        //            Debug.Break();
        //#endif
        //        }
        CloseAvatarPanel();

    }

    void DownloadPlayerAssets()
    {
        StartCoroutine(WaitAndDownloadFromRevert(0));
    }

    IEnumerator WaitAndDownloadFromRevert(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
            {

                string currentlink = "";
#if UNITY_ANDROID
                currentlink = _CharacterData.myItemObj[i].ItemLinkAndroid;
#else
currentlink = _CharacterData.myItemObj[i].ItemLinkIOS;
#endif

                if (_CharacterData.myItemObj[i].ItemID == 0)
                {
                    DefaultClothDatabase.instance.BindDefaultItems(_CharacterData.myItemObj[i]);
                }
                else
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(currentlink))   // if link is empty thn dont call it
                        {
                            GameManager.Instance.mainCharacter.GetComponent<AvatarController>().WearDefaultItem(_CharacterData.myItemObj[i].ItemType, GameManager.Instance.mainCharacter.gameObject, _CharacterData.gender);
                            //  Debug.Log("Downloading --- " + _CharacterData.myItemObj[i].ItemLink + " Link " + _CharacterData.myItemObj[i].ItemType);
                            string _temptype = _CharacterData.myItemObj[i].Slug;

                            ItemDetail itemobj = new ItemDetail();
                            itemobj.name = _CharacterData.myItemObj[i].ItemName.ToLower();
                            itemobj.id = _CharacterData.myItemObj[i].ItemID.ToString();
                            itemobj.assetLinkIos = _CharacterData.myItemObj[i].ItemLinkIOS;
                            itemobj.assetLinkAndroid = _CharacterData.myItemObj[i].ItemLinkAndroid;

                            if (!_CharacterData.myItemObj[i].ItemName.Contains("md", System.StringComparison.CurrentCultureIgnoreCase) &&
                                !_CharacterData.myItemObj[i].ItemName.Contains("default", System.StringComparison.CurrentCultureIgnoreCase))
                            {
                                StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(_CharacterData.myItemObj[i].ItemID, _CharacterData.myItemObj[i].ItemName, _CharacterData.myItemObj[i].ItemType, _CharacterData.gender != null ? _CharacterData.gender : "Male", GameManager.Instance.mainCharacter.GetComponent<AvatarController>(), Color.clear));
                            }
                            else
                            {
                                GameManager.Instance.mainCharacter.GetComponent<AvatarController>().WearDefaultItem(_CharacterData.myItemObj[i].ItemType, GameManager.Instance.mainCharacter.gameObject, _CharacterData.gender != null ? _CharacterData.gender : "Male");
                            }

                            //InventoryManager.instance._DownloadRigClothes.NeedToDownloadOrNot(itemobj, _CharacterData.myItemObj[i].ItemLinkAndroid, _CharacterData.myItemObj[i].ItemLinkIOS, _CharacterData.myItemObj[i].ItemType, _CharacterData.myItemObj[i].ItemName.ToLower(), _CharacterData.myItemObj[i].ItemID);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("<color = red>" + e.ToString() + "</color>");
                    }
                }
                yield return new WaitForSeconds(.05f);
            }
        }
    }


    //update existing user 
    public void UpdateExistingUserData()
    {
        if (avatarId != null)
        {
            PlayerPrefs.SetInt("presetPanel", 0);
            SaveCharacterProperties.instance.SavePlayerPropertiesInClassObj();
            OnUpdateExistingRemoveOld(avatarId);
            ServerSideUserDataHandler.Instance.UpdateUserOccupiedAsset(avatarId);
        }
    }


    public void OnUpdateExistingRemoveOld(string _avatarID)
    {
        for (int i = 0; i < contentParent.transform.childCount; i++)
        {
            if (contentParent.transform.GetChild(i).name == _avatarID)
            {
                avatarThumbnailUrl = contentParent.transform.GetChild(i).GetComponent<SavedPlayerDataJson>().avatarThumbnailLink;
                Destroy(contentParent.transform.GetChild(i).gameObject);
                break;
            }
        }
    }


    //Delete Avatar From Server
    public void DeleteAvatar()
    {
        if (currentSelected != null)
        {
            string token = ConstantsGod.AUTH_TOKEN;
            DeleteAvatarDataFromServer(token, avatarId);

            Destroy(currentSelected.gameObject, 0f);

            Invoke("SelectFirstAvatarOnDelete", 1);

            continueButton.interactable = false;
            deleteButton.interactable = false;
        }
    }


    void SelectFirstAvatarOnDelete()
    {
        if (contentParent.transform.childCount > 1)
        {
            contentParent.transform.GetChild(1).GetComponent<Button>().onClick.Invoke();
            if (currentSelected != null && isAlreadyRunning)
            {
                isAlreadyRunning = false;

                SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                _CharacterData = JsonUtility.FromJson<SavingCharacterDataClass>(avatarJson);
                //Lips Default
                if (_CharacterData.myItemObj.Count >= 5)
                {
                    _CharacterData.myItemObj[5].ItemLinkAndroid = "";
                    _CharacterData.myItemObj[5].ItemName = "";
                    _CharacterData.myItemObj[5].ItemID = 0;
                }

                //Lips
                _CharacterData.BodyFat = 0;
                File.WriteAllText((Application.persistentDataPath + "/SavingCharacterDataClass.json"), JsonUtility.ToJson(_CharacterData));
                DownloadPlayerAssets();
                //GameManager.Instance.mainCharacter.GetComponent<Equipment>().Start();
                SaveCharacterProperties.instance.LoadMorphsfromFile();

                isAlreadyRunning = true;
            }
        }
        else
        {
            avatarId = null;
        }

    }

    public bool loadNewPage = true;
    public void CheckForPagination()
    {
        if (loadNewPage && avatarScrollRect.verticalNormalizedPosition < -0f)
        {
            Debug.Log("Load New Page....." +loadNewPage);
            loadNewPage = false;
            StartCoroutine(GetAvatarData_Server(currentpageNum, pageSize));
        }
    }

    /// <summary>
    /// To delete all avatar data from the avatar container
    /// </summary>
    public void EmptyAvatarContainer()
    {
        int count = contentParent.transform.childCount - 1;
        Transform container = contentParent.transform;
        if (count >= 1)
        {
            for (int i = count; i >= 1; i--)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }
}
