using Windows.Storage;

namespace PlatformBindings.Models.FileSystem
{
    public interface IUWPFileSystemContainer
    {
        IStorageItem Item { get; }
    }
}