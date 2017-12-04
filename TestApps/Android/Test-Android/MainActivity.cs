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