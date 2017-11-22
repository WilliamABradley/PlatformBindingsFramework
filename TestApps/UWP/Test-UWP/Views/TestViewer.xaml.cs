using PlatformBindings;
using System;
using Test_UWP.Services;
using Tests;
using Tests.Tests;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Test_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestViewer : Page
    {
        public TestPage TestModel { get; private set; }

        public TestViewer()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DisplayTests();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            AppServices.Current.UI.TitleManager.WindowTitle = null;
        }

        private void DisplayTests()
        {
            try
            {
                var param = TestService.Navigation.Parameter;
                var type = Type.GetType(param);
                TestModel = (TestPage)Activator.CreateInstance(type, new UWPTestPageGenerator(this));
                if (TestModel.PageName != null) AppServices.Current.UI.TitleManager.WindowTitle = TestModel.PageName;
                TestModel.DisplayTests();
            }
            catch
            {
                AppServices.Current.UI.PromptUser("Error", "Loading Test Failed", "OK");
            }
        }
    }
}