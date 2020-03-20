using UnityEngine.HuaweiAppGallery.Listener;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class IncrementCallback : AndroidJavaProxy
    {
        private IIncrementListener _listener;

        public IncrementCallback(IIncrementListener listener) : base(
            "com.unity.udp.extension.sdk.games.achievement.AchievementsCallback$OnIncrement")
        {
            _listener = listener;
        }

        public void onSuccess(bool isSuccess)
        {
            if (_listener != null)
            {
                _listener.OnSuccess(isSuccess);
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