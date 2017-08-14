using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace PlatformBindings.Models.FileSystem
{
    public class WinFolderContainer : WinFileSystemContainer, IFolderContainer
    {
        public WinFolderContainer(StorageFolder Folder) : base(Folder)
        {
            this.Folder = Folder;
        }

        public async Task<IFileContainer> GetFile(string FileName)
        {
            var file = await Folder.GetFileAsync(FileName);
            return new WinFileContainer(file);
        }

        public async Task<IFileContainer> CreateFile(string FileName)
        {
            var file = await Folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            return new WinFileContainer(file);
        }

        public async Task<IFolderContainer> GetFolder(string FolderName)
        {
            var subFolder = await Folder.GetFolderAsync(FolderName);
            return new WinFolderContainer(subFolder);
        }

        public async Task<IFolderContainer> CreateFolder(string FolderName)
        {
            var subFolder = await Folder.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);
            return new WinFolderContainer(subFolder);
        }

        public async Task<IReadOnlyList<IFolderContainer>> GetFolders()
        {
            var folders = await Folder.GetFoldersAsync();
            return folders.Select(item => new WinFolderContainer(item)).ToList();
        }

        public async Task<IReadOnlyList<IFileContainer>> GetFiles()
        {
            var files = await Folder.GetFilesAsync();
            return files.Select(item => new WinFileContainer(item)).ToList();
        }

        public StorageFolder Folder { get; }
    }
}