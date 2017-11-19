
using Android.App;
using Android.OS;
using PlatformBindings.Activities;
using Tests.Tests;
using Test_Android.Services;

namespace Test_Android.Views
{
    [Activity(Label = "Setting Tests")]
    public class SettingTests : PlatformBindingActivity
    {
        public SettingTestPage Viewmodel { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Viewmodel = new SettingTestPage(new AndroidTestPageGenerator(this));
            Viewmodel.DisplayTests();
        }
    }
}