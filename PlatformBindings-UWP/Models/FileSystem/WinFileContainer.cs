using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace PlatformBindings.Models.FileSystem
{
    public class WinFileContainer : WinFileSystemContainer, IFileContainer
    {
        public WinFileContainer(StorageFile File) : base(File)
        {
            this.File = File;
        }

        public async Task<Stream> OpenAsStream(bool CanWrite)
        {
            if (CanWrite) return await File.OpenStreamForWriteAsync();
            else return await File.OpenStreamForReadAsync();
        }

        public async Task<string> ReadFileAsText()
        {
            return await FileIO.ReadTextAsync(File);
        }

        public async Task<bool> SaveText(string Text)
        {
            try
            {
                await FileIO.WriteTextAsync(File, Text);
                return true;
            }
            catch { return false; }
        }

        public StorageFile File { get; }
    }
}