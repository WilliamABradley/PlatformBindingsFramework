// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public class CoreFolderContainer : FolderContainer
    {
        public CoreFolderContainer(DirectoryInfo Folder)
        {
            _Path = Folder.FullName;
        }

        public CoreFolderContainer(string Path)
        {
            _Path = Path;
        }

        private string GetSubItemPath(string Name)
        {
            return Path + System.IO.Path.DirectorySeparatorChar + Name;
        }

        protected override Task<FileContainer> InternalCreateFileAsync(string FileName)
        {
            var file = new CoreFileContainer(GetSubItemPath(FileName));
            return Task.FromResult((FileContainer)file);
        }

        protected override Task<FolderContainer> InternalCreateFolderAsync(string FolderName)
        {
            var subfolder = Folder.CreateSubdirectory(FolderName);
            return Task.FromResult((FolderContainer)new CoreFolderContainer(subfolder));
        }

        public override Task<bool> DeleteAsync()
        {
            bool success = false;
            try
            {
                Folder.Delete(true);
                success = true;
            }
            catch { }

            return Task.FromResult(success);
        }

        public override Task<IReadOnlyList<FolderContainer>> GetFoldersAsync()
        {
            var folders = Folder.GetDirectories().Select(item => new CoreFolderContainer(item)).ToList();
            return Task.FromResult((IReadOnlyList<FolderContainer>)folders);
        }

        public override Task<IReadOnlyList<FileContainer>> GetFilesAsync()
        {
            var files = Folder.GetFiles().Select(item => new CoreFileContainer(item)).ToList();
            return Task.FromResult((IReadOnlyList<FileContainer>)files);
        }

        public override Task<bool> RenameAsync(string NewName)
        {
            return Task.Run(() =>
            {
                try
                {
                    string Parent = System.IO.Path.GetDirectoryName(Path);
                    var newpath = System.IO.Path.Combine(Parent, NewName);

                    if (Directory.Exists(Path))
                    {
                        Folder.MoveTo(System.IO.Path.Combine(Folder.Parent.FullName, NewName));
                    }

                    _Path = newpath;

                    return true;
                }
                catch { return false; }
            });
        }

        public override Task<bool> FileExists(string FileName)
        {
            return Task.Run(() => File.Exists(GetSubItemPath(FileName)));
        }

        public override Task<bool> FolderExists(string FolderName)
        {
            return Task.Run(() => Directory.Exists(GetSubItemPath(FolderName)));
        }

        public override bool CanWrite => !((Folder.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);

        private DirectoryInfo Folder => Directory.CreateDirectory(Path);

        public override string Name => _Path?.Split(new string[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries)?.Last();
        public override string Path => _Path;
        public string _Path;
    }
}