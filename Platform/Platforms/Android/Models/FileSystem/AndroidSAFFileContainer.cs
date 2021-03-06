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
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using PlatformBindings.Common;
using Android.Support.V4.Provider;
using Android.Net;

namespace PlatformBindings.Models.FileSystem
{
    public class AndroidSAFFileContainer : FileContainer, IAndroidSAFContainer
    {
        public AndroidSAFFileContainer(Android.Net.Uri Uri)
        {
            File = DocumentFile.FromSingleUri(AndroidHelpers.GetCurrentActivity(), Uri);
            CheckFile();
        }

        public AndroidSAFFileContainer(DocumentFile File)
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

        ~AndroidSAFFileContainer()
        {
            File?.Dispose();
        }

        public override bool CanWrite => File.CanWrite();

        public override string Name => File.Name;
        public override string Path => Uri.ToString();
        public DocumentFile File { get; }
        private ContentResolver Resolver => AndroidHelpers.GetCurrentActivity().ContentResolver;

        public Android.Net.Uri Uri => File.Uri;
    }
}