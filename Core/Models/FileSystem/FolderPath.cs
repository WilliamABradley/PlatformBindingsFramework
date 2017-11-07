using PlatformBindings.Enums;

namespace PlatformBindings.Models.FileSystem
{
    public class FolderPath
    {
        public FolderPath(PathRoot Root, string Path)
        {
            this.Root = Root;
            this.Path = Path;
        }

        public string Path { get; set; }
        public PathRoot Root { get; set; }
    }
}