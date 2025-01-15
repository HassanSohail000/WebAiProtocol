using System.Collections;
//using Assets.Scripts.Api;
//using ThirdParty.Facebook;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{
    private Camera newCam;
    private Texture2D screenshot;
    private RenderTexture screenshotRT;

    public void Capture()
    {
        GameObject g = new GameObject();
        g.transform.parent = Camera.main.transform;
        g.transform.localPosition = Vector3.zero;
        g.transform.localRotation = Quaternion.Euler(Vector3.zero);
        g.SetActive(false);
        newCam = g.AddComponent<Camera>();

        newCam.enabled = false;

        screenshotRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);

        StartCoroutine(LoadTexture());
    }

    IEnumerator LoadTexture()
    {
        newCam.aspect = Camera.main.aspect;
        newCam.fieldOfView = Camera.main.fieldOfView;
        newCam.orthographic = Camera.main.orthographic;
        newCam.orthographicSize = Camera.main.orthographicSize;
        newCam.backgroundColor = Camera.main.backgroundColor;
        newCam.cullingMask = 1 << 8;
        newCam.cullingMask = ~newCam.cullingMask;
        newCam.depth = 1;

        yield return new WaitForEndOfFrame();

        newCam.targetTexture = screenshotRT;
        newCam.Render();
        screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();
        newCam.targetTexture = null;

        string base64 = System.Convert.ToBase64String(screenshot.EncodeToJPG());

#if !UNITY_EDITOR
        UploadAndShare(base64);
#endif
    }

    void UploadAndShare(string base64)
    {
        /*CSRequestHelper request = new CSRequestHelper
        {
            Uri = RestApi.PostAPI_UploadScreenshot(GameManager.Instance.getGameData().GetAvatarId()),
            Method = "POST",
            Headers = GameManager.Instance.getGameData().buildAuthDict(),
            Body = new UploadScreenshotResponse.InputParams
            {
                image = base64
            }
        };

        ClientServerManager.SendServerCall<string>(request, response =>
        {
            UploadScreenshotResponse.RootObject rootObject = JsonUtility.FromJson<UploadScreenshotResponse.RootObject>(response);
            if (GameManager.Instance.Response_IsLoggedOut(rootObject))
            {
                return;
            }

            if (GameManager.Instance.Response_HasErrors(rootObject))
            {
                ErrorReporting.Send(request, response);
                GameManager.Instance.Response_ShowErrors("PostAPI_UploadScreenshot", rootObject);
                return;
            }
            GameManager.Instance.UpdateSessionAdsData(rootObject.session.ads);
            GameManager.Instance.getGameData().UpdateEnergyInfo(rootObject.session.avatar.energy_daily, rootObject.session.avatar.energy_premium);

            FBManager.Instance.ShareScreenshot("Hey, check this out!", rootObject.payload.url);
        });
        */
    }
}
