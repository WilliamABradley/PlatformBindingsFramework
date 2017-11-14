using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace PlatformBindings.Models.FileSystem
{
    public class WinFileContainer : FileContainer
    {
        public WinFileContainer(StorageFile File)
        {
            this.File = File;
        }

        public override async Task<Stream> OpenAsStream(bool CanWrite)
        {
            if (CanWrite) return await File.OpenStreamForWriteAsync();
            else return await File.OpenStreamForReadAsync();
        }

        public override async Task<string> ReadFileAsText()
        {
            return await FileIO.ReadTextAsync(File);
        }

        public override async Task<bool> SaveText(string Text)
        {
            try
            {
                await FileIO.WriteTextAsync(File, Text);
                return true;
            }
            catch { return false; }
        }

        public override async Task<bool> DeleteAsync()
        {
            try
            {
                await File.DeleteAsync();
                return true;
            }
            catch { return false; }
        }

        public override async Task<bool> RenameAsync(string NewName)
        {
            try
            {
                await File.RenameAsync(NewName);
                return true;
            }
            catch { return false; }
        }

        public StorageFile File { get; }

        public override string Name => File.Name;

        public override string Path => File.Path;

        public override bool CanWrite => !((File.Attributes & Windows.Storage.FileAttributes.ReadOnly) == Windows.Storage.FileAttributes.ReadOnly);
    }
}