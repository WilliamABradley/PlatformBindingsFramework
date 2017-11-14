using System.IO;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public class CoreFileContainer : FileContainer
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

        public override Task<Stream> OpenAsStream(bool CanWrite)
        {
            Stream result = null;
            if (CanWrite) result = File.OpenWrite(Path);
            else result = File.OpenRead(Path);

            return Task.FromResult(result);
        }

        public override Task<string> ReadFileAsText()
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

        public override Task<bool> SaveText(string Text)
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

        public override Task<bool> RenameAsync(string NewName)
        {
            return Task.Run(() =>
            {
                try
                {
                    string Directory = System.IO.Path.GetDirectoryName(Path);
                    File.Move(Path, System.IO.Path.Combine(Directory, NewName));
                    return true;
                }
                catch { return false; }
            });
        }

        public override Task<bool> DeleteAsync()
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

        public override string Name { get; }
        public override string Path { get; }

        public override bool CanWrite => !((File.GetAttributes(Path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
    }
}