using System;
using PlatformBindings.Services.Bindings;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.ApplicationModel;

namespace PlatformBindings.Services
{
    public class WinServiceBindings : IServiceBindings
    {
        public WinServiceBindings(bool HasUI = true)
        {
            this.HasUI = HasUI;
        }

        public void AttachDispatcher(CoreDispatcher MainDispatcher)
        {
            if (HasUI) UI = new WinUIBindings(MainDispatcher);
        }

        public Version GetAppVersion()
        {
            var appversion = Package.Current.Id.Version;
            return new Version(appversion.Major, appversion.Minor, appversion.Build, appversion.Revision);
        }

        public bool HasUI { get; }
        public UIBindingsBase UI { get; private set; }

        public IOBindings IO => new WinIOBindings();
        public ICredentialManager Credentials => new WinCredentialManager();
        public IOAuthBroker OAuth => new WinOAuthBroker();

        public bool HasInternetConnection
        {
            get
            {
                ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
                return connections != null && (connections.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.None);
            }
        }
    }
}