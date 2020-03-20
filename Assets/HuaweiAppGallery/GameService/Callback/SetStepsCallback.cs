using UnityEngine.HuaweiAppGallery.Listener;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class SetStepsCallback : AndroidJavaProxy
    {
        private ISetStepsListener _listener;

        public SetStepsCallback(ISetStepsListener listener) : base(
            "com.unity.udp.extension.sdk.games.achievement.AchievementsCallback$OnSetSteps")
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