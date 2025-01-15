using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace SuperStar.Api
{
    public class Config
    {
        /// <summary>
        /// GAME CONSTANTS READ FROM config.json
        /// </summary>
        /// 

        static Dictionary<string, string> jsonDict;

        public static bool IS_DEV_MODE
        {
            get
            {
                InitJsonDict();
                return GetBool("devMode");
            }
        }

        public static string GetString(string key)
        {
            InitJsonDict();
            return jsonDict.ContainsKey(key) ? jsonDict[key] : "";
        }

        public static int GetInt(string key)
        {
            InitJsonDict();
            return jsonDict.ContainsKey(key) ? int.Parse(jsonDict[key]) : 0;
        }

        public static bool GetBool(string key)
        {
            InitJsonDict();
            return jsonDict.ContainsKey(key) && bool.Parse(jsonDict[key]);
        }

        public static float GetFloat(string key)
        {
            InitJsonDict();
            return jsonDict.ContainsKey(key) ? float.Parse(jsonDict[key], CultureInfo.GetCultureInfo("en-US")) : 0.0f;
        }

        public static Color GetColor(string key)
        {
            string colorStr = GetString(key);
            if (!colorStr.StartsWith("#", System.StringComparison.Ordinal)) colorStr = "#" + colorStr;
            ColorUtility.TryParseHtmlString(colorStr, out Color color);
            return color;
        }


        public static float GetNPCTypingTime(string text)
        {
            return 0.5f + text.Length / 20f * 0.5f;
            //     ^^ const delay      +       ^^ text length dependent delay
        }

        public static float GetMeOrNarrativeReadText(string text)
        {
            return 0.5f + text.Length / 80f * 0.5f;
            //     ^^ const delay      +       ^^ text length dependent delay
        }

        public static void InitJsonDict()
        {
            if (jsonDict == null)
            {
                jsonDict = new Dictionary<string, string>
                {
#if PRODUCTION_BUILD 
                    { "apiUrlBase",                          "https://api-stories-elb.tictales.com/swiitromance/native/" },
                    { "apiUrlPayement",                      "https://api-stories-elb.tictales.com/swiitromance/native/" },  
                    { "apiUrlReporting",                     "https://api-stories-elb.tictales.com/swiitromance/native/" },

                    { "devMode",                             "false" },
#else
                    { "apiUrlBase",                          "https://api-front-app.tictales.com/swiitromance/native/" },
                    { "apiUrlPayement",                      "https://api-front-app.tictales.com/swiitromance/native/" },
                    { "apiUrlReporting",                     "https://api-front-app.tictales.com/swiitromance/native/" },

                    { "devMode",                             "true" },
#endif

                    { "screenTransitionAnim",               "false" },

                    { "retryDelay", "5" },
                    { "buildVersion", Application.version },

                    // Customizable by project
                    { "appleID", "1569601713" },
                    { "androidStorePage", "https://play.google.com/store/apps/dev?id=8584055574805329687" },
                    { "iOSStorePage", "https://apps.apple.com/us/developer/tictales/id1021093408" },
                    { "otherStorePage", "https://play.google.com/store/apps/dev?id=8584055574805329687" },

                    { "oneSignalAppId", "9012b9f0-3b93-431b-8121-1e6e3a58aeaf" },

                    { "adjust_appKey", "ba9526v0vf9c" },
                    { "initiate_checkout", "7kuc2b" },
                    { "purchase_cancelled", "9yacw3" },

                    // The field below is entered automatically, do not modify this line manually
                    { "localizationInStreamingAssets",       "2019-08-25 23:20:00" },

                    // MOVE TO BACKEND API
                    { "assets_expiration_default",           "30" },
                    { "assets_expiration_days_game_assets",  "3" },
                    { "assets_expiration_days_ui",           "1" }, // short therm updatable UI images
                    { "assets_expiration_days_for_thumbs",   "7" },

                    { "termsOfService",                      "https://www.tictales.studio/terms-and-conditions.html"},
                    { "privacyPolicy",                       "https://www.tictales.studio/privacy-policy.html"},
                    { "enableGDPR",                          "false" },

                    { "MopubInterstital_android",            "201010b3c08c44568f09622837f27925" },
                    { "MopubInterstital_ios",                "ded71750605a47fdb734d5f402d4a50b" },
                    { "MopubRewardedVideo_android",          "df7ce2ecc006411285f81b4a125c1623" },
                    { "MopubRewardedVideo_ios",              "f23a72c4edbb45afbdbf19fc614aedd3" },
                    { "MopubBanner_android",                 "" },
                    { "MopubBanner_ios",                     "" },


                    { "enableAutoplay",                      "false" },
                    { "gamePlay-inGameShopping",             "false"},
                    { "GDPR-enableRefuse",                   "false" },

                    // ALL THOSE DATA NEED TO COME FROM BACKEND ON AUTH/JOIN 
                    // We need to check togetther if keep or not
                    //{ "suscribtionUpdateEnable",             "true" },

                    // NEED TO CHECK THIS TO CLEAN 
                    //{ "menu_pictures",                       "true" },
                    //{ "menu_skins",                          "true" },
                    //{ "rateUsSpendingThreshold",             "2000" },
                    //{ "enableDressup",                       "true" },
                    //{ "ratingScreen-enableComments",         "true" },

                    //{ "ui-popupTitleFontSize",               "50" },
                    //{ "ui-tapBarIconColor",                  "bcbcbc" },
                    //{ "ui-tapBarIconColorSelected",          "FF0000" },
                    //{ "ui-tapBarBorderColor",                "EBEDF2" },
                    //{ "ui-topBarFontColor",                  "FFFFFF" },
                    //{ "ui-topBarIconColor",                  "bcbcbc" },
                    //{ "ui-topBarColor",                      "004C68" },
                    //{ "ui-logoColor",                        "B2CBFF" },
                    //{ "ui-panelContentBg",                   "FAFAFA" },
                    //{ "ui-btnBgColor",                       "FFFFFF" },
                    //{ "ui-color",                            "FF9200" },
                    //{ "ui-greyScale",                        "DDDDDD" },
                    //{ "ui-homescreenBtnColor",               "FFFFFF" },
                    //{ "ui-categoryTitleColor",               "676767" },
                    //{ "ui-bgcolor",                          "FFFFFF" },
                    //{ "ui-titleTextColor",                   "676767" },
                    //{ "ui-underlineColor",                   "CCCCCC" },

                    { "gamePlay-textTyperEnabled",           "false" },
                    { "gamePlay-textBubbleFadeIn",           "0" },
                    { "gamePlay-textBubbleFadeOut",          "0" },
                    { "gamePlay-typingSpeed",                "0" },   //time in seconds
                    { "gamePlay-inputDialogOKButton",        "FFFFFF" },
                    { "gamePlay-smsIn-labelHeader",          "999999" },
                    { "gamePlay-smsOut-labelHeader",         "999999" },
                    { "gamePlay-answer-background",          "870E0E" },
                    { "gamePlay-paidAnswer-background",      "ea8800" },
                    { "gamePlay-labelMe",                    "true" },
                    { "gamePlay-roundedBubbles",             "false" },
                    { "gamePlay-headerTextSkewAngles",       "0" },
                    { "gamePlay-animationsEnable",           "false" },    // true, if camera/actor animations are executed
                    { "gamePlay-localSaveGame",              "true" },
                    { "gamePlay-bubbleFontSize",             "42" },
                    { "gamePlay-tapDelayOnUser",             "0" },
                    { "gamePlay-tapDelayOnCollection",       "0" },
                    { "gamePlay-avatarsInSmsMode",           "false" },
                    { "gamePlayRoom-bubbleFontSize",         "42" },

                    { "gamePlay-handTapDelay",               "0" }, // in seconds

                    { "chatPlay-answersFadeIn",           "0" },
                    { "chatPlay-answersFadeOut",          "0" },

                    { "chatMode-smsOut-labelHeaderColor", "999999" }
                };
            }
        }

        public static string[] debugConsoleIdsAllowed =
        {
            "af265c94b4b62564e2c3136ad5beb50f763c42df", // Filip's Unity Editor
            "beb19e306f2840e4981549b8a6557435",         // Filip's phone

            "21a3c46b437185d82bbb81120be74a7d", // Christophe phone
        };


    }
}