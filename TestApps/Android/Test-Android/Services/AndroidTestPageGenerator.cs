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

using Android.App;
using Android.Views;
using Android.Widget;
using PlatformBindings.Common;
using Tests.TestGenerator;

namespace Test_Android.Services
{
    public class AndroidTestPageGenerator : ITestPageGenerator
    {
        public AndroidTestPageGenerator(Activity Activity)
        {
            this.Activity = Activity;
            Layout = new LinearLayout(Activity)
            {
                Orientation = Orientation.Vertical
            };
            var view = new ScrollView(Activity);
            view.AddView(Layout);
            Activity.SetContentView(view);
        }

        public void CreateTestUI(TestTask Test)
        {
            var button = new Button(Activity)
            {
                Text = Test.Name
            };
            button.Click += delegate { Test.RunTest(); };
            Test.AttachedUI = button;
            Layout.AddView(button);
        }

        public void CreateTestProperty(TestProperty Property)
        {
            ViewStates PropVisibility()
            {
                return Property.Value != null ? ViewStates.Visible : ViewStates.Gone;
            }

            var view = new TextView(Activity)
            {
                Text = Property.ToString(),
                Visibility = PropVisibility()
            };
            Property.PropertyUpdated += delegate
            {
                PlatformBindingHelpers.OnUIThread(() =>
                {
                    view.Text = Property.ToString();
                    view.Visibility = PropVisibility();
                });
            };
            Layout.AddView(view);
        }

        public Activity Activity { get; }
        private LinearLayout Layout { get; }

        public object UIInstance => Layout;
    }
}