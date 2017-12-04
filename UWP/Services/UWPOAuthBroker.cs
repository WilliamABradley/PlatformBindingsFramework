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
using PlatformBindings.Enums;
using PlatformBindings.Models;
using Windows.Security.Authentication.Web;
using PlatformBindings.Common;

namespace PlatformBindings.Services.Bindings
{
    public class UWPOAuthBroker : IOAuthBroker
    {
        public async Task<OAuthResponse> Authenticate(Uri RequestUri, Uri CallbackUri)
        {
            TaskCompletionSource<WebAuthenticationResult> waiter = new TaskCompletionSource<WebAuthenticationResult>();
            PlatformBindingHelpers.OnUIThread(async () =>
            {
                try
                {
                    var response = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, RequestUri, CallbackUri);
                    waiter.TrySetResult(response);
                }
                catch
                {
                    waiter.TrySetResult(null);
                }
            });

            var result = await waiter.Task;
            if (result == null) return new OAuthResponse { Result = OAuthResult.Failed };
            OAuthResult resultValue = OAuthResult.Failed;
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    resultValue = OAuthResult.Success;
                    break;

                case WebAuthenticationStatus.UserCancel:
                    resultValue = OAuthResult.Cancelled;
                    break;
            }

            return new OAuthResponse
            {
                Result = resultValue,
                Data = !string.IsNullOrWhiteSpace(result.ResponseData) ? result.ResponseData : null
            };
        }
    }
}