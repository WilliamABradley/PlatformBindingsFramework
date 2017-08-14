using System.IO;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public interface IFileContainer : IFileSystemContainer
    {
        Task<string> ReadFileAsText();

        Task<Stream> OpenAsStream(bool CanWrite);

        Task<bool> SaveText(string Text);
    }
}