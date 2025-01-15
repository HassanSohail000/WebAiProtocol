using System.Runtime.InteropServices;
using SuperStar.Helpers;

namespace SuperStar.Api
{
    class RestApi
    {
        public static string API_BASE_URL
        {
            get
            {
                return Config.GetString("apiUrlBase");
            }
        }

        private static string API_PAYMENT_BASE_URL
        {
            get
            {
                return Config.GetString("apiUrlPayement");
            }
        }

        private static string API_REPORTING_BASE_URL
        {
            get
            {
                return Config.GetString("apiUrlReporting");
            }
        }

        public static string GetPlatform()
        {
#if UNITY_ANDROID
            return "android";
#elif UNITY_IOS
            return "ios";
#elif UNITY_FACEBOOK
            return "fbGameroom";
#else
            return "unknown";
#endif
        }

        public static string GetAPI_DownloadLocalizationJson()
        {
            string locale = Utils.GetLocale();
            return API_BASE_URL + "translations/i18n/files/" + locale;
        }

        public static string PutAPI_AssignFB()
        {
            return API_BASE_URL + "users/current/assign/fb";
        }

        public static string PutAPI_LoginApple()
        {
            return API_BASE_URL + "users/current/assign/apple";
        }

        public static string PutAPI_UserProfile()
        {
            return API_BASE_URL + "users/current/profile";
        }

        public static string PutAPI_UserCurrentLanguageChanged()
        {
            return API_BASE_URL + "users/current/language";
        }

        public static string PostAPI_Auth()
        {
            return API_BASE_URL + "auth/join";
        }

        public static string DeleteAPI_RemoveAccount()
        {
            return API_BASE_URL + "users/current/remove";
        }

        public static string PostAPI_ValidatePurchase()
        {
            return API_PAYMENT_BASE_URL + "purchases";
        }

        public static string PostAPI_ReportSupport()
        {
            return API_BASE_URL + "support/conversations";
        }

        public static string PutAPI_StartStory(int storyId)
        {
            return API_BASE_URL + "story/" + storyId + "/play";
        }

        public static string PutAPI_SaveStory(int storyId, int leave)
        {
            return API_BASE_URL + "story/" + storyId + "/save?leave=" + leave;
        }

        public static string GetAPI_GetMatchForPreviousChapter(int matchId, int chapterId)
        {
            return API_BASE_URL + "games/matches/" + matchId + "/chapters/" + chapterId + "/history";
        }

        public static string PutAPI_RestartStory(int storyId)
        {
            return API_BASE_URL + "story/" + storyId + "/restart?animated=actors";
        }

        public static string PutAPI_RestartChapter(int storyId, int chapterId)
        {
            return API_BASE_URL + "story/" + storyId + "/chapters/" + chapterId + "/restart?animated=actors";
        }

        public static string PutAPI_ChooseStoryRush(int storyId, int chapterId)
        {
            return API_BASE_URL + "story/" + storyId + "/chapters/" + chapterId + "/rush";
        }


        public static string PutAPI_ChapterUnlockChoice(int storyId, int chapterId)
        {
            return API_BASE_URL + "story/" + storyId + "/chapters/" + chapterId + "/unlock/choice";
        }

        public static string PostAPI_ReportError()
        {
            return API_REPORTING_BASE_URL + "logs/track/critical";
        }

        public static string PostAPI_UnlockCollectionSwiit(int storyId, int chapterId)
        {
            return API_BASE_URL + "story/" + storyId + "/chapters/" + chapterId + "/unlock/media";
        }

		public static string GetAPI_RewardedBackFill()
        {
            return API_BASE_URL + "ad/rewarded/backfill" + "?os=" + GetPlatform();
        }
		        
        public static string PostAPI_RewardBonus()
        {
            return API_BASE_URL + "rewards/daily";
        }

        public static string PostAPI_PromoCode()
        {
            return API_BASE_URL + "rewards/promoteCode";
        }

        public static string PutAPI_ClaimOfferGift()
        {
            return API_BASE_URL + "rewards/daily/bank?os=" + GetPlatform();
        }

        public static string PostAPI_DailyReward()
        {
            return API_BASE_URL + "rewards/store/daily";
        }

        public static string PostAPI_VideoReward()
        {
            return API_BASE_URL + "rewards/store/video";
        }

        public static string PostAPI_LogPurchaseState()
        {
            return API_BASE_URL + "logs/purchases/step";
        }

        public static string GetAPI_Rankings()
        {
            return API_BASE_URL + "rankings";
        }

        public static string GetAPI_Quests()
        {
            return API_BASE_URL + "quests";
        }

        public static string GetAPI_Mailbox()
        {
            return API_BASE_URL + "mailbox";
        }

        public static string GetAPI_Notification(string id)
        {
            return API_BASE_URL + "notification/open/" + id;
        }

        public static string PutAPI_bookmark(int bookId)
        {
            return API_BASE_URL + "story/" + bookId + "/mark";
        }


