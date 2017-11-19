using Android.App;
using Android.OS;
using Tests.Tests;
using PlatformBindings.Activities;
using Test_Android.Services;

namespace Test_Android.Views
{
    [Activity(Label = "LoopTests")]
    public class LoopTests : PlatformBindingActivity
    {
        public LoopTimerTestPage Viewmodel { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Viewmodel = new LoopTimerTestPage(new AndroidTestPageGenerator(this));
            Viewmodel.DisplayTests();
        }
    }
}