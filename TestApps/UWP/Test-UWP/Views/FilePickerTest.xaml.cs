using Test_UWP.Services;
using Tests.Tests;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tests_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FilePickerTest : Page
    {
        public PickerTestPage Viewmodel { get; }

        public FilePickerTest()
        {
            this.InitializeComponent();
            Viewmodel = new PickerTestPage(new UWPTestPageGenerator(this));
            Viewmodel.DisplayTests();
        }
    }
}