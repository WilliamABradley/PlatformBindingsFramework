using System;

namespace PlatformBindings.Services
{
    public class XamarinServiceBindings : IServiceBindings
    {
        public XamarinServiceBindings(bool HasUI = true)
        {
            this.HasUI = HasUI;
            if (HasUI) UI = new XamarinUIBindings();
        }

        public bool HasUI { get; }
        public UIBindingsBase UI { get; }

        public IOBindings IO { get; }

        public ICredentialManager Credentials => throw new NotImplementedException();

        public IOAuthBroker OAuth => throw new NotImplementedException();

        public bool HasInternetConnection => throw new NotImplementedException();

        public Version GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}