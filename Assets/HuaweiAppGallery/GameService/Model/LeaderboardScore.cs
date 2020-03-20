namespace UnityEngine.HuaweiAppGallery.Model
{
    public class LeaderboardScore
    {
        public string DisplayRank { get; set; }
        public string LeaderboardDisplayScore { get; set; }
        public int TimeSpan { get; set; }
        public long PlayerRank { get; set; }
        public long PlayerRawScore { get; set; }
        public Player ScoreOwnerPlayer { get; set; }
        public string ScoreTag { get; set; }
        public long ScoreTimestamp { get; set; }
    }
}