namespace UnityEngine.HuaweiAppGallery.Model
{
    public class Achievement
    {
        public string AchievementId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnlockedImageUrl { get; set; }
        public string RevealedImageUrl { get; set; }
        public int TotalSteps { get; set; }
        public int CurrentSteps { get; set; }
        public int State { get; set; }
        public long LastUpdatedTimestamp { get; set; }
    }
}