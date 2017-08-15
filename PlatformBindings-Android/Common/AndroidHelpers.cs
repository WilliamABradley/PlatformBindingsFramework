using PlatformBindings.Services;

namespace PlatformBindings.Common
{
    public static class AndroidHelpers
    {
        public static LibActivity GetCurrentActivity(IUIBindingInfo UIBinding)
        {
            var uibinding = (UIBinding ?? AppServices.Services.UI.DefaultUIBinding) as AndroidUIBindingInfo;
            return uibinding.Activity;
        }
    }
}