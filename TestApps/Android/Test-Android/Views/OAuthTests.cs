
using Android.App;
using Android.OS;
using PlatformBindings.Activities;
using Tests.Tests;
using Test_Android.Services;

namespace Test_Android.Views
{
    [Activity(Label = "OAuth")]
    public class OAuthTests : PlatformBindingActivity
    {
        public OAuthTestPage Viewmodel { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Viewmodel = new OAuthTestPage(new AndroidTestPageGenerator(this));
            Viewmodel.DisplayTests();
        }
    }
}