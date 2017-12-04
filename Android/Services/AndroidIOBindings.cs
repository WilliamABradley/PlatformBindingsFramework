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

using Android.App;
using Android.Content;
using System;
using System.Threading.Tasks;
using PlatformBindings.Common;
using PlatformBindings.Enums;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Models.Settings;
using Android.OS;
using PlatformBindings.Exceptions;
using PlatformBindings.Models.FileSystem.Options;

namespace PlatformBindings.Services
{
    public class AndroidIOBindings : IOBindings
    {
        public override bool SupportsRoaming => false;
        public override bool SupportsOpenFolderForDisplay => false;
        public override bool SupportsOpenFileForDisplay => true;

        public override IFutureAccessManager FutureAccess => new AndroidFutureAccessManager();
        public override FileSystemPickers Pickers => new AndroidFileSystemPickers();

        public override ISettingsContainer LocalSettings { get; } = new AndroidSettingsContainer("Settings", null);

        public override FolderContainer GetBaseFolder(PathRoot Root)
        {
            FolderContainer Folder = null;
            switch (Root)
            {
                case PathRoot.TempAppStorage:
                    Folder = new CoreFolderContainer(Application.Context.CacheDir.Path);
                    break;

                case PathRoot.Application:
                    string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    Folder = new CoreFolderContainer(folderPath);
                    break;

                case PathRoot.AppStorageNoBackup:
                case PathRoot.LocalAppStorage:
                case PathRoot.RoamingAppStorage:
                    Folder = new CoreFolderContainer(Application.Context.FilesDir.Path);
                    break;
            }

            return Folder;
        }

        public override async Task OpenFileForDisplay(FileContainer File)
        {
            var activity = AndroidHelpers.GetCurrentActivity();
            ParcelFileDescriptor fd = null;
            Android.Net.Uri uri = null;
            try
            {
                if (File is AndroidSAFFileContainer providerfile)
                {
                    if (FileReshareProvider.Current != null)
                    {
                        var originaluri = providerfile.File.Uri;
                        fd = activity.ContentResolver.OpenFileDescriptor(originaluri, "r");
                        uri = FileReshareProvider.Current.GetShareableURI(fd, originaluri);
                    }
                }
                else
                {
                    Java.IO.File file = new Java.IO.File(File.Path);
                    file.SetReadable(true);
                    uri = Android.Net.Uri.FromFile(file);
                }

                Intent intent = new Intent(Intent.ActionView);
                var mimeType = activity.ContentResolver.GetType(uri);
                intent.SetDataAndType(uri, mimeType);

                await activity.StartActivityForResultAsync(intent);

                if (fd != null)
                {
                    fd.Dispose();
                }
            }
            catch (ActivityNotFoundException)
            {
                throw new DefaultAppNotFoundException(File.Name);
            }
        }

        //NOT SUPPORTED
        public override Task OpenFolderForDisplay(FolderContainer Folder, FolderOpenOptions Options)
        {
            throw new NotSupportedException();
        }
    }
}