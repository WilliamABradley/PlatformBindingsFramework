using System.Collections.Generic;

namespace PlatformBindings.Models.FileSystem
{
    public class FolderOpenOptions
    {
        public IList<IFileSystemContainer> ItemsToSelect { get; } = new List<IFileSystemContainer>();
    }
}