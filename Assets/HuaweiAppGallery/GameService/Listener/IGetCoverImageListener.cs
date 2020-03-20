namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetCoverImageListener
    {
        //AndroidJavaClass:android.graphics.Bitmap convert to Base64 string
        void OnSuccess(string coverImage);

        void OnFailure(int code, string message);
    }
}