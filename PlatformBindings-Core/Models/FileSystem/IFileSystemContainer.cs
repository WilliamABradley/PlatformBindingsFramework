using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public interface IFileSystemContainer
    {
        Task<bool> Delete();

        string Name { get; }

        string Path { get; }
    }
}