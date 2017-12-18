using System;
using PlatformBindings.Enums;

namespace PlatformBindings
{
    public class IOSAppServices : AppServices
    {
        public IOSAppServices(bool HasUI, Platform Platform) : base(HasUI, Platform)
        {
        }

        public override Version GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}