using Android.App;
using PlatformBindings.Activities;
using PlatformBindings.Models;

namespace PlatformBindings.Common
{
    public static class AndroidHelpers
    {
        public static Activity GetCurrentActivity()
        {
            return GetCurrentActivity(null);
        }

        public static Activity GetCurrentActivity(this IUIBindingInfo UIBinding)
        {
            var uibinding = (UIBinding ?? AppServices.UI.DefaultUIBinding) as AndroidUIBindingInfo;
            return uibinding.Activity;
        }

        public static ActivityHandler GetCurrentActivityHandler(this IUIBindingInfo UIBinding)
        {
            var uibinding = (UIBinding ?? AppServices.UI.DefaultUIBinding) as AndroidUIBindingInfo;
            return ActivityHandler.GetActivityHandler(uibinding.Activity);
        }

        public static ActivityHandler GetHandler(this Activity Activity)
        {
            return ActivityHandler.GetActivityHandler(Activity);
        }
    }
}