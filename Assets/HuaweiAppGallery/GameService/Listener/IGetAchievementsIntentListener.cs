namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetAchievementsIntentListener
    {
        //AndroidJavaClass: android.content.Intent
        void OnSuccess(AndroidJavaObject intent);

        void OnFailure(int code, string message);
    }
}