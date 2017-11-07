using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using PlatformBindings.Models;
using PlatformBindings.Services;
using System;
using System.Threading.Tasks;

namespace PlatformBindings.Activities
{
#if APPCOMPAT
    public class PlatformBindingActivity : Android.Support.V7.App.AppCompatActivity
#else
    public class PlatformBindingActivity : Activity
#endif
    {
        public PlatformBindingActivity()
        {
            Handler = ActivityHandler.GetActivityHandler(this);
        }

        protected override void OnCreate(Bundle bundle)
        {
            Handler.OnCreate(bundle);
            base.OnCreate(bundle);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
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