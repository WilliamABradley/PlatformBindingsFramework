using PlatformBindings.Services;
using System;

namespace PlatformBindings
{
    public class NETCoreServices : AppServices
    {
        public NETCoreServices() : base(true)
        {
            UI = new CoreUIBindings();
            IO = new CoreIOBindings();
        }

        public override Version GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}