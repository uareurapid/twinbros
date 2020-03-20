using UnityEngine.HuaweiAppGallery.Listener;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetLeaderboardIntentCallback : AndroidJavaProxy
    {
        private IGetLeaderboardIntentListener _listener;

        public GetLeaderboardIntentCallback(IGetLeaderboardIntentListener listener) : base(
            "com.unity.udp.extension.sdk.games.leaderboard.LeaderboardsCallback$OnGetLeaderboardIntent")
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