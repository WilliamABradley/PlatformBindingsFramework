using PlatformBindings.Services;
using System;

namespace PlatformBindings
{
    public class NETCoreServices : AppServices
    {
        public NETCoreServices() : base(true, Enums.Platform.NETCore)
        {
            UI = new CoreUIBindings();
            IO = new CoreIOBindings();
        }

        public static bool UseGlobalAppData = true;

        public override Version GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}