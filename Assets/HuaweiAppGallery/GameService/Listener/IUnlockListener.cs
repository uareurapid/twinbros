namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IUnlockListener
    {
        void OnSuccess();

        void OnFailure(int code, string message);
    }
}