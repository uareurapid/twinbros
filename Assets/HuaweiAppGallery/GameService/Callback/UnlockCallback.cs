using UnityEngine.HuaweiAppGallery.Listener;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class UnlockCallback : AndroidJavaProxy
    {
        private IUnlockListener _listener;

        public UnlockCallback(IUnlockListener listener) : base(
            "com.unity.udp.extension.sdk.games.achievement.AchievementsCallback$OnUnlock")
        {
            _listener = listener;
        }

        public void onSuccess()
        {
            if (_listener != null)
            {
                _listener.OnSuccess();
            }
        }

        public void onFailure(AndroidJavaObject exception, AndroidJavaObject result)
        {
            if (_listener != null && result != null)
            {
                _listener.OnFailure(result.Call<int>("getCode"), result.Call<string>("getMessage"));
            }
        }
    }
}