        public static string PostAPI_Share()
        {
            return API_BASE_URL + "share";
        }






        // Remove all those references 


        public static string PostAPI_PurchaseDressupItem()
        {
            return API_BASE_URL + "store/me";
        }

        public static string PutAPI_SaveDressupItemState()
        {
            return API_BASE_URL + "store/me/1";
        }

        public static string PostAPI_UnlockCollection(int storyId)
        {
            return API_BASE_URL + "users/current/games/" + storyId + "/collection";
        }


        public static string PutAPI_SendGDPRConsent()
        {
            return API_BASE_URL + "users/current/gdpr";
        }

        public static string PutAPI_GetRoomStory(int roomId)
        {
            return API_BASE_URL + "users/current/home/rooms/" + roomId + "/story";
        }

        public static string PutAPI_BoyfriendSelect()
        {
            return API_BASE_URL + "users/current/boyfriends/select?os=" + GetPlatform();
        }

        public static string PutAPI_BoyfriendLevelUp()
        {
            return API_BASE_URL + "users/current/boyfriends/levelup?os=" + GetPlatform();
        }

        public static string PutAPI_DrawCard()
        {
            return API_BASE_URL + "users/current/boyfriends/chest?os=" + GetPlatform();
        }

        public static string GetAPI_FetchAvatarData()
        {
            return API_BASE_URL + "store";
        }

        public static string GetAPI_FetchRoomData()
        {
            return API_BASE_URL + "users/current/home/rooms";
        }
        

        public static string PostAPI_UnlockFurniture(int roomId)
        {
            return API_BASE_URL + "users/current/home/rooms/" + roomId + "/furnitures";
        }

        public static string PostAPI_UnlockRoom()
        {
            return API_BASE_URL + "users/current/home/rooms";
        }


        //public static string PutAPI_StartEvent(int eventId)
        //{
        //    return API_BASE_URL + "games/events/" + eventId + "/play";
        //}

        //public static string PutAPI_PutUpdateMatchGameData(int matchId, int chapterId, int leave)
        //{
        //    return API_BASE_URL + "games/matches/" + matchId + "/chapters/" + chapterId + "/save?leave=" + leave;
        //}

        //public static string PutAPI_ChapterUnlockMatchChoice(int matchId, int chapterId)
        //{
        //    return API_BASE_URL + "games/matches/" + matchId + "/chapters/" + chapterId + "/unlock/choice";
        //}

        //public static string PutAPI_PutUpdateEventGameData(int eventId, int chapterId, int leave)
        //{
        //    return API_BASE_URL + "games/events/" + eventId + "/save?leave=" + leave;
        //}

        //public static string PutAPI_ChapterUnlockEventChoice(int eventId, int chapterId)
        //{
        //    return API_BASE_URL + "games/events/" + eventId + "/chapters/" + chapterId + "/unlock/choice";
        //}

                //    public static string GetAPI_UserGameCollection()
        //    {
        //        return API_BASE_URL + "users/current/collections";
        //    }

        //public static string PostAPI_UnlockCollectionSwiit_Match(int matchId, int chapterId)
        //{
        //    return API_BASE_URL + "games/matches/" + matchId + "/chapters/" + chapterId + "/unlock/media";
        //}

        //public static string PostAPI_UnlockCollectionSwiit_Event(int matchId, int chapterId)
        //{
        //    return API_BASE_URL + "games/events/" + matchId + "/chapters/" + chapterId + "/unlock/media";
        //}

        /*

                public static string PostAPI_UnlockSkin(int skinId)
                {
                    return API_BASE_URL + "skins/" +  skinId + "/unlock";
                }

                public static string PutAPI_EnableSkin(int skinId)
                {
                    return API_BASE_URL + "skins/" + skinId + "/enable";
                }

                public static string PostAPI_Subscribe()
                {
                    return API_BASE_URL + "subscriptions?os=" + GetPlatform();
                }

                public static string PutAPI_PurchaseAvatar(int storyId)
                {
                    return API_BASE_URL + "store/games/" + storyId + "?os=" + GetPlatform();
                }
                
        */
       /*                               

                public static string PutAPI_LikeProfile(int profileId)
                {
                    return API_BASE_URL + "profile/"+ profileId + "/swipe/like";
                }
                public static string PutAPI_DislikeProfile(int profileId)
                {
                    return API_BASE_URL + "profile/" + profileId + "/swipe/dislike";
                }

                

                

                public static string PostAPI_MoreProfiles()
                {
                    return API_BASE_URL + "profiles/more";
                }

                public static string PutAPI_UnmatchProfile(int profileId, int matchId)
                {
                    return API_BASE_URL + "profile/" + profileId + "/unmatch/" + matchId;
                }
        */
        
        //public static string PutAPI_StartMatch(int matchId)
        //{
        //    return API_BASE_URL + "games/matches/" + matchId + "/play";
        //}

    }
}