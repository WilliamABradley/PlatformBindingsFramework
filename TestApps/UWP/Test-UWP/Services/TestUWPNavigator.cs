using System;
using Test_UWP.Views;
using Tests;
using Windows.UI.Xaml.Controls;
using PlatformBindings.Services;
using System.Linq;

namespace Test_UWP.Services
{
    public class TestUWPNavigator : UWPNavigator<TestNavigationPage>
    {
        public TestUWPNavigator(Frame Frame) : base(Frame)
        {
        }

        public override bool Navigate(TestNavigationPage Page, string Parameter, bool ClearBackStack)
        {
            var pageType = TestService.TestRegister.FirstOrDefault(item => item.Value.Key == Page);
            if (pageType.HasValue)
            {
                var showBack = Page != TestNavigationPage.Home;
                return InternalNavigate(pageType.Value.Value, showBack);
            }
            return false;
        }

        protected bool InternalNavigate(Type Type, bool ShowBack)
        {
            return InternalNavigate(typeof(TestViewer), Type.AssemblyQualifiedName, ShowBack);
        }
    }
}