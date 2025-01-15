
using System.Collections.Generic;
using static System.Net.WebRequestMethods;

public class ConstantsGod

{
    public static string AUTH_TOKEN = "AUTH_TOKEN";
    public static string ANIMATIONNAME = "animation name";
    public static string PLAYERNAME = "player name";
    public static string WALLET_ADDRESS = "";
    public static string API_BASEURL = "https://app-api.xana.net";
    public static string XANALIA_SOCKET_ADDRESS = "http://prod-socket.xanalia.com/";
    public static string TELEGRAM_LOGIN_URL = "https://event-test.xana.net/app?builderLoginHash=";
    public static string SAVE_DEVICE_XANALIA = "auth/save-device-from-xanalia/";
    public static string ANDROIDPATH = "path";
    public static readonly string UPLOADVIDEOPATH = "uploadVideo";
    public static readonly string VIDEOPATH = "Video Path";
    public static readonly string DEFAULT_TOKEN = "piyush55";
    public static readonly string TOTAL_AUDIO_VOLUME = "TOTAL AUDIO VOLUME";
    public static readonly string BGM_VOLUME = "BGM VOLUME";
    public static readonly string MIC = "MIC VOLUME";
    public static readonly string VIDEO_VOLUME = "VIDEO VOLUME";
    public static readonly string CAMERA_SENSITIVITY = "CAMERA SENSITIVITY";
    //public static readonly string BASE_URL = "https://api-xana.angelium.net/api/";
    public static string ReactionThumb = "reaction thumb";
    public static string SENDMESSAGETEXT = "send message";
    public static string GUSTEUSERNAME = "guste user";
    public static string NFTTYPE = "nft type";

    public static string ANIMATION_DATA = "AnimationData";
    public static string EMOTE_SELECTION_INDEX = "EmoteAnimSelectionIndex";
    public static string SELECTED_ANIMATION_NAME = "selectedAnimName";

    public static string POSTTIMESTAMP = "post time";
    public static string POSTDESCRIPTION = "post description";
    public static string POSTUSERNAME = "post username";
    public static string POSTMEMBERSCOUNT = "member count";


    public static string NFTTHUMB = "nft thumb";
    public static string NFTOWNER = "nft owner";
    public static string NFTCREATOR = "nft creator";
    public static string NFTDES = "nft des";
    public static string NFTLINK = "nft link";
    public static string REFRESHXANATOKEN = "/auth/refresh-token";

    public static string API = "https://api.xana.net/";
    public static string SERVER = "ws://socket-lb-648131231.us-east-2.elb.amazonaws.com:3000";

    public static string UserPriorityRole = "Guest";
    public static List<string> UserRoles = new List<string>() { "Guest" };

    public static string SMTPSERVERAUTHDATA = "/admin/get-smtp-for-app";

    #region World Manager

    public static string GETENVIRONMENTSAPI = API + "xanaEvent/getEnvironments";
    public static string JWTTOKEN = "JWT eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoxOTc4NiwidXNlcm5hbWUiOiJueWxhIiwiZXhwIjoxNjAzMjYwNjUxLCJlbWFpbCI6Im5heWxhLm5vYm9yZGVyc0BnbWFpbC5jb20iLCJvcmlnX2lhdCI6MTYwMzI1NzA1MX0.zJKxLtvBLK-uf4kdmc5b20r4iSkpFLfv5So2c0oBc0U";


    //All Item Api
    public static string GETALLSTOREITEMCATEGORY = "/item/all-category";
    public static string GETALLSTOREITEMSUBCATEGORY = "/item/subcategories";
    public static string GETALLSTOREITEMS = /*"/item/all-items"*/ /*"/item/v2/all-items"*/ /*"/item/v3/all-items"*/"/item/v4/all-items";



    public static string GETENVIRONMENTSAPINew = "/item/environment/1/30";
    public static string GetAllMuseumsAPI = "/item/museums/2/1/25";

