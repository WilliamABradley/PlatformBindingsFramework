using System;

namespace PlatformBindings.Services
{
    public interface IServiceBindings
    {
        Version GetAppVersion();

        bool HasUI { get; }
        UIBindingsBase UI { get; }

        IOBindings IO { get; }
        ICredentialManager Credentials { get; }
        IOAuthBroker OAuth { get; }

        bool HasInternetConnection { get; }
    }
}