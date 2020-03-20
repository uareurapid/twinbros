using UnityEngine.HuaweiAppGallery.Listener;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetAchievementsIntentCallback : AndroidJavaProxy
    {
        private IGetAchievementsIntentListener _listener;

        public GetAchievementsIntentCallback(IGetAchievementsIntentListener listener) : base(
            "com.unity.udp.extension.sdk.games.achievement.AchievementsCallback$OnGetAchievementsIntent")
        {
            _listener = listener;
        }

        public void onSuccess(AndroidJavaObject intent)
        {
            if (intent == null)
            {
                return;
            }

            if (_listener != null)
            {
                _listener.OnSuccess(intent);
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