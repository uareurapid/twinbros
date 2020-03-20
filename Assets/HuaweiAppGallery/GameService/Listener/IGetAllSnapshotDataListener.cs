using System.Collections.Generic;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetAllSnapshotDataListener
    {
        void OnSuccess(List<SnapshotData> allSnapshotData);
        
        void OnFailure(int code, string message);
    }
}