using Android.App;
using Android.OS;
using Test_Android.Views;
using PlatformBindings;
using PlatformBindings.Activities;

namespace Test_Android
{
    [Activity(Label = "Test_Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : PlatformBindingActivity
    {
        public static AndroidAppServices Services { get; private set; }

        public MainActivity()
        {
            Services = new AndroidAppServices(true);
        }

        protected override void OnCreate(Bundle bundle)
        {
            // Build App Services before calling base, to allow binding.
            base.OnCreate(bundle);
            Tests.Preparation.Prepare();

            StartActivity(typeof(BindingTests));
            Finish();
        }
    }
}