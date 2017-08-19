using Android.App;
using Android.OS;
using Android.Content;
using Test_Android.Views;
using PlatformBindings;
using PlatformBindings.Models.Settings;

namespace Test_Android
{
    [Activity(Label = "Test_Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : LibActivity
    {
        public static AndroidAppServices Services = new AndroidAppServices(true);

        protected override void OnCreate(Bundle bundle)
        {
            // Build App Services before calling base, to allow binding.
            base.OnCreate(bundle);

            var intent = new Intent(this, typeof(BindingTests));
            StartActivity(intent);

            TimesRan.Value++;

            Finish();
        }

        public AppSetting<int> TimesRan = new AppSetting<int>();
    }
}