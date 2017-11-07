using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using PlatformBindings;
using PlatformBindings.Activities;

namespace Test_Android.Views
{
    [Activity(Label = "Tests")]
    public class BindingTests : PlatformBindingActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.BindingTests);

            var picker = FindViewById<Button>(Resource.Id.TestFilePickerButton);
            picker.Click += Picker_Click;

            var context = FindViewById<Button>(Resource.Id.TestContextMenuButton);
            context.Click += Context_Click;

            var looptests = FindViewById<Button>(Resource.Id.LoopTests);
            looptests.Click += Looptests_Click;
        }

        private void Looptests_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(LoopTests));
            StartActivity(intent);
        }

        private void Context_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ContextMenuTest));
            StartActivity(intent);
        }

        private void Picker_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(FilePickerTest));
            StartActivity(intent);
        }
    }
}