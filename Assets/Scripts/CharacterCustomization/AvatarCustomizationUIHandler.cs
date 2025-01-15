using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

[Serializable]
public struct Panel
{
    public GameObject m_Panel_Obj;
    public string m_PanelName_Str;
}

public class AvatarCustomizationUIHandler : MonoBehaviour
{
    public static AvatarCustomizationUIHandler Instance;
    [Header("Blink Panel Animation")]  // this is used, to change the panel from face to body customization ui panel
    public GameObject m_BlinkAnimationPanel;
    public AnimationCurve m_AnimCurve;
    public AnimationCurve m_AnimCurve1;
    public float m_AnimTime;

    [Header("Blink Colors")]
    public Color m_ColorOne;
    public Color m_ColorTwo;

    public GameObject BG_Plane;

    [Header("Character Camera Works")]
    public GameObject headCamera;

    bool l_ZoomInState;
    bool l_ZoomOutState;

    Dictionary<int, string> m_PanelNames_BodyCustomization = new Dictionary<int, string>();
    Dictionary<int, string> m_PanelNames_ClothesCustomization = new Dictionary<int, string>();

    public Camera m_MainCamera;
    public GameObject SlidersSaveButton;
    GameManager gameManager;
    private void Awake()
    {
        //gameManager = gameManager?? GameManager.Instance;
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        //SlidersSaveButton.transform.parent.GetComponent<Button>().onClick.AddListener(CustomSliderSaveBtnFtn);
        gameManager = gameManager ?? GameManager.Instance;
    }

    public void CustomSliderSaveBtnFtn()
    {
        SaveCharacterProperties.instance.SavePlayerProperties();
    }



    #region Load And Close Character Customization Feature Page

    public void LoadCharacterCustomizationPanel()
    {
        InventoryManager.instance.gameObject.SetActive(true);
        AvatarCustomizationManager.Instance.OnLoadCharacterCustomizationPanel();
        l_ZoomOutState = false;
        ZoomOutCamera();
    }

    public void CloseCharacterCustomizationPanel()
    {
        InventoryManager.instance.gameObject.SetActive(false);
        AvatarCustomizationManager.Instance.OnCloseCharacterCustomizationPanel();
    }

    #endregion


    #region Load Clothes And Body Panels, And Their Respective Panels

    public void LoadSection_BodyCustomization()
    {
        LoadPanel_BodyCustomization("My");
        ZoomInCamera();
        gameManager.ChangeCharacterAnimationState(true);
        AvatarCustomizationManager.Instance.ResetCharacterRotation(180f); 
    }

    public void LoadSection_ClothesCustomization()
    {
        ZoomOutCamera();

        AvatarCustomizationManager.Instance.ResetCharacterRotation(180f);
        gameManager.ChangeCharacterAnimationState(false);
    }

    public void LoadPanel_BodyCustomization(string panelName)
    {
        if (panelName == "Face")
        {
            gameManager.ChangeCharacterAnimationState(true);
            ZoomInCamera();
        }
        else
        {
            ZoomInCamera();
            gameManager.ChangeCharacterAnimationState(false);
        }
    }


    #endregion

    public void LoadCustomFaceCustomizerPanel()
    {
        m_MainCamera.transform.position = new Vector3(-0.03f, 1.12f, -4.5f);
        m_MainCamera.orthographic = true;
        m_MainCamera.orthographicSize = 0.7f;
        m_MainCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
        l_ZoomInState = false;
    }

    #region Load Custom Blend Shape Panel

    public void LoadCustomBlendShapePanel(string id)
    {
        AvatarCustomizationManager.Instance.m_IsCharacterRotating = false;

        gameManager.ChangeCharacterAnimationState(true);
        
        // Commented By WaqasAhmad
        //AvatarCustomizationManager.Instance.m_MainCharacter.GetComponent<Animator>().enabled = false;
        //AvatarCustomizationManager.Instance.f_MainCharacter.GetComponent<Animator>().enabled = false;

      InventoryManager.instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameManager.UiManager.faceMorphPanel.SetActive(true);
        gameManager.faceMorphCam.SetActive(true);
    }


