using System;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models;
using Windows.Security.Authentication.Web;

namespace PlatformBindings.Services.Bindings
{
    public class WinOAuthBroker : IOAuthBroker
    {
        public async Task<OAuthResponse> Authenticate(Uri RequestUri, Uri CallbackUri)
        {
            var broker = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, RequestUri, CallbackUri);
            OAuthResult Result = OAuthResult.Failed;

            switch (broker.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    Result = OAuthResult.Success;
                    break;

                case WebAuthenticationStatus.UserCancel:
                    Result = OAuthResult.Cancelled;
                    break;
            }

            return new OAuthResponse
            {
                Result = Result,
                Data = broker.ResponseData,
                StatusCode = broker.ResponseErrorDetail
            };
        }
    }
}