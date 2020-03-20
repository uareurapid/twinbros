using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetLeaderboardListener
    {
        void OnSuccess(LeaderboardProxy leaderboardProxy);
        
        void OnFailure(int code, string message);
    }
}