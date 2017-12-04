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

using Android.App;
using Android.OS;
using Test_Android.Views;
using PlatformBindings;
using PlatformBindings.Activities;
using Tests;
using Test_Android.Services;
using PlatformBindings.Services;
using Tests.Tests;
using Tests.TestGenerator;
using System.Threading.Tasks;
using PlatformBindings.Common;

namespace Test_Android
{
    [Activity(Label = "Platform Bindings Test App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : PlatformBindingActivity
    {
        public static AndroidAppServices Services { get; private set; } = new AndroidAppServices(true);

        protected override void OnCreate(Bundle bundle)
        {
            // Build App Services before calling base, to allow binding.
            base.OnCreate(bundle);
            TestService.Register(new TestAndroidNavigator());
            AppServices.Current.UI.NavigationManager = new AndroidActivityNavigationManager(TestService.Navigation);

            ExtendPlatformTasks();

            TestService.Navigation.Navigate(TestNavigationPage.Home);
            Finish();
        }

        private void ExtendPlatformTasks()
        {
            TestService.AddAddtionalTestItem(typeof(NavigationOptions), new TestTask
            {
                Name = "Test StartActivityForResultAsync",
                Test = ui => Task.Run(async () =>
                {
                    var result = await AndroidHelpers.GetCurrentActivity().StartActivityForResultAsync(typeof(ReturnActivity));
                    return $"RequestCode: {result.RequestCode}\nResponse: {result.ResultCode}";
                })
            });
        }
    }
}