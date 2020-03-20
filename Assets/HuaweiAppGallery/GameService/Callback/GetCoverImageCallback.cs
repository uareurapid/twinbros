using UnityEngine.HuaweiAppGallery.Listener;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetCoverImageCallback : AndroidJavaProxy
    {
        private IGetCoverImageListener _listener;

        public GetCoverImageCallback(IGetCoverImageListener listener) : base(
            "com.unity.udp.extension.sdk.games.extra.ExtrasCallback$OnGetCoverImage")
        {
            _listener = listener;
        }

        public void onSuccess(AndroidJavaObject coverImage)
        {
            if (coverImage == null)
            {
                return;
            }

            if (_listener != null)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity.udp.extension.sdk.utils.Utils");
                string coverImageString = jc.CallStatic<string>("convertBitmapToString", coverImage);
                _listener.OnSuccess(coverImageString);
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