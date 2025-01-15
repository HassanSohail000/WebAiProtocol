using UnityEngine;
using UnityEngine.Video;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager _InstanceGM;




    public Camera MainCamera;



    public bool _IsXRPlatform, _IsAndroid, _IsStandalone;

    public GameObject _AndroidPlatfrom;

    public GameObject _AndroidPlatfrom_Canvas;


    public GameObject _XR_Player;

    //public XRUIInputModule rUIInputModule;
    

    

    public GameObject[] env_Subparts;

    public Transform[] teleportPoints;


    public AudioClip reseptionAudioClip;

    public GameObject videoPlayer, staticLogoScreen;
    public bool _IsVideoYouTube;

    //public YoutubePlayerLivestream playerLivestream;

    public AudioSource m_youtubeAudio;

    public GameObject dummy, mainPlayer, dummyCamera, dummyCMFreeLook, mainCamera, mainCMFreelook;

    public Transform dummyTarget;
    public float xTime, waitTime;
    public bool moveDeummy;

    public bool _isNPCMoving;
    public int count = 0;

    public VideoPlayer ytVideoPlayer;


    public enum Platform { Android,Standalone,XRVR};

    public Platform platform;

    public GameObject m_TestObject;



    public bool _isCanvasActive;

    public GameObject exitPanel;
    
    public GameObject minimapParent;

    public string link;

    public bool checking_video;


    private void Awake()
    {
        if (_InstanceGM == null)
            _InstanceGM = this;
        else
            Destroy(this);


        Android_platform_();


    }



    void Start()
    {
      //  InvokeRepeating("test", 5f,10f);

     
    }


    void Update()
    {
        if(exitPanel)
        _isCanvasActive = exitPanel.activeInHierarchy;
    }

    
    void test()
    {
        
       
    }

    public void cloasecanvas()
    {

    }

    public void youtubecheck()
    {
        //playerLivestream.gameObject.SetActive(true);

        if (link == null || link == "")
        {
            _IsVideoYouTube = false;

        }
        else
        {
            //playerLivestream.GetLivestreamUrl(link);

            _IsVideoYouTube = true;

        }
        CheckVideo();
    }


    public void CheckVideo()
    {
        if(_IsVideoYouTube)
        {
            videoPlayer.SetActive(true);
            staticLogoScreen.SetActive(false);
        }
        else
        {
            videoPlayer.SetActive(false);
            staticLogoScreen.SetActive(true);
        }
    }

    public void UnMute(bool test)
    {
     
    }

    void Android_platform_()
    {
        //if (rUIInputModule)
        //    rUIInputModule.gameObject.SetActive(false);
       // else
           // Debug.LogError("Noruiinput");
      
        //else
           // Debug.LogError("Noinput module");
        if (_XR_Player)
            _XR_Player.SetActive(false);
        //else
           // Debug.LogError("NO xr Player");
        if (_AndroidPlatfrom)
        {
            PlatformCheck(true, _AndroidPlatfrom);
            _AndroidPlatfrom_Canvas.SetActive(true);
        }
       // else
           // Debug.LogError("NoAndriodPlatform");

    }
        void PlatformCheck(bool check, GameObject obj)
    {
        obj.SetActive(check);
    }

    public void Prolog()
    {
        moveDeummy = true;
    }

    public void OffDummy()
    {
        dummy.SetActive(false);
        dummyCamera.SetActive(false);
        dummyCMFreeLook.SetActive(false);
        Android_platform_();
    }


    void ActiveMain()
    {
        mainPlayer.SetActive(true);
        mainCamera.SetActive(true);
        mainCMFreelook.SetActive(true);
    }

}


class DataSet
{
    public string _id { get; set; }

    public object createdBy { get; set; }

    public string streamName { get; set; }

    public string streamLink { get; set; }
}