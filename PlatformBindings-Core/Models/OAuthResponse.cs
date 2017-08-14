using PlatformBindings.Enums;

namespace PlatformBindings.Models
{
    public class OAuthResponse
    {
        public OAuthResult Result { get; set; }
        public string Data { get; set; }
        public uint StatusCode { get; set; }
    }
}