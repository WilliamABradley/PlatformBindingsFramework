using System;
using System.Threading.Tasks;
using PlatformBindings.Models;
using PlatformBindings.Common;
using Android.Content;
using PlatformBindings.Models.OAuth;

namespace PlatformBindings.Services
{
    public class AndroidOAuthBroker : IOAuthBroker
    {
        public async Task<OAuthResponse> Authenticate(Uri RequestUri, Uri CallbackUri)
        {
            var activity = AndroidHelpers.GetCurrentActivity();

            var intent = new Intent(activity, typeof(AuthenticationActivity));
            intent.PutExtra("Uri", RequestUri.ToString());
            intent.PutExtra("Callback", CallbackUri.ToString());

            var result = await activity.StartActivityForResultAsync(intent);

            var response = new OAuthResponse();

            if (result.ResultCode == Android.App.Result.Ok)
            {
                var success = result.Data.GetBooleanExtra("Success", false);
                response.Result = success ? Enums.OAuthResult.Success : Enums.OAuthResult.Failed;
                response.Data = result.Data.GetStringExtra("Data");
            }
            else response.Result = Enums.OAuthResult.Cancelled;

            return response;
        }
    }
}