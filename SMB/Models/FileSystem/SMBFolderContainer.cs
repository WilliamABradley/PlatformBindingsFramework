using SharpCifs.Smb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public class SMBFolderContainer : FolderContainer
    {
        public SMBFolderContainer(SmbFile Folder)
        {
            SMBSettings.EnsureSMBReady();
            this.Folder = Folder;
            if (!Folder.Exists())
            {
                throw new FormatException("SmbFile is not a Directory");
            }
            Refresh();
        }

        public void Refresh()
        {
            _Name = Folder.GetName();
            _Name = _Name.Remove(_Name.Length - 1);
            _Path = Folder.GetUncPath();
            _CanWrite = Folder.CanWrite();
        }

        public override string Name => _Name;
        private string _Name;

        public override string Path => _Path;
        private string _Path;

        public override bool CanWrite => _CanWrite;
        private bool _CanWrite;

        public override async Task<bool> DeleteAsync()
        {
            try
            {
                await Folder.DeleteAsync();
                return true;
            }
            catch { }
            return false;
        }

        public override async Task<IReadOnlyList<FileContainer>> GetFilesAsync()
        {
            var items = await GetItemsAsync();
            return items.OfType<FileContainer>().ToList();
        }

        public override async Task<IReadOnlyList<FolderContainer>> GetFoldersAsync()
        {
            var items = await GetItemsAsync();
            return items.OfType<FolderContainer>().ToList();
        }

        public override async Task<IReadOnlyList<FileSystemContainer>> GetItemsAsync()
        {
            List<FileSystemContainer> Items = new List<FileSystemContainer>();
            var results = await Folder.ListFilesAsync();
            foreach (var result in results)
            {
                if (result.IsDirectory()) Items.Add(new SMBFolderContainer(result));
                else Items.Add(new SMBFileContainer(result));
            }

            return Items;
        }

        public override async Task<bool> RenameAsync(string NewName)
        {
            try
            {
                var newpath = System.IO.Path.Combine(Folder.GetParent(), NewName);
                await Folder.RenameToAsync(new SmbFile(newpath));
                return true;
            }
            catch { }
            return false;
        }

        protected override Task<FileContainer> InternalCreateFileAsync(string FileName)
        {
            return Task.Run(() =>
            {
                var newfile = new SmbFile(Folder, FileName);
                newfile.CreateNewFile();
                return (FileContainer)new SMBFileContainer(newfile);
            });
        }

        protected override async Task<FolderContainer> InternalCreateFolderAsync(string FolderName)
        {
            var newfolder = new SmbFile(Folder, FolderName);
            await newfolder.MkDirAsync();
            return new SMBFolderContainer(newfolder);
        }

        public override Task<bool> FileExists(string FileName)
        {
            return Exists(FileName);
        }

        public override Task<bool> FolderExists(string FolderName)
        {
            return Exists(FolderName);
        }

        private Task<bool> Exists(string Name)
        {
            return Task.Run(() =>
            {
                var newitem = new SmbFile(Folder, Name);
                return newitem.Exists();
            });
        }

        public SmbFile Folder { get; }
    }
}