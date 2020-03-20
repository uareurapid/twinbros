using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetSnapshotResultCallback : AndroidJavaProxy
    {
        private IGetSnapshotResultListener _listener;

        public GetSnapshotResultCallback(IGetSnapshotResultListener listener) : base(
            "com.unity.udp.extension.sdk.games.extra.ExtrasCallback$OnGetSnapshotResult")
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
                SnapshotResult snapshotResult = new SnapshotResult();
                AndroidJavaObject snapshotJo = jo.Call<AndroidJavaObject>("getSnapshot");
                if (snapshotJo != null)
                {
                    snapshotResult.Snapshot = ParseSnapshot(snapshotJo);
                }

                AndroidJavaObject snapshotConflictJo = jo.Call<AndroidJavaObject>("getSnapshotConflict");
                if (snapshotConflictJo != null)
                {
                    SnapshotConflict snapshotConflict = new SnapshotConflict();
                    AndroidJavaObject serverSnapshotJo =
                        snapshotConflictJo.Call<AndroidJavaObject>("getServerSnapshot");
                    snapshotConflict.ServerSnapshot = ParseSnapshot(serverSnapshotJo);
                    AndroidJavaObject conflictSnapshotJo =
                        snapshotConflictJo.Call<AndroidJavaObject>("getConflictSnapshot");
                    snapshotConflict.ConflictSnapshot = ParseSnapshot(conflictSnapshotJo);
                    snapshotResult.SnapshotConflict = snapshotConflict;
                }

                _listener.OnSuccess(snapshotResult);
            }
        }

        public void onFailure(AndroidJavaObject exception, AndroidJavaObject result)
        {
            if (_listener != null && result != null)
            {
                _listener.OnFailure(result.Call<int>("getCode"), result.Call<string>("getMessage"));
            }
        }

        private Snapshot ParseSnapshot(AndroidJavaObject snapshotJo)
        {
            if (snapshotJo == null)
            {
                return null;
            }

            Snapshot snapshot = new Snapshot();
            //snapshotData
            AndroidJavaObject snapshotDataJo = snapshotJo.Call<AndroidJavaObject>("getSnapshotData");
            if (snapshotDataJo != null)
            {
                SnapshotData snapshotData = new SnapshotData();
                snapshotData.SnapshotId = snapshotDataJo.Call<string>("getSnapshotId");
                snapshotData.UniqueName = snapshotDataJo.Call<string>("getUniqueName");
                AndroidJavaObject playerJo = snapshotDataJo.Call<AndroidJavaObject>("getPlayer");
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

                snapshotData.PlayedTime = snapshotDataJo.Call<long>("getPlayedTime");
                snapshotData.ProgressValue = snapshotDataJo.Call<long>("getProgressValue");
                snapshotData.CoverImageAspectRatio = snapshotDataJo.Call<float>("getCoverImageAspectRatio");
                snapshotData.HasCoverImage = snapshotDataJo.Call<bool>("isHasCoverImage");
                snapshotData.Description = snapshotDataJo.Call<string>("getDescription");
                AndroidJavaObject gameJo = snapshotDataJo.Call<AndroidJavaObject>("getGame");
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

                snapshotData.LastModifiedTimestamp = snapshotDataJo.Call<long>("getLastModifiedTimestamp");
                snapshot.SnapshotData = snapshotData;
            }

            //snapshotContent
            AndroidJavaObject snapshotContentJo = snapshotJo.Call<AndroidJavaObject>("getSnapshotContent");
            if (snapshotContentJo != null)
            {
                SnapshotContent snapshotContent = new SnapshotContent();
                snapshotContent.Content = snapshotContentJo.Call<byte[]>("getContent");
                snapshotContent.DstOffset = snapshotContentJo.Call<int>("getDstOffset");
                snapshotContent.SrcOffset = snapshotContentJo.Call<int>("getSrcOffset");
                snapshotContent.Count = snapshotContentJo.Call<int>("getCount");
                snapshot.SnapshotContent = snapshotContent;
            }

            return snapshot;
        }
    }
}