using Windows.Networking.Connectivity;

namespace PlatformBindings.Services
{
    public class WinConnectionTest : ConnectionTestBase
    {
        public override bool HasInternetConnection
        {
            get
            {
                ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
                return connections != null && (connections.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.None);
            }
        }
    }
}
