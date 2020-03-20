namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetLeaderboardIntentListener
    {
        //AndroidJavaClass: android.content.Intent
        void OnSuccess(AndroidJavaObject intent);

        void OnFailure(int code, string message);
    }
}