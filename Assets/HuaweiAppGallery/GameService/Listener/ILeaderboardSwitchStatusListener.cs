namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface ILeaderboardSwitchStatusListener
    {
        void OnSuccess(int statusValue);
        
        void OnFailure(int code, string message);
    }
}