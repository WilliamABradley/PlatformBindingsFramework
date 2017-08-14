using System;

namespace PlatformBindings.Services
{
    public interface IServiceBindings
    {
        Version GetAppVersion();

        bool HasUI { get; }
        IUIBindings UI { get; }

        IOBindings IO { get; }
        ICredentialManager Credentials { get; }
        IOAuthBroker OAuth { get; }

        bool HasInternetConnection { get; }
    }
}