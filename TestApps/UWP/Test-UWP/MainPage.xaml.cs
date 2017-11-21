using PlatformBindings;
using Test_UWP.Services;
using Tests_UWP.Views;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Test_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            AppServices.Current.UI.NavigationManager = new TestNavigationManager(MainFrame);
            MainFrame.Navigate(typeof(BindingTests));
        }
    }
}