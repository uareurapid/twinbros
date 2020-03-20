namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IDeleteSnapshotListener
    {
        void OnSuccess(string snapshotId);
        
        void OnFailure(int code, string message);
    }
}