using PlatformBindings.Services;
using System;

namespace PlatformBindings
{
    public class NETCoreServices : AppServices
    {
        public NETCoreServices() : base(true, Enums.Platform.NETCore)
        {
            IO = new CoreIOBindings();
            UI = new CoreUIBindings();
            NetworkUtilities = new NetworkUtilities();
        }

        public static bool UseGlobalAppData = true;

        public override Version GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}