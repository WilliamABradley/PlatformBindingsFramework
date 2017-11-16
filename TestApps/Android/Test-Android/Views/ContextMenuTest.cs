using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Tests.Tests;
using PlatformBindings.Common;
using PlatformBindings.Activities;
using PlatformBindings.Models;

namespace Test_Android.Views
{
    [Activity(Label = "ContextMenu")]
    public class ContextMenuTest : PlatformBindingActivity
    {
        public ContextMenuTests Viewmodel { get; } = new ContextMenuTests();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ContextMenu);

            var MenuOpener = FindViewById<Button>(Resource.Id.ContextMenuButton);
            MenuOpener.Click += MenuOpener_Click;

            var RegisteredOpener = FindViewById<Button>(Resource.Id.RegisteredContextMenu);
            this.AttachContextMenu(Viewmodel.Menu, new AndroidContextMenuBinding(this, RegisteredOpener));
        }

        private void MenuOpener_Click(object sender, EventArgs e)
        {
            Viewmodel.ShowMenu(new AndroidContextMenuBinding(this, sender as View));
        }
    }
}