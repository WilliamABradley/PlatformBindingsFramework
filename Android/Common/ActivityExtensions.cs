using Android.App;
using Android.Content;
using Android.OS;
using PlatformBindings.Models;
using PlatformBindings.Services;
using System;
using System.Threading.Tasks;

namespace PlatformBindings.Common
{
    public static class ActivityExtensions
    {
        public static void AttachContextMenu(this Activity Activity, Controls.MenuLayout.Menu Menu, AndroidContextMenuBinding Binding)
        {
            Activity.GetHandler().AttachContextMenu(Menu, Binding);
        }

        public static void OpenContextMenuForDisplay(this Activity Activity, Controls.MenuLayout.Menu Menu, AndroidContextMenuBinding Binding)
        {
            Activity.GetHandler().OpenContextMenuForDisplay(Menu, Binding);
        }

        public static Task<ActivityResult> StartActivityForResultAsync(this Activity Activity, Type activityType)
        {
            return Activity.GetHandler().StartActivityForResultAsync(activityType);
        }

        public static Task<ActivityResult> StartActivityForResultAsync(this Activity Activity, Intent intent)
        {
            return Activity.GetHandler().StartActivityForResultAsync(intent);
        }

        public static Task<ActivityResult> StartActivityForResultAsync(this Activity Activity, Intent intent, Bundle options)
        {
            return Activity.GetHandler().StartActivityForResultAsync(intent, options);
        }
    }
}