using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AvatarCustomizationManager : MonoBehaviour
{
    public static AvatarCustomizationManager Instance;

    [HideInInspector]
    public GameObject m_MainCharacter;
    [HideInInspector]
    public GameObject f_MainCharacter;

    [Header("Character Rotation")]

    public bool m_IsCharacterRotating;
    public bool m_CanRotateCharacter;
    public float m_CharacterRotationSpeed;

    public RectTransform avatarRenderTextureName;

    public Button m_FrontSidebtn, m_LeftSideBtn;  //Left and right buttons for face morphing

    public Transform _facemorphCamPosition;

    private Transform _ChFrontPos, CamFrontPos;


    GameObject PlayerCharacter;
    CharacterBodyParts _chbodyparts;
    //public InternetChecker checkInternet;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_MainCharacter = GameManager.Instance.mainCharacter;
        f_MainCharacter = GameManager.Instance.mainCharacter;

        m_IsCharacterRotating = true;
        m_CanRotateCharacter = true;

        PlayerCharacter = GameManager.Instance.mainCharacter;
        _chbodyparts = PlayerCharacter.GetComponent<CharacterBodyParts>();

        _ChFrontPos = GameManager.Instance.mainCharacter.transform;
        CamFrontPos = _facemorphCamPosition;
       // checkInternet = InternetChecker.instance;
    }

    private void Update()
    {
        //if (!(m_IsCharacterRotating && m_CanRotateCharacter)) return;

        //if (checkInternet.ispopUpClose)
        //{
        //#if UNITY_EDITOR
        //            //EditorControls();
        //#endif
        //#if UNITY_IOS || UNITY_ANDROID
        //            //MobileControls();
        //#endif
        //}
    }

    public void RotateAvatar()
    {
        //m_MainCharacter = GameManager.Instance.mainCharacter;
        if (AvatarCustomizationUIHandler.Instance.headCamera.activeSelf)
            m_CharacterRotationSpeed = 1.3f;
        else
            m_CharacterRotationSpeed = 2.5f;

        if (m_MainCharacter.activeInHierarchy)
        {
            m_MainCharacter.transform.rotation *= Quaternion.Euler(Vector3.up * -Input.GetAxis("Mouse X") * m_CharacterRotationSpeed);
        }
        else
        {
            // Update Reference
            m_MainCharacter = GameManager.Instance.mainCharacter;
        }
       
        //if (Input.GetMouseButton(0))
        //{
            //if (Input.mousePosition.y > Screen.height / 2 && Input.mousePosition.y < Screen.height - (Screen.height / 12) && GetMousePositionForAvatar() && Input.mousePosition.x > Screen.width / 2 && Input.mousePosition.x < Screen.width - (Screen.width / 2) && GetMousePositionForAvatar())
            //{
            //    if (m_MainCharacter.activeInHierarchy)
            //    {
            //        m_MainCharacter.transform.rotation *= Quaternion.Euler(Vector3.up * -Input.GetAxis("Mouse X") * m_CharacterRotationSpeed);
            //    }
            //    if (f_MainCharacter.activeInHierarchy)
            //    {
            //        f_MainCharacter.transform.rotation *= Quaternion.Euler(Vector3.up * -Input.GetAxis("Mouse X") * m_CharacterRotationSpeed);
            //    }
            //}
        //}
    }

    //public void MobileControls()
    //{

    //    if (Input.touchCount > 0)
    //    {
    //        //print(Input.GetTouch(0).position);
    //        Touch l_Touch = Input.GetTouch(0);

    //        if (Input.mousePosition.y > Screen.height / 2 && Input.mousePosition.y < Screen.height - (Screen.height / 12) && GetMousePositionForAvatar() && Input.mousePosition.x > Screen.width / 4 && Input.mousePosition.x < Screen.width - (Screen.width / 4) && GetMousePositionForAvatar())
    //        {
    //            if (m_MainCharacter.activeInHierarchy)
    //                m_MainCharacter.transform.rotation *= Quaternion.Euler(Vector3.up * -l_Touch.deltaPosition.x * Time.deltaTime * m_CharacterRotationSpeed);
    //            if (f_MainCharacter.activeInHierarchy)
    //                f_MainCharacter.transform.rotation *= Quaternion.Euler(Vector3.up * -l_Touch.deltaPosition.x * Time.deltaTime * m_CharacterRotationSpeed);
    //        }
    //    }
    //}
    public void OnLoadCharacterCustomizationPanel()
    {
        m_IsCharacterRotating = true;
        m_CanRotateCharacter = true;
    }

    public void OnCloseCharacterCustomizationPanel()
    {
        m_CanRotateCharacter = false;

        GameManager.Instance.ChangeCharacterAnimationState(false);

        m_MainCharacter.transform.rotation = Quaternion.Euler(Vector3.up * 180);
        f_MainCharacter.transform.rotation = Quaternion.Euler(Vector3.up * 180);


        //WaqasAhmad
        {
            m_MainCharacter.transform.position = new Vector3(0, m_MainCharacter.transform.position.y,
                                                            m_MainCharacter.transform.position.z);

            f_MainCharacter.transform.position = new Vector3(0, f_MainCharacter.transform.position.y,
                                                             f_MainCharacter.transform.position.z);
        }

        m_IsCharacterRotating = false;
    }


    public void OnLoadCustomBlendShapePanel()
    {
        m_MainCharacter.transform.rotation = Quaternion.Euler(Vector3.up * 180);
        f_MainCharacter.transform.rotation = Quaternion.Euler(Vector3.up * 180);
        m_IsCharacterRotating = false;
    }

    public void ResetCharacterRotation(float rotationValue)
    {
        m_MainCharacter.transform.rotation = Quaternion.Euler(Vector3.up * rotationValue);
        f_MainCharacter.transform.rotation = Quaternion.Euler(Vector3.up * rotationValue);

        m_MainCharacter.transform.position = new Vector3(0, m_MainCharacter.transform.position.y,
                                                         m_MainCharacter.transform.position.z);

        f_MainCharacter.transform.position = new Vector3(0, f_MainCharacter.transform.position.y,
                                                         f_MainCharacter.transform.position.z);
    }

    public void OnClosePanel()
    {
        AvatarCustomizationManager.Instance.m_IsCharacterRotating = true;
    }


    public void OnFrontSide()
    {
        GameManager.Instance.mainCharacter.GetComponent<Animator>().SetBool("Customization", true);
        GameManager.Instance.mainCharacter.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        m_LeftSideBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.3960f, 0.3960f, 0.3960f, 1f);
        m_FrontSidebtn.transform.GetChild(2).GetComponent<Text>().color = new Color(0.2274f, 0.5921f, 1f, 1f);

        m_FrontSidebtn.transform.GetChild(1).gameObject.SetActive(true);
        m_LeftSideBtn.transform.GetChild(1).gameObject.SetActive(false);

        //_facemorphCamPosition.localPosition = new Vector3(-0.031f, 1.485f, 4.673f);
        _facemorphCamPosition.localPosition = new Vector3(0f, 1.485f, 4.673f);

        //Commented By WaqasAhmad
        //GameManager.Instance.mainCharacter.transform.localPosition = new Vector3(0f, -1.34f, 4.905974f);

        SetCameraPosForFaceCustomization.instance.ChangePosition(true);
    }
    public void OnLeftSide()
    {
        GameManager.Instance.mainCharacter.transform.localEulerAngles = new Vector3(0f, 225f, 0f);
        m_FrontSidebtn.transform.GetChild(2).GetComponent<Text>().color = new Color(0.3960f, 0.3960f, 0.3960f, 1f);
        m_LeftSideBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.2274f, 0.5921f, 1f, 1f);

        m_FrontSidebtn.transform.GetChild(1).gameObject.SetActive(false);
        m_LeftSideBtn.transform.GetChild(1).gameObject.SetActive(true);

        _facemorphCamPosition.localPosition = new Vector3(0.15f, 1.485f, 4.673f);
       
        // Commented By WaqasAhmad
        //GameManager.Instance.mainCharacter.transform.localPosition = new Vector3(0.14f, -1.34f, 4.83f);
        SetCameraPosForFaceCustomization.instance.ChangePosition(false);
    }

    public void UpdateChBodyShape(int value)
    {
        // Commented By Talha Now use texture for Body
        //for (int i = 0; i < _chbodyparts.m_BodyParts.Count; i++)
        //{
        //    if (_chbodyparts.m_BodyParts[i].GetComponent<SkinnedMeshRenderer>() && _chbodyparts.m_BodyParts[i].GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount!=0)
        //        _chbodyparts.m_BodyParts[i].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, value);
        //}
        //_equipScript.wornChest.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, value);
        //_equipScript.wornLegs.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, value);

       // Debug.Log("Body Shape Resizing : " + value);
    }


    public void ApplyBlendShapeValuesToCustomFace(GameObject l_FaceObject)
    {
        if (BodyFaceCustomizer.Instance.m_IsFaceBlendShapeApplied)
        {
            int l_One = PlayerPrefs.GetInt("FaceMorphIndexOne");
            int l_Two = PlayerPrefs.GetInt("FaceMorphIndexTwo");

            l_FaceObject.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(l_One, 100);
            l_FaceObject.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(l_Two, 100);
        }
    }

    private bool GetMousePositionForAvatar()
    {

#if UNITY_EDITOR
        Vector2 localMousePos = avatarRenderTextureName.InverseTransformPoint(Input.mousePosition);
#else
        Vector2 localMousePos = avatarRenderTextureName.InverseTransformPoint(Input.GetTouch(0).position);
#endif
        if (avatarRenderTextureName.rect.Contains(localMousePos))
        {
            return true;
        }

        return false;
    }
}
