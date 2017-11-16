using Tests.Tests;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tests_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContextMenuTest : Page
    {
        public ContextMenuTests Viewmodel { get; } = new ContextMenuTests();

        public ContextMenuTest()
        {
            this.InitializeComponent();
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var source = sender as FrameworkElement;
            Viewmodel.ShowMenu(new ContextMenuBinding(source, e.GetPosition(source)));
        }
    }
}