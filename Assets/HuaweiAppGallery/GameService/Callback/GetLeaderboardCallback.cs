using System.Collections.Generic;
using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetLeaderboardCallback : AndroidJavaProxy
    {
        private IGetLeaderboardListener _listener;

        public GetLeaderboardCallback(IGetLeaderboardListener listener) : base(
            "com.unity.udp.extension.sdk.games.leaderboard.LeaderboardsCallback$OnGetLeaderboard")
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
                LeaderboardProxy leaderboardProxy = new LeaderboardProxy();
                leaderboardProxy.LeaderboardId = jo.Call<string>("getLeaderboardId");
                leaderboardProxy.LeaderboardDisplayName = jo.Call<string>("getLeaderboardDisplayName");
                AndroidJavaObject leaderboardImageJo = jo.Call<AndroidJavaObject>("getLeaderboardImageUri");
                if (leaderboardImageJo != null)
                {
                    leaderboardProxy.LeaderboardImageUri = leaderboardImageJo.Call<string>("toString");
                }

                leaderboardProxy.LeaderboardScoreOrder = jo.Call<int>("getLeaderboardScoreOrder");
                AndroidJavaObject variantsJo = jo.Call<AndroidJavaObject>("getLeaderboardVariants");
                if (variantsJo != null)
                {
                    int variantsSize = variantsJo.Call<int>("size");
                    List<LeaderboardVariant> variantList = new List<LeaderboardVariant>();
                    for (int j = 0; j < variantsSize; j++)
                    {
                        AndroidJavaObject variantJo = variantsJo.Call<AndroidJavaObject>("get", j);
                        LeaderboardVariant variant = new LeaderboardVariant();
                        variant.DisplayPlayerScore = variantJo.Call<string>("getDisplayPlayerScore");
                        variant.DisplayPlayerRank = variantJo.Call<string>("getDisplayPlayerRank");
                        variant.PlayerScoreTag = variantJo.Call<string>("getPlayerScoreTag");
                        variant.NumScores = variantJo.Call<long>("getNumScores");
                        variant.PlayerRank = variantJo.Call<long>("getPlayerRank");
                        variant.RawPlayerScore = variantJo.Call<long>("getRawPlayerScore");
                        variant.TimeSpan = variantJo.Call<int>("getTimeSpan");
                        variant.HasPlayerInfo = variantJo.Call<bool>("hasPlayerInfo");
                        variantList.Add(variant);
                    }

                    leaderboardProxy.LeaderboardVariants = variantList;
                }
                _listener.OnSuccess(leaderboardProxy);
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