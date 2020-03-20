namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface ISetStepsListener
    {
        void OnSuccess(bool isSuccess);

        void OnFailure(int code, string message);
    }
}