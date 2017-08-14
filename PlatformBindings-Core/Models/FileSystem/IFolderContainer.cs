using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public interface IFolderContainer : IFileSystemContainer
    {
        Task<IFolderContainer> GetFolder(string FolderName);

        Task<IFolderContainer> CreateFolder(string FolderName);

        Task<IReadOnlyList<IFolderContainer>> GetFolders();

        Task<IReadOnlyList<IFileContainer>> GetFiles();

        Task<IFileContainer> GetFile(string FileName);

        Task<IFileContainer> CreateFile(string FileName);
    }
}