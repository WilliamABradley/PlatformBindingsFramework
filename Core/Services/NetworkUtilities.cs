// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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