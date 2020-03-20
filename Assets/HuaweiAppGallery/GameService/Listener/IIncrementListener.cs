namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IIncrementListener
    {
        void OnSuccess(bool isSuccess);
        
        void OnFailure(int code, string message);
    }
}