using System;
using PlatformBindings.Enums;
using PlatformBindings.Services;

namespace PlatformBindings
{
    public class Win32AppServices : AppServices
    {
        public Win32AppServices(bool HasUI) : base(HasUI, Platform.Win32)
        {
            IO = new Win32IOBindings();
        }

        public override Version GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}