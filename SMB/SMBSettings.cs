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

using SharpCifs.Smb;

namespace PlatformBindings
{
    public static class SMBSettings
    {
        /// <summary>
        /// Reset invalid connections.
        /// Use this method to disposed the invalid connections without manipulating the setting values.
        /// </summary>
        /// <param name="Force">Include Active Connections (Be careful, It will dispose connections even during data transfer)</param>
        public static void ClearConnections(bool Force)
        {
            SmbTransport.ClearCachedConnections(Force);
        }

        /// <summary>
        /// Determines whether to use DFS(Distributed File System). <para/>
        /// Disabling DFS improves the speed of NetBios name resolution.
        /// DFS is disabled by Default.
        /// </summary>
        public static bool UseDFS
        {
            get { return !SharpCifs.Config.GetBoolean(PropDFSDisabled, true); }
            set { SharpCifs.Config.SetProperty(PropDFSDisabled, (!value).ToString()); }
        }

        /// <summary>
        /// Stores the Username for all SMB Access in this Session.
        /// </summary>
        public static string GlobalUsername
        {
            get { return (string)SharpCifs.Config.Get(PropGlobalUsername); }
            set { SharpCifs.Config.SetProperty(PropGlobalUsername, value); }
        }

        /// <summary>
        /// Stores the Password for all SMB Access in this Session.
        /// </summary>
        public static string GlobalPassword
        {
            get { return (string)SharpCifs.Config.Get(PropGlobalPassword); }
            set { SharpCifs.Config.SetProperty(PropGlobalPassword, value); }
        }

        /// <summary>
        /// Set Local IP Address. <para/>
        /// If a connection error occurs(ex: Failed to connect: [NET BIOS NAME]),
        /// Try to set the Local IP Address.
        /// When the host name of the device is invalid as the DNS name,
        /// it may be impossible to determine the local address.
        /// </summary>
        public static string LocalIPAddress
        {
            get { return (string)SharpCifs.Config.Get(PropLocalIPAddress); }
            set { SharpCifs.Config.SetProperty(PropLocalIPAddress, value); }
        }

        /// <summary>
        /// Sets Local UDP-Broadcast Port. <para/>
        /// When using the host name when connecting,
        /// Change default local port(137) to a value larger than 1024.
        /// In many cases, use of the well-known port is restricted.
        /// </summary>
        public static int LocalPort
        {
            get { return (int)SharpCifs.Config.Get(PropLocalPort); }
            set { SharpCifs.Config.SetProperty(PropLocalPort, value.ToString()); }
        }

        private static readonly string PropDFSDisabled = "jcifs.smb.client.dfs.disabled";
        private static readonly string PropGlobalUsername = "jcifs.smb.client.username";
        private static readonly string PropGlobalPassword = "jcifs.smb.client.password";
        private static readonly string PropLocalIPAddress = "jcifs.smb.client.laddr";
        private static readonly string PropLocalPort = "jcifs.smb.client.lport";
    }
}