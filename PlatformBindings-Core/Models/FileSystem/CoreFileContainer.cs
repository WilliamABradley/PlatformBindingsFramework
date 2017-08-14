using System.IO;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public class CoreFileContainer : IFileContainer
    {
        public CoreFileContainer(FileInfo File)
        {
            Name = File.Name;
            Path = File.FullName;
        }

        public CoreFileContainer(string Path)
        {
            this.Path = Path;
            Name = System.IO.Path.GetFileName(Path);
        }

        public Task<bool> Delete()
        {
            bool Success = false;
            try
            {
                File.Delete(Path);
                Success = true;
            }
            catch { }
            return Task.FromResult(Success);
        }

        public Task<Stream> OpenAsStream(bool CanWrite)
        {
            Stream result = null;
            if (CanWrite) result = File.OpenWrite(Path);
            else result = File.OpenRead(Path);

            return Task.FromResult(result);
        }

        public Task<string> ReadFileAsText()
        {
            try
            {
                using (var reader = File.OpenText(Path))
                {
                    return Task.FromResult(reader.ReadToEnd());
                }
            }
            catch { return Task.FromResult((string)null); }
        }

        public Task<bool> SaveText(string Text)
        {
            bool success = false;
            try
            {
                using (var writer = File.CreateText(Path))
                {
                    writer.Write(Text);
                    success = true;
                }
            }
            catch { }

            return Task.FromResult(success);
        }

        public string Name { get; set; }
        public string Path { get; set; }
    }
}