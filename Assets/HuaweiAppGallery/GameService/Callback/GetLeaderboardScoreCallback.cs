using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetLeaderboardScoreCallback : AndroidJavaProxy
    {
        private IGetLeaderboardScoreListener _listener;

        public GetLeaderboardScoreCallback(IGetLeaderboardScoreListener listener) : base(
            "com.unity.udp.extension.sdk.games.leaderboard.LeaderboardsCallback$OnGetLeaderboardScore")
        {
            _listener = listener;
        }

        public void onSuccess(AndroidJavaObject jo)
        {
            if (jo == null)
            {
                return;
            }

            if (_listener != null)
            {
                LeaderboardScore leaderboardScore = new LeaderboardScore();
                leaderboardScore.DisplayRank = jo.Call<string>("getDisplayRank");
                leaderboardScore.LeaderboardDisplayScore = jo.Call<string>("getLeaderboardDisplayScore");
                leaderboardScore.TimeSpan = jo.Call<int>("getTimeSpan");
                leaderboardScore.PlayerRank = jo.Call<long>("getPlayerRank");
                leaderboardScore.PlayerRawScore = jo.Call<long>("getPlayerRawScore");
                AndroidJavaObject playerJo = jo.Call<AndroidJavaObject>("getScoreOwnerPlayer");
                if (playerJo != null)
                {
                    Player player = new Player();
                    player.DisplayName = playerJo.Call<string>("getDisplayName");
                    AndroidJavaObject hiResImageJo = playerJo.Call<AndroidJavaObject>("getHiResImageUri");
                    if (hiResImageJo != null)
                    {
                        player.HiResImageUri = hiResImageJo.Call<string>("toString");
                    }

                    AndroidJavaObject iconImageJo = playerJo.Call<AndroidJavaObject>("getIconImageUri");
                    if (iconImageJo != null)
                    {
                        player.IconImageUri = iconImageJo.Call<string>("toString");
                    }

                    player.PlayerId = playerJo.Call<string>("getPlayerId");
                    player.SignTimestamp = playerJo.Call<string>("getSignTimestamp");
                    player.PlayerSign = playerJo.Call<string>("getPlayerSign");
                    player.Level = playerJo.Call<int>("getLevel");
                    leaderboardScore.ScoreOwnerPlayer = player;
                }

                leaderboardScore.ScoreTag = jo.Call<string>("getScoreTag");
                leaderboardScore.ScoreTimestamp = jo.Call<long>("getScoreTimestamp");
                _listener.OnSuccess(leaderboardScore);
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