using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class LoginCallback : AndroidJavaProxy
    {
        private ILoginListener _listener;

        public LoginCallback(ILoginListener listener) : base("com.unity.udp.extension.sdk.games.LoginCallback")
        {
            this._listener = listener;
        }

        public void onSuccess(AndroidJavaObject jo)
        {
            if (_listener != null && jo != null)
            {
                SignInAccountProxy signInAccountProxy = new SignInAccountProxy();
                signInAccountProxy.DisplayName = jo.Call<string>("getDisplayName");
                signInAccountProxy.Email = jo.Call<string>("getEmail");
                signInAccountProxy.FamilyName = jo.Call<string>("getFamilyName");
                signInAccountProxy.GivenName = jo.Call<string>("getGivenName");
                signInAccountProxy.Uid = jo.Call<string>("getUid");
                signInAccountProxy.OpenId = jo.Call<string>("getOpenId");
                signInAccountProxy.UnionId = jo.Call<string>("getUnionId");
                signInAccountProxy.IdToken = jo.Call<string>("getIdToken");
                signInAccountProxy.PhotoUriString = jo.Call<string>("getPhotoUriString");
                signInAccountProxy.AccessToken = jo.Call<string>("getAccessToken");
                signInAccountProxy.ServerAuthCode = jo.Call<string>("getServerAuthCode");
                signInAccountProxy.Status = jo.Call<int>("getStatus");
                signInAccountProxy.Gender = jo.Call<int>("getGender");
                signInAccountProxy.CountryCode = jo.Call<string>("getCountryCode");
                signInAccountProxy.ServiceCountryCode = jo.Call<string>("getServiceCountryCode");
                signInAccountProxy.ExpirationTimeSecs = jo.Call<long>("getExpirationTimeSecs");
                signInAccountProxy.AgeRange = jo.Call<string>("getAgeRange");
                _listener.OnSuccess(signInAccountProxy);
            }
        }

        public void onSignOut()
        {
            if (_listener != null)
            {
                _listener.OnSignOut();
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