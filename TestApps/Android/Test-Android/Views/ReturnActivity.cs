using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using PlatformBindings.Activities;

namespace Test_Android.Views
{
    [Activity(Label = "Return Activity")]
    public class ReturnActivity : PlatformBindingActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            var button = new Button(this)
            {
                Text = "Complete with Result",
            };
            button.Click += Button_Click;
            SetContentView(button);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            SetResult(Result.Ok);
            Finish();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}