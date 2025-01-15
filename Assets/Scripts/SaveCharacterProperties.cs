using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using System;

public class SaveCharacterProperties : MonoBehaviour
{
    public static SaveCharacterProperties instance;
    public SavingCharacterDataClass SaveItemList = new SavingCharacterDataClass();
    public FilterBlendShapeSettings _sliderindexes;
    [HideInInspector]
    public static int NeedToShowSplash;


    //private Equipment equipment;
    public CharacterBodyParts charcterBodyParts;
    public AvatarController characterController;

    private void Awake()
    {
        instance = this;
        //NeedToShowSplash = 1;
    }
    public void Start()
    {
        if (ConstantsHolder.xanaConstants.isXanaPartyWorld)
            return;
        StartLocal();

        SaveItemList.faceMorphed = false;
        SaveItemList.eyeBrowMorphed = false;
        SaveItemList.eyeMorphed = false;
        SaveItemList.noseMorphed = false;
        SaveItemList.lipMorphed = false;

        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            SaveItemList.myItemObj = _CharacterData.myItemObj;
            SaveItemList.BodyFat = _CharacterData.BodyFat;
            SaveItemList.FaceBlendsShapes = _CharacterData.FaceBlendsShapes;
            SaveItemList.faceMorphed = _CharacterData.faceMorphed;
            SaveItemList.eyeBrowMorphed = _CharacterData.eyeBrowMorphed;
            SaveItemList.eyeMorphed = _CharacterData.eyeMorphed;
            SaveItemList.noseMorphed = _CharacterData.noseMorphed;
            SaveItemList.lipMorphed = _CharacterData.lipMorphed;
            SaveItemList.HairColorPaletteValue = _CharacterData.HairColorPaletteValue;
            SaveItemList.EyeValue = _CharacterData.EyeValue;

            SaveItemList.PresetValue = _CharacterData.PresetValue;
            SaveItemList.SkinId = _CharacterData.SkinId;
            SaveItemList.FaceValue = _CharacterData.FaceValue;
            SaveItemList.EyeBrowValue = _CharacterData.EyeBrowValue;
            SaveItemList.EyeBrowColorPaletteValue = _CharacterData.EyeBrowColorPaletteValue;
            SaveItemList.EyeLashesValue = _CharacterData.EyeLashesValue;
            SaveItemList.EyeValue = _CharacterData.EyeValue;
            SaveItemList.EyesColorValue = _CharacterData.EyesColorValue;
            SaveItemList.EyesColorPaletteValue = _CharacterData.EyesColorPaletteValue;
            SaveItemList.NoseValue = _CharacterData.NoseValue;
            SaveItemList.LipsValue = _CharacterData.LipsValue;
            SaveItemList.LipsColorValue = _CharacterData.LipsColorValue;
            SaveItemList.LipsColorPaletteValue = _CharacterData.LipsColorPaletteValue;
            SaveItemList.BodyFat = _CharacterData.BodyFat;
            SaveItemList.MakeupValue = _CharacterData.MakeupValue;
        }
        AssignCustomSlidersData();
    }

    public void AssignSavedPresets()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            for (int i = 0; i < GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
            {
                if (_CharacterData.FaceBlendsShapes != null && i < _CharacterData.FaceBlendsShapes.Length)
                    GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[i]);
            }

            if (GameManager.Instance && GameManager.Instance.mainCharacter.name.Contains("Vtuber"))
                return;

            AvatarCustomizationManager.Instance.UpdateChBodyShape(_CharacterData.BodyFat);// Implementing Save Skin Color
            if (_CharacterData.Skin != null && _CharacterData.SssIntensity != null)
            {
                charcterBodyParts.ImplementSavedSkinColor(_CharacterData.Skin, _CharacterData.SssIntensity);
            }
            else
            {
                charcterBodyParts.ImplementSavedSkinColor(_CharacterData.Skin);
            }

            if (_CharacterData.SkinGerdientColor != null)
            {
                charcterBodyParts.ApplyGredientColor(_CharacterData.SkinGerdientColor, GameManager.Instance.mainCharacter);
            }
            else
            {
                charcterBodyParts.ApplyGredientDefault(GameManager.Instance.mainCharacter);
            }

        }
    }
    public void AssignSavedPresets_Hack()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            for (int i = 0; i < GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
            {
                if (_CharacterData.FaceBlendsShapes != null && i < _CharacterData.FaceBlendsShapes.Length)
                    GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[i]);
            }
            AvatarCustomizationManager.Instance.UpdateChBodyShape(_CharacterData.BodyFat);
        }
    }

    public void AssignCustomSlidersData()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            for (int i = 0; i < GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
            {
                if (_sliderindexes.ContainsIndex(i))
                {
                    if (_CharacterData.FaceBlendsShapes != null && i < _CharacterData.FaceBlendsShapes.Length)
                        GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[i]);
                }
            }
        }
    }
    public void AssignCustomsliderNewData()
    {
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();

            SkinnedMeshRenderer smr = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>();

            for (int i = 0; i < smr.sharedMesh.blendShapeCount; i++)
            {
                if (_CharacterData.FaceBlendsShapes != null && i < _CharacterData.FaceBlendsShapes.Length)
                    _CharacterData.FaceBlendsShapes[i] = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(i);
            }
            File.WriteAllText(GameManager.Instance.GetStringFolderPath(), JsonUtility.ToJson(_CharacterData));
            InventoryManager.instance.OnSaveBtnClicked();
        }
    }

    public void SavePlayerPropertiesInClassObj()
    {
        if(characterController == null)
            characterController = GameManager.Instance.mainCharacter.GetComponent<AvatarController>();

        SaveItemList.myItemObj.Clear();
        SaveItemList.id = LoadPlayerAvatar.avatarId;
        SaveItemList.name = LoadPlayerAvatar.avatarName;
        SaveItemList.thumbnail = LoadPlayerAvatar.avatarThumbnailUrl;
        InventoryManager.instance.GreyRibbonImage.SetActive(true);
        InventoryManager.instance.WhiteRibbonImage.SetActive(false);
        InventoryManager.instance.SaveStoreBtn.GetComponent<Image>().color = Color.white;
        if(characterController.wornPant!=null)
            SaveItemList.myItemObj.Add(new Item(characterController.wornPantId, characterController.wornPant.name, "Legs"));
        if(characterController.wornShirt!=null)
            SaveItemList.myItemObj.Add(new Item(characterController.wornShirtId, characterController.wornShirt.name, "Chest"));
        if (characterController.wornHair != null)
        {
            SaveItemList.myItemObj.Add(new Item(characterController.wornHairId, characterController.wornHair.name, "Hair"));
            SaveItemList.HairColor = charcterBodyParts.GetHairColor();
            //InventoryManager.instance.itemData.hair_color = SaveItemList.HairColor;
        }
        if(characterController.wornShoes!=null)
            SaveItemList.myItemObj.Add(new Item(characterController.wornShoesId, characterController.wornShoes.name, "Feet"));
        if (characterController.wornEyeWearable != null)
        {
            SaveItemList.myItemObj.Add(new Item(characterController.wornEyewearableId, characterController.wornEyeWearable.name, "EyeWearable"));
        }
        SaveItemList.HairColorPaletteValue = characterController.hairColorPaletteId;
        SaveItemList.myItemObj.Add(new Item(0, "", "Glove"));
        SaveItemList.myItemObj.Add(new Item(0, "", "Chain"));
        SaveItemList.SkinId = characterController.skinId;
        SaveItemList.PresetValue = characterController.presetValue;
        SaveItemList.FaceValue = characterController.faceId;
        SaveItemList.EyeBrowValue = characterController.eyeBrowId;
        SaveItemList.EyeBrowColorPaletteValue = characterController.eyeBrowColorPaletteId;
        SaveItemList.EyeLashesValue = characterController.eyeLashesId;
        SaveItemList.EyeValue = characterController.eyesId;
        SaveItemList.EyesColorValue = characterController.eyesColorId;
        SaveItemList.EyesColorPaletteValue = characterController.eyesColorPaletteId;
        SaveItemList.NoseValue = characterController.noseId;
        SaveItemList.LipsValue = characterController.lipsId;
        SaveItemList.LipsColorValue = characterController.lipsColorId;
        SaveItemList.LipsColorPaletteValue = characterController.lipsColorPaletteId;
        SaveItemList.BodyFat = characterController.bodyFat;
        SaveItemList.MakeupValue = characterController.makeupId;
        SaveItemList.eyeTextureName = characterController._PCharacterData.eyeTextureName;

        SaveItemList.avatarType = "NewAvatar";
        int totalBlendShapes = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount;
        SaveItemList.FaceBlendsShapes = new float[totalBlendShapes];

        for (int i = 0; i < totalBlendShapes; i++)
        {
            //if (i < SaveItemList.FaceBlendsShapes.Length)
                SaveItemList.FaceBlendsShapes[i] = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(i);
        }
        SaveItemList.SavedBones.Clear(); // Not Using Bones
        for (int i = 0; i < charcterBodyParts.BonesData.Count; i++)
        {
            Transform bone = charcterBodyParts.BonesData[i].Obj.transform;
            SaveItemList.SavedBones.Add(new BoneDataContainer(charcterBodyParts.BonesData[i].Name, bone.localPosition, bone.localEulerAngles, bone.localScale));
        }
        SaveItemList.faceMorphed = ConstantsHolder.xanaConstants.isFaceMorphed;
        SaveItemList.eyeBrowMorphed = ConstantsHolder.xanaConstants.isEyebrowMorphed;
        SaveItemList.eyeMorphed = ConstantsHolder.xanaConstants.isEyeMorphed;
        SaveItemList.noseMorphed = ConstantsHolder.xanaConstants.isNoseMorphed;
        SaveItemList.lipMorphed = ConstantsHolder.xanaConstants.isLipMorphed;
        SaveItemList.gender = CharacterHandler.instance.activePlayerGender.ToString();
        // UGC Removed By Ahsan
        //SaveItemList.ai_gender = InventoryManager.instance.itemData.gender;
        //SaveItemList.charactertypeAi = InventoryManager.instance.itemData.CharactertypeAi;

        //SaveItemList.hair_color = InventoryManager.instance.itemData.hair_color;
        //SaveItemList.lip_color = InventoryManager.instance.itemData.lips_color;
        //SaveItemList.skin_color = InventoryManager.instance.itemData.skin_color;
        //SaveItemList.faceItemData = InventoryManager.instance.itemData.faceItemData;
        //SaveItemList.lipItemData = InventoryManager.instance.itemData.lipItemData;
        //SaveItemList.noseItemData = InventoryManager.instance.itemData.noseItemData;
        //SaveItemList.hairItemData = InventoryManager.instance.itemData._hairItemData;
        //SaveItemList.eyeItemData = InventoryManager.instance.itemData._eyeItemData;
        //SaveItemList.eyeShapeItemData = InventoryManager.instance.itemData.eyeShapeItemData;

        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));
            _CharacterData.id = SaveItemList.id;
            _CharacterData.name = SaveItemList.name;
            _CharacterData.gender = SaveItemList.gender;
            _CharacterData.thumbnail = SaveItemList.thumbnail;
            _CharacterData.avatarType = SaveItemList.avatarType;
            _CharacterData.myItemObj = SaveItemList.myItemObj;
            _CharacterData.PresetValue = SaveItemList.PresetValue;
            _CharacterData.BodyFat = SaveItemList.BodyFat;
            _CharacterData.eyeTextureName = SaveItemList.eyeTextureName;

            // Old Characters Properties
            {
                //_CharacterData.HairColorPaletteValue = SaveItemList.HairColorPaletteValue;
                //_CharacterData.FaceValue = SaveItemList.FaceValue;
                //_CharacterData.EyeBrowValue = SaveItemList.EyeBrowValue;
                //_CharacterData.EyeBrowColorPaletteValue = SaveItemList.EyeBrowColorPaletteValue;
                //_CharacterData.EyeLashesValue = SaveItemList.EyeLashesValue;
                //_CharacterData.EyeValue = SaveItemList.EyeValue;
                //_CharacterData.EyesColorValue = SaveItemList.EyesColorValue;
                //_CharacterData.EyesColorPaletteValue = SaveItemList.EyesColorPaletteValue;
                //_CharacterData.NoseValue = SaveItemList.NoseValue;
                //_CharacterData.LipsValue = SaveItemList.LipsValue;
                //_CharacterData.LipsColorValue = SaveItemList.LipsColorValue;
                //_CharacterData.LipsColorPaletteValue = SaveItemList.LipsColorPaletteValue;
                //_CharacterData.MakeupValue = SaveItemList.MakeupValue;

                //_CharacterData.FaceBlendsShapes = SaveItemList.FaceBlendsShapes;
                //_CharacterData.faceMorphed = SaveItemList.faceMorphed;
                //_CharacterData.eyeBrowMorphed = SaveItemList.eyeBrowMorphed;
                //_CharacterData.eyeMorphed = SaveItemList.eyeMorphed;
                //_CharacterData.noseMorphed = SaveItemList.noseMorphed;
                //_CharacterData.lipMorphed = SaveItemList.lipMorphed;
                //_CharacterData.Skin = charcterBodyParts.GetBodyColor();
                //_CharacterData.SkinGerdientColor = charcterBodyParts.GetSkinGredientColor();
                //_CharacterData.SkinId = SaveItemList.SkinId;

                // Added By WaqasAhmad
                //_CharacterData.LipColor = charcterBodyParts.GetLipColor();
                //_CharacterData.HairColor = charcterBodyParts.GetHairColor();
                //_CharacterData.EyebrowColor = charcterBodyParts.GetEyebrowColor();
                //_CharacterData.EyeColor = charcterBodyParts.GetEyeColor();

                //_CharacterData.eyeTextureName = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().materials[0].GetTexture("_Main_Trexture").name;
                //_CharacterData.eyebrrowTexture = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().materials[1].GetTexture("_BaseMap").name;
                //_CharacterData.eyeLashesName = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().materials[3].GetTexture("_BaseMap").name;
                //_CharacterData.makeupName = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().materials[2].GetTexture("_Base_Texture").name;

            }
            _CharacterData.FaceBlendsShapes = SaveItemList.FaceBlendsShapes;
            _CharacterData.ai_gender = SaveItemList.ai_gender;
            _CharacterData.charactertypeAi = SaveItemList.charactertypeAi;

            _CharacterData.hair_color = SaveItemList.hair_color;
            _CharacterData.HairColor = SaveItemList.HairColor;

            _CharacterData.lip_color = SaveItemList.lip_color;
            _CharacterData.skin_color = SaveItemList.skin_color;
            _CharacterData.faceItemData = SaveItemList.faceItemData;
            _CharacterData.lipItemData = SaveItemList.lipItemData;
            _CharacterData.noseItemData = SaveItemList.noseItemData;
            _CharacterData.hairItemData = SaveItemList.hairItemData;
            _CharacterData.eyeItemData = SaveItemList.eyeItemData;
            _CharacterData.eyeShapeItemData = SaveItemList.eyeShapeItemData;

            string bodyJson = JsonUtility.ToJson(_CharacterData);
            File.WriteAllText(GameManager.Instance.GetStringFolderPath(), bodyJson);
        }
        else  // IF NOT EXISTS THEN WRITE THE NEW FILE
        {
            SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
            string bodyJson = JsonUtility.ToJson(SaveItemList, true);
            File.WriteAllText(GameManager.Instance.GetStringFolderPath(), bodyJson);
        }
    }
    public void SavePlayerProperties()
    {
        SavePlayerPropertiesInClassObj();
        if (PlayerPrefs.GetInt("IsLoggedIn") == 1)
            ServerSideUserDataHandler.Instance.CreateUserOccupiedAsset(() =>
            {
            });
    }

    public void CreateFileFortheFirstTime()
    {
        SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
        SubCatString.FaceBlendsShapes = new float[GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];
        string bodyJson = JsonUtility.ToJson(SubCatString);
        File.WriteAllText(GameManager.Instance.GetStringFolderPath(), bodyJson);
    }

    public void SetDefaultData()
    {
        SavingCharacterDataClass SubCatString = new SavingCharacterDataClass();
        string bodyJson = JsonUtility.ToJson(SaveItemList);
        File.WriteAllText(GameManager.Instance.GetStringFolderPath(), bodyJson);
    }

    public void LoadMorphsfromFile()
    {
        Start();
        //StartLocal();  inside start we are alreay calling startlocal so commented this
    }
    //local file loading
    #region Local
    public void StartLocal()
    {
        if(GameManager.Instance.m_ChHead)
            SaveItemList.FaceBlendsShapes = new float[GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];
        if (File.Exists(GameManager.Instance.GetStringFolderPath()) && File.ReadAllText(GameManager.Instance.GetStringFolderPath()) != "")
        {
            SavingCharacterDataClass _CharacterData = new SavingCharacterDataClass();
            _CharacterData = _CharacterData.CreateFromJSON(File.ReadAllText(GameManager.Instance.GetStringFolderPath()));

            CharacterHandler.instance.ActivateAvatarByGender(/*_CharacterData.gender*/ "VTuber_Female");   // Activate Avatar

            SaveItemList.myItemObj = _CharacterData.myItemObj;
            SaveItemList.BodyFat = _CharacterData.BodyFat;
            SaveItemList.FaceBlendsShapes = _CharacterData.FaceBlendsShapes;
            SaveItemList.gender = _CharacterData.gender;

            float[] blendValues = new float[GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];
            //for (int i = 0; i < GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
            //{
            //    if (!_sliderindexes.ContainsIndex(i))
            //    {
            //        blendValues[i] = GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(i);
            //        GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, 0);
            //    }
            //}

            SaveItemList.FaceBlendsShapes = blendValues;
            AvatarCustomizationManager.Instance.UpdateChBodyShape(_CharacterData.BodyFat);

            for (int i = 0; i < GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount; i++)
            {
                if (_sliderindexes.ContainsIndex(i))
                {
                    if (_CharacterData.FaceBlendsShapes != null && i < _CharacterData.FaceBlendsShapes.Length)
                        GameManager.Instance.m_ChHead.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, _CharacterData.FaceBlendsShapes[i]);
                }
            }
        }
    }
    #endregion
}

