using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;
using System;
using Random = UnityEngine.Random;
using UnityEditor;
using static InventoryManager;
using Photon.Pun.Demo.PunBasics;
using System.Text;

public class AvatarController : MonoBehaviour
{
    #region public Delegates
    public delegate void CharacterLoaded();
    public static event CharacterLoaded characterLoaded;
    #endregion

    #region public Var
    public bool isPlayerAvatar = false;
    public bool IsInit = false;
    public bool staticPlayer;
    public bool isWearOrNot = false;
    public bool isClothLoaded = false;
    public bool isLoadStaticClothFromJson;
    public string staticClothJson;
    public string clothJson;
    public string presetValue;
    public GameObject wornHair, wornPant, wornShirt, wornShoes, wornEyeWearable, wornGloves, wornChain;
    public GameObject[] wornEyebrow;
    public NFTColorCodes _nftAvatarColorCodes;
    public CharacterBodyParts characterBodyParts;
    public SavingCharacterDataClass _PCharacterData = new SavingCharacterDataClass();
    [SerializeField] RuntimeAnimatorController ArAnimator;

    public List<Item> ClothsToBeLoaded = new List<Item>();
    #endregion

    #region var Hide Inspector
    [HideInInspector]
    public int wornHairId, hairColorPaletteId, wornPantId, wornShirtId, wornShoesId, wornEyewearableId, skinId,
        faceId, eyeBrowId, eyeBrowColorPaletteId, eyesId, eyesColorId, eyesColorPaletteId, noseId, lipsId,
        lipsColorId, lipsColorPaletteId, bodyFat, makeupId, eyeLashesId, wornGlovesId, wornChainId;
    public List<Texture> masks = new List<Texture>();
    #endregion

    #region private var
    public Stitcher stitcher;
    private DefaultClothDatabase itemDatabase;
    private string sceneName;
    private Color presetHairColor;
    AddressableDownloader addressableDownloader;
    ConstantsHolder xanaConstants;
    #endregion

    #region Unity Default Functions
    private void Awake()
    {
        stitcher = new Stitcher();
        sceneName = SceneManager.GetActiveScene().name;
        characterBodyParts = this.GetComponent<CharacterBodyParts>();
        itemDatabase = DefaultClothDatabase.instance;
    }
    private void Start()
    {
        addressableDownloader = AddressableDownloader.Instance;
        xanaConstants = ConstantsHolder.xanaConstants;
        if (sceneName.Equals("InventoryScene"))
        {
            InventoryManager.instance.SceneLoader.SetActive(true);
            InitializeAvatar();
            InventoryManager.instance.SubmitUserDetailAPI();
            ConstantsHolder.xanaConstants.isStoreActive = true;
            InventoryManager.upateAssetOnGenderChanged?.Invoke();
            InventoryManager.instance.StoreItemsPanel.SetActive(true);
            InventoryManager.instance.SelectPanel(0);

            // Check if the file exists before calling ClickBtnFtn
          
            InventoryManager.instance.BtnsPanelAvatar.GetComponent<SubBottons>().ClickBtnFtn(10);

            GameManager.Instance.AvatarMenuBtnPressed();
            string filePath = Path.Combine(Application.persistentDataPath, "loginAsGuestClass.json");
            InventoryManager.instance.TurnOffLoader(1);
        }
    }


    public void OnEnable()
    {
        if (xanaConstants == null)
            xanaConstants = ConstantsHolder.xanaConstants;
        if (addressableDownloader == null)
            addressableDownloader = AddressableDownloader.Instance;

        if (xanaConstants != null)
        {
           
            Invoke(nameof(Deley_Custom_InitializeAvatar), 0.0f);

        }
    }

    void Deley_Custom_InitializeAvatar()
    {
        Custom_InitializeAvatar();
    }
    #endregion

    /// <summary>
    /// Setting Avatar default clothes with delay if it is not friend avatar
    /// </summary>
    /// <param name="_obj">GameObject of the avatar</param>
    /// <param name="_gender"> Gender of the avatar</param>
    /// <returns></returns>
    IEnumerator SetAvatarDefaultClothDelay(GameObject _obj, string _gender)
    {
        yield return new WaitForEndOfFrame(); 
        if (SceneManager.GetActiveScene().name != "Home" && !isWearOrNot)
        {
            SetAvatarClothDefault(_obj, _gender);
        }
    }


    /// <summary>
    /// Set Avatar with deault clothes
    /// </summary>
    /// <param name="applyOn"> Avatar to be applied</param>
    /// <param name="_gender"> Gender on which type is to be</param>
    public void SetAvatarClothDefault(GameObject applyOn, string _gender)
    {
        if (string.IsNullOrEmpty(_gender)) // In Case of VTuber Avatar -- Sending Null String
            return;

        IsInit = false;
        WearDefaultItem("Legs", applyOn.gameObject, _gender);
        WearDefaultItem("Chest", applyOn.gameObject, _gender);
        WearDefaultItem("Feet", applyOn.gameObject, _gender);
        WearDefaultItem("Hair", applyOn.gameObject, _gender);
        applyOn.GetComponent<CharacterBodyParts>().DefaultTexture(true, _gender); // Setting Default Texture according to Gender
    }

    /// <summary>
    /// To Inialize Character.
    ///  - Intilaze Store item 
    ///  - Intilaze Character customization (bones, morphes)
    /// </summary>
    public async void InitializeAvatar(bool canWriteFile = false, SavingCharacterDataClass _tempdata = null)
    {
        while (!ConstantsHolder.isAddressableCatalogDownload)
        {
            await Task.Yield();
        }
        Custom_InitializeAvatar(_tempdata);
    }

    public bool isStitchedSuccessfully;
    public SavingCharacterDataClass clothDataClass = new SavingCharacterDataClass();
    public List<string> clothsList = new List<string>();
    public Coroutine clothsStichedOrNotCoroutine;


    public void HandleCharacterParts(bool active)
    {
    }

    public void HandleCloths(bool active)
    {
       
    }
    public bool isClothStichedOrNot(List<string> clothsList, AvatarController avatar)
    {
        if (clothsList.Count == 0)
        {
            return true;
        }
        string pant = "";
        string shirt = "";
        string hair = "";
        string shoes = "";
        if (avatar.wornPant != null)
        {
            pant = avatar.wornPant.name;
        }
        if (avatar.wornShirt != null)
        {
            shirt = avatar.wornShirt.name;
        }
        if (avatar.wornHair != null)
        {
            hair = avatar.wornHair.name;
        }
        if (avatar.wornShoes != null)
        {
            shoes = avatar.wornShoes.name;
        }
        return clothsList[0] == pant && clothsList[1] == shirt && clothsList[2] == hair;
    }

