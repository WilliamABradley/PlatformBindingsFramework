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

using System;
using Test_UWP.Views;
using Tests;
using Windows.UI.Xaml.Controls;
using System.Linq;
using PlatformBindings.Services;

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