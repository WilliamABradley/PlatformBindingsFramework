using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformBindings.Common;
using Java.Net;
using Android.Support.V4.Provider;
using System.Linq;

namespace PlatformBindings.Models.FileSystem
{
    public class AndroidDocumentsProviderFolder : FolderContainer
    {
        public AndroidDocumentsProviderFolder(Android.Net.Uri Uri)
        {
            Folder = DocumentFile.FromTreeUri(AndroidHelpers.GetCurrentActivity(), Uri);
            CheckFolder();
        }

        public AndroidDocumentsProviderFolder(DocumentFile Folder)
        {
            this.Folder = Folder;
            CheckFolder();
        }

        private void CheckFolder()
        {
            if (!Folder.IsDirectory)
            {
                throw new FormatException("DocumentFile is not a Directory");
            }
        }

        public override Task<IReadOnlyList<FileSystemContainer>> GetItemsAsync()
        {
            return Task.Run(() =>
            {
                var Items = new List<FileSystemContainer>();
                foreach (var item in Folder.ListFiles())
                {
                    if (item.IsDirectory)
                    {
                        Items.Add(new AndroidDocumentsProviderFolder(item));
                    }
                    else
                    {
                        Items.Add(new AndroidDocumentsProviderFile(item));
                    }
                }
                return (IReadOnlyList<FileSystemContainer>)Items;
            });
        }

        public override async Task<IReadOnlyList<FolderContainer>> GetFoldersAsync()
        {
            var items = await GetItemsAsync();
            return items.OfType<FolderContainer>().ToList();
        }

        public override async Task<IReadOnlyList<FileContainer>> GetFilesAsync()
        {
            var items = await GetItemsAsync();
            return items.OfType<FileContainer>().ToList();
        }

        public override Task<FileContainer> GetFileAsync(string FileName)
        {
            return Task.Run(() =>
            {
                try
                {
                    var result = Folder.FindFile(FileName);
                    if (result != null)
                    {
                        return (FileContainer)new AndroidDocumentsProviderFile(result);
                    }
                }
                catch { }
                return null;
            });
        }

        protected override Task<FolderContainer> InternalCreateFolderAsync(string FolderName)
        {
            return Task.Run(() =>
            {
                var result = Folder.CreateDirectory(FolderName);
                if (result != null)
                {
                    return (FolderContainer)new AndroidDocumentsProviderFolder(result);
                }
                else return null;
            });
        }

        protected override Task<FileContainer> InternalCreateFileAsync(string FileName)
        {
            return Task.Run(() =>
            {
                var mimeType = URLConnection.GuessContentTypeFromName(FileName);
                var result = Folder.CreateFile(mimeType, FileName);
                if (result != null)
                {
                    return (FileContainer)new AndroidDocumentsProviderFile(result);
                }
                else return null;
            });
        }

        public override Task<bool> RenameAsync(string NewName)
        {
            return Task.Run(() =>
            {
                return Folder.RenameTo(NewName);
            });
        }

        public override Task<bool> DeleteAsync()
        {
            return Task.Run(() =>
            {
                return Folder.Delete();
            });
        }

        public override Task<bool> FileExists(string FileName)
        {
            return Task.Run(() => Folder.FindFile(FileName) != null);
        }

        ~AndroidDocumentsProviderFolder()
        {
            Folder?.Dispose();
        }

        public override bool CanWrite => Folder.CanWrite();

        public override string Name => Folder.Name;
        public override string Path => Folder.Uri.SchemeSpecificPart;
        public DocumentFile Folder { get; }
    }
}