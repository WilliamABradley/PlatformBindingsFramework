using System.Collections.Generic;

namespace PlatformBindings.Models.FileSystem
{
    public class FileQueryResult
    {
        internal List<IFileContainer> Files { get; } = new List<IFileContainer>();

        private IReadOnlyList<IFileContainer> GetFiles()
        {
            return Files;
        }

        public uint FileCount { get { return (uint)Files.Count; } }
    }
}