using System.Collections.Generic;

namespace PlatformBindings.Models.FileSystem
{
    public class FileQueryResult
    {
        internal List<FileContainerBase> Files { get; } = new List<FileContainerBase>();

        private IReadOnlyList<FileContainerBase> GetFiles()
        {
            return Files;
        }

        public uint FileCount { get { return (uint)Files.Count; } }
    }
}