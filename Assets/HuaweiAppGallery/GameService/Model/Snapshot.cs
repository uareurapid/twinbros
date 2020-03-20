namespace UnityEngine.HuaweiAppGallery.Model
{
    public class Snapshot
    {
        public SnapshotData SnapshotData  { get; set; }
        public SnapshotContent SnapshotContent  { get; set; }

        public AndroidJavaObject ConvertToJavaObject()
        {
            AndroidJavaObject jo = new AndroidJavaObject("com.unity.udp.extension.sdk.games.entity.SnapshotEntity");
            if (SnapshotData != null)
            {
                jo.Call<AndroidJavaObject>("setSnapshotData", SnapshotData.ConvertToJavaObject());
            }

            if (SnapshotContent != null)
            {
                jo.Call<AndroidJavaObject>("setSnapshotContent", SnapshotContent.ConvertToJavaObject());
            }

            return jo;
        }
    }
}