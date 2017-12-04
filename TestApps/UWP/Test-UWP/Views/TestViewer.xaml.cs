// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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