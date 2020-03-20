namespace UnityEngine.HuaweiAppGallery.Model
{
    public class SnapshotData
    {
        public string SnapshotId { get; set; }
        public string UniqueName { get; set; }
        public Player Player { get; set; }
        public long PlayedTime { get; set; }
        public long ProgressValue { get; set; }
        public float CoverImageAspectRatio { get; set; }
        public bool HasCoverImage { get; set; }
        public string Description { get; set; }
        public Game Game { get; set; }
        public long LastModifiedTimestamp { get; set; }

        public AndroidJavaObject ConvertToJavaObject()
        {
            AndroidJavaObject jo =
                new AndroidJavaObject("com.unity.udp.extension.sdk.games.entity.SnapshotDataEntity$Builder");
            jo.Call<AndroidJavaObject>("setSnapshotId", SnapshotId);
            jo.Call<AndroidJavaObject>("setUniqueName", UniqueName);
            if (Player != null)
            {
                jo.Call<AndroidJavaObject>("setPlayer", Player.ConvertToJavaObject());
            }

            jo.Call<AndroidJavaObject>("setPlayedTime", PlayedTime);
            jo.Call<AndroidJavaObject>("setProgressValue", ProgressValue);
            jo.Call<AndroidJavaObject>("setCoverImageAspectRatio", CoverImageAspectRatio);
            jo.Call<AndroidJavaObject>("setHasCoverImage", HasCoverImage);
            jo.Call<AndroidJavaObject>("setDescription", Description);
            if (Game != null)
            {
                jo.Call<AndroidJavaObject>("setGame", Game.ConvertToJavaObject());
            }

            jo.Call<AndroidJavaObject>("setLastModifiedTimestamp", LastModifiedTimestamp);
            return jo.Call<AndroidJavaObject>("build");
        }
    }
}