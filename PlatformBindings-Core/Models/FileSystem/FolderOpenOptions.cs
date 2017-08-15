using System.Collections.Generic;

namespace PlatformBindings.Models.FileSystem
{
    public class FolderOpenOptions
    {
        public IList<FileSystemContainerBase> ItemsToSelect { get; } = new List<FileSystemContainerBase>();
    }
}