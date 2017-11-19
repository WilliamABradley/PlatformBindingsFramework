using System.Threading.Tasks;
using SharpCifs.Smb;

namespace PlatformBindings.Models.FileSystem
{
    public class SMBPathResolver : IPathResolver
    {
        public Task<FileSystemContainer> TryResolve(string Path)
        {
            return Task.Run(() =>
            {
                var lowerpath = Path.ToLower();
                if (!lowerpath.StartsWith("smb://") && !lowerpath.StartsWith("\\\\"))
                {
                    return null;
                }

                var url = SMBExtensions.EnsureSafe(Path);

                var item = new SmbFile(url);
                if (item.IsDirectory()) return (FileSystemContainer)new SMBFolderContainer(item);
                else return new SMBFileContainer(item);
            });
        }
    }
}