using Android.App;
using Android.OS;
using PlatformBindings.Activities;
using Test_Android.Services;
using Tests.Tests;

namespace Test_Android.Views
{
    [Activity(Label = "File/Folder Tests")]
    public class FileTests : PlatformBindingActivity
    {
        public FileFolderTestPage Viewmodel { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Viewmodel = new FileFolderTestPage(new AndroidTestPageGenerator(this));
            Viewmodel.DisplayTests();
        }
    }
}