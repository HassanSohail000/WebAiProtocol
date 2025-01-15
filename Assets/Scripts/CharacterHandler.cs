using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static InventoryManager;

public class CharacterHandler : MonoBehaviour
{
    public static CharacterHandler instance;
    public AvatarGender activePlayerGender;
    public AvatarData maleAvatarData;
    public AvatarData femaleAvatarData;
    public AvatarData vt_Female;
    public GameObject vtuberLight;
    public GameObject playerNameCanvas;
    public GameObject playerPostCanvas;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if(SaveCharacterProperties.instance && !string.IsNullOrEmpty(SaveCharacterProperties.instance.SaveItemList.gender))
            ActivateAvatarByGender(SaveCharacterProperties.instance.SaveItemList.gender);
    }

    public void ActivateAvatarByGender(string gender)
    {
        switch (gender)
        {
            case "Male":
                if (femaleAvatarData.avatar_parent)
                    femaleAvatarData.avatar_parent.gameObject.SetActive(false);

                if (vt_Female.avatar_parent)
                    vt_Female.avatar_parent.gameObject.SetActive(false);

                if (maleAvatarData.avatar_parent)
                {
                    maleAvatarData.avatar_parent.gameObject.SetActive(true);
                    UpdateAvatarRefrences(maleAvatarData);
                }
                break;
            case "Female":
                if(maleAvatarData.avatar_parent)
                    maleAvatarData.avatar_parent.gameObject.SetActive(false);

                if (vt_Female.avatar_parent)
                    vt_Female.avatar_parent.gameObject.SetActive(false);

                CharacterLightStatus(false);


                if (femaleAvatarData.avatar_parent)
                {
                    femaleAvatarData.avatar_parent.gameObject.SetActive(true);
                    UpdateAvatarRefrences(femaleAvatarData);
                }
                break;

            case "VTuber_Female":
                if (maleAvatarData.avatar_parent)
                    maleAvatarData.avatar_parent.gameObject.SetActive(false);

                if (femaleAvatarData.avatar_parent)
                    femaleAvatarData.avatar_parent.gameObject.SetActive(false);

                if (vt_Female.avatar_parent)
                    vt_Female.avatar_parent.gameObject.SetActive(true);
                CharacterLightStatus(true);
                UpdateAvatarRefrences(vt_Female);
                break;

            default:
                Debug.LogError("Gender Not Matched");
                    break;

        }

        //InventoryManager.upateAssetOnGenderChanged?.Invoke();
        //if(ConstantsHolder.xanaConstants.isStoreActive)

    }
    void CharacterLightStatus(bool status)
    {
        vtuberLight.SetActive(status);
    }
    private void UpdateAvatarRefrences(AvatarData _avatarData)
    {
        //if (_avatarData.avatar_parent.GetComponent<EyesBlinking>() != null)
        //{
        //    _avatarData.avatar_parent.GetComponent<EyesBlinking>().StoreBlendShapeValues();
        //    if (activePlayerGender != _avatarData.avatar_Gender)
        //    {
        //        StartCoroutine(_avatarData.avatar_parent.GetComponent<EyesBlinking>().BlinkingStartRoutine());
        //    }
        //}

        activePlayerGender = _avatarData.avatar_Gender;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.mainCharacter = _avatarData.avatar_parent;
            GameManager.Instance.m_ChHead = _avatarData.avatar_face.gameObject;
            GameManager.Instance.m_CharacterAnimator = _avatarData.avatar_animator;
            GameManager.Instance.avatarController = _avatarData.avatar_parent.GetComponent<AvatarController>();
            GameManager.Instance.characterBodyParts = _avatarData.avatar_parent.GetComponent<CharacterBodyParts>();
            //GameManager.Instance.eyesBlinking = _avatarData.avatar_parent.GetComponent<EyesBlinking>();


            if (!ConstantsHolder.xanaConstants.isStoreActive)
            {
                GameManager.Instance.m_CharacterAnimator.SetBool("Action", true);
                //if(GameManager.Instance.ActorManager)
                //GameManager.Instance.ActorManager.Init();
            }
            else
            {
                GameManager.Instance.m_CharacterAnimator.SetBool("IdleMenu", true);
            }

        }
        if (SaveCharacterProperties.instance != null && GameManager.Instance != null)
        {
            SaveCharacterProperties.instance.charcterBodyParts = GameManager.Instance.characterBodyParts;
            SaveCharacterProperties.instance.characterController = GameManager.Instance.avatarController;

            string AvatarName = SaveCharacterProperties.instance.characterController.name;

            if (AvatarName.Contains("Vtuber"))
            {
                if(AvatarName.Contains("Female"))
                    SaveCharacterProperties.instance.SaveItemList.gender = "VTuber_Female";
            }
            else if (AvatarName.Contains("Female"))
                SaveCharacterProperties.instance.SaveItemList.gender = "Female";
            else
                SaveCharacterProperties.instance.SaveItemList.gender = "Male";
        }
        
        if (playerNameCanvas && playerPostCanvas && !ConstantsHolder.xanaConstants.isStoreActive)
        {
            UpdateNameAndPostTarget(_avatarData.avatar_parent);   // Update the target of the name and post canvas to the active player
        }
    }


    private void UpdateNameAndPostTarget(GameObject activePlayer)
    {
        //playerNameCanvas.GetComponent<FollowUser>().targ = activePlayer.transform;
        //playerNameCanvas.GetComponent<FollowUser>().newplayerTransform = activePlayer.transform;

        //playerPostCanvas.GetComponent<LookAtCamera>()._playerTransform = activePlayer.transform;
        //playerPostCanvas.GetComponent<LookAtCamera>().newplayerTransform = activePlayer.transform;

        if (!playerNameCanvas.activeInHierarchy)
            playerNameCanvas.SetActive(true);
        if (!playerPostCanvas.activeInHierarchy)
            playerPostCanvas.SetActive(true);
    }

    public AvatarData GetActiveAvatarData()
    {
        if (activePlayerGender == AvatarGender.Male)
        {
            return maleAvatarData;
        }
        else
        {
            return femaleAvatarData;
        }
    }


    [Serializable]
    public class AvatarData
    {
        public AvatarGender avatar_Gender;
        public GameObject avatar_parent;
        public SkinnedMeshRenderer avatar_body;
        public SkinnedMeshRenderer avatar_face;
        public Animator avatar_animator;
        public Texture DShirt_Texture, DPent_Texture, DShoe_Texture, DEye_texture, DFace_Texture, DSkin_Texture;
    }
}
