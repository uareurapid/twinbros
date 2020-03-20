using UnityEngine.HuaweiAppGallery.Listener;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class LimitSizeCallback : AndroidJavaProxy
    {
        private ILimitSizeListener _listener;

        public LimitSizeCallback(ILimitSizeListener listener) : base(
            "com.unity.udp.extension.sdk.games.extra.ExtrasCallback$OnLimitSize")
        {
            _listener = listener;
        }

        public void onSuccess(int limitSize)
        {
            if (_listener != null)
            {
                _listener.OnSuccess(limitSize);
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