namespace UnityEngine.HuaweiAppGallery.Model
{
    public interface ILeaderboardVariant
    {
        string GetDisplayPlayerScore();

        string GetDisplayPlayerRank();

        string GetPlayerScoreTag();

        long GetNumScores();

        long GetPlayerRank();

        long GetRawPlayerScore();

        int GetTimeSpan();

        bool HasPlayerInfo();
    }
}