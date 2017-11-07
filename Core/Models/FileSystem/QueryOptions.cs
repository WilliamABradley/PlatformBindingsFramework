using System.Collections.Generic;
using PlatformBindings.Enums;

namespace PlatformBindings.Models.FileSystem
{
    public class QueryOptions
    {
        public FolderDepth Depth { get; set; }
        public List<FileTypeFilter> FileTypes { get; } = new List<FileTypeFilter>();
    }
}