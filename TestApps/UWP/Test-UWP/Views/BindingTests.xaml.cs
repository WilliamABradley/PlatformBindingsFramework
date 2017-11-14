using PlatformBindings;
using Test_UWP.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tests_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BindingTests : Page
    {
        public BindingTests()
        {
            this.InitializeComponent();
        }

        private void ContextTest_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ContextMenuTest));
            ShowBack();
        }

        private void ShowBack()
        {
            AppServices.UI.NavigationManager.ShowBackButton = true;
        }

        private void PickerTest_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FilePickerTest));
            ShowBack();
        }

        private void LoopTest_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoopTests));
            ShowBack();
        }

        private void FileTest_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FileTests));
            ShowBack();
        }
    }
}