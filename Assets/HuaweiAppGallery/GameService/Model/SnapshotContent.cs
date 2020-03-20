namespace UnityEngine.HuaweiAppGallery.Model
{
    public class SnapshotContent
    {
        public int DstOffset  { get; set; }
        public byte[] Content  { get; set; }
        public int SrcOffset  { get; set; }
        public int Count  { get; set; }

        public AndroidJavaObject ConvertToJavaObject()
        {
            AndroidJavaObject jo =
                new AndroidJavaObject("com.unity.udp.extension.sdk.games.entity.SnapshotContentEntity", Content);
            return jo;
        }
    }
}