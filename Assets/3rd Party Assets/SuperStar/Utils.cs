using System.IO;
using SuperStar.Api;
using UnityEngine;
using UnityEngine.UI;

namespace SuperStar.Helpers
{
    public static class Utils
    {       
        public static bool IsDiskFull(System.Exception ex)
        {
            const int HR_ERROR_HANDLE_DISK_FULL = unchecked((int)0x80070027);
            const int HR_ERROR_DISK_FULL = unchecked((int)0x80070070);

            return ex.HResult == HR_ERROR_HANDLE_DISK_FULL
                || ex.HResult == HR_ERROR_DISK_FULL;
        }

        public static string ConvertSecondsToTimeString(float seconds)
        {
            System.TimeSpan t = System.TimeSpan.FromSeconds(seconds);
            return string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
        }

        public static string GetRemainingTime(float seconds)
        {
            System.TimeSpan t = System.DateTime.Now.AddSeconds(seconds) - System.DateTime.Now;
            return string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
        }

        public static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        //Email Validation
        public static bool IsEmailValid(string inputEmail, ref string errorMessage)
        {
            return inputEmail.Length > 0;
            //string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            //    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            //    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            //System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(strRegex);

            //return re.IsMatch(inputEmail);
        }

        //Password Validation
        public static bool IsPasswordValid(string inputPassword, ref string errorMessage)
        {
            return inputPassword.Length > 0;
        }

        public static void EnableDisableGrayscale(bool value, Image[] images, Color[] originalImageColors)
        {
            for (int i = 0; i < images.Length; i++)
            {
                EnableDisableGrayscale(value, images[i], originalImageColors[i]);
            }
        }

        public static void EnableDisableGrayscale(bool value, Image image, Color originalImageColor)
        {
            if (value)
            {
                image.color = new Color(0.4f, 0.4f, 0.4f);
                image.material = Resources.Load<Material>("GrayScale");
            }
            else
            {
                image.material = null;
                image.color = originalImageColor;
            }
        }