    /// <summary>
    /// Downloading Random Preset
    /// </summary>
    /// <param name="_CharacterData"> GameObject on which data to be applied</param>
    /// <param name="_rand"> Random numbe of the preset</param>
    void DownloadRandomPresets(SavingCharacterDataClass _CharacterData, int _rand)
    {
        CharacterHandler.instance.ActivateAvatarByGender(characterBodyParts.randomPresetData[_rand].GenderType);
        SetAvatarClothDefault(gameObject, characterBodyParts.randomPresetData[_rand].GenderType); //Set Default Cloth and Set texture according to it.
        if (_CharacterData.myItemObj == null || _CharacterData.myItemObj.Count == 0)
        {
            for (int i = 0; i < 4; i++)
                _CharacterData.myItemObj.Add(new Item(0, "", "", "", ""));
        }
        var randomPresetData = characterBodyParts.randomPresetData[_rand];
        _CharacterData.myItemObj[0].ItemName = randomPresetData.PantPresetData.ObjectName;
        _CharacterData.myItemObj[0].ItemType = randomPresetData.PantPresetData.ObjectType;

        _CharacterData.myItemObj[1].ItemName = randomPresetData.ShirtPresetData.ObjectName;
        _CharacterData.myItemObj[1].ItemType = randomPresetData.ShirtPresetData.ObjectType;

        _CharacterData.myItemObj[2].ItemName = randomPresetData.HairPresetData.ObjectName;
        _CharacterData.myItemObj[2].ItemType = randomPresetData.HairPresetData.ObjectType;

        _CharacterData.myItemObj[3].ItemName = randomPresetData.ShoesPresetData.ObjectName;
        _CharacterData.myItemObj[3].ItemType = randomPresetData.ShoesPresetData.ObjectType;

        CharacterHandler.instance.ActivateAvatarByGender(randomPresetData.GenderType);

        if (_CharacterData.myItemObj.Count > 0)
        {
            for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
            {
                if (!string.IsNullOrEmpty(_CharacterData.myItemObj[i].ItemName))
                {
                    HashSet<string> itemTypes = new HashSet<string> { "Legs", "Chest", "Feet", "Hair", "EyeWearable", "Glove", "Chain" };

                    if (itemTypes.Any(item => _CharacterData.myItemObj[i].ItemType.Contains(item)))
                    {
                        if (!_CharacterData.myItemObj[i].ItemName.ToLowerInvariant().Contains("md"))
                        {
                            var item = _CharacterData.myItemObj[i];
                            var gender = _CharacterData.gender ?? "Male";
                            var avatarController = this.gameObject.GetComponent<AvatarController>();
                            StartCoroutine(addressableDownloader.DownloadAddressableObj(item.ItemID, item.ItemName, item.ItemType, gender, avatarController, Color.clear));
                        }
                        else
                        {
                            if (PlayerPrefs.HasKey("Equiped") || xanaConstants.isNFTEquiped)
                            {
                                if (_CharacterData.myItemObj[i].ItemType.IndexOf("Chest", StringComparison.Ordinal) >= 0)
                                {
                                    if (wornShirt)
                                    {
                                        UnStichItem("Chest");
                                        characterBodyParts.TextureForShirt(null);
                                    }
                                }
                                else if (_CharacterData.myItemObj[i].ItemType.IndexOf("Hair", StringComparison.Ordinal) >= 0)
                                {
                                    if (wornHair)
                                        UnStichItem("Hair");
                                }
                                else if (_CharacterData.myItemObj[i].ItemType.IndexOf("Legs", StringComparison.Ordinal) >= 0)
                                {
                                    if (wornPant)
                                    {
                                        UnStichItem("Legs");
                                        characterBodyParts.TextureForPant(null);
                                    }
                                }
                                else if (_CharacterData.myItemObj[i].ItemType.IndexOf("Feet", StringComparison.Ordinal) >= 0)
                                {
                                    if (wornShoes)
                                    {
                                        UnStichItem("Feet");
                                        characterBodyParts.TextureForShoes(null);
                                    }
                                }
                                else if (_CharacterData.myItemObj[i].ItemType.IndexOf("EyeWearable", StringComparison.Ordinal) >= 0)
                                {
                                    if (wornEyeWearable)
                                        UnStichItem("EyeWearable");
                                }
                                else if (_CharacterData.myItemObj[i].ItemType.IndexOf("Glove", StringComparison.Ordinal) >= 0)
                                {
                                    if (wornGloves)
                                    {
                                        UnStichItem("Glove");
                                        characterBodyParts.TextureForGlove(null);
                                    }
                                }
                                else if (_CharacterData.myItemObj[i].ItemType.IndexOf("Chain", StringComparison.Ordinal) >= 0)
                                {
                                    if (wornChain)
                                        UnStichItem("Chain");
                                }
                            }
                            else
                            {
                                var item = _CharacterData.myItemObj[i];
                                var gender = _CharacterData.gender ?? "Male";
                                WearDefaultItem(item.ItemType, this.gameObject, gender);
                            }
                        }
                    }
                    else
                    {
                        var item = _CharacterData.myItemObj[i];
                        var gender = _CharacterData.gender ?? "Male";
                        WearDefaultItem(item.ItemType, this.gameObject, gender);
                    }
                }
            }
        }

        _CharacterData.gender = randomPresetData.GenderType;
        _CharacterData.avatarType = "NewAvatar";
        var filePath = Path.Combine(Application.persistentDataPath, "loginAsGuestClass.json");
        File.WriteAllText(filePath, JsonUtility.ToJson(_CharacterData));

        GameManager.Instance.selectedPresetData = JsonUtility.ToJson(_CharacterData);

        if (_CharacterData.HairColor != null)
            xanaConstants.isPresetHairColor = true;

        SavePresetOnServer(_CharacterData);
    }

    /// <summary>
    /// To Save player asset file on server
    /// </summary>
    /// <param name="savingCharacterDataClass">Player assets data class</param>
    void SavePresetOnServer(SavingCharacterDataClass savingCharacterDataClass)
    {
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        {
            var filePath = Path.Combine(Application.persistentDataPath, "logIn.json");
            File.WriteAllText(filePath, JsonUtility.ToJson(savingCharacterDataClass));
            ServerSideUserDataHandler.Instance.CreateUserOccupiedAsset(() => { });
        }
    }

    /// <summary>
    /// Initializing Avatar with json file from the server.
    /// </summary>
    async void Custom_InitializeAvatar(SavingCharacterDataClass _data = null)
    {
        string folderPath = GameManager.Instance.GetStringFolderPath();
        if (!File.Exists(folderPath))
        {
            // Create a new file if it doesn't exist
            using (FileStream fs = File.Create(folderPath))
            {
                // Optionally write some default content to the file
                byte[] defaultContent = new UTF8Encoding(true).GetBytes("{}");
                fs.Write(defaultContent, 0, defaultContent.Length);
            }
        }

        if (File.Exists(folderPath) && File.ReadAllText(folderPath) != "") // Check if data exists
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

            if (ConstantsHolder.isPenguin)
                return;
            else if (ConstantsHolder.isFixedHumanoid)
            {
                _CharacterData = _CharacterData.CreateFromJSON(XANASummitDataContainer.FixedAvatarJson);
                clothJson = XANASummitDataContainer.FixedAvatarJson;
            }
            else if (_data != null)
                _CharacterData = _data;
            else
            {
                _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(folderPath));
                clothJson = File.ReadAllText(folderPath);
            }

            _PCharacterData = _CharacterData;

