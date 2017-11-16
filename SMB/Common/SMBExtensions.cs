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

        private static string EnsureSafe(string URL)
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