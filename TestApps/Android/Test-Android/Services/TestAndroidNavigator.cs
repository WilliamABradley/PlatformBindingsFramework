using Tests;
using Test_Android.Views;
using System;
using System.Linq;
using PlatformBindings.Services;

namespace Test_Android.Services
{
    public class TestAndroidNavigator : AndroidActivityNavigator<TestNavigationPage>
    {
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