            var gender = _CharacterData.gender ?? "Male";
            var avatarController = this.gameObject.GetComponent<AvatarController>();
            sceneName = SceneManager.GetActiveScene().name; // updating scene name if scene changed.
            if (sceneName.Equals("InventoryScene")) // for store/ main menu
            {
                CharacterHandler.instance.ActivateAvatarByGender("VTuber_Female"/*_CharacterData.gender*/);
                if (_CharacterData.myItemObj.Count > 0)
                {
                    for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
                    {
                        var item = _CharacterData.myItemObj[i];
                        string type = _CharacterData.myItemObj[i].ItemType;
                        if (!string.IsNullOrEmpty(_CharacterData.myItemObj[i].ItemName) &&
                            !item.ItemName.Contains("default", System.StringComparison.CurrentCultureIgnoreCase))
                        {
                            HashSet<string> itemTypes = new HashSet<string> { "Legs", "Chest", "Feet", "Hair", "EyeWearable", "Glove", "Chain" };
                            if (itemTypes.Any(item => type.Contains(item)))
                            {
                                if (!item.ItemName.Contains("md", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    if (type.Contains("Hair"))
                                    {
                                        if (!string.IsNullOrEmpty(_CharacterData.hairItemData) && _CharacterData.hairItemData.Contains("No hair") && wornHair)
                                            UnStichItem("Hair");
                                        else
                                            StartCoroutine(addressableDownloader.DownloadAddressableObj(item.ItemID, item.ItemName, type, gender, avatarController, _CharacterData.HairColor));
                                    }
                                    else
                                        StartCoroutine(addressableDownloader.DownloadAddressableObj(item.ItemID, item.ItemName, type, gender, avatarController, Color.clear));
                                }
                                else
                                {
                                    if (PlayerPrefs.HasKey("Equiped") || xanaConstants.isNFTEquiped)
                                    {
                                        if (item.ItemType.Contains("Chest") && wornShirt)
                                        {
                                            UnStichItem("Chest");
                                            characterBodyParts.TextureForShirt(null);
                                        }
                                        else if (item.ItemType.Contains("Hair") && wornHair)
                                        {
                                            UnStichItem("Hair");
                                        }
                                        else if (item.ItemType.Contains("Legs") && wornPant)
                                        {
                                            UnStichItem("Legs");
                                            characterBodyParts.TextureForPant(null);
                                        }
                                        else if (item.ItemType.Contains("Feet") && wornShoes)
                                        {
                                            UnStichItem("Feet");
                                            characterBodyParts.TextureForShoes(null);
                                        }
                                        else if (item.ItemType.Contains("EyeWearable") && wornEyeWearable)
                                        {
                                            UnStichItem("EyeWearable");
                                        }
                                        else if (item.ItemType.Contains("Glove") && wornGloves)
                                        {
                                            UnStichItem("Glove");
                                            characterBodyParts.TextureForGlove(null);
                                        }
                                        else if (item.ItemType.Contains("Chain") && wornChain)
                                        {
                                            UnStichItem("Chain");
                                        }
                                    }
                                    else
                                    {
                                        WearDefaultItem(type, this.gameObject, gender);
                                    }
                                }
                            }
                            else
                            {
                                WearDefaultItem(type, this.gameObject, gender);
                            }
                        }
                        else // wear the default item of that specific part.
                        {
                            if (xanaConstants.isNFTEquiped && type.Contains("Chest"))
                            {
                                if (wornShirt)
                                    UnStichItem("Chest");
                                characterBodyParts.TextureForShirt(null);
                            }
                            else
                            {
                                if (_CharacterData.myItemObj[0].ItemName == "Boy_Pant_V009" && _CharacterData.myItemObj[1].ItemName == "Boy_Shirt_V009")
                                {
                                    if (item.ItemType.Contains("Feet"))
                                        UnStichItem("Feet");
                                }
                                else
                                {
                                    WearDefaultItem(type, this.gameObject, gender);
                                }
                            }
                        }
                    }
                }
                else
                {
                    WearDefaultItem("Legs", this.gameObject, gender);
                    WearDefaultItem("Chest", this.gameObject, gender);
                    WearDefaultItem("Feet", this.gameObject, gender);
                    WearDefaultItem("Hair", this.gameObject, gender);

                    if (wornEyeWearable)
                        UnStichItem("EyeWearable");
                    if (wornChain)
                        UnStichItem("Chain");
                    if (wornGloves)
                    {
                        UnStichItem("Glove");
                        characterBodyParts.TextureForGlove(null);
                    }
                }
                if (_CharacterData.charactertypeAi == true)
                {
                    ApplyAIData(_CharacterData, this.gameObject);
                }

                characterBodyParts.LoadBlendShapes(_CharacterData, this.gameObject);
                

                if (_CharacterData.eyeTextureName != "" && _CharacterData.eyeTextureName != null)
                {
                    StartCoroutine(addressableDownloader.DownloadAddressableTexture(_CharacterData.eyeTextureName, this.gameObject, CurrentTextureType.EyeLense));
                }

                if (!string.IsNullOrEmpty(_CharacterData.eyebrrowTexture) && !_CharacterData.eyebrrowTexture.Contains("default"))
                {
                    StartCoroutine(addressableDownloader.DownloadAddressableTexture(_CharacterData.eyebrrowTexture, this.gameObject, CurrentTextureType.EyeBrows));
                }
                else
                {
                    characterBodyParts.SetTextureDefault(CurrentTextureType.EyeBrows, this.gameObject);
                    characterBodyParts.ChangeEyebrowColor(characterBodyParts.DefaultEyebrowColor);
                }
            }
        }
        isClothLoaded = true;
    }

    IEnumerator WaitForCallback()
    {
        while (true)
        {
            int clothsloaded = 0;
            foreach (var item in ClothsToBeLoaded)
            {

                switch (item.ItemType)
                {

                    case "Chest":

                        if (wornShirtId == item.ItemID)
                            clothsloaded++;

                        break;

                    case "Legs":
                        if (wornPantId == item.ItemID)
                            clothsloaded++;


                        break;

                    case "Hair":

                        if (wornHairId == item.ItemID)
                            clothsloaded++;


                        break;

                    case "Feet":

                        if (wornShoesId == item.ItemID)
                            clothsloaded++;


                        break;

                    case "EyeWearable":

                        if (wornEyewearableId == item.ItemID)
                            clothsloaded++;

                        break;

                    case "Chain":

                        if (wornChainId == item.ItemID)
                            clothsloaded++;

                        break;

                    case "Glove":

                        if (wornGlovesId == item.ItemID)
                            clothsloaded++;

                        break;
                }
            }

            if (clothsloaded == ClothsToBeLoaded.Count)
            {
             //   GameplayEntityLoader.instance.ClothsLoaded = true;
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }

    }

    /// <summary>
    /// To Wear DefaultItem according to the gender
    /// </summary>
    /// <param name="type"> Type of object like hair, shirt,pent etc </param>
    /// <param name="applyOn"></param>
    /// <param name="gender"></param>
    public void WearDefaultItem(string type, GameObject applyOn, string gender)
    {
        CharacterBodyParts bodyParts = characterBodyParts;

        //Debug.Log("Item: " + type + " --- Gender: " + gender);

        if (string.IsNullOrEmpty(gender))
        {
            if (applyOn.name.Contains("Female", System.StringComparison.CurrentCultureIgnoreCase))
                gender = "Female";
            else
                gender = "Male";
        }


        if (itemDatabase == null)
        {
            itemDatabase = DefaultClothDatabase.instance;
        }
        if (gender == "Male") // if avatar is Male
        {
            switch (type)
            {
                case "Legs":
                    if (itemDatabase.maleAvatarDefaultCostume.DefaultPent != null)
                        StichItem(-1, itemDatabase.maleAvatarDefaultCostume.DefaultPent, type, applyOn);
                    break;
                case "Chest":
                    if (itemDatabase.maleAvatarDefaultCostume.DefaultShirt != null)
                        StichItem(-1, itemDatabase.maleAvatarDefaultCostume.DefaultShirt, type, applyOn);
                    break;
                case "Feet":
                    if (itemDatabase.maleAvatarDefaultCostume.DefaultShoes != null)
                        StichItem(-1, itemDatabase.maleAvatarDefaultCostume.DefaultShoes, type, applyOn);
                    break;
                case "Hair":
                    if (itemDatabase.maleAvatarDefaultCostume.DefaultHair != null)
                        StichItem(-1, itemDatabase.maleAvatarDefaultCostume.DefaultHair, type, applyOn);
                    break;
                default:
                    break;
            }
        }
        else if (gender == "Female") // if avatar is female
        {
            switch (type)
            {
                case "Legs":
                    if (itemDatabase.femaleAvatarDefaultCostume.DefaultPent != null)
                        StichItem(-1, itemDatabase.femaleAvatarDefaultCostume.DefaultPent, type, applyOn);
                    break;
                case "Chest":
                    if (itemDatabase.femaleAvatarDefaultCostume.DefaultShirt != null)
                        StichItem(-1, itemDatabase.femaleAvatarDefaultCostume.DefaultShirt, type, applyOn);
                    break;
                case "Feet":
                    if (itemDatabase.femaleAvatarDefaultCostume.DefaultShoes != null)
                        StichItem(-1, itemDatabase.femaleAvatarDefaultCostume.DefaultShoes, type, applyOn);
                    break;
                case "Hair":
                    if (itemDatabase.femaleAvatarDefaultCostume.DefaultHair != null)
                        StichItem(-1, itemDatabase.femaleAvatarDefaultCostume.DefaultHair, type, applyOn);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// To Wear default hairs for multiplier
    /// </summary>
    /// <param name="applyOn"></param>
    /// <param name="hairColor"></param>
    public void WearDefaultHair(GameObject applyOn, Color hairColor)
    {
        StichHairWithColor(-1, itemDatabase.DefaultHair, "Hair", applyOn, hairColor, true);
    }

    /// <summary>
    /// 1 - Update body according to fat
    /// 2 -Fit cloth according to the selected body type
    /// </summary>
    public void ResizeClothToBodyFat(GameObject applyOn, int bodyFat)
    {
        CharacterBodyParts bodyparts = applyOn.GetComponent<CharacterBodyParts>();

        float _size3 = 1f + ((float)bodyFat / 100f);

        Debug.Log("Resizing Body Parts & Cloths : " + bodyFat + "  :  " + _size3);

        if (bodyparts._scaleBodyParts.Count > 0)
        {
            for (int i = 0; i < bodyparts._scaleBodyParts.Count; i++)
            {
                if (bodyparts._scaleBodyParts[i])
                    bodyparts._scaleBodyParts[i].transform.localScale = new Vector3(_size3, 1, _size3);
            }
        }
    }

    /// <summary>
    /// To Load data from file
    /// </summary>
    /// <param name="_CharacterData"> player data save in file</param>
    /// <param name="applyOn">Object on which data is going to apply</param>
    public void LoadBonesData(SavingCharacterDataClass _CharacterData, GameObject applyOn)
    {
        CharacterBodyParts parts = applyOn.GetComponent<CharacterBodyParts>();
        if (applyOn != null)
        {
            if (_CharacterData != null)
            {
                List<BoneDataContainer> boneData = _CharacterData.SavedBones;
                if (boneData.Count > 0)
                {
                    for (int i = 0; i < boneData.Count; i++)
                    {
                        if (i < parts.BonesData.Count && boneData[i] != null)
                        {
                            var bone = parts.BonesData[i].Obj.transform;
                            bone.localPosition = boneData[i].Pos;
                            bone.localScale = boneData[i].Scale;
                        }
                    }
                }
                else
                {
                    ResetBonesDefault(parts);
                }
            }
            else
            {
                if (parts.BonesData.Count > 0)
                {
                    ResetBonesDefault(parts);
                }
            }
        }
    }

    /// <summary>
    /// To reset bones to default pos and scale
    /// </summary>
    /// <param name="parts"> CharcterBodyParts </param>
    public void ResetBonesDefault(CharacterBodyParts parts)
    {
        if (parts != null && parts.BonesData.Count > 0)
        {
            for (int i = 0; i < parts.BonesData.Count; i++)
            {
                var bone = parts.BonesData[i].Obj.transform;
                if (bone != null)
                {
                    bone.localPosition = parts.BonesData[i].Pos;
                    bone.localScale = parts.BonesData[i].Scale;
                    bone.localEulerAngles = parts.BonesData[i].Rotation;
                }
            }
        }
    }

    /// <summary>
    /// To stich item on player rig
    /// </summary>
    /// <param name="item">Cloth to wear</param>
    /// <param name="applyOn">Player that are going to wear the dress</param>
    public void StichItem(int itemId, GameObject item, string type, GameObject applyOn, bool applyHairColor = true)
    {
        CharacterBodyParts tempBodyParts = applyOn.gameObject.GetComponent<CharacterBodyParts>();
        EffectedParts effectedParts = item.GetComponent<EffectedParts>();


        if (effectedParts && effectedParts.texture != null)
        {
            Texture tempTex = item.GetComponent<EffectedParts>().texture;
            masks.Add(tempTex);
            tempBodyParts.ApplyMaskTexture(type, tempTex, this.gameObject);
        }
        if (effectedParts && effectedParts.variation_Texture != null)
        {
            string baseMap = "_BaseMap";

            if (item.name.StartsWith("VT_"))
            {
                baseMap = "_MainTex"; //_MainTex -- BaseMap
            }

            item.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.SetTexture(baseMap, item.GetComponent<EffectedParts>().variation_Texture);
        }

        if (applyOn.name.Contains("Vtuber") || applyOn.name.Contains("VT_"))
            item = this.stitcher.Stitch_Vtuber(item, applyOn, characterBodyParts);
        else
            item = this.stitcher.Stitch(item, applyOn);

        if (type == "Hair" && !item.name.Contains("VT_Girl_"))
        {
            if (applyHairColor /*&& _CharData.HairColor != null && getHairColorFormFile */)
            {
                SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
                if (isLoadStaticClothFromJson)
                {
                    _CharacterData = _CharacterData.CreateFromJSON(staticClothJson);
                    if (!ConstantsHolder.xanaConstants.isStoreActive) // Changing Hair no need to apply color from file
                        StartCoroutine(tempBodyParts.ImplementColors(_CharacterData.HairColor, SliderType.HairColor, applyOn));
                }
                else
                {
                    var filePath = GameManager.Instance.GetStringFolderPath();
                    if (File.Exists(filePath))
                    {
                        var fileContent = File.ReadAllText(filePath);
                        if (!string.IsNullOrEmpty(fileContent))
                        {
                            _CharacterData = _CharacterData.CreateFromJSON(fileContent);
                            if (!ConstantsHolder.xanaConstants.isStoreActive) // Changing Hair no need to apply color from file
                                StartCoroutine(tempBodyParts.ImplementColors(_CharacterData.HairColor, SliderType.HairColor, applyOn));
                        }
                    }
                    else
                    {
                        StartCoroutine(tempBodyParts.ImplementColors(Color.black, SliderType.HairColor, applyOn));
                        // Hairs Default Color
                        //StartCoroutine(tempBodyParts.ImplementColors(new Color(0.9058824f, 0.5137255f, 0.4039216f,1f), SliderType.HairColor, applyOn));
                    }
                }
                if (_CharacterData?.charactertypeAi == true)
                {
                    StartCoroutine(tempBodyParts.ImplementColors(_CharacterData.hair_color, SliderType.HairColor, applyOn));
                }
               
            }
            else if (type == "Hair" && xanaConstants.isPresetHairColor && presetHairColor != null)
            {
                //getHairColorFormFile = false;
                StartCoroutine(tempBodyParts.ImplementColors(presetHairColor, SliderType.HairColor, applyOn));
                presetHairColor = Color.clear;
            }
        }

        if (SceneManager.GetActiveScene().name != "Home")
        {
            item.layer = 22;
        }
        else
        {
            if (xanaConstants == null)
            {
                xanaConstants = ConstantsHolder.xanaConstants;
            }
            item.layer = 11;
        }
        UnStichItem(type);
        switch (type)
        {
            case "Chest":
                wornShirt = item;
                wornShirtId = itemId;
                //wornShirt.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
                break;

            case "Legs":
                wornPant = item;
                wornPantId = itemId;
                //wornPant.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
                break;

            case "Hair":
                wornHair = item;
                wornHairId = itemId;
                //wornHair.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
                break;

            case "Feet":
                wornShoes = item;
                wornShoesId = itemId;
                //wornShoes.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
                break;

            case "EyeWearable":
                wornEyeWearable = item;
                wornEyewearableId = itemId;
                break;

            case "Chain":
                wornChain = item;
                wornChainId = itemId;
                break;

            case "Glove":
                wornGloves = item;
                Material m = new Material(wornGloves.GetComponent<SkinnedMeshRenderer>().materials[0]);
                wornGloves.GetComponent<SkinnedMeshRenderer>().materials[0] = m;
                wornGlovesId = itemId;
                break;
        }


        if (item.name.Contains("Full_Costume", System.StringComparison.CurrentCultureIgnoreCase) || (item.name.Contains("VT_Girl_Costume_", System.StringComparison.CurrentCultureIgnoreCase)))
        {
            // Disable Pant
            if (wornPant)
            {
                UnStichItem("Legs");
                // wornPant.GetComponent<SkinnedMeshRenderer>().enabled = false; 
            }

            // Also Remove Pant Mask
            tempBodyParts.ApplyMaskTexture("Legs", null, this.gameObject);
        }
        else if (type == "Legs" && (wornShirt && ((wornShirt.name.Contains("Full_Costume", System.StringComparison.CurrentCultureIgnoreCase)) || (wornShirt.name.Contains("VT_Girl_Costume_", System.StringComparison.CurrentCultureIgnoreCase)))))
        {
            if (SceneManager.GetActiveScene().name != "Home")
                WearDefaultItem("Chest", applyOn.gameObject, CharacterHandler.instance.activePlayerGender.ToString());
            // Apply Mask For Default Shirt
            tempBodyParts.DefaultTextureForNewCharacter_Single("Shirt");
            if (CharacterHandler.instance.activePlayerGender == AvatarGender.Male)
                StichItem(-1, itemDatabase.maleAvatarDefaultCostume.DefaultShirt, "Chest", applyOn);
            else
                StichItem(-1, itemDatabase.femaleAvatarDefaultCostume.DefaultShirt, "Chest", applyOn);

            
        }
        else if (type == "Chest" && (wornShirt && ((!wornShirt.name.Contains("Full_Costume", System.StringComparison.CurrentCultureIgnoreCase)) || (wornShirt.name.Contains("VT_Girl_Costume_", System.StringComparison.CurrentCultureIgnoreCase)))))
        {
            if (wornPant == null)
            {
                if (CharacterHandler.instance.activePlayerGender == AvatarGender.Male)
                    StichItem(-1, itemDatabase.maleAvatarDefaultCostume.DefaultPent, "Legs", applyOn);
                else
                    StichItem(-1, itemDatabase.femaleAvatarDefaultCostume.DefaultPent, "Legs", applyOn);
            }
        }
       
        if (PlayerPrefs.GetInt("presetPanel") != 1)
        {
            if (InventoryManager.instance != null && InventoryManager.instance.loaderForItems)
                InventoryManager.instance.loaderForItems.SetActive(false);
        }
    }

    /// <summary>
    /// As Home Scene has now friends so we need to change color of hairs as require
    /// </summary>
    public void StichHairWithColor(int itemId, GameObject item, string type, GameObject applyOn, Color hairColor, bool isMultiPlayer)
    {
        CharacterBodyParts tempBodyParts = applyOn.gameObject.GetComponent<CharacterBodyParts>();
        EffectedParts effectedParts = item.GetComponent<EffectedParts>();
        UnStichItem(type);

        if (applyOn.name.Contains("Vtuber") || applyOn.name.Contains("VT_"))
        {
            item = this.stitcher.Stitch_Vtuber(item, applyOn, characterBodyParts);

            if (effectedParts && effectedParts.variation_Texture != null)
                item.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.SetTexture("_MainTex", effectedParts.variation_Texture);
        }
        else
        {
            item = this.stitcher.Stitch(item, applyOn);
            StartCoroutine(tempBodyParts.ImplementColors(hairColor, SliderType.HairColor, applyOn));
        }

        if (isMultiPlayer)
        {
            item.layer = 22;
        }
        else
        {
            item.layer = 11;
        }
        wornHair = item;
        wornHairId = itemId;
        wornHair.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;

        if (PlayerPrefs.GetInt("presetPanel") != 1)
        {
            if (InventoryManager.instance != null && InventoryManager.instance.loaderForItems)
                InventoryManager.instance.loaderForItems.SetActive(false);
        }
    }


    /// <summary>
    /// For Multiplayer Hairs Only
    /// </summary>
    /// <param name="itemId"> asset id </param>
    /// <param name="item"> item to wear</param>
    /// <param name="type">item type</param>
    /// <param name="applyOn">object on which item is to apply</param>
    /// <param name="hairColor"> hair color code</param>
    public void StichItem(int itemId, GameObject item, string type, GameObject applyOn, Color hairColor)
    {
        CharacterBodyParts tempBodyParts = applyOn.gameObject.GetComponent<CharacterBodyParts>();
        EffectedParts effectedParts = item.GetComponent<EffectedParts>();
        UnStichItem(type);
        if (effectedParts && effectedParts.texture != null)
        {
            Texture tempTex = effectedParts.texture;
            masks.Add(tempTex);
            tempBodyParts.ApplyMaskTexture(type, tempTex, this.gameObject);
        }

        if (effectedParts && effectedParts.variation_Texture != null)
        {
            item.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.SetTexture("_BaseMap", effectedParts.variation_Texture);
        }

        if (applyOn.name.Contains("Vtuber"))
            item = this.stitcher.Stitch_Vtuber(item, applyOn, characterBodyParts);
        else
            item = this.stitcher.Stitch(item, applyOn);

        if (type == "Hair")
        {
            StartCoroutine(tempBodyParts.ImplementColors(hairColor, SliderType.HairColor, applyOn));
        }

        item.layer = 22;
        wornHair = item;
        wornHairId = itemId;
        wornHair.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
    }

    /// <summary>
    /// To Ubstich item from the object
    /// </summary>
    /// <param name="type"></param>
    public void UnStichItem(string type)
    {


        switch (type)
        {
            case "Chest":
                Destroy(wornShirt);

                //if (xanaConstants.isStoreActive && characterBodyParts.dynamicBoneParent.Count > 0 )
                //{
                //    Debug.LogError("Waqas: Dress : " + characterBodyParts.dynamicBoneParent.Count);
                //    foreach (var item in characterBodyParts.dynamicBoneParent)
                //    {
                //        if (item != null && item.name.Contains("Costume"))
                //        {
                //            Destroy(item);
                //        }
                //    }

                //}
                //if (friendController)
                //{
                //    Destroy(friendController.wornShirt);
                //}
                break;
            case "Legs":
                Destroy(wornPant);
                //if (friendController)
                //{
                //    Destroy(friendController.wornPant);
                //}
                break;
            case "Hair":
                Destroy(wornHair);
                //if (characterBodyParts.dynamicBoneParent.Count > 0)
                //{
                //    Debug.LogError("WaqasHaere : " + characterBodyParts.dynamicBoneParent.Count);
                //    foreach (var item in characterBodyParts.dynamicBoneParent)
                //    {
                //        if (item != null && item.name.Contains("Hair"))
                //        {
                //            Destroy(item);
                //        }
                //    }

                //}
                //if (friendController)
                //{
                //    Destroy(friendController.wornHair);
                //}
                break;
            case "Feet":
                Destroy(wornShoes);
                //if (friendController)
                //{
                //    Destroy(friendController.wornShose);
                //}
                break;
            case "EyeWearable":
                Destroy(wornEyeWearable);
                //if (friendController)
                //{
                //    Destroy(friendController.wornEyewearable);
                //}
                break;
            case "Chain":
                Destroy(wornChain);
                //if (friendController)
                //{
                //    Destroy(friendController.wornChain);
                //}
                break;
            case "Glove":
                Destroy(wornGloves);
                //if (friendController)
                //{
                //    Destroy(friendController.wornGloves);
                //}
                break;
        }
    }

    ///// <summary>
    ///// Applying assets mask
    ///// </summary>
    ///// <param name="applyOn">Multiplayer Avatar on which asset is too be bind</param>
    ///// <returns></returns>
    //public IEnumerator RPCMaskApply(GameObject applyOn)
    //{
    //    yield return new WaitForSeconds(1);
    //    if (masks.Count > 0)
    //    {
    //        foreach (var mask in masks)
    //        {
    //            applyOn.GetComponent<CharacterBodyParts>().ApplyMaskTexture(mask.name, mask, this.gameObject);
    //        }
    //    }
    //}

    ///// <summary>
    ///// Update Keys for Boxer NFT's
    ///// </summary>
    ///// <param name="Key"> item key </param>
    ///// <param name="prefixAdded"> prefec to be added</param>
    ///// <returns></returns>
    //string GetUpdatedKey(string Key, string prefixAdded)
    //{
    //    string tempKey;
    //    if (Key.Contains(" - "))
    //        tempKey = Key.Replace(" - ", "_");
    //    else
    //        tempKey = Key;
    //    tempKey = tempKey.Replace(" ", "");
    //    tempKey = prefixAdded + tempKey;
    //    return tempKey;
    //}

    ///// <summary>
    ///// Get Color Code for NFT
    ///// </summary>
    ///// <param name="key"> item key </param>
    ///// <returns></returns>
    //string GetColorCodeFromNFTKey(string key)
    //{
    //    string tempKey;
    //    if (key.Contains(" - "))
    //    {
    //        tempKey = key.Replace(" - ", "_");
    //        tempKey = tempKey.Split('_').Last();
    //    }
    //    else
    //        tempKey = key;

    //    tempKey = tempKey.Replace(" ", "");
    //    return tempKey;
    //}

    /// <summary>
    /// Get Color Code for NFT
    /// </summary>
    /// <param name="colorCode"></param>
    /// <returns></returns>
    public Color GetColorCode(string colorCode)
    {
        for (int i = 0; i < _nftAvatarColorCodes.colorCodes.Count; i++)
        {
            if (colorCode.ToLower() == _nftAvatarColorCodes.colorCodes[i].colorName.ToLower())
            {
                return _nftAvatarColorCodes.colorCodes[i].updatedColor;
            }
        }
        return Color.black;
    }

    /// <summary>
    /// To get NFT light preset
    /// </summary>
    /// <param name="colorCode"></param>
    /// <returns></returns>
    public LightPresetNFT GetLightPresetValue(Color colorCode)
    {
        for (int i = 0; i < _nftAvatarColorCodes.colorCodes.Count; i++)
        {
            if (colorCode == _nftAvatarColorCodes.colorCodes[i].updatedColor)
            {
                return _nftAvatarColorCodes.colorCodes[i].LightPresetNFT;
            }
        }
        return LightPresetNFT.DefaultSkin;
    }

    /// <summary>
    /// Set avatar according to the AI 
    /// </summary>
    /// <param name="_CharacterData"> AI Character data</param>
    void ApplyAIData(SavingCharacterDataClass _CharacterData, GameObject applyOn)
    {
        string pathPath = "Assets/Store Items Addressables/";
        characterBodyParts.head.SetBlendShapeWeight(_CharacterData.faceItemData, 100);
        characterBodyParts.head.SetBlendShapeWeight(_CharacterData.lipItemData, 100);
        characterBodyParts.head.SetBlendShapeWeight(_CharacterData.noseItemData, 100);
        characterBodyParts.head.SetBlendShapeWeight(_CharacterData.eyeShapeItemData, 100);
        string gender = _CharacterData.gender != null ? _CharacterData.gender : "Male";
        if (_CharacterData.lip_color != null)
        {
            characterBodyParts.head.materials[2].SetColor("_Lips_Color", _CharacterData.lip_color);
        }
        if (_CharacterData.eyeItemData != "" && _CharacterData.eyeItemData != null)
        {
            if (addressableDownloader == null)
            {
                addressableDownloader = AddressableDownloader.Instance;
            }
            StartCoroutine(addressableDownloader.DownloadAddressableTexture(_CharacterData.eyeItemData, this.gameObject, CurrentTextureType.EyeLense));
        }
        if (_CharacterData.skin_color != "" && _CharacterData.Skin != null)
        {
            if (_CharacterData.ai_gender == "male")
            {
                StartCoroutine(addressableDownloader.DownloadAddressableTextureByName(pathPath + "1k_Boy_Face_Texture", _CharacterData.skin_color, this.gameObject, CurrentTextureType.Face));
                StartCoroutine(addressableDownloader.DownloadAddressableTextureByName(pathPath + "1k_Boy_Body_Texture", _CharacterData.skin_color, this.gameObject, CurrentTextureType.Skin));
            }
            else
            {
                StartCoroutine(addressableDownloader.DownloadAddressableTextureByName(pathPath + "1k_Girl_Face_Textures", _CharacterData.skin_color, this.gameObject, CurrentTextureType.Face));
                StartCoroutine(addressableDownloader.DownloadAddressableTextureByName(pathPath + "1k_Girl_Body_Texture", _CharacterData.skin_color, this.gameObject, CurrentTextureType.Skin));
            }
        }
        if (_CharacterData.hairItemData != null)
        {
            if (_CharacterData.hairItemData.Equals("No hair"))
            {
                if (wornHair)
                    UnStichItem("Hair");
            }
            else
                StartCoroutine(addressableDownloader.DownloadAddressableObj(-1, _CharacterData.hairItemData, "Hair", gender, applyOn.GetComponent<AvatarController>(), _CharacterData.hair_color, true));
        }
    }

    ///// <summary>
    ///// To initialize Friend Avatar according with its data
    ///// </summary>
    ///// <param name="_CharacterData"></param>
    ///// <param name="applyOn"></param>
    //public void InitializeFrndAvatar(SavingCharacterDataClass _CharacterData, GameObject applyOn)
    //{
    //    CharacterBodyParts bodyParts = applyOn.GetComponent<CharacterBodyParts>();
    //    if (_CharacterData.avatarType == null || _CharacterData.avatarType == "OldAvatar")
    //    {
    //        int _rand = Random.Range(0, 7);
    //        DownloadRandomFrndPresets(_rand);
    //    }
    //    else
    //    {
    //        SetAvatarClothDefault(applyOn.gameObject, _CharacterData.gender);


    //        if (_CharacterData.myItemObj.Count > 0)
    //        {
    //            for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
    //            {
    //                string type = _CharacterData.myItemObj[i].ItemType;
    //                Item item = _CharacterData.myItemObj[i];
    //                string gender = _CharacterData.gender != null ? _CharacterData.gender : "Male";
    //                if (!string.IsNullOrEmpty(_CharacterData.myItemObj[i].ItemName) && !_CharacterData.myItemObj[i].ItemName.Contains("default", System.StringComparison.CurrentCultureIgnoreCase) && !_CharacterData.myItemObj[i].ItemName.Contains("boy_h_066", System.StringComparison.CurrentCultureIgnoreCase)) // last condition is for the deault hairs
    //                {
    //                    HashSet<string> itemTypes = new HashSet<string> { "Legs", "Chest", "Feet", "Hair", "EyeWearable", "Glove", "Chain" };
    //                    if (itemTypes.Any(item => type.Contains(item)))
    //                    {
    //                        //getHairColorFormFile = true;
    //                        if (!item.ItemName.Contains("md", System.StringComparison.CurrentCultureIgnoreCase) &&
    //                            !item.ItemName.Contains("default", System.StringComparison.CurrentCultureIgnoreCase))
    //                        {
    //                            if (type.Contains("Hair") && (_CharacterData.hairItemData != null && _CharacterData.hairItemData.Contains("No hair")))
    //                            {
    //                                if (wornHair)
    //                                    UnStichItem("Hair");
    //                            }
    //                            else
    //                            {
    //                                //StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(item.ItemID, item.ItemName, type, gender, this.gameObject.GetComponent<AvatarController>(), Color.clear));
    //                                StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(item.ItemID, item.ItemName, type, gender, this.gameObject.GetComponent<AvatarController>(), _CharacterData.HairColor));
    //                            }
    //                        }
    //                        else
    //                        {
    //                            if (PlayerPrefs.HasKey("Equiped") || ConstantsHolder.xanaConstants.isNFTEquiped)
    //                            {
    //                                if (type.Contains("Chest"))
    //                                {
    //                                    if (wornShirt)
    //                                    {
    //                                        UnStichItem("Chest");
    //                                        bodyParts.TextureForShirt(null);
    //                                    }
    //                                }
    //                                else if (type.Contains("Hair"))
    //                                {
    //                                    if (wornHair)
    //                                        UnStichItem("Hair");
    //                                }
    //                                else if (type.Contains("Legs"))
    //                                {
    //                                    // IF fullcostume[3 piece suit] than remove bottom
    //                                    if (wornPant)
    //                                    {
    //                                        UnStichItem("Legs");
    //                                        bodyParts.TextureForPant(null);
    //                                    }
    //                                }
    //                                else if (type.Contains("Feet"))
    //                                {
    //                                    if (wornShoes)
    //                                    {
    //                                        UnStichItem("Feet");
    //                                        bodyParts.TextureForShoes(null);
    //                                    }

    //                                }
    //                                else if (type.Contains("EyeWearable"))
    //                                {
    //                                    if (wornEyeWearable)
    //                                        UnStichItem("EyeWearable");
    //                                }
    //                                else if (type.Contains("Glove"))
    //                                {
    //                                    if (wornGloves)
    //                                    {
    //                                        UnStichItem("Glove");
    //                                        bodyParts.TextureForGlove(null);
    //                                    }

    //                                }
    //                                else if (type.Contains("Chain"))
    //                                {
    //                                    if (wornChain)
    //                                        UnStichItem("Chain");
    //                                }

    //                            }
    //                            else
    //                            {
    //                                WearDefaultItem(type, this.gameObject, gender);
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        WearDefaultItem(type, this.gameObject, gender);
    //                    }
    //                }
    //                else // wear the default item of that specific part.
    //                {
    //                    if (ConstantsHolder.xanaConstants.isNFTEquiped && type.Contains("Chest"))
    //                    {
    //                        if (wornShirt)
    //                            UnStichItem("Chest");
    //                        bodyParts.TextureForShirt(null);
    //                    }
    //                    else
    //                    {
    //                        WearDefaultItem(type, this.gameObject, gender);
    //                    }
    //                }
    //            }
    //        }
    //        // Added By WaqasAhmad
    //        // When User Reset From Store 
    //        // _CharacterData file clear & no Data is available
    //        // Implemented Default Cloths
    //        else
    //        {
    //            string gender = _CharacterData.gender != null ? _CharacterData.gender : "Male";
    //            WearDefaultItem("Legs", this.gameObject, gender);
    //            WearDefaultItem("Chest", this.gameObject, gender);
    //            WearDefaultItem("Feet", this.gameObject, gender);
    //            WearDefaultItem("Hair", this.gameObject, gender);

    //            if (wornEyeWearable)
    //                UnStichItem("EyeWearable");
    //            if (wornChain)
    //                UnStichItem("Chain");
    //            if (wornGloves)
    //            {
    //                UnStichItem("Glove");
    //                bodyParts.TextureForGlove(null);
    //            }
    //        }
    //        if (_CharacterData.charactertypeAi == true)
    //        {
    //            ApplyAIData(_CharacterData, applyOn);
    //        }
    //        else
    //        {
    //            ApplyDefaultFrndData(_CharacterData, applyOn);
    //        }
    //        characterBodyParts.LoadBlendShapes(_CharacterData, applyOn);
    //        #region Xana Avatar 1.0   //--> remove for xana avatar2.0
    //        //if (_CharacterData.eyeTextureName != "" && _CharacterData.eyeTextureName != null)
    //        //{
    //        //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeTextureName, this.gameObject, CurrentTextureType.EyeLense));
    //        //}

    //        //if (_CharacterData.eyeLashesName != "" && _CharacterData.eyeLashesName != null)
    //        //{
    //        //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeLashesName, this.gameObject, CurrentTextureType.EyeBrowPoints));
    //        //}
    //        //if (_CharacterData.eyebrrowTexture != "" && _CharacterData.eyebrrowTexture != null)
    //        //{
    //        //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyebrrowTexture, this.gameObject, CurrentTextureType.EyeBrows));
    //        //}

    //        //if (_CharacterData.makeupName != "" && _CharacterData.makeupName != null)
    //        //{
    //        //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.makeupName, this.gameObject, CurrentTextureType.Makeup));
    //        //}

    //        //New texture are downloading for Boxer NFT 
    //        //if (!string.IsNullOrEmpty(_CharacterData.faceTattooTextureName) && _CharacterData.faceTattooTextureName != null)
    //        //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.faceTattooTextureName, this.gameObject, CurrentTextureType.FaceTattoo));
    //        //else
    //        //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.FaceTattoo);

    //        //if (!string.IsNullOrEmpty(_CharacterData.chestTattooTextureName) && _CharacterData.chestTattooTextureName != null)
    //        //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.chestTattooTextureName, this.gameObject, CurrentTextureType.ChestTattoo));
    //        //else
    //        //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.ChestTattoo);

    //        //if (!string.IsNullOrEmpty(_CharacterData.legsTattooTextureName) && _CharacterData.legsTattooTextureName != null)
    //        //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.legsTattooTextureName, this.gameObject, CurrentTextureType.LegsTattoo));
    //        //else
    //        //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.LegsTattoo);

    //        //if (!string.IsNullOrEmpty(_CharacterData.armTattooTextureName) && _CharacterData.armTattooTextureName != null)
    //        //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.armTattooTextureName, this.gameObject, CurrentTextureType.ArmTattoo));
    //        //else
    //        //    this.GetComponent<CharcterBodyParts>().RemoveTattoo(null, this.gameObject, CurrentTextureType.ArmTattoo);

    //        //if (!string.IsNullOrEmpty(_CharacterData.mustacheTextureName) && _CharacterData.mustacheTextureName != null)
    //        //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.mustacheTextureName, this.gameObject, CurrentTextureType.Mustache));
    //        //else
    //        //    this.GetComponent<CharcterBodyParts>().RemoveMustacheTexture(null, this.gameObject);


    //        //LoadBonesData(_CharacterData, this.gameObject);

    //        // Seperate 
    //        //if (_CharacterData.Skin != null)
    //        //{
    //        //    StartCoroutine(bodyParts.ImplementColors(_CharacterData.Skin, SliderType.Skin, this.gameObject));
    //        //    if (ConstantsHolder.xanaConstants.isNFTEquiped)
    //        //    {
    //        //        BoxerNFTEventManager._lightPresetNFT = GetLightPresetValue(_CharacterData.Skin);
    //        //        BoxerNFTEventManager.NFTLightUpdate?.Invoke(BoxerNFTEventManager._lightPresetNFT);
    //        //    }
    //        //}
    //        //if (_CharacterData.EyeColor != null)
    //        //{
    //        //    StartCoroutine(bodyParts.ImplementColors(_CharacterData.EyeColor, SliderType.EyesColor, this.gameObject));
    //        //}
    //        //if (_CharacterData.LipColor != null)
    //        //{
    //        //    StartCoroutine(bodyParts.ImplementColors(_CharacterData.LipColor, SliderType.LipsColor, this.gameObject));
    //        //}

    //        //if (_CharacterData.EyebrowColor != null)
    //        //{
    //        //    Color tempColor = _CharacterData.EyebrowColor;
    //        //    tempColor.a = 1;
    //        //    _CharacterData.EyebrowColor = tempColor;
    //        //    StartCoroutine(bodyParts.ImplementColors(_CharacterData.EyebrowColor, SliderType.EyeBrowColor, this.gameObject));
    //        //}

    //        //if (_CharacterData.SkinGerdientColor != null)
    //        //{
    //        //    bodyParts.ApplyGredientColor(_CharacterData.SkinGerdientColor, this.gameObject);
    //        //}
    //        //else
    //        //{
    //        //    bodyParts.ApplyGredientDefault(this.gameObject);
    //        //}

    //        //if (_CharacterData.SssIntensity != null)
    //        //{
    //        //    bodyParts.SetSssIntensity(_CharacterData.SssIntensity, this.gameObject);
    //        //}
    //        //else
    //        //{
    //        //    bodyParts.SetSssIntensity(bodyParts.defaultSssValue, this.gameObject);
    //        //}

    //        //SetItemIdsFromFile(_CharacterData);
    //        //bodyParts.LoadBlendShapes(_CharacterData, this.gameObject);
    //        #endregion  
    //    }
    //}

    ///// <summary>
    ///// To Download Random Presets
    ///// </summary>
    ///// <param name="_rand"> Rand number</param>
    //public void DownloadRandomFrndPresets(int _rand)
    //{
    //    SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
    //    SetAvatarClothDefault(gameObject, characterBodyParts.randomPresetData[_rand].GenderType);

    //    if (_CharacterData.myItemObj == null || _CharacterData.myItemObj.Count == 0)
    //    {
    //        _CharacterData.myItemObj = new List<Item>();
    //        for (int i = 0; i < 4; i++)
    //            _CharacterData.myItemObj.Add(new Item(0, "", "", "", ""));
    //    }

    //    _CharacterData.myItemObj[0].ItemName = characterBodyParts.randomPresetData[_rand].PantPresetData.ObjectName;
    //    _CharacterData.myItemObj[0].ItemType = characterBodyParts.randomPresetData[_rand].PantPresetData.ObjectType;

    //    _CharacterData.myItemObj[1].ItemName = characterBodyParts.randomPresetData[_rand].ShirtPresetData.ObjectName;
    //    _CharacterData.myItemObj[1].ItemType = characterBodyParts.randomPresetData[_rand].ShirtPresetData.ObjectType;

    //    _CharacterData.myItemObj[2].ItemName = characterBodyParts.randomPresetData[_rand].HairPresetData.ObjectName;
    //    _CharacterData.myItemObj[2].ItemType = characterBodyParts.randomPresetData[_rand].HairPresetData.ObjectType;

    //    _CharacterData.myItemObj[3].ItemName = characterBodyParts.randomPresetData[_rand].ShoesPresetData.ObjectName;
    //    _CharacterData.myItemObj[3].ItemType = characterBodyParts.randomPresetData[_rand].ShoesPresetData.ObjectType;

    //    if (_CharacterData.myItemObj.Count > 0)
    //    {
    //        for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
    //        {
    //            if (!string.IsNullOrEmpty(_CharacterData.myItemObj[i].ItemName))
    //            {
    //                HashSet<string> itemTypes = new HashSet<string> { "Legs", "Chest", "Feet", "Hair", "EyeWearable", "Glove", "Chain" };

    //                if (itemTypes.Any(item => _CharacterData.myItemObj[i].ItemType.Contains(item)))
    //                {
    //                    if (!_CharacterData.myItemObj[i].ItemName.ToLowerInvariant().Contains("md"))
    //                    {
    //                        var item = _CharacterData.myItemObj[i];
    //                        var gender = _CharacterData.gender ?? characterBodyParts.randomPresetData[_rand].GenderType;
    //                        var avatarController = this.gameObject.GetComponent<AvatarController>();
    //                        if (addressableDownloader == null)
    //                        {
    //                            addressableDownloader = AddressableDownloader.Instance;
    //                        }
    //                        StartCoroutine(addressableDownloader.DownloadAddressableObj(item.ItemID, item.ItemName, item.ItemType, gender, avatarController, Color.clear));
    //                    }
    //                    else
    //                    {
    //                        if (PlayerPrefs.HasKey("Equiped") || xanaConstants.isNFTEquiped)
    //                        {
    //                            if (_CharacterData.myItemObj[i].ItemType.IndexOf("Chest", StringComparison.Ordinal) >= 0)
    //                            {
    //                                if (wornShirt)
    //                                {
    //                                    UnStichItem("Chest");
    //                                    characterBodyParts.TextureForShirt(null);
    //                                }
    //                            }
    //                            else if (_CharacterData.myItemObj[i].ItemType.IndexOf("Hair", StringComparison.Ordinal) >= 0)
    //                            {
    //                                if (wornHair)
    //                                    UnStichItem("Hair");
    //                            }
    //                            else if (_CharacterData.myItemObj[i].ItemType.IndexOf("Legs", StringComparison.Ordinal) >= 0)
    //                            {
    //                                if (wornPant)
    //                                {
    //                                    UnStichItem("Legs");
    //                                    characterBodyParts.TextureForPant(null);
    //                                }
    //                            }
    //                            else if (_CharacterData.myItemObj[i].ItemType.IndexOf("Feet", StringComparison.Ordinal) >= 0)
    //                            {
    //                                if (wornShoes)
    //                                {
    //                                    UnStichItem("Feet");
    //                                    characterBodyParts.TextureForShoes(null);
    //                                }
    //                            }
    //                            else if (_CharacterData.myItemObj[i].ItemType.IndexOf("EyeWearable", StringComparison.Ordinal) >= 0)
    //                            {
    //                                if (wornEyeWearable)
    //                                    UnStichItem("EyeWearable");
    //                            }
    //                            else if (_CharacterData.myItemObj[i].ItemType.IndexOf("Glove", StringComparison.Ordinal) >= 0)
    //                            {
    //                                if (wornGloves)
    //                                {
    //                                    UnStichItem("Glove");
    //                                    characterBodyParts.TextureForGlove(null);
    //                                }
    //                            }
    //                            else if (_CharacterData.myItemObj[i].ItemType.IndexOf("Chain", StringComparison.Ordinal) >= 0)
    //                            {
    //                                if (wornChain)
    //                                    UnStichItem("Chain");
    //                            }
    //                        }
    //                        else
    //                        {
    //                            var item = _CharacterData.myItemObj[i];
    //                            var gender = _CharacterData.gender ?? "Male";
    //                            WearDefaultItem(item.ItemType, this.gameObject, gender);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    var item = _CharacterData.myItemObj[i];
    //                    var gender = _CharacterData.gender ?? "Male";
    //                    WearDefaultItem(item.ItemType, this.gameObject, gender);
    //                }
    //            }
    //        }
    //    }
    //}

    ///// <summary>
    ///// Apply Default Data to Frnd 
    ///// </summary>
    ///// <param name="_CharacterData"></param>
    ///// <param name="applyOn"></param>
    //void ApplyDefaultFrndData(SavingCharacterDataClass _CharacterData, GameObject applyOn)
    //{
    //    if (_CharacterData.gender == AvatarGender.VTuber_Female.ToString())
    //        return;


    //    if (_CharacterData.gender == AvatarGender.Male.ToString())
    //    {
    //        characterBodyParts.head.materials[2].SetColor("_BaseColor", new Color(1, 1, 1, 1));
    //        characterBodyParts.head.materials[2].SetColor("_Lips_Color", new Color(0.9137255f, 0.4431373f, 0.4352941f, 1));
    //        characterBodyParts.body.materials[0].SetColor("_BaseColor", new Color(1, 1, 1, 1));
    //        characterBodyParts.ApplyEyeLenTexture(CharacterHandler.instance.maleAvatarData.DEye_texture, characterBodyParts.gameObject);
    //    }
    //    else if (_CharacterData.gender == AvatarGender.Female.ToString())
    //    {
    //        characterBodyParts.head.materials[2].SetColor("_BaseColor", new Color(1, 1, 1, 1));
    //        characterBodyParts.head.materials[2].SetColor("_Lips_Color", new Color(0.9137255f, 0.4431373f, 0.4352941f, 1));
    //        characterBodyParts.body.materials[0].SetColor("_BaseColor", new Color(1, 1, 1, 1));
    //        characterBodyParts.ApplyEyeLenTexture(CharacterHandler.instance.femaleAvatarData.DEye_texture, characterBodyParts.gameObject);
    //    }

    //    if (_CharacterData.skin_color != "" && _CharacterData.Skin != null)
    //    {
    //        if (_CharacterData.ai_gender == "male")
    //        {
    //            StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTextureByName("Assets/Store Items Addressables/1k_Boy_Face_Texture", _CharacterData.skin_color, this.gameObject, CurrentTextureType.Face));
    //            StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTextureByName("Assets/Store Items Addressables/1k_Boy_Body_Texture", _CharacterData.skin_color, this.gameObject, CurrentTextureType.Skin));
    //        }
    //        else
    //        {
    //            StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTextureByName("Assets/Store Items Addressables/1k_Girl_Face_Textures", _CharacterData.skin_color, this.gameObject, CurrentTextureType.Face));
    //            StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTextureByName("Assets/Store Items Addressables/1k_Girl_Body_Texture", _CharacterData.skin_color, this.gameObject, CurrentTextureType.Skin));
    //        }
    //    }
    //    if (_CharacterData.hairItemData != null)
    //    {
    //        characterBodyParts.head.materials[2].SetColor("_BaseColor", new Color(1, 1, 1, 1));
    //        characterBodyParts.head.materials[2].SetColor("_Lips_Color", new Color(0.9137255f, 0.4431373f, 0.4352941f, 1));
    //        characterBodyParts.body.materials[0].SetColor("_BaseColor", new Color(1, 1, 1, 1));
    //        characterBodyParts.ApplyEyeLenTexture(CharacterHandler.instance.femaleAvatarData.DEye_texture, characterBodyParts.gameObject);
    //    }
    //    for (int i = 0; i < characterBodyParts.head.sharedMesh.blendShapeCount - 1; i++)
    //    {
    //        characterBodyParts.head.SetBlendShapeWeight(i, 0);
    //    }
    //}

    ///// <summary>
    ///// Set player for AR scene
    ///// </summary>
    //public void SetAvatarForAR()
    //{
    //    Animator anim = GetComponent<Animator>();
    //    anim.SetFloat("Blend", 0.0f);
    //    anim.SetBool("Action", false);
    //    anim.SetBool("isMoving", false);
    //    anim.SetBool("idel", true);
    //   // gameObject.GetComponent<CharacterOnScreenNameHandler>().enabled = false;
    //    anim.runtimeAnimatorController = ArAnimator;
    //}


    #region Commented Function
    // void OnBecameInvisible()
    //{
    //    isVisibleOnCam = false;
    //}

    //void OnBecameVisible()
    //{
    //    isVisibleOnCam = true;
    //}
    //public void ApplyPreset(SavingCharacterDataClass _CharacterData)
    //{
    //    presetHairColor = _CharacterData.HairColor;
    //    if (_CharacterData.myItemObj.Count > 0)
    //    {
    //        for (int i = 0; i < _CharacterData.myItemObj.Count; i++)
    //        {
    //            if (!string.IsNullOrEmpty(_CharacterData.myItemObj[i].ItemName))
    //            {
    //                string type = _CharacterData.myItemObj[i].ItemType;
    //                if (type.Contains("Legs") || type.Contains("Chest") || type.Contains("Feet") || type.Contains("Hair") || type.Contains("EyeWearable"))
    //                {
    //                    if (!_CharacterData.myItemObj[i].ItemName.Contains("md", System.StringComparison.CurrentCultureIgnoreCase))
    //                    {
    //                        StartCoroutine(AddressableDownloader.Instance.DownloadAddressableObj(_CharacterData.myItemObj[i].ItemID, _CharacterData.myItemObj[i].ItemName, type, _CharacterData.gender != null ? _CharacterData.gender : "Male", this.gameObject.GetComponent<AvatarController>(), Color.clear));
    //                    }
    //                    else
    //                    {
    //                        WearDefaultItem(type, this.gameObject, _CharacterData.gender != null ? _CharacterData.gender : "Male");
    //                    }
    //                }
    //                else
    //                {
    //                    WearDefaultItem(_CharacterData.myItemObj[i].ItemType, this.gameObject, _CharacterData.gender != null ? _CharacterData.gender : "Male");
    //                }
    //            }
    //            else // wear the default item of that specific part.
    //            {
    //                WearDefaultItem(_CharacterData.myItemObj[i].ItemType, this.gameObject, _CharacterData.gender != null ? _CharacterData.gender : "Male");
    //            }
    //        }
    //    }

    //    #region Xana Avatar 1.0 //--> remove for xana avatar2.0
    //    //if (_CharacterData.eyeTextureName != "" && _CharacterData.eyeTextureName != null)
    //    //{
    //    //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeTextureName, this.gameObject, CurrentTextureType.EyeLense));
    //    //}
    //    //if (_CharacterData.eyebrrowTexture != "" && _CharacterData.eyebrrowTexture != null)
    //    //{
    //    //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyebrrowTexture, this.gameObject, CurrentTextureType.EyeBrows));
    //    //}
    //    //if (_CharacterData.eyeLashesName != "" && _CharacterData.eyeLashesName != null)
    //    //{
    //    //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.eyeLashesName, this.gameObject, CurrentTextureType.EyeBrowPoints));
    //    //}
    //    //if (_CharacterData.makeupName != "" && _CharacterData.makeupName != null)
    //    //{
    //    //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture(_CharacterData.makeupName, this.gameObject, CurrentTextureType.Makeup));
    //    //}
    //    //else
    //    //{
    //    //    StartCoroutine(AddressableDownloader.Instance.DownloadAddressableTexture("nomakeup", this.gameObject, CurrentTextureType.Makeup));
    //    //}
    //    //LoadBonesData(_CharacterData, this.gameObject);

    //    //if (_CharacterData.Skin != null && _CharacterData.LipColor != null && _CharacterData.HairColor != null && _CharacterData.EyebrowColor != null && _CharacterData.EyeColor != null)
    //    //{
    //    //    // Seperate 
    //    //    if (_CharacterData.Skin != null)
    //    //    {
    //    //        StartCoroutine(characterBodyParts.ImplementColors(_CharacterData.Skin, SliderType.Skin, this.gameObject));
    //    //    }
    //    //    if (_CharacterData.EyeColor != null)
    //    //    {
    //    //        StartCoroutine(characterBodyParts.ImplementColors(_CharacterData.EyeColor, SliderType.EyesColor, this.gameObject));
    //    //    }
    //    //    if (_CharacterData.LipColor != null)
    //    //    {
    //    //        StartCoroutine(characterBodyParts.ImplementColors(_CharacterData.LipColor, SliderType.LipsColor, this.gameObject));
    //    //    }

    //    //    if (_CharacterData.EyebrowColor != null)
    //    //    {
    //    //        StartCoroutine(characterBodyParts.ImplementColors(_CharacterData.EyebrowColor, SliderType.EyeBrowColor, this.gameObject));
    //    //    }
    //    //}

    //    //if (_CharacterData.SkinGerdientColor != null)
    //    //{
    //    //    characterBodyParts.ApplyGredientColor(_CharacterData.SkinGerdientColor, this.gameObject);
    //    //}
    //    //else
    //    //{
    //    //    characterBodyParts.ApplyGredientDefault(this.gameObject);
    //    //}

    //    //if (_CharacterData.SssIntensity != null)
    //    //{
    //    //    characterBodyParts.SetSssIntensity(_CharacterData.SssIntensity, this.gameObject);
    //    //}
    //    //else
    //    //{
    //    //    characterBodyParts.SetSssIntensity(characterBodyParts.defaultSssValue, this.gameObject);
    //    //}

    //    //SetItemIdsFromFile(_CharacterData);

    //    //EyesBlinking.instance.isBlinking = false;
    //    //characterBodyParts.LoadBlendShapes(_CharacterData, this.gameObject);
    //    //characterBodyParts.ApplyPresiteGredient();

    //    //EyesBlinking.instance.StoreBlendShapeValues();          // Added by Ali Hamza
    //    #endregion
    //}
    //private void SetItemIdsFromFile(SavingCharacterDataClass _CharacterData)
    //{
    //    presetValue = _CharacterData.PresetValue;
    //    hairColorPaletteId = _CharacterData.HairColorPaletteValue;
    //    skinId = _CharacterData.SkinId;
    //    faceId = _CharacterData.FaceValue;
    //    eyeBrowId = _CharacterData.EyeBrowValue;
    //    eyeBrowColorPaletteId = _CharacterData.EyeBrowColorPaletteValue;
    //    eyesId = _CharacterData.EyeValue;
    //    eyesColorId = _CharacterData.EyesColorValue;
    //    eyesColorPaletteId = _CharacterData.EyesColorPaletteValue;
    //    noseId = _CharacterData.NoseValue;
    //    lipsId = _CharacterData.LipsValue;
    //    lipsColorId = _CharacterData.LipsColorValue;
    //    lipsColorPaletteId = _CharacterData.LipsColorPaletteValue;
    //    bodyFat = _CharacterData.BodyFat;
    //    eyeLashesId = _CharacterData.EyeLashesValue;
    //    makeupId = _CharacterData.MakeupValue;
    //}

    //public void ResetForLastSaved()
    //{
    //    //body fats
    //    SaveCharacterProperties.instance.SaveItemList.BodyFat = 0;
    //    //body blends
    //    AvatarCustomizationManager.Instance.UpdateChBodyShape(0);

    //    itemDatabase.RevertSavedCloths();
    //}

    //public void LastSaved_Reset()
    //{
    //    itemDatabase.RevertSavedCloths();
    //}
    #endregion
}