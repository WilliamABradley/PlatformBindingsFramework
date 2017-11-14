using Android.App;
using Android.OS;
using Android.Widget;
using PlatformBindings.Activities;
using Tests.Tests;

namespace Test_Android.Views
{
    [Activity]
    public class FileTests : PlatformBindingActivity
    {
        public FileFolderTests Viewmodel { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Viewmodel = new FileFolderTests();
            var view = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical
            };

            var testGetItems = new Button(this)
            {
                Text = "Get Directory Contents",
            };
            testGetItems.Click += delegate { Viewmodel.PickFolderGetItems(); };

            var testSubSubFolders = new Button(this)
            {
                Text = "Get Sub Folders Sub Folders"
            };
            testSubSubFolders.Click += delegate { Viewmodel.PickFolderGetSubFolderFolders(); };

            var testCreateFile = new Button(this)
            {
                Text = "Create Test Text File"
            };
            testCreateFile.Click += delegate { Viewmodel.PickFolderCreateFile(); };

            var testReadText = new Button(this)
            {
                Text = "Read Text From File"
            };
            testReadText.Click += delegate { Viewmodel.PickFileReadText(); };

            view.AddView(testGetItems);
            view.AddView(testSubSubFolders);
            view.AddView(testCreateFile);
            view.AddView(testReadText);
            SetContentView(view);
        }
    }
}