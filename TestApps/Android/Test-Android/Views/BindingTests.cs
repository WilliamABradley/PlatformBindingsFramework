using System;
using Android.App;
using Android.OS;
using Android.Widget;
using PlatformBindings.Activities;
using PlatformBindings.Common;
using PlatformBindings;

namespace Test_Android.Views
{
    [Activity(Label = "Tests")]
    public class BindingTests : PlatformBindingActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.BindingTests);

            Pickers.Click += delegate { StartActivity(typeof(FilePickerTest)); };
            Context.Click += delegate { StartActivity(typeof(ContextMenuTest)); };
            LoopTests.Click += delegate { StartActivity(typeof(LoopTests)); };
            TestAsyncActivity.Click += TestAsyncActivity_Click;
        }

        private async void TestAsyncActivity_Click(object sender, EventArgs e)
        {
            var result = await this.StartActivityForResultAsync(typeof(ReturnActivity));
            AppServices.UI.PromptUser("Activity Returned", $"RequestCode: {result.RequestCode}\nResponse: {result.ResultCode}", "OK");
        }

        public Button Pickers { get { return _Pickers ?? (_Pickers = FindViewById<Button>(Resource.Id.TestFilePickerButton)); } }
        private Button _Pickers;

        public Button Context { get { return _Context ?? (_Context = FindViewById<Button>(Resource.Id.TestContextMenuButton)); } }
        private Button _Context;

        public Button LoopTests { get { return _LoopTests ?? (_LoopTests = FindViewById<Button>(Resource.Id.LoopTests)); } }
        private Button _LoopTests;

        public Button TestAsyncActivity { get { return _TestAsyncActivity ?? (_TestAsyncActivity = FindViewById<Button>(Resource.Id.Test_AsyncAct)); } }
        private Button _TestAsyncActivity;
    }
}