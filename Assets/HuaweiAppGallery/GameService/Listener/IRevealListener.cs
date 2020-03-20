namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IRevealListener
    {
        void OnSuccess();

        void OnFailure(int code, string message);
    }
}