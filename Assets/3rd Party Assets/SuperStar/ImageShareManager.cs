using System;
using System.Collections;
//using SuperStar.Audio;
using UnityEngine;

namespace SuperStar
{
    public class ImageShareManager : MonoBehaviour
    {
        public static ImageShareManager Instance { get; private set; }


        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(gameObject);

            DontDestroyOnLoad(gameObject);

        }

        public void TakeScreenShot(string title)
        {
            StartCoroutine("ShareImage", title);
        }

        public IEnumerator ShareImage(string title)
        {
            string timeStamp = DateTime.Now.ToString("dd-mm-yyyy-HH-MM-ss");

            string fileName = "ss" + timeStamp + ".png";

            ScreenCapture.CaptureScreenshot(fileName);

            yield return new WaitForSeconds(1f);
            //AudioManager.Instance.PlayCaptureSound();
            //NativeShare nativeShare = new NativeShare();
            //nativeShare.SetTitle(title);
            //nativeShare.AddFile(Application.persistentDataPath + "/" + fileName);

            //nativeShare.Share();
        }

    }
}
