using System.Collections.Generic;

namespace PlatformBindings.Models.FileSystem
{
    public class FileQueryResult
    {
        internal List<FileContainer> Files { get; } = new List<FileContainer>();

        private IReadOnlyList<FileContainer> GetFiles()
        {
            return Files;
        }

        public uint FileCount { get { return (uint)Files.Count; } }
    }
}