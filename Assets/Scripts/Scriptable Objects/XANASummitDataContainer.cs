using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
//using static SummitAIChatHandler;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObjects/SummitDataContainer", fileName = "ScriptableObjects/SummitDataContainer")]
public class XANASummitDataContainer : ScriptableObject
{

    public GameObject maleAIAvatar;
    public GameObject femaleAIAvatar;
    public GameObject penguinAvatar;
    public string[] avatarJson;
    public DomeData summitData = new DomeData();
    public AIData aiData = new AIData();
    public static string FixedAvatarJson;
    public static Stack<StackInfoWorld> LoadedScenesInfo = new Stack<StackInfoWorld>();
    public static List<GameObject> SceneTeleportingObjects = new List<GameObject>();
    public static bool Penpenz=false;
    string[] s = { "ZONE-X", "ZONE X Musuem", "Xana Lobby", "XANA Festival Stage", "Xana Festival", "THE RHETORIC STAR", "ROCK?N ROLL CIRCUS", "MASAMI TANAKA", "Koto-ku Virtual Exhibition", "JJ MUSEUM", "HOKUSAI KATSUSHIKA", "Green Screen Studio", "GOZANIMATOR HARUNA GOUZU GALLERY 2021", "Genesis ART Metaverse Museum", "FIVE ELEMENTS", "DEEMO THE MOVIE Metaverse Museum", "D_Infinity_Labo", "BreakingDown Arena", "Astroboy x Tottori Metaverse Museum" };

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
#endif
    }

    public async void GetAllDomesData()
    {
        string url = ConstantsGod.API_BASEURL + ConstantsGod.GETALLDOMES;
        string result = await GetAsyncRequest(url);
        summitData = JsonUtility.FromJson<DomeData>(result);

        // Activate Map
        //ReferencesForGamePlay.instance.FullScreenMapStatus(true);
    }

    public async Task<bool> GetAIData(int domeId)
    {
        string url = ConstantsGod.API_BASEURL + ConstantsGod.GETDOMENPCINFO + domeId + "/" + 1;
        string result = await GetAsyncRequest(url);
        aiData = JsonUtility.FromJson<AIData>(result);

        return aiData.npcData.Count > 0;
    }


    public async Task<SingleDomeData> GetSingleDomeData(int _DomeId)
    {
        string url = ConstantsGod.API_BASEURL + ConstantsGod.GETSINGLEDOME + _DomeId;
        string result = await GetAsyncRequest(url);

        return JsonUtility.FromJson<SingleDomeData>(result);

    }

    async Task<string> GetAsyncRequest(string url)
    {
        string response=string.Empty;
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            await www.SendWebRequest();
            while (!www.isDone)
                await System.Threading.Tasks.Task.Yield();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                response =www.error;
            }
            else
            {
                response=www.downloadHandler.text;
            }
            www.Dispose();
        };
        return response;

    }

    async Task<string> GetTokenBasedAsyncRequest(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Authorization", ConstantsGod.AUTH_TOKEN);
        await www.SendWebRequest();
        while (!www.isDone)
            await System.Threading.Tasks.Task.Yield();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            return www.error;
        }
        else
            return www.downloadHandler.text;

    }


    public async Task<string> GetAudioFile(int domeId)
    {
        while (summitData.domes.Count==0)
        {
            await Task.Delay(1000);
        }
        for (int i = 0; i < summitData.domes.Count; i++)
        {
            if (domeId == summitData.domes[i].id)
            {
                return summitData.domes[i].bgm;
            }
        }

        return string.Empty;
    }

    public string[] GetDomeImage(int DomeId)
    {
        for (int i = 0; i < summitData.domes.Count; i++)
        {
            if (DomeId == summitData.domes[i].id)
            {
                if (LocalizationManager.forceJapanese || GameManager.currentLanguage == "ja")
                {
                    return new[] { summitData.domes[i].world360Image, summitData.domes[i].jpWorldName, summitData.domes[i].companyLogo };
                }
                else
                {
                    return new[] { summitData.domes[i].world360Image, summitData.domes[i].name, summitData.domes[i].companyLogo };
                }
            }
        }

        return new[] { string.Empty, string.Empty, string.Empty };
    }

    public DomeGeneralData GetDomeData(int DomeId)
    {
        for (int i = 0; i < summitData.domes.Count; i++)
        {
            if (DomeId == summitData.domes[i].id)
            {
                return summitData.domes[i];
            }
        }
        return null;
    }

    public async Task<int> GetVisitorCount(string worldId)
    {
        string apiUrl = ConstantsGod.API_BASEURL + ConstantsGod.VISITORCOUNT + worldId;
        string reponse = await GetTokenBasedAsyncRequest(apiUrl);
        VisitorInfo visitorInfo = JsonUtility.FromJson<VisitorInfo>(reponse);
        if (visitorInfo.success)
            return visitorInfo.data.total_visit;
        else
            return 100;
    }

