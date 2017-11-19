using SharpCifs.Smb;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public class SMBFileContainer : FileContainer
    {
        public SMBFileContainer(SmbFile File)
        {
            this.File = File;
            if (!File.IsFile())
            {
                throw new FormatException("SmbFile is not a File");
            }
            Refresh();
        }

        public void Refresh()
        {
            _Name = File.GetName();
            _Path = File.GetPath();
            _CanWrite = File.CanWrite();
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
                await File.DeleteAsync();
                return true;
            }
            catch { }
            return false;
        }

        public override async Task<Stream> OpenAsStream(bool CanWrite)
        {
            if (CanWrite) return await File.GetOutputStreamAsync();
            else return await File.GetInputStreamAsync();
        }

        public override async Task<bool> RenameAsync(string NewName)
        {
            try
            {
                var newpath = System.IO.Path.Combine(File.GetParent(), NewName);
                await File.RenameToAsync(new SmbFile(newpath));
                return true;
            }
            catch { }
            return false;
        }

        public SmbFile File { get; }
    }
}