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

using PlatformBindings;
using System;
using System.Threading.Tasks;
using Tests.TestGenerator;

namespace Tests.Tests
{
    public class OAuthTestPage : TestPage
    {
        public OAuthTestPage(ITestPageGenerator PageGenerator) : base("OAuth Tests", PageGenerator)
        {
            AddTestItem(new TestTask
            {
                Name = "Test OAuth Service",
                Test = ui => Task.Run(async () =>
                {
                    var result = await AppServices.Current.OAuth.Authenticate(OAuthAuthorizationUri, new Uri(Redirect));
                    return $"Result: {result.Result}\nData: {result.Data}";
                })
            });
        }

        public Uri OAuthAuthorizationUri => new Uri($"Https://trakt.tv/oauth/authorize?response_type=code&client_id={ClientID}&redirect_uri={Redirect}&state={AntiForgeryToken}");

        public string ClientID => "3f3c1d9fd5efda2308b2d69e29cf2005fc3a93860656c89b28ec51424a030a46";
        public string Redirect => "https://zapp.com";
        private string AntiForgeryToken = Guid.NewGuid().ToString();
    }
}