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
using Android.App;
using Android.OS;
using Tests.Tests;
using Tests;
using Test_Android.Services;
using PlatformBindings;
using PlatformBindings.Activities;

namespace Test_Android.Views
{
    [Activity]
    public class TestViewer : PlatformBindingActivity
    {
        public TestPage TestModel { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                var param = TestService.Navigation.Parameter;
                var type = Type.GetType(param);
                TestModel = (TestPage)Activator.CreateInstance(type, new AndroidTestPageGenerator(this));
                if (TestModel.PageName != null) AppServices.Current.UI.TitleManager.PageTitle = TestModel.PageName;
                TestModel.DisplayTests();
            }
            catch (Exception ex)
            {
                AppServices.Current.UI.PromptUser("Loading Test Failed", $"{ex.GetType().Name}:\n{ex.Message}", "OK");
            }
        }
    }
}