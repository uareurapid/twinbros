using UnityEngine.HuaweiAppGallery.Callback;
using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery
{
    public class HuaweiGameService
    {
        private static AndroidJavaClass gameServiceClass;
        private static AndroidJavaClass achievementsClass;
        private static AndroidJavaClass eventsClass;
        private static AndroidJavaClass leaderboardsClass;
        private static AndroidJavaClass extrasClass;
        private const string UNITY_PLAYER = "com.unity3d.player.UnityPlayer";
        private const string UDP_APPLICATION = "com.unity.udp.extension.sdk.UdpExtension";
        private const string UDP_GAME_SERVICE = "com.unity.udp.extension.sdk.games.UdpGames";

        private const string UDP_ACHIEVEMENT_CLASS =
            "com.unity.udp.extension.sdk.games.achievement.UdpAchievementsImpl";

        private const string UDP_EVENT_CLASS = "com.unity.udp.extension.sdk.games.event.UdpEventsImpl";

        private const string UDP_LEADERBOARD_CLASS =
            "com.unity.udp.extension.sdk.games.leaderboard.UdpLeaderboardsImpl";

        private const string UDP_EXTRA_CLASS = "com.unity.udp.extension.sdk.games.extra.UdpExtrasImpl";

        public static void AppInit()
        {
            // need application
            AndroidJavaClass applicationClass = new AndroidJavaClass(UDP_APPLICATION);
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            applicationClass.CallStatic("appInit", activity);
        }

        public static void Init()
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            safetyServiceClass().CallStatic("init", activity);
        }

        public static void Login(ILoginListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            LoginCallback callback = new LoginCallback(listener);
            safetyServiceClass().CallStatic("login", activity, callback);
        }

        public static void SilentSignIn(ILoginListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            LoginCallback callback = new LoginCallback(listener);
            safetyServiceClass().CallStatic("silentSignIn", activity, callback);
        }

        public static void SignOut(ILoginListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            LoginCallback callback = new LoginCallback(listener);
            safetyServiceClass().CallStatic("signOut", activity, callback);
        }

        private static AndroidJavaClass safetyServiceClass()
        {
            if (gameServiceClass == null)
            {
                gameServiceClass = new AndroidJavaClass(UDP_GAME_SERVICE);
            }

            return gameServiceClass;
        }

        private static AndroidJavaClass getAchievementsClass()
        {
            if (achievementsClass == null)
            {
                achievementsClass = new AndroidJavaClass(UDP_ACHIEVEMENT_CLASS);
            }

            return achievementsClass;
        }

        private static AndroidJavaClass getEventsClass()
        {
            if (eventsClass == null)
            {
                eventsClass = new AndroidJavaClass(UDP_EVENT_CLASS);
            }

            return eventsClass;
        }

        private static AndroidJavaClass getLeaderboardsClass()
        {
            if (leaderboardsClass == null)
            {
                leaderboardsClass = new AndroidJavaClass(UDP_LEADERBOARD_CLASS);
            }

            return leaderboardsClass;
        }

        private static AndroidJavaClass getExtrasClass()
        {
            if (extrasClass == null)
            {
                extrasClass = new AndroidJavaClass(UDP_EXTRA_CLASS);
            }

            return extrasClass;
        }

        /*** Achievement interface ***/
        public static void GetAchievementList(bool forceReload, IGetAchievementListListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetAchievementListCallback callback = new GetAchievementListCallback(listener);
            getAchievementsClass().CallStatic("getAchievementList", activity, forceReload, callback);
        }

        public static void GetAchievementsIntent(IGetAchievementsIntentListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetAchievementsIntentCallback callback = new GetAchievementsIntentCallback(listener);
            getAchievementsClass().CallStatic("getAchievementsIntent", activity, callback);
        }

        public static void Reveal(string achievementId)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            getAchievementsClass().CallStatic("reveal", activity, achievementId);
        }

        public static void AsyncReveal(string achievementId, IRevealListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            RevealCallback callback = new RevealCallback(listener);
            getAchievementsClass().CallStatic("asyncReveal", activity, achievementId);
        }

        public static void Increment(string achievementId, int numSteps)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            getAchievementsClass().CallStatic("increment", activity, achievementId, numSteps);
        }

        public static void AsyncIncrement(string achievementId, int numSteps, IIncrementListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            IncrementCallback callback = new IncrementCallback(listener);
            getAchievementsClass().CallStatic("asyncIncrement", activity, achievementId, numSteps, callback);
        }

        public static void SetSteps(string achievementId, int numSteps)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            getAchievementsClass().CallStatic("setSteps", activity, achievementId, numSteps);
        }

        public static void AsyncSetSteps(string achievementId, int numSteps, ISetStepsListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            SetStepsCallback callback = new SetStepsCallback(listener);
            getAchievementsClass().CallStatic("asyncSetSteps", activity, achievementId, numSteps, callback);
        }

        public static void Unlock(string achievementId)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            getAchievementsClass().CallStatic("unlock", activity, achievementId);
        }

        public static void AsyncUnlock(string achievementId, IUnlockListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            UnlockCallback callback = new UnlockCallback(listener);
            getAchievementsClass().CallStatic("asyncUnlock", activity, achievementId, callback);
        }

        /*** leaderboards ***/
        public static void GetLeaderboardSwitchStatus(ILeaderboardSwitchStatusListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            LeaderboardSwitchStatusCallback callback = new LeaderboardSwitchStatusCallback(listener);
            getLeaderboardsClass().CallStatic("getLeaderboardSwitchStatus", activity, callback);
        }

        public static void SetLeaderboardSwitchStatus(int status, ILeaderboardSwitchStatusListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            LeaderboardSwitchStatusCallback callback = new LeaderboardSwitchStatusCallback(listener);
            getLeaderboardsClass().CallStatic("setLeaderboardSwitchStatus", activity, status, callback);
        }

        public static void SubmitScore(string leaderboardId, int score)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            getLeaderboardsClass().CallStatic("submitScore", activity, leaderboardId, score);
        }

        public static void AsyncSubmitScore(string leaderboardId, int score, ISubmitScoreListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            SubmitScoreCallback callback = new SubmitScoreCallback(listener);
            getLeaderboardsClass().CallStatic("asyncSubmitScore", activity, leaderboardId, score, callback);
        }

        public static void SubmitScore(string leaderboardId, int score, string scoreTag)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            getLeaderboardsClass().CallStatic("submitScore", activity, leaderboardId, score, scoreTag);
        }

        public static void AsyncSubmitScore(string leaderboardId, int score, string scoreTag,
            ISubmitScoreListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            SubmitScoreCallback callback = new SubmitScoreCallback(listener);
            getLeaderboardsClass().CallStatic("asyncSubmitScore", activity, leaderboardId, score, scoreTag, callback);
        }

        public static void GetAllLeaderboardsIntent(IGetLeaderboardIntentListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetLeaderboardIntentCallback callback = new GetLeaderboardIntentCallback(listener);
            getLeaderboardsClass().CallStatic("getAllLeaderboardsIntent", activity, callback);
        }

        public static void GetLeaderboardIntent(string leaderboardId, IGetLeaderboardIntentListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetLeaderboardIntentCallback callback = new GetLeaderboardIntentCallback(listener);
            getLeaderboardsClass().CallStatic("getLeaderboardIntent", activity, leaderboardId, callback);
        }

        public static void GetLeaderboardIntent(string leaderboardId, int timeSpan,
            IGetLeaderboardIntentListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetLeaderboardIntentCallback callback = new GetLeaderboardIntentCallback(listener);
            getLeaderboardsClass().CallStatic("getLeaderboardIntent", activity, leaderboardId, timeSpan, callback);
        }

        public static void GetLeaderboardsData(bool isRealTime, IGetLeaderboardsListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetLeaderboardsCallback callback = new GetLeaderboardsCallback(listener);
            getLeaderboardsClass().CallStatic("getLeaderboardsData", activity, isRealTime, callback);
        }

        public static void GetLeaderboardData(string leaderboardId, bool isRealTime, IGetLeaderboardListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetLeaderboardCallback callback = new GetLeaderboardCallback(listener);
            getLeaderboardsClass().CallStatic("getLeaderboardData", activity, leaderboardId, isRealTime, callback);
        }

        public static void GetLeaderboardTopScores(string leaderboardId, int timeSpan, int maxResults, bool isRealTime,
            IGetLeaderboardScoresListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetLeaderboardScoresCallback callback = new GetLeaderboardScoresCallback(listener);
            getLeaderboardsClass().CallStatic("getLeaderboardTopScores", activity, leaderboardId, timeSpan, maxResults,
                isRealTime, callback);
        }

        public static void GetLeaderboardTopScores(string leaderboardId, int timeSpan, int maxResults, long offset,
            int pageDirection, IGetLeaderboardScoresListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetLeaderboardScoresCallback callback = new GetLeaderboardScoresCallback(listener);
            getLeaderboardsClass().CallStatic("getLeaderboardTopScores", activity, leaderboardId, timeSpan, maxResults,
                offset, pageDirection,
                callback);
        }

        public static void GetCurrentPlayerLeaderboardScore(string leaderboardId, int timeSpan,
            IGetLeaderboardScoreListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetLeaderboardScoreCallback callback = new GetLeaderboardScoreCallback(listener);
            getLeaderboardsClass().CallStatic("getCurrentPlayerLeaderboardScore", activity, leaderboardId, timeSpan,
                callback);
        }

        public static void GetPlayerCenteredLeaderboardScores(string leaderboardId, int timeSpan, int maxResults,
            bool isRealTime, IGetLeaderboardScoresListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetLeaderboardScoresCallback callback = new GetLeaderboardScoresCallback(listener);
            getLeaderboardsClass().CallStatic("getPlayerCenteredLeaderboardScores", activity, leaderboardId, timeSpan,
                maxResults, isRealTime,
                callback);
        }

        public static void GetPlayerCenteredLeaderboardScores(string leaderboardId, int timeSpan, int maxResults,
            long offset, int pageDirection, IGetLeaderboardScoresListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetLeaderboardScoresCallback callback = new GetLeaderboardScoresCallback(listener);
            getLeaderboardsClass().CallStatic("getPlayerCenteredLeaderboardScores", activity, leaderboardId, timeSpan,
                maxResults, offset,
                pageDirection, callback);
        }

        public static void GetMoreLeaderboardScores(string leaderboardId, long offset, int maxResults,
            int pageDirection, int timeSpan, IGetLeaderboardScoresListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetLeaderboardScoresCallback callback = new GetLeaderboardScoresCallback(listener);
            getLeaderboardsClass().CallStatic("getMoreLeaderboardScores", activity, leaderboardId, offset, maxResults,
                pageDirection, timeSpan,
                callback);
        }


        /*** events ***/
        public static void EventIncrement(string eventId, int incrementAmount)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            getEventsClass().CallStatic("increment", activity, eventId, incrementAmount);
        }

        public static void GetEventList(bool forceReload, IGetEventListListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetEventListCallback callback = new GetEventListCallback(listener);
            getEventsClass().CallStatic("getEventList", activity, forceReload, callback);
        }

        public static void GetEventListByIds(bool forceReload, string[] eventIds, IGetEventListListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetEventListCallback callback = new GetEventListCallback(listener);
            getEventsClass().CallStatic("getEventListByIds", activity, forceReload, eventIds, callback);
        }

        /*** extras ***/
        public static void GetCurrentPlayer(bool isRealTime, IGetPlayerListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetPlayerCallback callback = new GetPlayerCallback(listener);
            getExtrasClass().CallStatic("getCurrentPlayer", activity, callback);
        }

        public static void GetGamePlayerStatistics(bool isRealTime, IGetPlayerStatisticsListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetPlayerStatisticsCallback callback = new GetPlayerStatisticsCallback(listener);
            getExtrasClass().CallStatic("getGamePlayerStatistics", activity, isRealTime, callback);
        }

        public static void GetGame(IGetGameListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetGameCallback callback = new GetGameCallback(listener);
            getExtrasClass().CallStatic("getGame", activity, callback);
        }

        public static void GetLocalGame(IGetGameListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetGameCallback callback = new GetGameCallback(listener);
            getExtrasClass().CallStatic("getLocalGame", activity, callback);
        }

        public static void ShowFloatWindow()
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            getExtrasClass().CallStatic("showFloatWindow", activity);
        }

        public static void HideFloatWindow()
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            getExtrasClass().CallStatic("hideFloatWindow", activity);
        }

        public static void GrantDriveAccess()
        {
            getExtrasClass().CallStatic("grantDriveAccess");
        }

        public static void GetLimitThumbnailSize(ILimitSizeListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            LimitSizeCallback callback = new LimitSizeCallback(listener);
            getExtrasClass().CallStatic("getLimitThumbnailSize", activity, callback);
        }

        public static void GetLimitDetailsSize(ILimitSizeListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            LimitSizeCallback callback = new LimitSizeCallback(listener);
            getExtrasClass().CallStatic("getLimitThumbnailSize", activity, callback);
        }

        public static void AddSnapshot(SnapshotContent snapshot, SnapshotChange snapshotChange, bool isSupportCache,
            IGetSnapshotDataListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetSnapshotDataCallback callback = new GetSnapshotDataCallback(listener);
            getExtrasClass().CallStatic("addSnapshot", activity, snapshot.ConvertToJavaObject(), snapshotChange.ConvertToJavaObject(), isSupportCache, callback);
        }

        public static void GetSnapshotDataList(bool isRealTime, IGetAllSnapshotDataListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetAllSnapshotDataCallback callback = new GetAllSnapshotDataCallback(listener);
            getExtrasClass().CallStatic("getSnapshotDataList", activity, isRealTime, callback);
        }

        public static void LoadCoverImage(SnapshotData snapshotData, IGetCoverImageListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetCoverImageCallback callback = new GetCoverImageCallback(listener);
            getExtrasClass().CallStatic("loadCoverImage", activity, snapshotData.ConvertToJavaObject(), callback);
        }

        public static void LoadSnapshotContents(string snapshotId, int conflictPolicy,
            IGetSnapshotResultListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetSnapshotResultCallback callback = new GetSnapshotResultCallback(listener);
            getExtrasClass().CallStatic("loadSnapshotContents", activity, snapshotId, conflictPolicy, callback);
        }

        public static void UpdateSnapshot(Snapshot snapshot, IGetSnapshotResultListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetSnapshotResultCallback callback = new GetSnapshotResultCallback(listener);
            getExtrasClass().CallStatic("updateSnapshot", activity, snapshot.ConvertToJavaObject(), callback);
        }

        public static void UpdateSnapshot(string snapshotId, SnapshotChange snapshotChange,
            SnapshotContent snapshotContent, IGetSnapshotResultListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            GetSnapshotResultCallback callback = new GetSnapshotResultCallback(listener);
            getExtrasClass().CallStatic("updateSnapshot", activity, snapshotId, snapshotChange.ConvertToJavaObject(), snapshotContent.ConvertToJavaObject(),
                callback);
        }

        public static void DeleteSnapshot(SnapshotData snapshotData, IDeleteSnapshotListener listener)
        {
            AndroidJavaClass player = new AndroidJavaClass(UNITY_PLAYER);
            AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            DeleteSnapshotCallback callback = new DeleteSnapshotCallback(listener);
            getExtrasClass().CallStatic("deleteSnapshot", activity, snapshotData.ConvertToJavaObject(), callback);
        }
    }
}