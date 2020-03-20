namespace UnityEngine.HuaweiAppGallery.Model
{
    public class SnapshotChange
    {
        public string Description { get; set; }
        public long PlayedTimeMillis { get; set; }
        public long CurrentProgress { get; set; }
        public string CoverImage { get; set; }
        public string ImageMimeType { get; set; }

        public AndroidJavaObject ConvertToJavaObject()
        {
            AndroidJavaObject jo =
                new AndroidJavaObject("com.unity.udp.extension.sdk.games.entity.SnapshotChangeEntity$Builder");
            jo.Call<AndroidJavaObject>("setDescription", Description);
            jo.Call<AndroidJavaObject>("setPlayedTimeMillis", PlayedTimeMillis);
            jo.Call<AndroidJavaObject>("setCurrentProgress", CurrentProgress);
            jo.Call<AndroidJavaObject>("setImageMimeType", ImageMimeType);
            return jo.Call<AndroidJavaObject>("build");
        }
    }
}