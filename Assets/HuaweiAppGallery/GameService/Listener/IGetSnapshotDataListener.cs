using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetSnapshotDataListener
    {
        void OnSuccess(SnapshotData snapshotData);

        void OnFailure(int code, string message);
    }
}