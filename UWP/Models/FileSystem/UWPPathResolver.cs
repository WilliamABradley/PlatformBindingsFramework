using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace PlatformBindings.Models.FileSystem
{
    public class UWPPathResolver : IPathResolver
    {
        public async Task<FileSystemContainer> TryResolve(string Path)
        {
            try
            {
                return new UWPFileContainer(await StorageFile.GetFileFromPathAsync(Path));
            }
            catch
            {
                try
                {
                    return new UWPFolderContainer(await StorageFolder.GetFolderFromPathAsync(Path));
                }
                catch
                {
                }
            }
            return null;
        }
    }
}