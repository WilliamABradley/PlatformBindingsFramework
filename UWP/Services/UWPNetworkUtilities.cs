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