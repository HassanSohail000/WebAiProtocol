using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AvatarSelfie : MonoBehaviour
{
    [Header("Selfie Camera")]
    public GameObject selfieCam;
    public Animator m_CharacterAnimator;
    //[Header("Selfie Panel")]
    //public GameObject m_captureImage2;

    //[Header("Captured Image")]
    //public RawImage m_CapturedImage;

    public RenderTexture m_RenderTexture;
    Texture2D m_Texture2D;
    public Color[] CamBG;

    public static AvatarSelfie instance;
    bool lastAnimatorState=false; // to reterive animation state after taking selife 

    private void Start()
    {
        instance = this;

    }

    /// <summary>
    /// Take avatar screenshot and avatar preset data on server or local
    /// </summary>
    public void TakeScreenShootAndSaveData(Action<bool> CallBack)
    {
        //if (!UserPassManager.Instance.CheckSpecificItem("Selfie Button"))
        //{
        //    //UserPassManager.Instance.PremiumUserUI.SetActive(true);
        //    //print("Please Upgrade to Premium account");
        //    return;
        //}
        //else
        //{
        //    //print("Horayyy you have Access");
        //}

        m_CharacterAnimator = GameManager.Instance.m_CharacterAnimator;
        lastAnimatorState = m_CharacterAnimator.GetBool("Idle");
        m_CharacterAnimator.SetBool("Idle", true);
        m_CharacterAnimator.transform.eulerAngles = new Vector3(0, 180, 0);
        LoadPlayerAvatar.instance_loadplayer.PlayerPanelSaveButton.interactable = false;
        //LoadPlayerAvatar.instance_loadplayer.screenShotloader.SetActive(true);
        selfieCam.GetComponent<Camera>().backgroundColor = CamBG[(UnityEngine.Random.Range(0, CamBG.Length))];
        selfieCam.SetActive(true);

        //m_captureImage2.SetActive(true);
        //m_CapturedImage.gameObject.SetActive(false);
        //m_CapturedImage.texture = null;
        StartCoroutine(TakeScreenShootAndSaveToGallary((IsSucess)=> 
        {
            if (IsSucess)
                CallBack(true);
            else
                CallBack(false);
        }));

        //return LoadPlayerAvatar.avatarThumbnailUrl;
    }

    IEnumerator TakeScreenShootAndSaveToGallary(Action<bool> CallBack)
    {
        yield return new WaitForSeconds(.2f);

        Texture2D l_Texture2d = new Texture2D(m_RenderTexture.width, m_RenderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = m_RenderTexture;
        l_Texture2d.ReadPixels(new Rect(0, 0, m_RenderTexture.width, m_RenderTexture.height), 0, 0);
        l_Texture2d.Apply();
        selfieCam.SetActive(false);

        //m_CapturedImage.texture = l_Texture2d;

        //m_CapturedImage.gameObject.SetActive(true);

        //m_Texture2D = l_Texture2d;
        //m_CapturedImage.texture = m_Texture2D;

        Byte[] imageData = l_Texture2d.EncodeToPNG();

        StartCoroutine(UploadImageOnServer(imageData, (IsSucess)=> 
        {
            if (IsSucess)
                CallBack(true);
            else
                CallBack(false);
        }));
        selfieCam.SetActive(false);
        m_CharacterAnimator.SetBool("Idle", lastAnimatorState);

        m_RenderTexture.Release();      // AR changes
    }

    IEnumerator UploadImageOnServer(Byte[] imageData, Action<bool> CallBack)
    {
        WWWForm wWWForm = new WWWForm();
        wWWForm.AddBinaryData("file", imageData);

        UnityWebRequest unityWebRequest = UnityWebRequest.Post(ConstantsGod.API_BASEURL + ConstantsGod.UPLOADFILECLOUDIMAGE, wWWForm);
        unityWebRequest.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);

        UnityWebRequestAsyncOperation www = unityWebRequest.SendWebRequest();

        while (!www.isDone)
        {
            yield return null;
        }
        Debug.Log(unityWebRequest.downloadHandler.text);

        ImageData data = new ImageData();
        data = JsonUtility.FromJson<ImageData>(unityWebRequest.downloadHandler.text);
        if (data.success == "true")
        {
            LoadPlayerAvatar.avatarThumbnailUrl = data.cdn_link;
            CallBack(true);
        }
        else
            CallBack(false);
        //LoadPlayerAvatar.instance_loadplayer.screenShotloader.SetActive(false);
        LoadPlayerAvatar.instance_loadplayer.PlayerPanelSaveButton.interactable = true;
        //if (updateAvatar && LoadPlayerAvatar.instance_loadplayer.contentParent.transform.childCount>1)
        //{
        //    LoadPlayerAvatar.instance_loadplayer.UpdateExistingUserData();

        //}
        //else
        //{
        //    InventoryManager.instance.OnSaveBtnClicked();
        //}

        LoadPlayerAvatar.instance_loadplayer.CloseAvatarPanel();
    }



    //IEnumerator SaveImageLocally()
    //{

    //byte[] l_Bytes = m_Texture2D.EncodeToPNG();





    //#if UNITY_EDITOR

    //        string path = Application.streamingAssetsPath + "/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".png";
    //        File.WriteAllBytes(path, l_Bytes);
    //        if (l_Bytes != null)
    //        {
    //            if (GameManager.currentLanguage == "ja")
    //            {
    //                //Debug.LogError("Image save successfully!");
    //                //showToast(ShowToastMessage, "写真フォルダへ保存しました！", 2);
    //            }
    //            else
    //            {
    //                //Debug.LogError("Image save successfully!");
    //                //showToast(ShowToastMessage, "Image save successfully!", 2);
    //            }
    //        }

    //#endif



    //#if UNITY_ANDROID



    //        //if (!Directory.Exists(PlayerPrefs.GetString(ConstantsGod.ANDROIDPATH) + "/DCIM/XanaB"))
    //        //{
    //        //    Directory.CreateDirectory(PlayerPrefs.GetString(ConstantsGod.ANDROIDPATH) + "/DCIM/XanaB");
    //        //}


    //        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(l_Bytes, "/XanaB2", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".png");
    //        //    string path1 = PlayerPrefs.GetString(ConstantsGod.ANDROIDPATH) + "/DCIM/Xana/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".png";
    //        //    File.WriteAllBytes(path1, l_Bytes);

    //        if (l_Bytes != null)
    //        {
    //            if (GameManager.currentLanguage == "ja")
    //            {
    //                Debug.LogError("Image save successfully!");
    //                //showToast(ShowToastMessage, "写真フォルダへ保存しました！", 2);
    //            }
    //            else
    //            {
    //                Debug.LogError("Image save successfully!");
    //                //showToast(ShowToastMessage, "Image save successfully!", 2);
    //            }
    //        }

    //#endif

    //#if UNITY_IOS

    //        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(l_Bytes, "GalleryTest", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".png");

    //        if (l_Bytes != null)
    //        {
    //            if (GameManager.currentLanguage == "ja")
    //            {
    //                showToast(ShowToastMessage, "写真フォルダへ保存しました！", 2);
    //            }
    //            else
    //            {
    //                showToast(ShowToastMessage, "Image save successfully!", 2);
    //            }
    //        }
    //#endif
    //}


}
    [Serializable]
    public class ImageData
    {
        public string success;
        public data data;
        public string cdn_link;
        public string msg;
    }

    [Serializable]
   public class data
    {
        public string file;
    // New Parameters added for 'TagDetails' Call Response 
        public string count;
        public RowDataTags[] rows;
    }


[Serializable]
public class RowDataTags
{
    public string id;
    public string tagName;
}