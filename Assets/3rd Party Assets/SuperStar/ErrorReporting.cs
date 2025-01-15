//using System.Text;
////using Assets.Scripts.GamePlay;
//using BestHTTP;
////using SimpleDiskUtils;
//using SuperStar.Api;
//using SuperStar.ClientServerCommunication;
////using Tictales.Localization;
////using Tictales.Screens.MessageBox;
//using UnityEngine;

//namespace SuperStar.Helpers
//{
//    public static class ErrorReporting
//    {
//        readonly static string NEW_LINE = "\n";

//        public static void SendMediationException(string failureMessage, string adType)
//        {
//            string str = "***Mediation Exception***" + NEW_LINE + "Failure Reason: " + failureMessage + NEW_LINE;

//            str += "Ad Type: " + adType + NEW_LINE;
//        }

//        public static void SendVideoFailure(string failureMessage)
//        {
//            string str = "Video Failure Reason: " + failureMessage + NEW_LINE;
//        }

//        public static void SendAssetCacheError(string errorType, string exceptionMessage)
//        {
//            string str = "";
//            str += "Resulting Exception: " + NEW_LINE;
//            str += (exceptionMessage ?? "null") + NEW_LINE;
//            str += "Error Type: " + errorType + NEW_LINE;
//        }

//        public static void Send(CSRequestHelper currentRequest, HTTPRequest req, HTTPResponse resp, string errorType)
//        {
//            if (req.Exception != null)
//            {
//                errorType += NEW_LINE + "HTTPRequest.Exception: " + req.Exception.Message;
//            }

//            Send(currentRequest, (resp != null) ? resp.StatusCode : 0, errorType, (resp != null) ? resp.DataAsText : "null");
//        }

//        public static void Send(CSRequestHelper currentRequest, long nStatusCode, string errorType, string stringData)
//        {
//            string str = "";
//            str += "URL: " + currentRequest.Uri + NEW_LINE;
//            str += "BODY: " + currentRequest.BodyString + NEW_LINE;
//            str += "METHOD: " + currentRequest.Method + NEW_LINE;
//            str += "Status Code: " + nStatusCode + NEW_LINE;
//            str += "Error Type: " + errorType + NEW_LINE;

//            str += "Data: " + NEW_LINE;
//            str += (stringData ?? "null") + NEW_LINE;

//            Send(str);
//        }

//        public static void Send(string msg)
//        {
//#if UNITY_EDITOR
//            if ("af265c94b4b62564e2c3136ad5beb50f763c42df".Equals(SystemInfo.deviceUniqueIdentifier))
//            {
//                msg = "## FILIP's Unity Editor ##\n" + msg;
//            }
//            else if ("03FC5894-4E77-51D8-AE3F-BFBBE400C561".Equals(SystemInfo.deviceUniqueIdentifier))
//            {
//                msg = "## SYED's Unity Editor ##\n" + msg;
//            }
//            else
//            {
//                msg = "## Unity Editor ##\n" + msg;
//            }
//#endif

//            msg += NEW_LINE + NEW_LINE + "App Version: " + Application.version + NEW_LINE;
//            msg += "Device: " + SystemInfo.deviceModel + NEW_LINE;
//            msg += "OS Version: " + SystemInfo.operatingSystem + NEW_LINE;
//            msg += NEW_LINE + "buildVersion: " + Config.GetString("buildVersion");

//            /*if (GameManager.Instance.GetGameData().User != null)
//            {
//                msg += NEW_LINE + NEW_LINE + "User ID: " + GameManager.Instance.GetGameData().User.UserID + NEW_LINE;
//                msg += "UDID: " + GameManager.Instance.GetGameData().User.UDID + NEW_LINE;
//                msg += "User Token: " + GameManager.Instance.GetGameData().User.AuthToken;
//            }*/

//            ErrorReport error = new ErrorReport
//            {
//                message = msg
//            };
//            string errorJson = JsonUtility.ToJson(error, false);

//            HTTPRequest bestHttpRequest = new HTTPRequest(new System.Uri(RestApi.PostAPI_ReportError()), HTTPMethods.Post, (req, resp) => { });
            
//            Debug.Log("Error Message posting to server = " + errorJson);

//            bestHttpRequest.RawData = Encoding.UTF8.GetBytes(errorJson);

//            bestHttpRequest.SetHeader("Content-Type", "application/json; charset=UTF-8");
//           /* if (GameManager.Instance != null && GameManager.Instance.GetGameData() != null && GameManager.Instance.GetGameData().User != null && !string.IsNullOrEmpty(GameManager.Instance.GetGameData().User.AuthToken))
//            {
//                bestHttpRequest.AddHeader("Authorization", "Bearer " + GameManager.Instance.GetGameData().User.AuthToken);
//            }*/

//            bestHttpRequest.Send();
//        }

//        [System.Serializable]
//        class ErrorReport
//        {
//            public string message;
//        }

//        /*public static string BuildSoftError_GameInfo(GameDataWrapper.InGameWrapperState inGameState)
//        {
//            string s;
//            s = "Game ID: " + inGameState.game_id + " Chapter ID: " + inGameState.chapter_id + " Chapter Slug: " + inGameState.chapter_slug + "\n";
//            s += "Seq: " + inGameState.sequence_no + " Plan: " + inGameState.plan_no + " Dialog: " + inGameState.dialog_no + "\n";
//            return s;
//        }*/

//        public static string BuildSoftError_ExceptionInfo(System.Exception e)
//        {
//            return "Exception: " + e.Message + "\nStacktrace: " + e.StackTrace + "\n";
//        }

//        public static void SendSoftErrorInfo(string error, string message)
//        {
//            //Send("Try/Catch error: " + error + "\n" + message);
//        }

//        public static void CheckDiskIfLow()
//        {
//            try
//            {
//                //long lAvailableMegaBytesOnDisk = DiskUtils.CheckAvailableSpace();
//               /* if (lAvailableMegaBytesOnDisk < 10) // if below 10MB - warn
//                {*/
//                    /*MessageBoxScreen.ShowDialogYesNo(
//                        LocalizationManager.Instance.GetValueForKey("DISK_SPACE_ALERT_TITLE"),
//                        LocalizationManager.Instance.GetValueForKey("DISK_SPACE_ALERT_TEXT"), btnResult =>
//                        {
//                            if (btnResult == MessageBoxScreen.DIALOG_OK)
//                                Application.Quit(0);
//                        });*/
//               // }
//            }
//            catch (System.Exception)
//            {
//                // it crashes rarely on some Android devices
//            }
//        }
//    }
//}