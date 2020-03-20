using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetLeaderboardScoreListener
    {
        void OnSuccess(LeaderboardScore leaderboardScore);

        void OnFailure(int code, string message);
    }
}