[System.Serializable]
public class SavingCharacterDataClass
{
    public string id;
    public string name;
    public string thumbnail;
    public string gender;

    public string avatarType = "OldAvatar";
    public List<Item> myItemObj;

    public string ai_gender;
    public Color hair_color;
    public string skin_color;
    public Color lip_color;
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
    public Color HairColor;
    public Color EyebrowColor;
    public Color EyeColor;
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
    //public string eyeBrowName;
    public string makeupName;
    public float SssIntensity;
    public Color SkinGerdientColor;
    public string eyebrrowTexture;

    //nft avatar extra keys added.
    public string mustacheTextureName;
    public string faceTattooTextureName;
    public string chestTattooTextureName;
    public string legsTattooTextureName;
    public string armTattooTextureName;
    public string eyeLidTextureName;

    public float[] FaceBlendsShapes;

    public int stamina;
    public int speed;
    public string profile;
    public int defence;
    public int special_move;
    public int punch;
    public int kick;

    public SavingCharacterDataClass CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<SavingCharacterDataClass>(jsonString);
    }
    public SavingCharacterDataClass()
    {

    }

}
//Fighter Module Removed By Ahsan
//public class BoxerNFTDataClass
//{
//    public bool isNFTAquiped = false;
//    public string id;
//    //public List<BoneDataContainer> SavedBones;
//    public string Gloves;
//    public string Glasses;
//    public string Full_Costumes;
//    public string Chains;
//    public string Hairs;
//    public string Face_Tattoo;
//    public string Forehead_Tattoo;
//    public string Chest_Tattoo;
//    public string Arm_Tattoo;
//    public string Legs_Tattoo;
//    public string Shoes;
//    public string Mustache;
//    public string Pants;
//    public string Eyebrows;
//    public string Lips;
//    public string Heads;
//    public string Eye_Shapes;
//    public string Skin;
//    public string Eye_Lense;
//    public string Eyelid;
//    public int stamina;
//    public int speed;
//    public string profile;
//    public int defence;
//    public int special_move;
//    public int punch;
//    public int kick;

//    //public float[] FaceBlendsShapes;

//    public BoxerNFTDataClass CreateFromJSON(string jsonString)
//    {
//        return JsonUtility.FromJson<BoxerNFTDataClass>(jsonString);
//    }
//}