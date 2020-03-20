using System.Collections.Generic;

namespace UnityEngine.HuaweiAppGallery.Model
{
    public class LeaderboardScores
    {
        public LeaderboardProxy LeaderboardProxy { get; set; }
        public List<LeaderboardScore> LeaderboardScoreList { get; set; }
    }
}