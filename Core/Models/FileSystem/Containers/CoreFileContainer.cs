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

using System.IO;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    public class CoreFileContainer : FileContainer
    {
        public CoreFileContainer(FileInfo File)
        {
            Name = File.Name;
            _Path = File.FullName;
        }

        public CoreFileContainer(string Path)
        {
            _Path = Path;
            Name = System.IO.Path.GetFileName(Path);
        }

        public override Task<Stream> OpenAsStream(bool CanWrite)
        {
            Stream result = null;
            if (CanWrite) result = File.OpenWrite(Path);
            else result = File.OpenRead(Path);

            return Task.FromResult(result);
        }

        public override Task<bool> RenameAsync(string NewName)
        {
            return Task.Run(() =>
            {
                try
                {
                    string Directory = System.IO.Path.GetDirectoryName(Path);
                    var newpath = System.IO.Path.Combine(Directory, NewName);
                    if (File.Exists(Path))
                    {
                        File.Move(Path, newpath);
                    }
                    _Path = newpath;
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

        public override string Path => _Path;
        private string _Path;

        public override bool CanWrite => !((File.GetAttributes(Path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
    }
}