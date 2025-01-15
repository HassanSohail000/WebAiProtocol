using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class PresetData_Jsons : MonoBehaviour
{
    public bool DefaultPreset;
    public string JsonDataPreset;
    public static string clickname;
    public bool IsStartUp_Canvas;   // if preset panel is appeared on start then allow it to change 
    private bool presetAlreadySaved = false;
    [SerializeField] private Texture eyeTex;
    public AvatarGender avatarGender;

    private Button button;
    private string filePath;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(ChangecharacterFromPresetPanel);
        }

        filePath = Path.Combine(Application.persistentDataPath, "loginAsGuestClass.json");
        if (DefaultPreset && !File.Exists(filePath))
        {
            ChangecharacterFromPresetPanel();
            InventoryManager.instance.TurnOffLoader(30);
        }
    }

    public void callit()
    {
        clickname = "";
    }

    public void ChangecharacterFromPresetPanel()
    {
        ConstantsHolder.xanaConstants.registerFirstTime = true;

        // Delete Old Preset Bones
        int oldBonesCount = GameManager.Instance.characterBodyParts.dynamicBone_Dress.Count;
        StartCoroutine(RemoveOldBones(oldBonesCount));

        if (!IsStartUp_Canvas)
        {
            if (clickname == gameObject.name)
                return;

            clickname = gameObject.name;
        }

        GameManager.Instance.characterBodyParts.DefaultTexture(false);

        if (/*!IsStartUp_Canvas && !UserPassManager.Instance.CheckSpecificItem(PresetNameinServer)*/ false)
        {
            Debug.Log("Please Upgrade to Premium account");
            return;
        }

        ConstantsHolder.xanaConstants.avatarStoreSelection[ConstantsHolder.xanaConstants.currentButtonIndex] = this.gameObject;
        ConstantsHolder.xanaConstants._curretClickedBtn = this.gameObject;

        if (ConstantsHolder.xanaConstants._lastClickedBtn && ConstantsHolder.xanaConstants._curretClickedBtn == ConstantsHolder.xanaConstants._lastClickedBtn && !IsStartUp_Canvas)
        {
            Debug.Log("Same Button Clicked");
            return;
        }

        GameManager.Instance.isStoreAssetDownloading = true;
        ConstantsHolder.xanaConstants._curretClickedBtn.transform.GetChild(0).gameObject.SetActive(true);

        if (ConstantsHolder.xanaConstants._lastClickedBtn && !IsStartUp_Canvas)
        {
            if (ConstantsHolder.xanaConstants._lastClickedBtn.GetComponent<PresetData_Jsons>())
                ConstantsHolder.xanaConstants._lastClickedBtn.transform.GetChild(0).gameObject.SetActive(false);
        }

        ConstantsHolder.xanaConstants._lastClickedBtn = this.gameObject;
        ConstantsHolder.xanaConstants.PresetValueString = gameObject.name;
        PlayerPrefs.SetInt("presetPanel", 1);

        // Hack for latest update // keep all preset body fat to 0
        //change lips to default

        SavingCharacterDataClass _CharacterData = JsonUtility.FromJson<SavingCharacterDataClass>(JsonDataPreset);
        _CharacterData.BodyFat = 0;
        _CharacterData.PresetValue = gameObject.name;

        if (_CharacterData.gender == "VTuber_Female" && _CharacterData.FaceBlendsShapes == null)
        {
            _CharacterData.FaceBlendsShapes = new float[58];
            _CharacterData.FaceBlendsShapes[52] = 100;
        }

        SaveCharacterProperties.instance.SaveItemList.gender = _CharacterData.gender;
        ConstantsHolder.xanaConstants.bodyNumber = 0;

        if (IsStartUp_Canvas || DefaultPreset)
        {
            File.WriteAllText(filePath, JsonUtility.ToJson(_CharacterData));
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "logIn.json"), JsonUtility.ToJson(_CharacterData));
            GetSavedPreset();
            SavePresetOnServer(_CharacterData);
        }

        // Set the position, rotation of the character 
        string oldSelectedGender = CharacterHandler.instance.activePlayerGender.ToString();

        if (oldSelectedGender != _CharacterData.gender)
        {
            Transform oldTransform = null;
            if (oldSelectedGender == "Female")
                oldTransform = CharacterHandler.instance.femaleAvatarData.avatar_parent.transform;
            else if (oldSelectedGender == "Male")
                oldTransform = CharacterHandler.instance.maleAvatarData.avatar_parent.transform;
            else if (oldSelectedGender == "VTuber_Female")
                oldTransform = CharacterHandler.instance.vt_Female.avatar_parent.transform;

            if (oldTransform != null)
            {
                CharacterHandler.instance.maleAvatarData.avatar_parent.transform.SetPositionAndRotation(oldTransform.position, oldTransform.rotation);
                CharacterHandler.instance.femaleAvatarData.avatar_parent.transform.SetPositionAndRotation(oldTransform.position, oldTransform.rotation);
                CharacterHandler.instance.vt_Female.avatar_parent.transform.SetPositionAndRotation(oldTransform.position, oldTransform.rotation);
            }

            if (ConstantsHolder.xanaConstants.isStoreActive)
                InventoryManager.instance.DeletePreviousItems();
        }

        GameManager.Instance.selectedPresetData = JsonUtility.ToJson(_CharacterData);
        CharacterHandler.instance.ActivateAvatarByGender(_CharacterData.gender);

        if (_CharacterData.gender == "Male")
        {
            CharacterHandler.instance.maleAvatarData.avatar_parent.GetComponent<AvatarController>().SetAvatarClothDefault(CharacterHandler.instance.maleAvatarData.avatar_parent, _CharacterData.gender);
        }
        else if (_CharacterData.gender == "Female")
        {
            CharacterHandler.instance.femaleAvatarData.avatar_parent.GetComponent<AvatarController>().SetAvatarClothDefault(CharacterHandler.instance.femaleAvatarData.avatar_parent, _CharacterData.gender);
        }
        else if (_CharacterData.gender == "VTuber_Female")
        {
            //CharacterHandler.instance.vt_Female.avatar_parent.GetComponent<CharacterBodyParts>().ApplyEyeLenTexture(eyeTex, CharacterHandler.instance.vt_Female.avatar_parent);
        }

        if (InventoryManager.instance.StartPanel_PresetParentPanel.activeSelf || InventoryManager.instance.selfiePanel.activeSelf || InventoryManager.instance.StartPanel_PresetParentPanelSummit.activeSelf)
        {
            InventoryManager.instance.StartPanel_PresetParentPanel.SetActive(false);
            InventoryManager.instance.StartPanel_PresetParentPanelSummit.SetActive(false);
            InventoryManager.instance.selfiePanel.SetActive(false);

            if (GameManager.Instance.UiManager.isAvatarSelectionBtnClicked)
            {
                GameManager.Instance.UiManager.isAvatarSelectionBtnClicked = false;
                GameManager.Instance.m_RenderTextureCamera.gameObject.SetActive(false);
            }
        }
        else
        {
            if (this.gameObject.name != PlayerPrefs.GetString("PresetValue"))
            {
                InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
                InventoryManager.instance.GreyRibbonImage.SetActive(false);
                InventoryManager.instance.WhiteRibbonImage.SetActive(true);
            }

            ConstantsHolder.xanaConstants._lastClickedBtn = this.gameObject;
            InventoryManager.upateAssetOnGenderChanged?.Invoke();
        }

        if (GameManager.Instance.avatarController.wornEyeWearable != null)
        {
            GameManager.Instance.avatarController.UnStichItem("EyeWearable");
        }

        if (_CharacterData.HairColor != null)
            ConstantsHolder.xanaConstants.isPresetHairColor = true;

        ApplyPreset(_CharacterData);

        if (!presetAlreadySaved)
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

    IEnumerator RemoveOldBones(int oldBonesCount)
    {
        yield return null;

        if (name.StartsWith("VT-"))
        {
            List<GameObject> boneList = GameManager.Instance.characterBodyParts.dynamicBone_Dress;
            for (int i = oldBonesCount - 1; i >= 0; i--)
            {
                if (i > -1 && boneList[i])
                {
                    Destroy(boneList[i].gameObject);
                    GameManager.Instance.characterBodyParts.dynamicBone_Dress.RemoveAt(i);
                }
            }
        }
    }

    void SavedButtonClickedBlue()
    {
        InventoryManager.instance.SaveStoreBtn.SetActive(true);
        InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = new Color(0f, 0.5f, 1f, 0.8f);
        InventoryManager.instance.GreyRibbonImage.SetActive(false);
        InventoryManager.instance.WhiteRibbonImage.SetActive(true);
    }

    public void GetSavedPreset()
    {
        string savedFilePath = PlayerPrefs.GetInt("IsLoggedIn") == 1 ? Path.Combine(Application.persistentDataPath, "logIn.json") : filePath;

        if (File.Exists(savedFilePath) && File.ReadAllText(savedFilePath) != "")
        {
            SavingCharacterDataClass _CharacterData1 = new SavingCharacterDataClass();
            _CharacterData1 = _CharacterData1.CreateFromJSON(File.ReadAllText(savedFilePath));
            if (this.gameObject.name == _CharacterData1.PresetValue)
                presetAlreadySaved = true;
        }
    }

    public void ApplyPreset(SavingCharacterDataClass _data)
    {
        if (PlayerPrefs.GetInt("presetPanel") == 1)
            PlayerPrefs.SetInt("presetPanel", 0);

        GameManager.Instance.avatarController.InitializeAvatar(false, _data);
    }

    void SavePresetOnServer(SavingCharacterDataClass savingCharacterDataClass)
    {
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
        {
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "logIn.json"), JsonUtility.ToJson(savingCharacterDataClass));
            ServerSideUserDataHandler.Instance.CreateUserOccupiedAsset(() => { });
        }
    }
}




public enum AvatarGender
{
    Male, Female, VTuber_Male, VTuber_Female
}
