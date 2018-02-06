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

using Tests;
using Test_Android.Views;
using System;
using System.Linq;
using PlatformBindings.Services;
using PlatformBindings.Models;

namespace Test_Android.Services
{
    public class TestAndroidNavigator : AndroidActivityNavigator<TestNavigationPage>
    {
        public override bool Navigate(TestNavigationPage Page, NavigationParameters Parameters, bool ClearBackStack)
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
            var param = new NavigationParameters
            {
                { "type", Type.AssemblyQualifiedName }
            };

            return InternalNavigate(typeof(TestViewer), param, ShowBack);
        }
    }
}