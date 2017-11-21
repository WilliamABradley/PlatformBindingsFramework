using Android.App;
using PlatformBindings.Activities;
using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Webkit;
using Android.Graphics;
using Android.Content;
using Android.Widget;

namespace PlatformBindings.Models.OAuth
{
    [Activity(Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    internal class AuthenticationActivity : PlatformBindingActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var uri = Intent.GetStringExtra("Uri");
            var callback = Intent.GetStringExtra("Callback");

            WebView = new WebView(this);
            var callbackClient = new CallBackWebClient(callback);
            WebView.SetWebViewClient(callbackClient);
            WebView.LoadUrl(uri);

            SetContentView(WebView);

            callbackClient.Waiter.Task.ContinueWith(result =>
            {
                var results = result.Result;

                Intent.PutExtra("Success", results.Success);
                Intent.PutExtra("Data", results.Data);

                SetResult(Result.Ok, Intent);
                Finish();
            });
        }

        public WebView WebView { get; private set; }

        private class OAuthResponseMessage
        {
            public bool Success { get; set; }
            public string Data { get; set; }
        }

        private class CallBackWebClient : WebViewClient
        {
            public CallBackWebClient(string CallbackUrl)
            {
                this.CallbackUrl = new Uri(CallbackUrl);
            }

            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                base.OnPageStarted(view, url, favicon);
                Timeout = true;

                if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                {
                    if (uri.IsBaseOf(CallbackUrl))
                    {
                        Waiter.TrySetResult(new OAuthResponseMessage { Data = url, Success = true });
                    }
                }

                Task.Delay(AndroidAppServices.OAuthTimeOut).ContinueWith(task =>
                {
                    if (Timeout)
                    {
                        Fail();
                    }
                });
            }

            public override void OnPageFinished(WebView view, string url)
            {
                base.OnPageFinished(view, url);
                Timeout = false;
            }

            public override void OnReceivedError(WebView view, IWebResourceRequest request, WebResourceError error)
            {
                base.OnReceivedError(view, request, error);
                Fail();
            }

            private void Fail()
            {
                Waiter.TrySetResult(new OAuthResponseMessage { Success = false });
            }

            private Uri CallbackUrl { get; }
            private bool Timeout = false;

            public TaskCompletionSource<OAuthResponseMessage> Waiter { get; } = new TaskCompletionSource<OAuthResponseMessage>();
        }
    }
}