using PlatformBindings.Services;
using Windows.UI.Xaml.Controls;

namespace Test_UWP.Services
{
    public class TestNavigationManager : WinNavigationManager
    {
        public TestNavigationManager(Frame Frame) : base(Frame)
        {
        }

        public override bool MenuOpen { get; set; }

        public override void Navigate(object PageRequest, object Parameter = null)
        {
        }
    }
}