namespace UnityEngine.HuaweiAppGallery.Model
{
    public class Player
    {
        public string DisplayName { get; set; }
        public string HiResImageUri { get; set; }
        public string IconImageUri { get; set; }
        public string PlayerId { get; set; }
        public string SignTimestamp { get; set; }
        public string PlayerSign { get; set; }
        public int Level { get; set; }

        public AndroidJavaObject ConvertToJavaObject()
        {
            AndroidJavaObject jo =
                new AndroidJavaObject("com.unity.udp.extension.sdk.games.entity.PlayerEntity$Builder");
            jo.Call<AndroidJavaObject>("setDisplayName", DisplayName);
            jo.Call<AndroidJavaObject>("setPlayerId", PlayerId);
            jo.Call<AndroidJavaObject>("setSignTimestamp", SignTimestamp);
            jo.Call<AndroidJavaObject>("setPlayerSign", PlayerSign);
            jo.Call<AndroidJavaObject>("setLevel", Level);
            return jo.Call<AndroidJavaObject>("build");
        }
    }
}