    // public static string GetAllMuseumsAPI = "/item/museums/v2/1/20";
    public static string BACKGROUNDFILES = "/item/background-files";
    public static string ANIMATIONFILES = "/item/animations";
    public static string FILTERPROFILE = "/item/get-filter-assets";
    public static string UPLOADFILE = "/item/upload-file";
    public static string UPLOADFILECLOUDIMAGE = "/item/upload-file-v3";
    //public static string OCCUPIDEASSETS = "/item/get-user-occupied-asset/";
    public static string OCCUPIDEASSETS = "/item/v2/get-user-occupied-asset/";
    public static string USERLATESTOCCUPIEDASSET = "/item/get-latest-user-occupied-asset/";
    public static string DELETEOCCUPIDEUSER = "/item/delete-user-occupied-asset/";
    //public static string CREATEOCCUPIDEUSER = "/item/create-user-occupied-asset";
    public static string CREATEOCCUPIDEUSER = "/item/v2/create-user-occupied-asset";
    public static string UPDATEOCCUPIDEUSER = "/item/update-user-occupied-asset/";
    public static string SHARELINKS = "/item/shareLinks";
    public static string SHAREDEMOS = "/item/shareDemos";
    public static string YOUTUBEVIDEOBYSCENE = "/item/v2/shareLinks/"; //scene name 
    public static string SUMMITYOUTUBEVIDEOBYID = "/domes/getSummitVideoByAreaIdIndex/";//Add areaId and index
    public static string GetStreamableYoutubeUrl = "/item/get-yt-downloadable-url";
    public static string SENDYTLIVEHLSURLDATA = "/item/save-hls-for-env";
    public static string GETYTLIVEHLSURLDATA = "/item/get-hls-url-for-env";
    public static string SENDYTLIVEHLSURLDATAAPIKEY = "CzjFgyveM6uPGTU1eaCz";

    public static string GetDefaultAPI = "/items/get-items-with-defaults";
    // public static string GetUserDetailsAPI = "users/single-user";
    public static string PurchasedAPI = "/items/purchase-items";
    public static string SendCoinsAPI = "/users/update-coins";
    public static string WALLETSTATUS = "/auth/get-wallet-status";
    public static string GETSETS = "/item/get-sets";
    public static string LogoutFromotherDeviceAPI = "/auth/logout-from-other";
    public static string SendEmailOTP = "/auth/send-otp-for-email";
    public static string VerifyEmailOTP = "/auth/otp-verify-for-email";
    public static string RegisterWithEmail = "/auth/register-with-email";
    public static string LoginAPIURL = "/auth/sign-in";
    public static string NameAPIURL = "/users/set-name";
    public static string ChangePasswordAPI = "/users/change-password";
    public static string UpdateProfileAPI = "/users/update-profile";
    public static string DeleteAPI = "/users/delete-account";
    public static string GetUserDetailsAPI = "/users/single-user";
    public static string SetDeviceTokenAPI = "/users/set-device-token";
    public static string LogOutAPI = "/users/logout";
    public static string UpdateAvatarAPI = "/users/update-avatar";
    //[Space(10)]
    //[Header("Total-API-Phone")]
    public static string SendPhoneOTPAPI = "/auth/send-otp-for-phone";
    public static string VerifyPhoneOTPAPI = "/auth/otp-verify-for-phone";
    public static string RegisterPhoneAPI = "/auth/register-with-phone";
    public static string ResendOTPAPI = "/auth/resend-otp";
    //[Header("Total-API-ForgetPassword")]
    public static string ForgetPasswordAPI = "/auth/forgot-password";
    public static string ForgetPasswordOTPAPI = "/auth/verify-forgot-password-otp";
    public static string ForgetPasswordResetAPI = "/auth/reset-password";
    //[Header("Guest API")]
    public static string guestAPI = "/auth/login-as-guest";
    //  public static string GetAllAnimatons = "/item/animations";
    public static string GetAllAnimatons = "/item/v2/animations";
    public static string GetAllReactions = "/item/get-all-reactions";
    public static string GetVersion = "/item/get-version";
    public static string MaintenanceAPI = "/item/get-version/";
    public static string YtSetStreamUrl = "/users/set-youtube-url";
    public static string YtStreamUrl = "/users/get-youtube-url/";

    // Xana Lobby WOrlds
    public static string GetXanaLobbyWorlds = "/item/get-xana-lobby-worlds";
    public static string GetXanaLobbyDynamicText = "/item/get-xana-lobby-dynamic-data";

    public static string GETXANAOFFICIALWORLDBYID = "";

    #endregion

    #region SNS Managers    
    public static string r_privacyPolicyLink = "https://cdn.xana.net/xanaprod/privacy-policy/PRIVACYPOLICY-2.pdf";
    public static string r_termsAndConditionLink = "https://cdn.xana.net/xanaprod/privacy-policy/termsofuse.pdf";

    // public static string r_mainURL = "https://app-api.xana.net";
    public static string r_AWSImageKitBaseUrl = "https://aydvewoyxq.cloudimg.io/_xana_/";
    public static string AWS_VIDEO_BASE_URL = "https://s3-congnito-file-upload-demo.s3.ap-southeast-1.amazonaws.com/";

