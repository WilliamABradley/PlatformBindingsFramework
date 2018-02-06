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
using System.Windows.Controls;
using Test_WPF.Services;
using Tests;
using Tests.Tests;

namespace Test_WPF.Views
{
    /// <summary>
    /// Interaction logic for TestViewer.xaml
    /// </summary>
    public partial class TestViewer : Page
    {
        public TestPage TestModel { get; private set; }

        public TestViewer()
        {
            InitializeComponent();
        }

        private void DisplayTests()
        {
            try
            {
                var param = TestService.Navigation.Parameters;
                var type = Type.GetType(param["type"]);
                TestModel = (TestPage)Activator.CreateInstance(type, new WPFTestPageGenerator(this));
                if (TestModel.PageName != null) Title = TestModel.PageName;
                TestModel.DisplayTests();
            }
            catch
            {
                AppServices.Current.UI.PromptUser("Error", "Loading Test Failed", "OK");
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DisplayTests();
        }
    }
}