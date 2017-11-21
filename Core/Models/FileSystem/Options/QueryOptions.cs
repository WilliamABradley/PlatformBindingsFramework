﻿using System.Collections.Generic;
using PlatformBindings.Enums;

namespace PlatformBindings.Models.FileSystem.Options
{
    public class QueryOptions
    {
        public FolderDepth Depth { get; set; }
        public List<FileTypeFilter> FileTypes { get; } = new List<FileTypeFilter>();
    }
}