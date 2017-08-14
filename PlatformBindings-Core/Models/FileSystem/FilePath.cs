using PlatformBindings.Enums;

namespace PlatformBindings.Models.FileSystem
{
    public class FilePath : FolderPath
    {
        public FilePath(PathRoot Root, string Path, string FileName) : base(Root, Path)
        {
            this.FileName = FileName;
        }

        public FilePath(FolderPath Path, string FileName) : base(Path.Root, Path.Path)
        {
            this.FileName = FileName;
        }

        public string FileName { get; set; }
    }
}