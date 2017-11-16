using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace PlatformBindings.Services
{
    public class NetworkUtilities
    {
        /// <summary>
        /// Gets whether the Application can access the Internet.
        /// </summary>
        public virtual bool HasInternetConnection => NetworkInterface.GetIsNetworkAvailable();

        /// <summary>
        /// Gets the Local IP Address of the current Device.
        /// </summary>
        public virtual string LocalIPAddress
        {
            get
            {
                var interfaces = NetworkInterface.GetAllNetworkInterfaces();
                var addresses = interfaces.Where(face => face.OperationalStatus == OperationalStatus.Up)
                    .Select(face => face.GetIPProperties().UnicastAddresses.FirstOrDefault(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(addr.Address)));
                return addresses.Any() ? addresses.First().Address?.ToString() : null;
            }
        }
    }
}