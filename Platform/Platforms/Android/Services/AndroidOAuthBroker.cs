// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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