    public void CloseCustomBlendShapePanel()
    {
        gameManager.UiManager._footerCan.SetActive(true);
        InventoryManager.instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // Commented By Ahsan
        //if (InventoryManager.instance.ParentOfBtnsAvatarEyeBrows.gameObject.activeSelf)
        //{
        //    for (int i = 0; i < InventoryManager.instance.ParentOfBtnsAvatarEyeBrows.transform.childCount; i++)
        //    {
        //        InventoryManager.instance.ParentOfBtnsAvatarEyeBrows.transform.GetChild(i).gameObject.SetActive(false);
        //    }
        //    InventoryManager.instance.SubmitAllItemswithSpecificSubCategory(InventoryManager.instance.SubCategoriesList[ConstantsHolder.xanaConstants.currentButtonIndex + 8].id, true);
        //}
        gameManager.UiManager.faceMorphPanel.SetActive(false);
        gameManager.faceMorphCam.SetActive(false);
        AvatarCustomizationManager.Instance.m_IsCharacterRotating = true;

        AvatarCustomizationManager.Instance.m_MainCharacter.GetComponent<Animator>().enabled = true;
        AvatarCustomizationManager.Instance.f_MainCharacter.GetComponent<Animator>().enabled = true;
        //---------------------------------

        gameManager.mainCharacter.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        AvatarCustomizationManager.Instance._facemorphCamPosition.localPosition = new Vector3(-0.031f, 1.485f, 4.673f);
        //gameManager.mainCharacter.transform.localPosition = new Vector3(0f, -1.34f, 4.905974f);
        //gameManager.mainCharacter.transform.localPosition = new Vector3(0f, -1.48f, 6.41f);
        gameManager.mainCharacter.transform.localPosition = new Vector3(0f, -1.6f, 6.41f);

        AvatarCustomizationManager.Instance.m_LeftSideBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.3960f, 0.3960f, 0.3960f, 1f);
        AvatarCustomizationManager.Instance.m_FrontSidebtn.transform.GetChild(2).GetComponent<Text>().color = new Color(0.2274f, 0.5921f, 1f, 1f);

        AvatarCustomizationManager.Instance.m_FrontSidebtn.transform.GetChild(1).gameObject.SetActive(true);
        AvatarCustomizationManager.Instance.m_LeftSideBtn.transform.GetChild(1).gameObject.SetActive(false);
        //----------------------
        SetCameraPosForFaceCustomization.instance.ChangeCameraToProspective();

        //Comment because file is rewriting the vale here again...(Abdullah)
        //SaveCharacterProperties.instance.AssignCustomSlidersData();
        InventoryManager.instance.ResetMorphBooleanValues();
        gameManager.BlendShapeManager.TurnOffAllObjects();

        //gameManager.mainCharacter.GetComponent<EyesBlinking>().StoreBlendShapeValues();          // Added by Ali Hamza
        //if (!gameManager.mainCharacter.GetComponent<EyesBlinking>().isCoroutineRunning)
        //{
        //    StartCoroutine(gameManager.mainCharacter.GetComponent<EyesBlinking>().BlinkingStartRoutine());
        //}

