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
    public class CorePathResolver : IPathResolver
    {
        public Task<StorageContainer> TryResolve(string Path)
        {
            return Task.Run(() =>
            {
                if (File.Exists(Path))
                {
                    return (StorageContainer)new CoreFileContainer(Path);
                }
                else if (Directory.Exists(Path))
                {
                    return new CoreFolderContainer(Path);
                }
                return null;
            });
        }
    }
}