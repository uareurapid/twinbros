namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface ILimitSizeListener
    {
        void OnSuccess(int limitSize);
        
        void OnFailure(int code, string message);
    }
}