using UnityEngine.HuaweiAppGallery.Listener;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class DeleteSnapshotCallback : AndroidJavaProxy
    {
        private IDeleteSnapshotListener _listener;

        public DeleteSnapshotCallback(IDeleteSnapshotListener listener) : base(
            "com.unity.udp.extension.sdk.games.extra.ExtrasCallback$OnDeleteSnapshot")
        {
            _listener = listener;
        }

        public void onSuccess(string snapshotId)
        {
            if (snapshotId == null)
            {
                return;
            }

            if (_listener != null)
            {
                _listener.OnSuccess(snapshotId);
            }
        }

        public void onFailure(AndroidJavaObject exception, AndroidJavaObject result)
        {
            if (_listener != null && result != null)
            {
                _listener.OnFailure(result.Call<int>("getCode"), result.Call<string>("getMessage"));
            }
        }
    }
}