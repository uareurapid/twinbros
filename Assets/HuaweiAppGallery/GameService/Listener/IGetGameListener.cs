using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetGameListener
    {
        void OnSuccess(Game game);

        void OnFailure(int code, string message);
    }
}