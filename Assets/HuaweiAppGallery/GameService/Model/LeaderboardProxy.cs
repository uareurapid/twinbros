using System.Collections.Generic;

namespace UnityEngine.HuaweiAppGallery.Model
{
    public class LeaderboardProxy
    {
        public string LeaderboardId { get; set; }
        public string LeaderboardDisplayName { get; set; }
        public string LeaderboardImageUri { get; set; }
        public int LeaderboardScoreOrder { get; set; }
        public List<LeaderboardVariant> LeaderboardVariants { get; set; }
    }
}