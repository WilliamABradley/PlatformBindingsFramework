using Android.App;
using Android.OS;
using PlatformBindings.Activities;
using Test_Android.Services;
using Tests.Tests;

namespace Test_Android.Views
{
    [Activity(Label = "FilePickerTest")]
    public class FilePickerTest : PlatformBindingActivity
    {
        public PickerTestPage Viewmodel { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Viewmodel = new PickerTestPage(new AndroidTestPageGenerator(this));
            Viewmodel.DisplayTests();
        }
    }
}