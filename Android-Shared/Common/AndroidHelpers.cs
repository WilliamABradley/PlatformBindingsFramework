using Android.App;
using PlatformBindings.Activities;
using PlatformBindings.Services;

namespace PlatformBindings.Common
{
    public static class AndroidHelpers
    {
        public static Activity GetCurrentActivity(IUIBindingInfo UIBinding)
        {
            var uibinding = (UIBinding ?? AppServices.UI.DefaultUIBinding) as AndroidUIBindingInfo;
            return uibinding.Activity;
        }

        public static ActivityHandler GetCurrentActivityHandler(IUIBindingInfo UIBinding)
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