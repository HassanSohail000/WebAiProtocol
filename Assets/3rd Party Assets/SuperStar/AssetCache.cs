using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
//using SuperStar.Audio;
//using SuperStar.Helpers.UI;
//using SuperStar.Screens.MessageBox;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Collections;
using SuperStar.Api;
using UnityEngine.InputSystem;

namespace SuperStar.Helpers
{
    public static class ExpirationModes
    {
        public const int EXPIRATION_NEVER = -1;
        public const int EXPIRATION_NO_CACHE = -2; // risky to use, as it will always redownload an image on Enqueue...
        public const int EXPIRATION_DEFAULT = -3; // default, meaning Config('assets_expiration_default') will be used
    }

    class ActionSet
    {
        public UnityAction<bool> finishEvent;
        public UnityAction<string, int, int> progressEvent;
        public int count;
        public int progressCnt;
        public int allCnt;
        public int errorCount;

        public ActionSet()
        {
            count = 0;
            progressCnt = 0;
            progressEvent = null;
            finishEvent = null;
        }

        public void Consume()
        {
            count--;
            progressCnt++;
        }
    };

    public class Asset
    {
        public string srcUrl;
        public DateTime timeCreated;
        public int expirationDays; // default value
        public string forceLocalFileName = "M";
        public SpriteMeshType meshType = SpriteMeshType.FullRect;

        private Asset() { }

        static public Asset Create(string srcUrl, int expirationDays = ExpirationModes.EXPIRATION_DEFAULT, SpriteMeshType meshType = SpriteMeshType.FullRect, string forceLocalFileName = "M")
        {
            Asset a = new Asset();
            a.srcUrl = srcUrl;
            a.meshType = meshType;
            a.timeCreated = DateTime.Now;
            if (expirationDays == ExpirationModes.EXPIRATION_DEFAULT)
            {
                expirationDays = Config.GetInt("assets_expiration_default");
            }
            a.expirationDays = expirationDays;

            a._AdjustForThumbs();
            a.forceLocalFileName = forceLocalFileName;
            return a;
        }

        //static public Asset Create(string srcUrl, SpriteMeshType meshType = SpriteMeshType.FullRect, string forceLocalFileName = "M")
        //{
        //    return Create(srcUrl, ExpirationModes.EXPIRATION_DEFAULT, meshType, forceLocalFileName);
        //}

        private void _AdjustForThumbs()
        {
            if (string.IsNullOrEmpty(srcUrl) == false && (srcUrl.ToLower().EndsWith("thumb.jpg") || srcUrl.ToLower().EndsWith("thumb.png")))
            {
                expirationDays = Config.GetInt("assets_expiration_days_for_thumbs");
            }
        }
    }

    class LocalAsset
    {
        public string fileName;
        public DateTime timeCreated;
        public int expirationDays;
        public bool isAsStreamingAsset;
        public SpriteMeshType meshType = SpriteMeshType.FullRect;
    }

    class SpriteCacheItem
    {
        public Sprite sprite;
        public int refCnt;
        public SpriteCacheItem(Sprite sprite, int refCnt)
        {
            this.sprite = sprite;
            this.refCnt = refCnt;
        }
    }

    public class AssetCache : MonoBehaviour
    {
        private static AssetCache _instance;

        private static int globalSetCnt;

        private Dictionary<int, ActionSet> actionDict = new Dictionary<int, ActionSet>();

        private Dictionary<string, LocalAsset> items;
        private Dictionary<string, SpriteCacheItem> sprites;

        private static string BASE_PATH;
        private static string itemsDictFilePath;

        public static bool flagReloadAssets;

        public const int BYTEBUFFER_SIZE = 2 * 1024 * 1024;
        public static byte[] bytebuffer = new byte[BYTEBUFFER_SIZE];

        public static AssetCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("_AssetCache").AddComponent<AssetCache>();

                    DontDestroyOnLoad(_instance.gameObject);

                    _instance.Init();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            BASE_PATH = Application.persistentDataPath + "/res2/";
            itemsDictFilePath = BASE_PATH + "cached_files";
        }

