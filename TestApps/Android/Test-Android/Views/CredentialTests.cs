
using Android.App;
using Android.OS;
using PlatformBindings.Activities;
using Tests.Tests;
using Test_Android.Services;

namespace Test_Android.Views
{
    [Activity(Label = "Credential Tests")]
    public class CredentialTests : PlatformBindingActivity
    {
        public CredentialTestPage Viewmodel { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Viewmodel = new CredentialTestPage(new AndroidTestPageGenerator(this));
            Viewmodel.DisplayTests();
        }
    }
}