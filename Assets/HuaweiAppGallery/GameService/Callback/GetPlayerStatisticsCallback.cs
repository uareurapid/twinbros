using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetPlayerStatisticsCallback : AndroidJavaProxy
    {
        private IGetPlayerStatisticsListener _listener;

        public GetPlayerStatisticsCallback(IGetPlayerStatisticsListener listener) : base(
            "com.unity.udp.extension.sdk.games.extra.ExtrasCallback$OnGetPlayerStatistics")
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
                PlayerStatistics playerStatistics = new PlayerStatistics();
                playerStatistics.AverageSessionLength = jo.Call<float>("getAverageSessionLength");
                playerStatistics.DaysSinceLastPlayed = jo.Call<int>("getDaysSinceLastPlayed");
                playerStatistics.NumberOfSessions = jo.Call<int>("getNumberOfSessions");
                playerStatistics.NumberOfPurchases = jo.Call<int>("getNumberOfPurchases");
                playerStatistics.TotalPurchasesAmountRange = jo.Call<int>("getTotalPurchasesAmountRange");
                _listener.OnSuccess(playerStatistics);
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