        public static void EnableDisableGrayscale(bool value, Text[] texts, Color[] originalTextColors)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                EnableDisableGrayscale(value, texts[i], originalTextColors[i]);
            }
        }

        public static void EnableDisableGrayscale(bool value, Text text, Color originalTextColor)
        {
            if (value)
            {
                text.color = new Color(0.4f, 0.4f, 0.4f);
                text.material = Resources.Load<Material>("GrayScale");
            }
            else
            {
                text.color = originalTextColor;
                text.material = null;
            }
        }

        public static string GetStoreLink()
        {
            string link = ""; // Default link

#if UNITY_EDITOR
            link = "https://play.google.com/store/apps/details?id=" + Application.identifier;
#elif UNITY_ANDROID
            link = "market://details?id=" + Application.identifier;
#elif UNITY_IOS
            link = "itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=" + Config.GetString("appleID");
#elif UNITY_FACEBOOK
            link = "https://play.google.com/store/apps/details?id=" + Application.identifier;
#endif

            return link;
        }

        public static bool IsLocaleAvailable()
        {
            return PlayerPrefs.HasKey("_locale");
        }

        public static void SetLocale(SystemLanguage language)
        {
            SetLocale(GetLocale(language));
            SetLocaleID(language);
        }

        public static void SetLocale(string code)
        {
            PlayerPrefs.SetString("_locale", code);
            SetLocaleID(GetLanguage(code));
        }

        public static string GetLocale()
        {
            return PlayerPrefs.HasKey("_locale") ? PlayerPrefs.GetString("_locale", "en-US") : GetLocale(Application.systemLanguage);
        }

        public static string GetLocale(SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.French:
                    return "fr-FR";
                case SystemLanguage.English:
                    return "en-US";
                case SystemLanguage.Spanish:
                    return "es-ES";
                //case SystemLanguage.Japanese:
                //    return "ja-JP";
                case SystemLanguage.Portuguese:
                    return "pt-PT";
                case SystemLanguage.Russian:
                    return "ru-RU";
                case SystemLanguage.German:
                    return "de-DE";
                case SystemLanguage.Italian:
                    return "it-IT";
                //case SystemLanguage.Polish:
                //    return "pl-PL";
            }
            return "en-US";
        }

        public static string GetLanguageName(string locale)
        {
            return GetLanguage(locale).ToString();
        }

        public static int GetLocaleID()
        {
            return PlayerPrefs.GetInt("_localeId", 1);
        }

        private static void SetLocaleID(SystemLanguage language)
        {
            int code = 1;
            switch (language)
            {
                case SystemLanguage.English:
                    code = 1;
                    break;
                case SystemLanguage.French:
                    code = 2;
                    break;
                case SystemLanguage.German:
                    code = 3;
                    break;
                case SystemLanguage.Spanish:
                    code = 4;
                    break;
                case SystemLanguage.Italian:
                    code = 5;
                    break;
                case SystemLanguage.Portuguese:
                    code = 6;
                    break;
                case SystemLanguage.Russian:
                    code = 7;
                    break;
            }
            PlayerPrefs.SetInt("_localeId", code);
        }

        private static SystemLanguage GetLanguage(string locale)
        {
            switch (locale)
            {
                case "fr-FR":
                    return SystemLanguage.French;
                case "en-US":
                    return SystemLanguage.English;
                case "es-ES":
                    return SystemLanguage.Spanish;
                // case "ja-JP":
                //    return SystemLanguage.Japanese;
                case "pt-PT":
                    return SystemLanguage.Portuguese;
                case "pt-BR":
                    return SystemLanguage.Portuguese;    
                case "ru-RU":
                    return SystemLanguage.Russian;
                case "de-DE":
                    return SystemLanguage.German;
                case "it-IT":
                    return SystemLanguage.Italian;
                // case "pl-PL":
                //    return SystemLanguage.Polish;
            }
            return SystemLanguage.English;
        }

        public static GameObject Instantiate(GameObject prefab, Transform parent = null, int siblingIndex = -1)
        {
            GameObject gameObject;
            if (parent != null)
                gameObject = GameObject.Instantiate(prefab, parent);
            else
                gameObject = GameObject.Instantiate(prefab);

            if (siblingIndex != -1)
                gameObject.transform.SetSiblingIndex(siblingIndex);

            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;

            return gameObject;
        }

        public static void RemoveAllChildren(Transform parent)
        {
            // old foreach way:
            foreach (Transform child in parent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static string UrlAsKey(string file)
        {
            if (file.StartsWith("http://")) file = file.Substring(7);
            if (file.StartsWith("https://")) file = file.Substring(8);
            return file.Replace("/", "_");
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static Color GetColor(string colorStr)
        {
            if (!string.IsNullOrWhiteSpace(colorStr))
            {
                if (!colorStr.StartsWith("#", System.StringComparison.Ordinal)) colorStr = "#" + colorStr;
                return ColorUtility.TryParseHtmlString(colorStr, out Color color) ? color : Color.gray;
            }
            return Color.gray;
        }

        public static bool IsNotchExists()
        {
            if (SystemInfo.deviceModel.IndexOf("iPhone") > -1)
            {
                float screenRatio = (1.0f * Screen.width) / (1.0f * Screen.height);
                if (screenRatio >= 2.1 && screenRatio <= 2.2)
                {
                    return true;
                }
            }
            return false;
        }

        public static string RemoveHTMLTagsFromText(string s)
        {
            int j1, j2;
            do
            {
                j1 = s.IndexOf("<span");
                if (j1 >= 0)
                {
                    j2 = s.IndexOf(">", j1);
                    if (j2 >= 0)
                    {
                        s = s.Substring(0, j1) + s.Substring(j2 + 1);
                    }
                }
            } while (j1 >= 0);
            do
            {
                j1 = s.IndexOf("<div");
                if (j1 >= 0)
                {
                    j2 = s.IndexOf(">", j1);
                    if (j2 >= 0)
                    {
                        s = s.Substring(0, j1) + s.Substring(j2 + 1);
                    }
                }
            } while (j1 >= 0);
            s = s.Replace("</span>", "");
            s = s.Replace("</div>", "");
            return s;
        }

        public static string GetStoreLinkForFB()
        {
#if UNITY_ANDROID
            string link = Config.GetString("androidStorePage");
#elif UNITY_IOS
            string link = Config.GetString("iOSStorePage");
#else
             string link = Config.GetString("otherStorePage");
#endif
            return link;
        }

        public static GameObject CreateAd(GameObject prefab, Transform parent, int siblingIndex)
        {
            GameObject nativeAdGO = GameObject.Instantiate(prefab);
            nativeAdGO.transform.SetParent(parent);
            nativeAdGO.transform.SetSiblingIndex(siblingIndex);
            nativeAdGO.transform.localScale = Vector3.one;
            nativeAdGO.transform.position = Vector3.zero;
            nativeAdGO.transform.localPosition = Vector3.zero;

            return nativeAdGO;
        }

        public static byte[] File_ReadAllBytes(string path, byte[] buffer)
        {
        
            long fileLen = new FileInfo(path).Length;
            using (Stream source = File.OpenRead(path))
            {
                int pos = 0;
                int bytesRead;

                bytesRead = source.Read(buffer, pos, buffer.Length - pos);
                if (bytesRead < fileLen)
                {
                    return File.ReadAllBytes(path);
                }
                pos += bytesRead;
                while (pos < buffer.Length)
                {
                    buffer[pos++] = 0;
                }
                return buffer;
            }
        }
    }
}