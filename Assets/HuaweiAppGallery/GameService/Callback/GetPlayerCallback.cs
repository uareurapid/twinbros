using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetPlayerCallback : AndroidJavaProxy
    {
        private IGetPlayerListener _listener;

        public GetPlayerCallback(IGetPlayerListener listener) : base(
            "com.unity.udp.extension.sdk.games.extra.ExtrasCallback$OnGetPlayer")
        {
            this._listener = listener;
        }

        public void onSuccess(AndroidJavaObject jo)
        {
            if (_listener != null && jo != null)
            {
                Player player = new Player();
                player.DisplayName = jo.Call<string>("getDisplayName");
                AndroidJavaObject hiResImageJo = jo.Call<AndroidJavaObject>("getHiResImageUri");
                if (hiResImageJo != null)
                {
                    player.HiResImageUri = hiResImageJo.Call<string>("toString");
                }

                AndroidJavaObject iconImageJo = jo.Call<AndroidJavaObject>("getIconImageUri");
                if (iconImageJo != null)
                {
                    player.IconImageUri = iconImageJo.Call<string>("toString");
                }

                player.PlayerId = jo.Call<string>("getPlayerId");
                player.SignTimestamp = jo.Call<string>("getSignTimestamp");
                player.PlayerSign = jo.Call<string>("getPlayerSign");
                player.Level = jo.Call<int>("getLevel");
                _listener.OnSuccess(player);
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