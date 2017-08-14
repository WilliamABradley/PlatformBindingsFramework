using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public class CoreFolderContainer : IFolderContainer
    {
        public CoreFolderContainer(DirectoryInfo Folder)
        {
            this.Folder = Folder;
        }

        public CoreFolderContainer(string Path)
        {
            Folder = Directory.CreateDirectory(Path);
        }

        public string Name => Folder.Name;

        public string Path => Folder.FullName;

        public Task<IFileContainer> CreateFile(string FileName)
        {
            var file = new CoreFileContainer(Path + System.IO.Path.DirectorySeparatorChar + FileName);
            return Task.FromResult((IFileContainer)file);
        }

        public Task<IFolderContainer> CreateFolder(string FolderName)
        {
            var subfolder = Folder.CreateSubdirectory(FolderName);
            return Task.FromResult((IFolderContainer)new CoreFolderContainer(subfolder));
        }

        public Task<bool> Delete()
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

        public async Task<IFileContainer> GetFile(string FileName)
        {
            var files = await GetFiles();
            var file = files.FirstOrDefault(item => item.Name == FileName);

            return file;
        }

        public Task<IFolderContainer> GetFolder(string FolderName)
        {
            return CreateFolder(FolderName);
        }

        public Task<IReadOnlyList<IFolderContainer>> GetFolders()
        {
            var folders = Folder.GetDirectories().Select(item => new CoreFolderContainer(item)).ToList();
            return Task.FromResult((IReadOnlyList<IFolderContainer>)folders);
        }

        public Task<IReadOnlyList<IFileContainer>> GetFiles()
        {
            var files = Folder.GetFiles().Select(item => new CoreFileContainer(item)).ToList();
            return Task.FromResult((IReadOnlyList<IFileContainer>)files);
        }

        public DirectoryInfo Folder { get; }
    }
}