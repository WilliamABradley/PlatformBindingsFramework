using System;
using Android.App;
using Android.OS;
using PlatformBindings.Activities;
using Tests.Tests;
using Tests;
using Test_Android.Services;
using PlatformBindings;

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