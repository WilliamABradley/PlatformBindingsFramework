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

using PlatformBindings.Common;
using System.Windows;
using System.Windows.Controls;
using Tests.TestGenerator;

namespace Test_WPF.Services
{
    public class WPFTestPageGenerator : ITestPageGenerator
    {
        public WPFTestPageGenerator(Page Page)
        {
            this.Page = Page;
            var view = new ScrollViewer
            {
                Content = ContentPanel
            };
            Page.Content = view;
            ContentPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        public void CreateTestUI(TestTask Test)
        {
            var testbutton = new Button
            {
                Content = Test.Name,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 40
            };
            testbutton.Click += delegate { Test.RunTest(); };
            Test.AttachedUI = testbutton;
            ContentPanel.Children.Add(testbutton);
        }

        public void CreateTestProperty(TestProperty Property)
        {
            Visibility PropVisibility()
            {
                return Property.Value != null ? Visibility.Visible : Visibility.Collapsed;
            }

            var block = new TextBlock
            {
                Text = Property.ToString(),
                Visibility = PropVisibility()
            };
            Property.PropertyUpdated += delegate
            {
                PlatformBindingHelpers.OnUIThread(() =>
                {
                    block.Text = Property.ToString();
                    block.Visibility = PropVisibility();
                });
            };
            ContentPanel.Children.Add(block);
        }

        public Page Page { get; }
        public StackPanel ContentPanel { get; } = new StackPanel();
        public object UIInstance => ContentPanel;
    }
}