    public static string r_url_UploadFile = "/item/upload-file-with-conversion";
    public static string r_url_GetAllUsersWithFeeds = "/hot/all-users-with-feeds";
    public static string r_url_GetHotFeeds = "/feeds/all-hot-feeds";
    public static string r_url_GetFeedsByUserId = "/hot/feeds";
    public static string r_url_GetFeedsByFollowingUser = "/feeds/following";
    public static string r_url_GetTaggedFeedsByUserId = "/hot/tagged-feeds";

    public static string r_url_FollowAUser = "/follow/user";
    public static string r_url_GetAllFollowing = /*"/follow/get-all-following"*/ "/social/follow/get-all-following/";
    public static string r_url_AdFrndGetAllAolowing = "/social/follow/get-all-following/";
    public static string r_url_GetAllFollowers = "/follow/get-all-followers";
    public static string r_url_MakeFavouriteFollower = "/follow/make-fav";
    public static string r_url_UnFollowAUser = "/follow/unfollow-user";
    public static string r_url_OnlineFriends = "/social/follow/get-user-online-friends";

    public static string r_url_AllFeed = "/feeds";
    public static string r_url_CommentFeed = "/feeds/comment-feed";
    public static string r_url_FeedCommentList = "/feeds/feed-comment-list";
    public static string r_url_CreateFeed = "/feeds/single-feed";
    public static string r_url_DeleteFeed = "/feeds/delete-feed";
    public static string r_url_EditFeed = "/feeds/edit-feed";
    public static string r_url_DeleteComment = "/feeds/feed-comment-delete";

    public static string r_url_FeedLikeDisLike = "/feeds/like-dislike-post";

    public static string r_url_SearchUser = /*"/users/search-user"*/ "/users/v2/search-user/";
    public static string r_url_HotUsers = /*"/social/get-non-friends/"*/ "/users/hot-users/";
    public static string r_url_RecommendedUser = "/social/get-friends-recommendations/";
    public static string r_url_MutalFrnd = "/social/follow/get-user-mutual-followers/";
    public static string r_url_GetBestFrnd = "/social/get-close-friends/";
    public static string r_url_AdBestFrnd = "/social/create-close-friend/";
    public static string r_url_RemoveBestFrnd = "/social/remove-close-friend/";
    public static string r_url_WebsiteValidation = "/auth/check-website-validity";

    public static string r_url_SetName = "/users/set-name";
    public static string r_url_GetUserDetails = "/users/single-user";
    public static string r_url_UpdateUserAvatar = "/users/update-avatar";
    //public static string r_url_UpdateUserProfile = "/users/update-profile";
    //public static string r_url_UpdateUserProfile = "/users/update-user-profile-details";
    public static string r_url_UpdateUserProfile = "/users/v2/update-user-profile-details";
    public static string r_url_GetSingleUserProfile = "/follow/get-single-profile";
    public static string r_url_GetSingleUserRole = "/user/get-user-role?xanaId=";
    public static string r_url_DeleteAccount = "/users/delete-account";

    public static string r_url_ChatCreateGroup = "/chat/create-group";
    public static string r_url_AddGroupMember = "/chat/add-group-member";
    public static string r_url_UpdateGroupInfo = "/chat/update-group";
    public static string r_url_ChatCreateMessage = "/chat/create-msg";
    public static string r_url_ChatGetAttachments = "/chat/get-attachments";
    public static string r_url_ChatGetConversation = "/chat/get-conversation";
    public static string r_url_ChatMuteUnMuteConversation = "/chat/mute-unmute-conversation";
    public static string r_url_ChatGetMessages = "/chat/get-messages";
    public static string r_url_GetAllChatUnReadMessagesCount = "/chat/get-all-message-unreadCount";
    public static string r_url_LeaveTheChat = "/chat/leave-chat-group";
    public static string r_url_DeleteConversation = "/chat/delete-conversation";
    public static string r_url_DeleteChatGroup = "/chat/delete-chat-group";
    public static string r_url_RemoveGroupMember = "/chat/remove-group-member";
    #endregion

    #region Xanalia Api and Wallet Connection
    public static string API_BASEURL_XANALIA = "https://api.xanalia.com";

    public static readonly string userMy_Collection_Xanalia = "/user/my-collection";
    public static readonly string getUserProfile_Xanalia = "/user/get-user-profile";

    public const string xanaliaTestAPI = "https://backend.xanalia.com";
    public const string xanaliaProductionAPI = "https://prod-backend.xanalia.com";
    public static readonly string loginExternalWalletURL = "/auth/login-external-wallet";

