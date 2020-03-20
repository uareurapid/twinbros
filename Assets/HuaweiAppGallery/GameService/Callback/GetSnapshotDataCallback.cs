using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetSnapshotDataCallback : AndroidJavaProxy
    {
        private IGetSnapshotDataListener _listener;

        public GetSnapshotDataCallback(IGetSnapshotDataListener listener) : base(
            "com.unity.udp.extension.sdk.games.extra.ExtrasCallback$OnGetSnapshotData")
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
                SnapshotData snapshotData = new SnapshotData();
                snapshotData.SnapshotId = jo.Call<string>("getSnapshotId");
                snapshotData.UniqueName = jo.Call<string>("getUniqueName");
                AndroidJavaObject playerJo = jo.Call<AndroidJavaObject>("getPlayer");
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
                    snapshotData.Player = player;
                }

                snapshotData.PlayedTime = jo.Call<long>("getPlayedTime");
                snapshotData.ProgressValue = jo.Call<long>("getProgressValue");
                snapshotData.CoverImageAspectRatio = jo.Call<float>("getCoverImageAspectRatio");
                snapshotData.HasCoverImage = jo.Call<bool>("isHasCoverImage");
                snapshotData.Description = jo.Call<string>("getDescription");
                AndroidJavaObject gameJo = jo.Call<AndroidJavaObject>("getGame");
                if (gameJo != null)
                {
                    Game game = new Game();
                    game.AchievementTotalCount = gameJo.Call<int>("getAchievementTotalCount");
                    game.ApplicationId = gameJo.Call<string>("getApplicationId");
                    game.Description = gameJo.Call<string>("getDescription");
                    game.DisplayName = gameJo.Call<string>("getDisplayName");
                    AndroidJavaObject hiResImageJo = gameJo.Call<AndroidJavaObject>("getHiResImageUri");
                    if (hiResImageJo != null)
                    {
                        game.HiResImageUri = hiResImageJo.Call<string>("toString");
                    }

                    AndroidJavaObject iconImageJo = gameJo.Call<AndroidJavaObject>("getIconImageUri");
                    if (iconImageJo != null)
                    {
                        game.IconImageUri = iconImageJo.Call<string>("toString");
                    }

                    game.LeaderboardCount = gameJo.Call<int>("getLeaderboardCount");
                    game.PrimaryCategory = gameJo.Call<string>("getPrimaryCategory");
                    game.SecondaryCategory = gameJo.Call<string>("getSecondaryCategory");
                    snapshotData.Game = game;
                }

                snapshotData.LastModifiedTimestamp = jo.Call<long>("getLastModifiedTimestamp");
                _listener.OnSuccess(snapshotData);
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