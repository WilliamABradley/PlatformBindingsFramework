using System.Linq;
using Windows.Networking.Connectivity;

namespace PlatformBindings.Services
{
    public class UWPNetworkUtilities : NetworkUtilities
    {
        public override bool HasInternetConnection
        {
            get
            {
                ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
                if (connections != null)
                {
                    var levels = connections.GetNetworkConnectivityLevel();
                    return levels == NetworkConnectivityLevel.InternetAccess || levels == NetworkConnectivityLevel.ConstrainedInternetAccess;
                }
                return false;
            }
        }

        public override string LocalIPAddress
        {
            get
            {
                var icp = NetworkInformation.GetInternetConnectionProfile();

                if (icp?.NetworkAdapter == null) return null;

                var hostname =
                    NetworkInformation.GetHostNames()
                        .SingleOrDefault(
                            hn =>
                                hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                == icp.NetworkAdapter.NetworkAdapterId);

                return hostname?.CanonicalName;
            }
        }
    }
}