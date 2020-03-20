using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetPlayerStatisticsListener
    {
        void OnSuccess(PlayerStatistics playerStatistics);

        void OnFailure(int code, string message);
    }
}