using System.Collections.Generic;
using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetLeaderboardScoresCallback : AndroidJavaProxy
    {
        private IGetLeaderboardScoresListener _listener;

        public GetLeaderboardScoresCallback(IGetLeaderboardScoresListener listener) : base(
            "com.unity.udp.extension.sdk.games.leaderboard.LeaderboardsCallback$OnGetLeaderboardScores")
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
                LeaderboardScores entity = new LeaderboardScores();
                AndroidJavaObject leaderboardJo = jo.Call<AndroidJavaObject>("getLeaderboard");

                LeaderboardProxy leaderboardProxy = new LeaderboardProxy();
                leaderboardProxy.LeaderboardId = leaderboardJo.Call<string>("getLeaderboardId");
                leaderboardProxy.LeaderboardDisplayName = leaderboardJo.Call<string>("getLeaderboardDisplayName");
                AndroidJavaObject leaderboardImageJo = leaderboardJo.Call<AndroidJavaObject>("getLeaderboardImageUri");
                if (leaderboardImageJo != null)
                {
                    leaderboardProxy.LeaderboardImageUri = leaderboardImageJo.Call<string>("toString");
                }

                leaderboardProxy.LeaderboardScoreOrder = leaderboardJo.Call<int>("getLeaderboardScoreOrder");
                AndroidJavaObject variantsJo = leaderboardJo.Call<AndroidJavaObject>("getLeaderboardVariants");
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

                entity.LeaderboardProxy = leaderboardProxy;

                AndroidJavaObject leaderboardScores = jo.Call<AndroidJavaObject>("getLeaderboardScores");
                if (leaderboardScores != null)
                {
                    int size = leaderboardScores.Call<int>("size");
                    List<LeaderboardScore> list = new List<LeaderboardScore>();
                    for (int i = 0; i < size; i++)
                    {
                        AndroidJavaObject leaderboardScoreJo = leaderboardScores.Call<AndroidJavaObject>("get", i);
                        LeaderboardScore leaderboardScore = new LeaderboardScore();
                        leaderboardScore.DisplayRank = leaderboardScoreJo.Call<string>("getDisplayRank");
                        leaderboardScore.LeaderboardDisplayScore = leaderboardScoreJo.Call<string>("getLeaderboardDisplayScore");
                        leaderboardScore.TimeSpan = leaderboardScoreJo.Call<int>("getTimeSpan");
                        leaderboardScore.PlayerRank = leaderboardScoreJo.Call<long>("getPlayerRank");
                        leaderboardScore.PlayerRawScore = leaderboardScoreJo.Call<long>("getPlayerRawScore");
                        AndroidJavaObject playerJo = leaderboardScoreJo.Call<AndroidJavaObject>("getScoreOwnerPlayer");
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

                        leaderboardScore.ScoreTag = leaderboardScoreJo.Call<string>("getScoreTag");
                        leaderboardScore.ScoreTimestamp = leaderboardScoreJo.Call<long>("getScoreTimestamp");
                        list.Add(leaderboardScore);
                    }

                    entity.LeaderboardScoreList = list;
                }

                _listener.OnSuccess(entity);
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