using System.Collections.Generic;

namespace UnityEngine.HuaweiAppGallery.Model
{
    public class ScoreSubmission
    {
        public string LeaderboardId { get; set; }
        public string PlayerId { get; set; }
        public Dictionary<int, ScoreSubmission.Result> ScoreResults { get; set; }
        
        public class Result
        {
            public long RawScore { get; set; }
            public string FormattedScore { get; set; }
            public string ScoreTag { get; set; }
            public bool IsBest { get; set; }
        }
    }
}