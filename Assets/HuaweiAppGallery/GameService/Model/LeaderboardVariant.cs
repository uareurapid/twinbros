namespace UnityEngine.HuaweiAppGallery.Model
{
    public class LeaderboardVariant
    {
        public string DisplayPlayerScore { get; set; }
        public string DisplayPlayerRank { get; set; }
        public string PlayerScoreTag { get; set; }
        public long NumScores { get; set; }
        public long PlayerRank { get; set; }
        public long RawPlayerScore { get; set; }
        public int TimeSpan { get; set; }
        public Player PlayerInfo { get; set; }
        public bool HasPlayerInfo { get; set; }
    }
}