#if UNITY_EDITOR
    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredPlayMode:
                summitData.domes.Clear();
                break;

            case PlayModeStateChange.ExitingPlayMode:
                summitData.domes.Clear();
                break;
        }
    }
#endif
    #region DomeInfo

    [System.Serializable]
    public class DomeData
    {
        public List<DomeGeneralData> domes;
    }

    [System.Serializable]
    public class SingleDomeData
    {
        public DomeGeneralData dome;
    }

    [System.Serializable]
    public class DomeGeneralData
    {
        public int id;
        public string name;
        public string description;
        public string creatorName;
        public string jpWorldName;
        public string jpDescription;
        public string jpCreatorName;
        public string bgm;
        public string thumbnail;
        public bool worldType;
        public int worldId;
        public string world;
        public string experienceType;
        public int builderWorldId;
        public bool IsPenguin;
        public bool Ishumanoid;
        public int AvatarIndex;
        public string Avatarjson;
        public string world360Image;
        public string companyLogo;
        public int maxPlayer;
        public bool is_penpenz;
        public List<SubWorldInfo> SubWorlds;
        public bool isSubWorld;
        public string domeType;
        public string domeCategory;
        public string mediaType;
        public string proportionType;
        public bool isYoutubeUrl;
        public string videoType;
        public string mediaUpload;
        public bool isBuilderGame;
        public List<InstructionData> instruction;
    }


    [System.Serializable]
    public class SubWorldInfo
    {
        public bool builderWorld;
        public bool officialWorld;
        public OfficialWorldDetails selectWorld;
        public string builderSubWorldId;
    }

    [System.Serializable]
    public class InstructionData
    {
        public string title_EN;
        public string title_JP;
        public string ImageLink;
        public string Desc_JP;
        public string Desc_EN;
    }


    [System.Serializable]
    public class OfficialWorldDetails
    {
        public int id;
        public string label;
        public string icon;
        public int userLimit;
        public string subWorldType;
        public string subWorldCategory;
        public string creatorName;
        public string description;
    }


    [System.Serializable]
    public class StackInfoWorld
    {
        public string id;
        public string name;
        public int user_limit;
        public string thumbnail;
        public string banner;
        public string thumbnail_new;
        public string description;
        public string creator;
        public bool isBuilderWorld;
        public int domeId;
        public bool haveSubWorlds;
        public bool isFromSummitWorld;
        public Vector3[] playerTrasnform;
    }
    #endregion



    #region AINPC Data Classes
    [System.Serializable]
    public class AIData
    {
        public List<AINPCInfo> npcData;
    }

    [System.Serializable]
    public class AINPCInfo
    {
        public int id;
        public int domeId;
        public string language;
        public string name;
        public string jpName;
        public int avatarId;
        public string avatarCategory;
        public string personalityURL;
        public float[] rotationPositionArray;
        public float[] spawnPositionArray;
        public bool isAvatarPerformer;
        public AnimationData[] animations;
    }
    [System.Serializable]
    public class AnimationData
    {
        public string name;
        public float playTime;
    }
    #endregion

    #region Visitor Count
    [System.Serializable]
    public class VisitorInfo
    {
        public bool success;
        public VisitorData data;
    }

    [System.Serializable]
    public class VisitorData
    {
        public int total_visit;
    }
    #endregion

}
