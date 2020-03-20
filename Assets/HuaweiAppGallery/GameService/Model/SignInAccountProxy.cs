namespace UnityEngine.HuaweiAppGallery.Model
{
    public class SignInAccountProxy
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string Uid { get; set; }
        public string OpenId { get; set; }
        public string UnionId { get; set; }
        public string IdToken { get; set; }
        public string PhotoUriString { get; set; }
        public string AccessToken { get; set; }
        public string ServerAuthCode { get; set; }
        public int Status { get; set; }
        public int Gender { get; set; }
        public string CountryCode { get; set; }
        public string ServiceCountryCode { get; set; }
        public long ExpirationTimeSecs { get; set; }
        public string AgeRange { get; set; }
    }
}