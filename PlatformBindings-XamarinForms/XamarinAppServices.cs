using System;
using PlatformBindings.Services;

namespace PlatformBindings
{
    public class XamarinAppServices : AppServices
    {
        public XamarinAppServices(bool HasUI) : base(HasUI)
        {
            if (HasUI) UI = new XamarinUIBindings();
        }

        public override Version GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}