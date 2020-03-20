using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetSnapshotResultListener
    {
        void OnSuccess(SnapshotResult snapshotResult);

        void OnFailure(int code, string message);
    }
}