    public static readonly string GetUserNounceURL = "/auth/get-user-nonce";
    public static readonly string VerifySignedURL = "/auth/verify-signature";
    //public static readonly string NameAPIURL = "";
    public static readonly string VerifySignedXanaliaURL = "/auth/verify-signature";
    public static readonly string GetXanaliaNounceURL = "/auth/get-address-nonce";
    public static readonly string GetXanaliaNFTURL = "/xanalia/mydata";
    public static readonly string GetGroupDetailsAPI = "/item/get-sets";
    public static readonly string SaveNonce = "/auth/save-user-nonce";


    #endregion


    #region XENY Api

    public static string GetUserXenyCoinsApi
    {
        get
        {
            if (APIBasepointManager.instance.IsXanaLive)
            {
                return "https://prod-backend.xanalia.com/sale-nft/get-xeny-balance-by-user-in-eth";
            }
            else
            {
                return "https://backend.xanalia.com/sale-nft/get-xeny-balance-by-user-in-eth";
            }
        }
    }

    #endregion

    #region GrammyAward Api

    public static readonly string getAnimationTime = "/item/get-timeCount";
    #endregion

    #region Analatic Api's

    public static string API_GetWorldId = "/analytical/enter-xana-world";
    public static string API_GetWorldId_Guest = "/analytical/guest-enter-xana-world";

    public static string API_GetSingleWorldStats = "/analytical/get-single-world-stats/";
    public static string API_GetSingleWorldStats_Guest = "/analytical/guest-get-single-world-stats/";

    public static string API_UpdateWorldStats = "/analytical/update-xana-world-stats";
    public static string API_UpdateWorldStats_Guest = "/analytical/guest-update-xana-world-stats";

    #endregion

    #region XANABuilder Api's
    //public static string MUSEUMENVBUILDERWORLDSCOMBINED = "/item/get-world-creator-list-paginated/";    //"/item/v3/get-xana-universe/";
    public static string MUSEUMENVBUILDERWORLDSCOMBINED = "/item/v2/get-world-creator-list-paginated/";
    public static string BUILDERGETSINGLEWORLDBYID = "/item/get-single-world/";
    public static string MYBUILDERWORLDS = "/item/v2/get-worlds/";  //status/pagenumber/pagecount
    public static string ALLBUILDERWORLDS = "/item/get-all-worlds/";  //status/pagenumber/pagecount
    public static string WORLDSBYCATEGORY = "/item/get-worlds-by-category/"; //:pageNumber/:pageSize/:status/:category
    //public static string SearchWorldAPI = "/item/v2/search-worlds/";  //:name/:pageNumber/:pageSize
    public static string SearchWorldAPI = "/item/v3/search-worlds/";  //:name/:pageNumber/:pageSize
    //public static string SEARCHWORLDBYTAG = "/item/search-worlds-by-tag/";  //:tag/:pageNumber/:pageSize
    public static string SEARCHWORLDBYTAG = "/item/v2/search-worlds-by-tag/";  //:tag/:pageNumber/:pageSize
    public static string USERTAGS = "/users/get-user-tags";

    //public static string FEATUREDSPACES = "/world/get-featured-spaces/";//pageNumber/pageSize
    public static string FEATUREDSPACES = "/world/v2/get-featured-spaces/";//pageNumber/pageSize
    //public static string HOTSPACES = "/world/get-most-visited-hot-spaces/";//pageNumber/pageSize
    public static string HOTSPACES = "/world/v2/get-most-visited-hot-spaces/";//pageNumber/pageSize
    //public static string HOTGAMES = "/world/get-most-visited-hot-games/";//pageNumber/pageSize
    public static string HOTGAMES = "/world/v2/get-most-visited-hot-games/";//pageNumber/pageSize
    //public static string FOLLOWINGSPACES = "/world/get-favourite-space-list-for-xana/";//pageNumber/pageSize
    public static string FOLLOWINGSPACES = "/world/v2/get-favourite-space-list-for-xana/";//pageNumber/pageSize
    public static string MOSTVISITEDTAG = "/world/get-most-visited-tags/";  //pageNumber/pageSize
    public static string FOLLOWWORLD = "/world/mark-favourite-space/";  //:worldId    //same work for both follow world and unfollow world.
    public static string SINGLEWORLDINFO = "/item/get-one-world-data?worldId=";   //:id/:type
    public static string SINGLEWORLDINFOBYNAME = "/item/get-one-world-data?worldName=";   //:id/:type

    #endregion

