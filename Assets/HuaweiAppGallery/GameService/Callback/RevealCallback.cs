using UnityEngine.HuaweiAppGallery.Listener;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class RevealCallback : AndroidJavaProxy
    {
        private IRevealListener _listener;

        public RevealCallback(IRevealListener listener) : base(
            "com.unity.udp.extension.sdk.games.achievement.AchievementsCallback$OnReveal")
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