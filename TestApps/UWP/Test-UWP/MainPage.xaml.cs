using PlatformBindings;
using PlatformBindings.Services;
using Test_UWP.Services;
using Tests;
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
            TestService.Register(new TestUWPNavigator(MainFrame));
            AppServices.Current.UI.NavigationManager = new UWPNavigationManager(TestService.Navigation, MainFrame);
            TestService.Navigation.Navigate(TestNavigationPage.Home);
        }
    }
}