        //  SaveCharacterProperties.instance.AssignCustomsliderNewData();
    }


    // save morph to server 
    public void CloseCustomBlendShapePanelSave_Morphs()
    {
        gameManager.UiManager._footerCan.SetActive(true);
        InventoryManager.instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameManager.UiManager.faceMorphPanel.SetActive(false);
        gameManager.faceMorphCam.SetActive(false);
        AvatarCustomizationManager.Instance.m_IsCharacterRotating = true;

        AvatarCustomizationManager.Instance.m_MainCharacter.GetComponent<Animator>().enabled = true;
        AvatarCustomizationManager.Instance.f_MainCharacter.GetComponent<Animator>().enabled = true;
        //---------------------------------

        gameManager.mainCharacter.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        AvatarCustomizationManager.Instance._facemorphCamPosition.localPosition = new Vector3(-0.031f, 1.485f, 4.673f);
        //gameManager.mainCharacter.transform.localPosition = new Vector3(0f, -1.34f, 4.905974f);
        gameManager.mainCharacter.transform.localPosition = new Vector3(0f, -1.48f, 6.41f);

        AvatarCustomizationManager.Instance.m_LeftSideBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.3960f, 0.3960f, 0.3960f, 1f);
        AvatarCustomizationManager.Instance.m_FrontSidebtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.2274f, 0.5921f, 1f, 1f);

        AvatarCustomizationManager.Instance.m_FrontSidebtn.transform.GetChild(1).gameObject.SetActive(true);
        AvatarCustomizationManager.Instance.m_LeftSideBtn.transform.GetChild(1).gameObject.SetActive(false);
        //----------------------
        SetCameraPosForFaceCustomization.instance.ChangeCameraToProspective();
        // SaveCharacterProperties.instance.AssignCustomSlidersData();

        InventoryManager.instance.SaveStoreBtn.GetComponent<Button>().onClick.Invoke();
        Debug.Log("<color=red>CustomizationManager AssignLastClickedBtnHere</color>");
        ConstantsHolder.xanaConstants._lastClickedBtn = gameObject;
        //  SaveCharacterProperties.instance.AssignCustomsliderNewData();

    }

    public void LoadMyFaceCustomizationPanel()
    {
        AvatarCustomizationManager.Instance.ResetCharacterRotation(180f);
        GameManager.Instance.ChangeCharacterAnimationState(true);
        ZoomInCamera();
    }

    public void LoadMyClothCustomizationPanel()
    {
        AvatarCustomizationManager.Instance.ResetCharacterRotation(180f);
        AvatarCustomizationManager.Instance.m_IsCharacterRotating = true;
        GameManager.Instance.ChangeCharacterAnimationState(true);
        ZoomOutCamera();
    }
    #endregion

    #region Zoom In And Zoom Out Camera
    public void ResetScrolltoStart(RectTransform ScrollToReset)
    {
        ScrollToReset.anchoredPosition = new Vector2(0f, ScrollToReset.anchoredPosition.y);
    }
    public void ZoomInCamera()
    {
        if (!l_ZoomInState)
        {
            ChangeHeadCamera(true);

            return;

        }
    }

    public void ZoomOutCamera()
    {
        if (!l_ZoomOutState)
        {
            ChangeHeadCamera(false);
            return;
        }
    }

    void ChangeHeadCamera(bool _active)
    {
        headCamera.SetActive(_active);
    }

    #endregion

    #region Panel Blinking Animation

    public void StartPanelBlinkAnimation()
    {
        StartCoroutine(PanelBlinkAnimation(m_AnimTime));
    }

    IEnumerator PanelBlinkAnimation(float l_TimeLimit)
    {
        float l_t = 0;

        Color c_from = m_ColorOne;
        Color c_to = m_ColorTwo;

        float t1 = 0, t2 = 0;

        while (l_t <= l_TimeLimit)
        {
            l_t += Time.fixedDeltaTime;

            if (l_t <= l_TimeLimit / 2)
            {
                t1 += Time.fixedDeltaTime;
                m_BlinkAnimationPanel.GetComponent<Image>().color = Color.Lerp(c_from, c_to, m_AnimCurve.Evaluate(t1 / (l_TimeLimit / 2)));
            }
            else
            {
                t2 += Time.fixedDeltaTime;
                m_BlinkAnimationPanel.GetComponent<Image>().color = Color.Lerp(c_to, c_from, m_AnimCurve1.Evaluate(t2 / (l_TimeLimit / 2)));
            }

            yield return null;
        }
    }

    #endregion
}
