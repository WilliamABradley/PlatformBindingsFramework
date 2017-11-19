using System.IO;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public class CorePathResolver : IPathResolver
    {
        public Task<FileSystemContainer> TryResolve(string Path)
        {
            return Task.Run(() =>
            {
                if (File.Exists(Path))
                {
                    return (FileSystemContainer)new CoreFileContainer(Path);
                }
                else if (Directory.Exists(Path))
                {
                    return new CoreFolderContainer(Path);
                }
                return null;
            });
        }
    }
}