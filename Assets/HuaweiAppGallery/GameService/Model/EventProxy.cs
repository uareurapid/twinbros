namespace UnityEngine.HuaweiAppGallery.Model
{
    public class EventProxy
    {
        public string EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ThumbnailUri { get; set; }
        public long Value { get; set; }
        public string LocaleValue { get; set; }
        public bool IsVisible { get; set; }
        public Player Player { get; set; }
    }
}