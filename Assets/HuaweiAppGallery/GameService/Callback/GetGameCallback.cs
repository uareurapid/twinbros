using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetGameCallback : AndroidJavaProxy
    {
        private IGetGameListener _listener;

        public GetGameCallback(IGetGameListener listener) : base(
            "com.unity.udp.extension.sdk.games.extra.ExtrasCallback$OnGetGame")
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
                Game game = new Game();
                game.AchievementTotalCount = jo.Call<int>("getAchievementTotalCount");
                game.ApplicationId = jo.Call<string>("getApplicationId");
                game.Description = jo.Call<string>("getDescription");
                game.DisplayName = jo.Call<string>("getDisplayName");
                AndroidJavaObject hiResImageJo = jo.Call<AndroidJavaObject>("getHiResImageUri");
                if (hiResImageJo != null)
                {
                    game.HiResImageUri = hiResImageJo.Call<string>("toString");
                }

                AndroidJavaObject iconImageJo = jo.Call<AndroidJavaObject>("getIconImageUri");
                if (iconImageJo != null)
                {
                    game.IconImageUri = iconImageJo.Call<string>("toString");
                }

                game.LeaderboardCount = jo.Call<int>("getLeaderboardCount");
                game.PrimaryCategory = jo.Call<string>("getPrimaryCategory");
                game.SecondaryCategory = jo.Call<string>("getSecondaryCategory");
                _listener.OnSuccess(game);
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