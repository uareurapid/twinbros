using System.Collections.Generic;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetLeaderboardsListener
    {
        void OnSuccess(List<LeaderboardProxy> leaderboards);

        void OnFailure(int code, string message);
    }
}