        // retrieved cached sprite object!
        public Sprite LoadSprite(string key, Sprite placeholder = null)
        {
            if (sprites.ContainsKey(key))
            {
                sprites.TryGetValue(key, out SpriteCacheItem spriteCacheItem);

                if (spriteCacheItem.sprite == null) // this normally should not happen,.. but if it does, let's just reload it
                {
                    sprites.Remove(key);
                    return LoadSprite(key);
                }

                spriteCacheItem.refCnt++;
                return spriteCacheItem.sprite;
            }
            else
            {
                Texture2D texture = LoadImage(key);
                if (texture != null)
                {
                    try
                    {
                        LocalAsset la = GetValue(key);

                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100, 1, la.meshType);
                        sprites.Add(key, new SpriteCacheItem(sprite, 1));
                        return sprite;
                    }
                    catch (IOException e)
                    {
                        //ErrorReporting.SendSoftErrorInfo("IOException while reading image " + key, e.Message);
                        // invalid asset for some reason
                        DeleteAsset(key);
                        flagReloadAssets = true;
                    }
                    catch (Exception e)
                    {
                        //ErrorReporting.SendSoftErrorInfo("AssetCache:LoadImage(II)", ErrorReporting.BuildSoftError_ExceptionInfo(e) + "\nerror loading texture from key: " + key);
                        // invalid asset for some reason
                        DeleteAsset(key);
                        flagReloadAssets = true;
                    }
                }
                if (placeholder != null) return placeholder;
                return Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
            }
        }

        public TextAsset GetTextAsset(string key)
        {
            LocalAsset la = GetValue(key);

            if (la != null)
            {
                if (la.isAsStreamingAsset)
                {
                    return new TextAsset(BetterStreamingAssets.ReadAllText("GameAssets/" + la.fileName));
                }
                else
                {
                    string imageUrl = GetValue(key).fileName;
                    var fileName = GetLocalPathForFile(imageUrl);
                    if (File.Exists(fileName))
                    {
                        return new TextAsset(File.ReadAllText(fileName));
                    }
                }
            }
            return null;
        }

        public long GetAssetSize(string key)
        {
            LocalAsset la = GetValue(key);
            if (la != null)
            {
                if (la.isAsStreamingAsset)
                {
                    string fileName = GetLocalPathForFile(la.fileName);
                    return BetterStreamingAssets.ReadAllBytes("GameAssets/" + fileName).Length;
                }
                else
                {
                    string imageUrl = la.fileName;
                    string fileName = GetLocalPathForFile(imageUrl);
                    if (File.Exists(fileName))
                    {
                        long length = new System.IO.FileInfo(fileName).Length;
                        return length;
                    }
                }
            }
            return 0;
        }

        public Texture2D LoadImage(string key)
        {
            LocalAsset la = GetValue(key);
            if (la != null)
            {
                if (la.isAsStreamingAsset)
                {
                    Texture2D texture = null;
                    try
                    {
                        //string url = "file://" + Path.Combine(Path.Combine(Application.streamingAssetsPath, "GameAssets"), la.fileName);
                        texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                        byte[] bb = BetterStreamingAssets.ReadAllBytes("GameAssets/" + la.fileName);
                        texture.LoadImage(bb);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                    return texture;
                }
                else
                {
                    string imageUrl = GetValue(key).fileName;
                    var fileName = GetLocalPathForFile(imageUrl);
                    if (File.Exists(fileName))
                    {
                        byte[] bb = Utils.File_ReadAllBytes(fileName, bytebuffer);

                        Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                        // Texture2D texture = new Texture2D(1, 1);

                        try
                        {
                            //texture.LoadImage(bytes);
                            texture.LoadImage(bb);
                            // a hack -> red "?" bitmap is loaded, when data was corrupted
                            if ((texture.width == 8) && (texture.height == 8))
                            {
                               // ErrorReporting.SendSoftErrorInfo("AssetCache:LoadImage(III)", "error loading texture from file, key: " + key);
                                DeleteAsset(key);
                                AssetCache.flagReloadAssets = true;
                                return null;
                            }
                        }
                        catch (IOException e)
                        {
                         //   ErrorReporting.SendSoftErrorInfo("IOException while reading image", e.Message);
                            // invalid asset for some reason
                            DeleteAsset(key);
                            AssetCache.flagReloadAssets = true;
                        }
                        catch (Exception e)
                        {
                          //  ErrorReporting.SendSoftErrorInfo("AssetCache:LoadImage(II)", ErrorReporting.BuildSoftError_ExceptionInfo(e) + "\nerror loading texture from url: " + imageUrl);

                            // invalid asset for some reason
                            DeleteAsset(key);
                            AssetCache.flagReloadAssets = true;
                        }
                        //texture.Compress(true);
                        return texture;
                    }
                }
            }
            return null;
        }

        public void PlayAudio(string key, bool backgroundMusic = false, bool repeat = false)
        {
            if (backgroundMusic)
            {
                if (!string.IsNullOrEmpty(key))
                    StartCoroutine(PlayBGMusic(key));
            }
            else
            {
                StartCoroutine(PlayAudioCoroutine(key, repeat));
            }
        }

        public void RemoveAllFiles()
        {
            this.CancelInvoke();

            DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);

            FileInfo[] files = di.GetFiles();
            for (int x = 0; x < files.Length; x++)
            {
                files[x].Delete();
            }

            DirectoryInfo[] dirs = di.GetDirectories();
            for (int y = 0; y < dirs.Length; y++)
            {
                dirs[y].Delete(true);
            }

            items.Clear();
            sprites.Clear();
        }

        public void DeleteAsset(string key, bool saveDB = true)
        {
            string filePath = GetLocalPathForFile(GetValue(key).fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                items.Remove(key);
            }

            if (sprites.ContainsKey(key))
            {
                sprites.TryGetValue(key, out SpriteCacheItem spriteCacheItem);
                try
                {
                    Destroy(spriteCacheItem.sprite);
                }
                catch { }

                sprites.Remove(key);
            }

            // get rid of it here too
            items.Remove(key);
            if (saveDB)
                SaveDB();
        }

        public void EnqueueOneResAndWait(string key, string srcUrl, UnityAction<bool> finishEvent, int expirationDays = ExpirationModes.EXPIRATION_DEFAULT, string forceLocalFileName = "M")
        {
            Asset entry = Asset.Create(expirationDays: expirationDays, srcUrl: srcUrl, forceLocalFileName: forceLocalFileName);

            Dictionary<string, Asset> f = new Dictionary<string, Asset>
            {
                { key, entry }
            };
            EnqueueResAndWait(f, finishEvent);
        }

        public bool HasFile(string key)
        {
            LocalAsset la = GetValue(key);
            if (la != null)
            {
                if (la.isAsStreamingAsset)
                {
                    return true;
                }
                else
                {
                    string imageUrl = GetValue(key).fileName;
                    var fileName = GetLocalPathForFile(imageUrl);
                    if (File.Exists(fileName))
                    {
                        if (new FileInfo(fileName).Length == 0)
                            return false;
                        return true;
                    }
                }
            }
            return false;
        }

        public void EnqueueResAndWait(Dictionary<string, Asset> files, UnityAction<bool> finishEvent, UnityAction<string, int, int> progressEvent = null)
        {
            ActionSet as1 = new ActionSet();
            globalSetCnt++;
            int thisSetId = globalSetCnt;

            as1.finishEvent = finishEvent;
            as1.progressEvent = progressEvent;

            actionDict.Add(thisSetId, as1);

            var enumerator = files.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, Asset> entry = enumerator.Current;

                string fname = GetLocalFileName(entry.Value.srcUrl, entry.Value.forceLocalFileName);
                if (!HasUpdatedValue(entry.Key) || (!IsFileExistOnDisk(fname)))
                {
                    if (IsFileExistOnStreamingAssets(Path.GetFileName(entry.Value.srcUrl)))
                    {
                        if (items.ContainsKey(entry.Key) == false)
                            items.Add(entry.Key, new LocalAsset { fileName = Path.GetFileName(entry.Value.srcUrl), expirationDays = entry.Value.expirationDays, timeCreated = DateTime.Now, isAsStreamingAsset = true, meshType = entry.Value.meshType });
                    }
                    else
                    {
                        if (entry.Value.srcUrl.Length == 0)
                        {
                            Debug.Log("error -> not existing image: " + entry.Key);
                        }

                        // start coroutine to download it
                        as1.count++;
                        as1.allCnt++;
                        if (entry.Value.srcUrl.Length == 0) Debug.Log("ZERO resource: " + entry.Key);
                        StartCoroutine(LoadResourceRemote(thisSetId, entry.Key, entry.Value.srcUrl, entry.Value.forceLocalFileName, entry.Value.expirationDays, entry.Value.meshType));
                    }
                }
            }
            enumerator.Dispose();

            progressEvent?.Invoke("", 0, as1.allCnt);

            if (as1.count == 0)
            {
                actionDict.Remove(thisSetId);
                finishEvent(as1.errorCount == 0);
            }
        }

        private bool IsFileExistOnStreamingAssets(string fileName)
        {
            return false;
            //return string.IsNullOrEmpty(fileName) == false && BetterStreamingAssets.FileExists("GameAssets/" + fileName);
        }

        private bool IsFileExistOnDisk(string fileName)
        {
            string filePath = GetLocalPathForFile(fileName);
            return File.Exists(filePath);
        }

        private string GetLocalFileName(string url, string forceLocalName)
        {
            if (forceLocalName.Equals("M"))
            {
                return GenMD5(url);
            }
            return forceLocalName.Length > 0 ? forceLocalName : Path.GetFileName(url);
        }

        private string GetLocalPathForURL(string url)
        {
            return BASE_PATH + Path.GetFileName(url);
        }

        // in: filename from JSON
        private string GetLocalPathForFile(string filename)
        {
            return BASE_PATH + filename;
        }

        private IEnumerator LoadResourceRemote(int id, string key, string imageUrl, string forceLocalFileName, int _expirationDays, SpriteMeshType meshType)
        {
            UnityWebRequest www = UnityWebRequest.Get(imageUrl);
            www.SendWebRequest();

            while (!www.isDone)
            {
                yield return null;
            }

            if (!string.IsNullOrEmpty(www.error))
            {
                if (imageUrl.StartsWith("https://"))
                {
                    imageUrl = imageUrl.Remove(4, 1); // remove "s"
                    www = UnityWebRequest.Get(imageUrl);
                    www.SendWebRequest();
                    while (!www.isDone)
                    {
                        yield return null;
                    }
                }
            }

            int timeoutRetryCounter = 0;
            while (!string.IsNullOrEmpty(www.error) && (timeoutRetryCounter < 3))
            {
                if (www.error.Contains("timeout"))
                {
                    www = UnityWebRequest.Get(imageUrl);
                    www.SendWebRequest();
                    while (!www.isDone)
                    {
                        yield return null;
                    }
                }
                timeoutRetryCounter++;
            }

            if (!string.IsNullOrEmpty(www.error))
            {
                ActionSet setError;
                actionDict.TryGetValue(id, out setError);
                setError.Consume();
                setError.errorCount++;

                //InfoPanel.Instance.ShowMessage("Error loading for KEY : "+ key +" URL : "+imageUrl + " error Message : " + www.error);
                //ErrorReporting.SendAssetCacheError("Error loading asset: " + imageUrl, www.error);
                if (setError.count == 0)
                {
                    actionDict.Remove(id);
                    setError.finishEvent(setError.errorCount == 0);
                }
            }
            else
            {
                /*ErrorReporting.CheckDiskIfLow();*/

                string localFileName = GetLocalFileName(imageUrl, forceLocalFileName);
                try
                {
                    //Debug.LogError("Assetcache url path:" + imageUrl + "    :path:" + GetLocalPathForFile(localFileName) + "  :key:"+key);
                    File.WriteAllBytes(GetLocalPathForFile(localFileName), www.downloadHandler.data);
                }
                catch (Exception e)
                {
                    /*InfoPanel.Instance.ShowException(e, string.Format("LoadResourceRemote: id={0}, key={1}, url={2}", id, key, imageUrl));

                    if (Utils.IsDiskFull(e))
                    {
                        MessageBoxScreen.ShowDialogOK(Localization.LocalizationManager.Instance.GetValueForKey("ERROR"),
                                                      Localization.LocalizationManager.Instance.GetValueForKey("ERR_DISK_FULL"), null);
                    }*/
                }

                ActionSet set;
                actionDict.TryGetValue(id, out set);
                if (set == null)
                {
                    //here
                    set = null;
                }
                set.Consume();

                set.progressEvent?.Invoke(imageUrl, set.progressCnt, set.allCnt);

                if (items.ContainsKey(key))
                {
                    items.Remove(key);
                }
                items.Add(key, new LocalAsset { fileName = localFileName, expirationDays = _expirationDays, timeCreated = DateTime.Now, meshType = meshType });

                SaveDB();
                if (set.count == 0)
                {
                    actionDict.Remove(id);
                    set.finishEvent(set.errorCount == 0);
                }
            }

            www.Dispose();
        }

        private IEnumerator PlayAudioCoroutine(string key, bool repeat)
        {
            string fileLocal = GetValue(key).fileName;
            var fileName = "file://" + BASE_PATH + Path.GetFileName(fileLocal);

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileName, AudioType.MPEG))
            {
                yield return www.SendWebRequest();
                /*if (string.IsNullOrEmpty(www.error))
                    AudioManager.Instance.PlayExtraSoundClip(DownloadHandlerAudioClip.GetContent(www), repeat);
                else
                    Debug.Log("Error: " + www.error);*/
            }
        }

        private IEnumerator<UnityWebRequestAsyncOperation> PlayBGMusic(string key)
        {
            LocalAsset la = GetValue(key);
            if (la == null)
                yield break;

            var fileName = "file://" + BASE_PATH + Path.GetFileName(la.fileName);

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileName, AudioType.MPEG))
            {
                yield return www.SendWebRequest();
                /*if (string.IsNullOrEmpty(www.error))
                    AudioManager.Instance.PlayGamePlayMusic(0, DownloadHandlerAudioClip.GetContent(www));*/
            }
        }

        private LocalAsset GetValue(string key)
        {
            items.TryGetValue(key, out LocalAsset outv);
            return outv;
        }

        private bool HasUpdatedValue(string key)
        {
            items.TryGetValue(key, out LocalAsset la);
            if (la != null)
            {
                if (la.expirationDays == -2) // never cache
                {
                    return false;
                }
                if (la.expirationDays == -1 || ((DateTime.Now - la.timeCreated).TotalHours < la.expirationDays * 24))
                {
                    return true;
                }
            }
            return false;
        }

        private void SaveDB()
        {
            try
            {
                string serializedData = JsonConvert.SerializeObject(items, Formatting.None);

                // create directory if not created yet.
                if (!Directory.Exists(BASE_PATH))
                    Directory.CreateDirectory(BASE_PATH);
                using (var writer = new StreamWriter(File.Open(itemsDictFilePath, FileMode.Create)))
                {
                    writer.WriteLine(serializedData);
                }
            }
            catch (IOException e)
            {
                /*if (Utils.IsDiskFull(e))
                {
                    MessageBoxScreen.ShowDialogOK(Localization.LocalizationManager.Instance.GetValueForKey("ERROR"),
                                                  Localization.LocalizationManager.Instance.GetValueForKey("ERR_DISK_FULL"), null);
                }*/
            }
            catch { }
        }

        private void LoadDB()
        {
            try
            {
                // create directory if not created yet.
                if (!Directory.Exists(BASE_PATH))
                    Directory.CreateDirectory(BASE_PATH);
                using (var reader = new StreamReader(File.OpenRead(itemsDictFilePath)))
                {
                    string serializedData = reader.ReadLine();
                    items = JsonConvert.DeserializeObject<Dictionary<string, LocalAsset>>(serializedData);

                    /*// THIS is because in res2/cached_files we store this value,... and some old users might have "Tight" value there
                    DateTime dtCompare = new DateTime(2020, 10, 09);
                    foreach (LocalAsset value in items.Values)
                    {
                        if (value.timeCreated < dtCompare)
                        {
                            value.meshType = SpriteMeshType.FullRect;
                        }
                    }*/
                }
            }
            catch (Exception)
            { }
        }

        private void Init()
        {
            items = new Dictionary<string, LocalAsset>();
            sprites = new Dictionary<string, SpriteCacheItem>();
            LoadDB();
        }

        internal static bool CheckIfNotValidJSON(TextAsset skeletonJson)
        {
            if (skeletonJson.text.StartsWith("<"))
            {
                return true;
            }
            return false;
        }

        private AssetCache()
        {
        }

        private string GenMD5(string s)
        {
            using (var provider = System.Security.Cryptography.MD5.Create())
            {
                StringBuilder builder = new StringBuilder();

                byte[] b = provider.ComputeHash(Encoding.UTF8.GetBytes(s));
                for (int x = 0; x < b.Length; x++)
                    builder.Append(b[x].ToString("x2").ToLower());

                return builder.ToString();
            }
        }
        public void RemoveFromMemoryDelayCoroutine(string key, bool force)
        {
            StartCoroutine(RemoveFromMemoryDelay(key, force));
        }
        public IEnumerator RemoveFromMemoryDelay(string key, bool force)
        {
            yield return new WaitForEndOfFrame();
            if (sprites.ContainsKey(key))
            {
                sprites.TryGetValue(key, out SpriteCacheItem spriteCacheItem);
                spriteCacheItem.refCnt--;
                if ((spriteCacheItem.refCnt <= 0) || force)
                {
                    try
                    {
                        Destroy(spriteCacheItem.sprite.texture);
                    }
                    catch { }
                    try
                    {
                        Destroy(spriteCacheItem.sprite);
                    }
                    catch { }
                    sprites.Remove(key);
                }
            }
        }



        public void RemoveFromMemory(string key, bool force)
        {
            if (sprites.ContainsKey(key))
            {
                sprites.TryGetValue(key, out SpriteCacheItem spriteCacheItem);
                spriteCacheItem.refCnt--;
                if ((spriteCacheItem.refCnt <= 0) || force)
                {
                    try
                    {
                        Destroy(spriteCacheItem.sprite.texture);
                    }
                    catch { }
                    try
                    {
                        Destroy(spriteCacheItem.sprite);
                    }
                    catch { }
                    sprites.Remove(key);
                }
            }
        }

        public void RemoveLoadedImagesFromMemory()
        {
            while (sprites.Count > 0)
            {
                RemoveFromMemory(sprites.ElementAt(0).Key, true);
            }
        }


        internal void IncRefCntSprite(Sprite sprite)
        {
            var enum1 = sprites.GetEnumerator();
            while (enum1.MoveNext())
            {
                KeyValuePair<string, SpriteCacheItem> item = enum1.Current;
                if (item.Value.sprite == sprite)
                {
                    item.Value.refCnt++;
                }
            }
            enum1.Dispose();
        }

        internal void RemoveFromMemory(Sprite sprite)
        {
            if (sprite != null)
            {
                var enum1 = sprites.GetEnumerator();
                while (enum1.MoveNext())
                {
                    KeyValuePair<string, SpriteCacheItem> item = enum1.Current;
                    if (item.Value.sprite == sprite)
                    {
                        RemoveFromMemory(item.Key, false);
                        enum1.Dispose();
                        return;
                    }
                }
                enum1.Dispose();
            }
        }

        public void TotalDestruction()
        {
            while (sprites.Count > 0)
            {
                RemoveFromMemory(sprites.ElementAt(0).Key, true);
            }
            while (items.Count > 0)
            {
                DeleteAsset(items.ElementAt(0).Key, false);
            }
            SaveDB();
            //BASE_PATH = Application.persistentDataPath + "/res2/";
            //itemsDictFilePath = BASE_PATH + "cached_files";
        }

        public Sprite LoadSpriteIntoImage(UnityEngine.UI.Image img, string key, Sprite placeholder = null, bool changeAspectRatio = false)
        {
            try
            {
                Sprite sp = LoadSprite(key, placeholder);
                img.sprite = sp;
                if (changeAspectRatio && sp != null)
                {
                    img.GetComponent<UnityEngine.UI.AspectRatioFitter>().aspectRatio = (float)sp.texture.width / sp.texture.height;
                }
                return sp;
            }
            catch (System.Exception _)
            {
                return null;
            }
        }

        public Texture2D LoadTexture2DIntoRawImage(UnityEngine.UI.RawImage img, string key, bool changeAspectRatio = false)
        {
            try
            {
                Texture2D tex = LoadImage(key);
                img.texture = tex;
                if (changeAspectRatio && tex != null)
                {
                    img.GetComponent<UnityEngine.UI.AspectRatioFitter>().aspectRatio = (float)tex.width / tex.height;
                }
                return tex;
            }
            catch (System.Exception _)
            {
                return null;
            }
        }


        public void SaveImageEnqueueOneResAndWait(string key, string srcUrl, byte[] data, UnityAction<bool> finishEvent, int expirationDays = ExpirationModes.EXPIRATION_DEFAULT, string forceLocalFileName = "M")
        {
            Asset entry = Asset.Create(expirationDays: expirationDays, srcUrl: srcUrl, forceLocalFileName: forceLocalFileName);

            Dictionary<string, Asset> f = new Dictionary<string, Asset>
            {
                { key, entry }
            };
            SaveImageEnqueueResAndWait(data, f, finishEvent);
        }

        public void SaveImageEnqueueResAndWait(byte[] data, Dictionary<string, Asset> files, UnityAction<bool> finishEvent, UnityAction<string, int, int> progressEvent = null)
        {
            ActionSet as1 = new ActionSet();
            globalSetCnt++;
            int thisSetId = globalSetCnt;

            as1.finishEvent = finishEvent;
            as1.progressEvent = progressEvent;

            actionDict.Add(thisSetId, as1);

            var enumerator = files.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, Asset> entry = enumerator.Current;

                string fname = GetLocalFileName(entry.Value.srcUrl, entry.Value.forceLocalFileName);
                if (!HasUpdatedValue(entry.Key) || (!IsFileExistOnDisk(fname)))
                {
                    if (IsFileExistOnStreamingAssets(Path.GetFileName(entry.Value.srcUrl)))
                    {
                        if (items.ContainsKey(entry.Key) == false)
                            items.Add(entry.Key, new LocalAsset { fileName = Path.GetFileName(entry.Value.srcUrl), expirationDays = entry.Value.expirationDays, timeCreated = DateTime.Now, isAsStreamingAsset = true, meshType = entry.Value.meshType });
                    }
                    else
                    {
                        if (entry.Value.srcUrl.Length == 0)
                        {
                            Debug.Log("error -> not existing image: " + entry.Key);
                        }

                        // start coroutine to download it
                        as1.count++;
                        as1.allCnt++;
                        if (entry.Value.srcUrl.Length == 0) Debug.Log("ZERO resource: " + entry.Key);
                        SaveImageOnFile(data, entry.Key, entry.Value.srcUrl, entry.Value.forceLocalFileName, entry.Value.expirationDays, entry.Value.meshType);
                    }
                }
            }
            enumerator.Dispose();

            progressEvent?.Invoke("", 0, as1.allCnt);

            if (as1.count == 0)
            {
                actionDict.Remove(thisSetId);
                finishEvent(as1.errorCount == 0);
            }
            else
            {
                finishEvent(as1.errorCount == 0);
            }
        }

        public void SaveImageOnFile(byte[] data, string key, string imageUrl, string forceLocalFileName, int _expirationDays, SpriteMeshType meshType)
        {
            string localFileName = GetLocalFileName(imageUrl, forceLocalFileName);
            try
            {
                Debug.LogError("Assetcache SaveImageOnFile path:" + GetLocalPathForFile(localFileName));
                File.WriteAllBytes(GetLocalPathForFile(localFileName), data);
            }
            catch (Exception e)
            {
                Debug.LogError("SaveImageOnFile Field:" + e);
            }

            if (items.ContainsKey(key))
            {
                items.Remove(key);
            }
            items.Add(key, new LocalAsset { fileName = localFileName, expirationDays = _expirationDays, timeCreated = DateTime.Now, meshType = meshType });

            SaveDB();
        }
    }
}