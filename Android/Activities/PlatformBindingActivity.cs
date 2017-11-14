using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;

namespace PlatformBindings.Activities
{
    public class PlatformBindingActivity : Activity
    {
        public PlatformBindingActivity()
        {
            Handler = ActivityHandler.GetActivityHandler(this);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Handler.UpdateCurrentActivity();
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            Handler.UpdateCurrentActivity();
            base.OnResume();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            Handler.UpdateCurrentActivity();
            Handler.OnActivityResult(requestCode, resultCode, data);
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (!Handler.OnCreateContextMenu(menu, v, menuInfo))
            {
                base.OnCreateContextMenu(menu, v, menuInfo);
            }
        }

        public ActivityHandler Handler { get; }
    }
}