    #region UserPost
    public static string SendPostToServer = "/item/new-text-post";
    public static string GetPostSentToServer = "/item/get-latest-text-post/";///"/item/get-user-text-post/1/100";
    public static string GetUserAllTextPosts = "/item/get-user-text-post/";
    #endregion
    #region Get App Features List Api's
    public static string FeaturesListApi = "/admin/v2/get-features-list";
    #endregion


    #region Jj World Api's
    public static string JJWORLDASSET = "/item/jjWorld/get-museum-all-assets/";
    #endregion

    public static string availableTags = "/users/get-user-tags";

    #region UGC Api's
    public static string API_BASEURL_UGC = "https://ugcfacial-aiprod.xana.net"; // for main
    public static string UGCAiApi = "/analyze-image/";   // for main  

    public static string UGCGetBackground = "/users/get-user-background";
    public static string UGCAddBackground = "/users/add-user-background";
    #endregion

    #region Feed Api's

    public static string FeedGetAllByUserId = "/item/get-following-text-post/";
    public static string FeedLikeDislikePost = "/item/like-text-post";
    public static string FeedSearch = "/item/search-following-text-post";
    #endregion

    #region PMY World Api's
    public static string toyotaApi = "/toyotaAichiWorlds/get-all-assets-by-worldId/";//"/pmyWorlds/get-all-assets-by-worldId/";
    #endregion
    #region JJ Test World Api's
    public static string JjTestWorldAssets = "/airin/get-all-assets-by-worldId/"; 
    #endregion

    #region XANA Summit Api's
    public static string GETDOMENPCINFO = "/domes/getDomeNPCInfo/";  //:domeId/:type
    //public static string GETALLDOMES = "/domes/getcreatedDomes";
    public static string GETALLDOMES = "/domes/v2/getcreatedDomes";
    public static string GETSINGLEDOMEAUTHEMAILS = "/domes/get-domes-participants/";
    public static string GETSINGLEDOME = "/domes/getDomeGeneralInfoById/";  //:domeId
    public static string GETVISITDOMES = "/domes/getvisitedomes";
    public static string SETVISITDOMEWITHID = "/domes/domesvisits/";
    public static string GETUSERDETAIL = "/users/get-custom-intro";
    public static string VISITORCOUNT = "/world/get-total-visit/";  //:world_id

    public static string GETDOMENFT = "/domes/getDomecontent/";     //:domeId
    public static string LIVEVIDEOAPI = "https://webgl-proxy-video.xana.net/get-stream-url/";
    public static string PRERECORDEDVIDEOAPI = "https://webgl-proxy-video.xana.net/get-video-url/";
    
    #endregion
    #region Toyota Email Api's
    public static string toyotaEmailApi = "/toyotaAichiWorlds/get-all-space-email-public/";
    public static string toyotaNotificationApi = "/toyotaAichiWorlds/save-user-token";
    public static string joinmeetingroom = "/toyotaAichiWorlds/join-meeting-room";
    public static string leavemeetingroom = "/toyotaAichiWorlds/leave-meeting-room";
    public static string getmeetingroomcount = "/toyotaAichiWorlds/check-meeting-members/";
    public static string wrapobjectApi = "/toyotaAichiWorlds/get-status-of-worldId/";
    #endregion

    #region Quest API's
    public static string GetAllTaskDataFromCurrentQuest = "/quest/get-current-quest-task-list/";
    public static string UpdateQuestTaskData = "/quest/edit-quest-task/";
    public static string UpdateQuestTaskDataPerformance = "/quest/update-user-performance";
    public static string UpdateComapreQuestTaskDataPerformance = "/quest/get-user-performed-list";
    public static string ClaimQuestReward = "/quest/claim-quest-reward";
    public static string ClaimQuestRewardCheque = "/quest/claim-status/";
    #endregion
    #region Dome reward API's
    public static string UpdateVisitedDomes = "/domes/domesvisits/";
    public static string UpdateUserRaffleTickets = "/domes/updatetickets";
    public static string GetUserRaffleTickets = "/domes/getusertickets";
    public static string GetUserVisitedDomes = "/domes/getvisitedomes";
    #endregion

    #region Bundle Update Api
    public static string BUNDLEUPDATEAPI = "/app-bundles/get";
    #endregion

    #region XANA PARTY WORLD Api's
    public static string API_BASEURL_Penpenz = "https://penpenz-prod.xana.net/";
    public static string GetXanaPartyWorlds = "/item/v1/get-world-by-world-type/1"; // 1 for xana party and 0 for simple builder  
    public static string CreateUser_Penpenz = "api/users";
    public static string GetRankPoints_Penpenz = "api/rankpoints";
    public static string StartRace_Penpenz = "api/races/start";
    #endregion
}