using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace PlatformBindings.Models.FileSystem
{
    public class WinFolderContainer : FolderContainerBase
    {
        public WinFolderContainer(StorageFolder Folder)
        {
            this.Folder = Folder;
        }

        public override async Task<FileContainerBase> GetFileAsync(string FileName)
        {
            var file = await Folder.GetFileAsync(FileName);
            return new WinFileContainer(file);
        }

        public override async Task<FileContainerBase> CreateFileAsync(string FileName, Enums.CreationCollisionOption options)
        {
            var file = await Folder.CreateFileAsync(FileName, (CreationCollisionOption)options);
            return new WinFileContainer(file);
        }

        public override async Task<FolderContainerBase> GetFolderAsync(string FolderName)
        {
            var subFolder = await Folder.GetFolderAsync(FolderName);
            return new WinFolderContainer(subFolder);
        }

        public override async Task<FolderContainerBase> CreateFolderAsync(string FolderName, Enums.CreationCollisionOption options)
        {
            var subFolder = await Folder.CreateFolderAsync(FolderName, (CreationCollisionOption)options);
            return new WinFolderContainer(subFolder);
        }

        public override async Task<IReadOnlyList<FolderContainerBase>> GetFoldersAsync()
        {
            var folders = await Folder.GetFoldersAsync();
            return folders.Select(item => new WinFolderContainer(item)).ToList();
        }

        public override async Task<IReadOnlyList<FileContainerBase>> GetFilesAsync()
        {
            var files = await Folder.GetFilesAsync();
            return files.Select(item => new WinFileContainer(item)).ToList();
        }

        public override async Task<bool> DeleteAsync()
        {
            try
            {
                await Folder.DeleteAsync();
                return true;
            }
            catch { return false; }
        }

        public override async Task<bool> RenameAsync(string NewName)
        {
            try
            {
                await Folder.RenameAsync(NewName);
                return true;
            }
            catch { return false; }
        }

        public StorageFolder Folder { get; }

        public override string Name => Folder.Name;

        public override string Path => Folder.Path;
    }
}