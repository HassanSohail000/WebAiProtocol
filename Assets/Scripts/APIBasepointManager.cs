using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using MoralisUnity;

//[ExecuteInEditMode]
public class APIBasepointManager : MonoBehaviour
{
    public bool IsXanaLive = false;
    public static APIBasepointManager instance;
    private bool firsttimecallXanaLive = false;
    public string apiversion = "";
    public string apiversionForAnimation = "2";

    //public MoralisServerSettings _moralisServerSettings;  
    //[Header("TestnetMoralis")]
    //public string testDappURL;
    //public string TestAppID;

    //[Header("MainnetMoralis")]
    //public string MainDappURL;
    //public string MainAppID;  
       
        

    // Start is called before the first frame update
    void Start()
    {
        //ConstantsGod.API_BASEURL = "https://api-test.xana.net";
        //ConstantsGod.API_BASEURL_XANALIA = "https://testapi.xanalia.com";
    }

    private void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
            SetBaseURL();
        }
       

    }

    void SetBaseURL()
    {
        if (IsXanaLive)
        {
            ConstantsGod.API_BASEURL = "https://app-api.xana.net";
            ConstantsGod.API_BASEURL_XANALIA = "https://api.xanalia.com";
            ConstantsGod.XANALIA_SOCKET_ADDRESS = "https://prod-socket.xanalia.com/";
            ConstantsGod.TELEGRAM_LOGIN_URL = "https://xana.net/app?builderLoginHash=";

            ConstantsGod.API_BASEURL_Penpenz = "https://penpenz-prod.xana.net/"; // For Penpenz
            //_moralisServerSettings.DappIconUrl = MainDappURL;
            //_moralisServerSettings.DappId = MainAppID;
        }
        else
        {
            ConstantsGod.API_BASEURL = "https://api-test.xana.net";
            ConstantsGod.API_BASEURL_XANALIA = "https://testapi.xanalia.com";
            ConstantsGod.XANALIA_SOCKET_ADDRESS = "https://socket.xanalia.com/";
            ConstantsGod.TELEGRAM_LOGIN_URL = "https://event-test.xana.net/app?builderLoginHash=";

            ConstantsGod.API_BASEURL_Penpenz = "https://penpenz-dev.xana.net/"; // For Penpenz
            //_moralisServerSettings.DappIconUrl = testDappURL;
            //_moralisServerSettings.DappId = TestAppID;
        }

 
    }

    // Update is called once per frame
    /*void Update()
    {
        if (IsXanaLive)
        {
            if (!firsttimecallXanaLive)
            {
                firsttimecallXanaLive = true;
                ConstantsGod.API_BASEURL = "https://app-api.xana.net";
                ConstantsGod.API_BASEURL_XANALIA = "https://api.xanalia.com";
            }
        }
        else
        {
            if (firsttimecallXanaLive)
            {
                firsttimecallXanaLive = false;
                ConstantsGod.API_BASEURL = "https://api-test.xana.net";
                ConstantsGod.API_BASEURL_XANALIA = "https://testapi.xanalia.com";
            }
        }
    }*/

    IEnumerator getServerStatus()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("https://api-test.xana.net/auth/get-server-status");
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            //Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            try
            {
                //Debug.Log("Response server===" + uwr.downloadHandler.text);
                GetServerDetils bean = JsonUtility.FromJson<GetServerDetils>(uwr.downloadHandler.text.ToString().Trim());
                if (bean.success)
                {
                    IsXanaLive = bean.data.isServerLive;
                }
                else
                {
                    IsXanaLive = false;
                }
                SetBaseURL();
            }
            catch
            {
            }
        }
    }
    [System.Serializable]
    public class Data
    {
        public int id;
        public bool isServerLive;
        public DateTime createdAt;
        public DateTime updatedAt;
    }
    [System.Serializable]
    public class GetServerDetils
    {
        public bool success;
        public Data data;
        public string msg;
    }
}
