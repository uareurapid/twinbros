using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetLeaderboardScoresListener
    {
        void OnSuccess(LeaderboardScores leaderboardScores);

        void OnFailure(int code, string message);
    }
}