using System;
using PlatformBindings.Services;
using PlatformBindings.Enums;

namespace PlatformBindings
{
    public class XamarinAppServices : AppServices
    {
        public XamarinAppServices(bool HasUI) : base(HasUI, Platform.XamarinForms)
        {
            if (HasUI) UI = new XamarinUIBindings();
        }

        public override Version GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}