using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetPlayerListener
    {
        void OnSuccess(Player player);

        void OnFailure(int code, string message);
    }
}