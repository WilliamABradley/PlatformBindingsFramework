using PlatformBindings.Models.FileSystem;
using PlatformBindings.Services;

namespace PlatformBindings
{
    public static class SMBService
    {
        public static void Register()
        {
            var address = AppServices.Current.NetworkUtilities.LocalIPAddress;
            if (address != null)
            {
                SMBSettings.LocalIPAddress = address;
            }
            IOBindings.AddResolver(new SMBPathResolver());
        }
    }
}