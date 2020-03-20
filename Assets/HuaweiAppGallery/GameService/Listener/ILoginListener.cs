using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface ILoginListener
    {
        void OnSuccess(SignInAccountProxy signInAccountProxy);

        void OnSignOut();

        void OnFailure(int code, string message);
    }
}