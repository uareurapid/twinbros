namespace UnityEngine.HuaweiAppGallery.Model
{
    public class Game
    {
        public int AchievementTotalCount { get; set; }
        public string ApplicationId { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string HiResImageUri { get; set; }
        public string IconImageUri { get; set; }
        public int LeaderboardCount { get; set; }
        public string PrimaryCategory { get; set; }
        public string SecondaryCategory { get; set; }

        public AndroidJavaObject ConvertToJavaObject()
        {
            AndroidJavaObject jo = new AndroidJavaObject("com.unity.udp.extension.sdk.games.entity.GameEntity$Builder");
            jo.Call<AndroidJavaObject>("setAchievementTotalCount", AchievementTotalCount);
            jo.Call<AndroidJavaObject>("setApplicationId", ApplicationId);
            jo.Call<AndroidJavaObject>("setDescription", Description);
            jo.Call<AndroidJavaObject>("setDisplayName", DisplayName);
            jo.Call<AndroidJavaObject>("setLeaderboardCount", LeaderboardCount);
            jo.Call<AndroidJavaObject>("setPrimaryCategory", PrimaryCategory);
            jo.Call<AndroidJavaObject>("setSecondaryCategory", SecondaryCategory);
            return jo.Call<AndroidJavaObject>("build");
        }
    }
}