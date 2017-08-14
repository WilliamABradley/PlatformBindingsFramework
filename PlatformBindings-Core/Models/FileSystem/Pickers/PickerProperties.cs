using System.Collections.Generic;
using PlatformBindings.Enums;

namespace PlatformBindings.Models.FileSystem
{
    public abstract class PickerProperties
    {
        public IList<FileTypeFilter> FileTypes { get; } = new List<FileTypeFilter>();
        public PathRoot? StartingLocation { get; set; }
    }
}