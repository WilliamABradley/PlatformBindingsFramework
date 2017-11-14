using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using PlatformBindings.Common;
using Android.Support.V4.Provider;

namespace PlatformBindings.Models.FileSystem
{
    public class AndroidDocumentsProviderFile : FileContainer
    {
        public AndroidDocumentsProviderFile(Android.Net.Uri Uri)
        {
            File = DocumentFile.FromSingleUri(AndroidHelpers.GetCurrentActivity(), Uri);
            CheckFile();
        }

        public AndroidDocumentsProviderFile(DocumentFile File)
        {
            this.File = File;
            CheckFile();
        }

        private void CheckFile()
        {
            if (!File.IsFile)
            {
                throw new FormatException("DocumentFile is not a File");
            }
        }

        public override Task<Stream> OpenAsStream(bool CanWrite)
        {
            return Task.FromResult(CanWrite ? Resolver.OpenOutputStream(File.Uri) : Resolver.OpenInputStream(File.Uri));
        }

        public override async Task<string> ReadFileAsText()
        {
            using (var stream = await OpenAsStream(false))
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        public override async Task<bool> SaveText(string Text)
        {
            using (var stream = await OpenAsStream(true))
            {
                using (var writer = new StreamWriter(stream))
                {
                    await writer.WriteAsync(Text);
                    return true;
                }
            }
        }

        public override Task<bool> RenameAsync(string NewName)
        {
            return Task.Run(() =>
            {
                return File.RenameTo(NewName);
            });
        }

        public override Task<bool> DeleteAsync()
        {
            return Task.Run(() =>
            {
                return File.Delete();
            });
        }

        ~AndroidDocumentsProviderFile()
        {
            File?.Dispose();
        }

        public override bool CanWrite => File.CanWrite();

        public override string Name => File.Name;
        public override string Path => File.Uri.SchemeSpecificPart;
        public DocumentFile File { get; }
        private ContentResolver Resolver => AndroidHelpers.GetCurrentActivity().ContentResolver;
    }
}