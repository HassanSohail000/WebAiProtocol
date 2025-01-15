using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class ScreenOrientationManager : MonoBehaviour
{
    public List<GameObject> landscapeObj;
    public List<GameObject> potraitObj;

    public bool isPotrait = false;
    public static ScreenOrientationManager _instance;
    public static Action<bool> switchOrientation;

    [HideInInspector]
    public float joystickInitPosY = 0;
    public GameObject JyosticksObject;
    CanvasGroup landscapeCanvas;
    CanvasGroup potraitCanvas;

    //public AvatarSpawnerOnDisconnect ref_avatarManager;
    //public AvatarSpawnerOnDisconnect ref_avatarManager_Portrait;

    private void Awake()
    {
        _instance = this;
        landscapeCanvas = landscapeObj[1].GetComponent<CanvasGroup>();
        potraitCanvas = potraitObj[1].GetComponent<CanvasGroup>();

        //Invoke("CheckOrienataionWhenComeFromLobby", 1f);
        CheckOrienataionWhenComeFromLobby();
    }


    void CheckOrienataionWhenComeFromLobby()
    {
        if (ConstantsHolder.xanaConstants.isFromXanaLobby && ConstantsHolder.xanaConstants.orientationchanged)
        {
            MyOrientationChangeCode(DeviceOrientation.Portrait);
        }
        else
        {
            ConstantsHolder.xanaConstants.orientationchanged = false;
        }

        isPotrait = ConstantsHolder.xanaConstants.orientationchanged;
    }

    private void OnEnable()
    {

        //ChangeOrientation_Main.OnOrientationChange += MyOrientationChangeCode;
    }

    private void OnDisable()
    {
        //ChangeOrientation_Main.OnOrientationChange -= MyOrientationChangeCode;
    }

    public void MyOrientationChangeCode(DeviceOrientation orientation)
    {
        //FadeBothCanvas();
        print("Waqas Orientation Changed : " + orientation);

        switch (orientation)
        {
            case DeviceOrientation.LandscapeLeft:
                StartCoroutine(ChangeOrientation(false));
                break;
            case DeviceOrientation.Portrait:

                StartCoroutine(ChangeOrientation(true));
                break;
        }
    }


    IEnumerator ChangeOrientation(bool orientation)
    {
        isPotrait = orientation;
        ConstantsHolder.xanaConstants.orientationchanged = isPotrait;
        //BuilderEventManager.BuilderSceneOrientationChange?.Invoke(orientation);
        landscapeCanvas.DOKill();
        landscapeCanvas.alpha = 0;
        landscapeCanvas.blocksRaycasts = false;
        landscapeCanvas.interactable = false;
        potraitCanvas.DOKill();
        potraitCanvas.alpha = 0;
        potraitCanvas.blocksRaycasts = false;
        potraitCanvas.interactable = false;
        yield return new WaitForSeconds(0.1f);
        //AvatarSpawnerOnDisconnect.Instance = null;
        //if (isPotrait)  // // Commented out the portrait code in gameplay since it's not needed for Summit.
        //{
        //    AvatarSpawnerOnDisconnect.Instance = ref_avatarManager_Portrait;
        //    potraitCanvas.DOFade(1, 0.5f);
        //    potraitCanvas.blocksRaycasts = true;
        //    potraitCanvas.interactable = true;
        //    Screen.orientation = ScreenOrientation.Portrait;
        //}
        //else
        //{
            //AvatarSpawnerOnDisconnect.Instance = ref_avatarManager;
            landscapeCanvas.DOFade(1, 0.5f);
            landscapeCanvas.blocksRaycasts = true;
            landscapeCanvas.interactable = true;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        //}

        //if (ArrowManager.Instance && !ConstantsHolder.isPenguin)
        //{
        //    AvatarSpawnerOnDisconnect.Instance.currentDummyPlayer = ArrowManager.Instance.gameObject;
        //    ReferencesForGamePlay.instance.MainPlayerParent.GetComponent<PlayerController>().restJoyStick();
        //}

        for (int i = 0; i < landscapeObj.Count; i++)
        {
            landscapeObj[i].SetActive(!isPotrait);
            potraitObj[i].SetActive(isPotrait);
        }

       
    }

    public void ChangeOrientation_editor()
    {
        isPotrait = !isPotrait;
        ChangeGameplayBtnStates();
        StartCoroutine(ChangeOrientation(isPotrait));
        if (switchOrientation != null)
            switchOrientation.Invoke(isPotrait);
    }

    public void ChangeGameplayBtnStates()
    {
        //if (isPotrait)
        //{
        //    //Set Camera view switching button state
        //    StateHandlingOfGPBtns(potraitObj[4].GetComponent<Enable_DisableObjects>().ButtontoUninteractable[5].GetComponent<Image>(), 
        //        landscapeObj[4].GetComponent<Enable_DisableObjects>().ButtontoUninteractable[5].GetComponent<Image>());

        //    //Set Chat button and panel state
        //    StateHandlingOfGPBtns(potraitObj[4].GetComponent<Enable_DisableObjects>().ButtontoUninteractable[10].GetComponent<Image>(),
        //        landscapeObj[4].GetComponent<Enable_DisableObjects>().ButtontoUninteractable[10].GetComponent<Image>());
        //        ReferencesForGamePlay.instance.ChatSystemRef.OpenCloseChatDialog(
        //            landscapeObj[4].GetComponent<Enable_DisableObjects>().ButtontoUninteractable[10].GetComponent<Image>().isActiveAndEnabled);
        //}
        //else
        //{
        //    //Set Camera view switching button state
        //    StateHandlingOfGPBtns(landscapeObj[4].GetComponent<Enable_DisableObjects>().ButtontoUninteractable[5].GetComponent<Image>(),
        //        potraitObj[4].GetComponent<Enable_DisableObjects>().ButtontoUninteractable[5].GetComponent<Image>());

        //    //Set Chat button and panel state
        //    StateHandlingOfGPBtns(landscapeObj[4].GetComponent<Enable_DisableObjects>().ButtontoUninteractable[10].GetComponent<Image>(),
        //    potraitObj[4].GetComponent<Enable_DisableObjects>().ButtontoUninteractable[10].GetComponent<Image>());
        //    ReferencesForGamePlay.instance.ChatSystemRef.OpenCloseChatDialog(
        //                        potraitObj[4].GetComponent<Enable_DisableObjects>().ButtontoUninteractable[10].GetComponent<Image>().isActiveAndEnabled);
        //}
    }

    void StateHandlingOfGPBtns(Image _objectStateToApply, Image _objStateToCheck)
    {
        _objectStateToApply.enabled = _objStateToCheck.isActiveAndEnabled;
    }

}