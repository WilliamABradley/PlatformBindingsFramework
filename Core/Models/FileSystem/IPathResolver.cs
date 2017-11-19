using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public interface IPathResolver
    {
        Task<FileSystemContainer> TryResolve(string Path);
    }
}