using PlatformBindings.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public class CoreFolderContainer : FolderContainerBase
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

        public override Task<FileContainerBase> CreateFileAsync(string FileName, CreationCollisionOption Options)
        {
            var file = new CoreFileContainer(Path + System.IO.Path.DirectorySeparatorChar + FileName);
            return Task.FromResult((FileContainerBase)file);
        }

        public override Task<FolderContainerBase> CreateFolderAsync(string FolderName, CreationCollisionOption Options)
        {
            var subfolder = Folder.CreateSubdirectory(FolderName);
            return Task.FromResult((FolderContainerBase)new CoreFolderContainer(subfolder));
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

        public override async Task<FileContainerBase> GetFileAsync(string FileName)
        {
            var files = await GetFilesAsync();
            var file = files.FirstOrDefault(item => item.Name == FileName);

            return file;
        }

        public override Task<FolderContainerBase> GetFolderAsync(string FolderName)
        {
            return CreateFolderAsync(FolderName);
        }

        public override Task<IReadOnlyList<FolderContainerBase>> GetFoldersAsync()
        {
            var folders = Folder.GetDirectories().Select(item => new CoreFolderContainer(item)).ToList();
            return Task.FromResult((IReadOnlyList<FolderContainerBase>)folders);
        }

        public override Task<IReadOnlyList<FileContainerBase>> GetFilesAsync()
        {
            var files = Folder.GetFiles().Select(item => new CoreFileContainer(item)).ToList();
            return Task.FromResult((IReadOnlyList<FileContainerBase>)files);
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

        public DirectoryInfo Folder { get; }
    }
}