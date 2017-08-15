using System;
using PlatformBindings.Services;

namespace PlatformBindings.ConsoleTools
{
    public class ConsoleServiceBindings : IServiceBindings
    {
        public bool HasUI => true;

        public UIBindingsBase UI => new ConsoleUIBindings();

        public IOBindings IO => new ConsoleIOBindings();

        public ICredentialManager Credentials => throw new NotImplementedException();

        public IOAuthBroker OAuth => throw new NotImplementedException();

        public bool HasInternetConnection => throw new NotImplementedException();

        public Version GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}