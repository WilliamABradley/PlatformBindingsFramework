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