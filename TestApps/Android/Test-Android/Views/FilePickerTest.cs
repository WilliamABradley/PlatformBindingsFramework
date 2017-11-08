using Android.App;
using Android.OS;
using Android.Widget;
using PlatformBindings.Activities;
using Tests.Tests;

namespace Test_Android.Views
{
    [Activity(Label = "FilePickerTest")]
    public class FilePickerTest : PlatformBindingActivity
    {
        public PickerTests Viewmodel { get; } = new PickerTests();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Pickers);

            FindViewById<Button>(Resource.Id.PickFileButton).Click += delegate { Viewmodel.PickFile(); };
            FindViewById<Button>(Resource.Id.PickFilesButton).Click += delegate { Viewmodel.PickFiles(); };
            FindViewById<Button>(Resource.Id.PickFolderButton).Click += delegate { Viewmodel.PickFolder(); };
        }
    }
}