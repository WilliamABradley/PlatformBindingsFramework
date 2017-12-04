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

using PlatformBindings.Models;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Services;
using SharpCifs.Smb;

namespace PlatformBindings
{
    public static class SMBExtensions
    {
        public static FolderContainer GetSMBFolder(this IOBindings IOBinding, string URL, NTLMAuthentication Authentication)
        {
            URL = EnsureSafe(URL);
            SmbFile folder = null;
            if (Authentication != null)
            {
                var auth = new NtlmPasswordAuthentication(Authentication.Domain, Authentication.Username, Authentication.Password);
                folder = new SmbFile(URL, auth);
            }
            else
            {
                folder = new SmbFile(URL);
            }
            return new SMBFolderContainer(folder);
        }

        public static FileContainer GetSMBFile(this IOBindings IOBinding, string URL, NTLMAuthentication Authentication)
        {
            URL = EnsureSafe(URL);
            SmbFile file = null;
            if (Authentication != null)
            {
                var auth = new NtlmPasswordAuthentication(Authentication.Domain, Authentication.Username, Authentication.Password);
                file = new SmbFile(URL, auth);
            }
            else
            {
                file = new SmbFile(URL);
            }
            return new SMBFileContainer(file);
        }

        internal static string EnsureSafe(string URL)
        {
            var result = URL;
            if (!URL.StartsWith("smb://"))
            {
                return new UNCPath(URL).GetURL();
            }
            else if (!URL.EndsWith("/"))
            {
                result += "/";
            }
            return result;
        }

        public static FolderContainer GetSMBFolder(this IOBindings IOBinding, UNCPath Path)
        {
            return GetSMBFolder(IOBinding, Path.GetURL(), null);
        }

        public static FolderContainer GetSMBFolder(this IOBindings IOBinding, UNCPath Path, NTLMAuthentication Authentication)
        {
            return GetSMBFolder(IOBinding, Path.GetURL(), Authentication);
        }

        public static FolderContainer GetSMBFolder(this IOBindings IOBinding, string URL)
        {
            return GetSMBFolder(IOBinding, URL, null);
        }

        public static FileContainer GetSMBFile(this IOBindings IOBinding, UNCPath Path)
        {
            return GetSMBFile(IOBinding, Path.GetURL(), null);
        }

        public static FileContainer GetSMBFile(this IOBindings IOBinding, UNCPath Path, NTLMAuthentication Authentication)
        {
            return GetSMBFile(IOBinding, Path.GetURL(), Authentication);
        }

        public static FileContainer GetSMBFile(this IOBindings IOBinding, string URL)
        {
            return GetSMBFile(IOBinding, URL, null);
        }
    }
}