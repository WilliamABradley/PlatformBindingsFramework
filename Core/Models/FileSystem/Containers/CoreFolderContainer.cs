using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public class CoreFolderContainer : FolderContainer
    {
        public CoreFolderContainer(DirectoryInfo Folder)
        {
            this.Folder = Folder;
        }

        public CoreFolderContainer(string Path)
        {
            Folder = Directory.CreateDirectory(Path);
        }

        public override string Name => Folder.Name;

        public override string Path => Folder.FullName;

        private string GetSubItemPath(string Name)
        {
            return Path + System.IO.Path.DirectorySeparatorChar + Name;
        }

        protected override Task<FileContainer> InternalCreateFileAsync(string FileName)
        {
            var file = new CoreFileContainer(GetSubItemPath(FileName));
            return Task.FromResult((FileContainer)file);
        }

        protected override Task<FolderContainer> InternalCreateFolderAsync(string FolderName)
        {
            var subfolder = Folder.CreateSubdirectory(FolderName);
            return Task.FromResult((FolderContainer)new CoreFolderContainer(subfolder));
        }

        public override Task<bool> DeleteAsync()
        {
            bool success = false;
            try
            {
                Folder.Delete(true);
                success = true;
            }
            catch { }

            return Task.FromResult(success);
        }

        public override async Task<FileContainer> GetFileAsync(string FileName)
        {
            var files = await GetFilesAsync();
            var file = files.FirstOrDefault(item => item.Name == FileName);

            return file;
        }

        public override Task<FolderContainer> GetFolderAsync(string FolderName)
        {
            return CreateFolderAsync(FolderName);
        }

        public override Task<IReadOnlyList<FolderContainer>> GetFoldersAsync()
        {
            var folders = Folder.GetDirectories().Select(item => new CoreFolderContainer(item)).ToList();
            return Task.FromResult((IReadOnlyList<FolderContainer>)folders);
        }

        public override Task<IReadOnlyList<FileContainer>> GetFilesAsync()
        {
            var files = Folder.GetFiles().Select(item => new CoreFileContainer(item)).ToList();
            return Task.FromResult((IReadOnlyList<FileContainer>)files);
        }

        public override Task<bool> RenameAsync(string NewName)
        {
            return Task.Run(() =>
            {
                try
                {
                    Folder.MoveTo(System.IO.Path.Combine(Folder.Parent.FullName, NewName));
                    return true;
                }
                catch { return false; }
            });
        }

        public override Task<bool> FileExists(string FileName)
        {
            return Task.Run(() => File.Exists(GetSubItemPath(FileName)));
        }

        public override Task<bool> FolderExists(string FolderName)
        {
            return Task.Run(() => Directory.Exists(GetSubItemPath(FolderName)));
        }

        public override bool CanWrite => !((Folder.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);

        public DirectoryInfo Folder { get; }
    }
}