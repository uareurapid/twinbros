using UnityEngine.HuaweiAppGallery.Listener;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class LeaderboardSwitchStatusCallback : AndroidJavaProxy
    {
        private ILeaderboardSwitchStatusListener _listener;

        public LeaderboardSwitchStatusCallback(ILeaderboardSwitchStatusListener listener) : base(
            "com.unity.udp.extension.sdk.games.leaderboard.LeaderboardsCallback$OnLeaderboardSwitchStatus")
        {
            _listener = listener;
        }

        public void onSuccess(int statusValue)
        {
            if (_listener != null)
            {
                _listener.OnSuccess(statusValue);
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