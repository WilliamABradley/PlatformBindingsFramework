using Android.App;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformBindings.Common;
using PlatformBindings.Enums;
using PlatformBindings.Models;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Models.Settings;
using Android.OS;
using PlatformBindings.Exceptions;

namespace PlatformBindings.Services
{
    public class AndroidIOBindings : IOBindings
    {
        public override async Task<FileContainer> CreateFile(FilePath Path)
        {
            var folder = await GetFolder(Path);
            return await folder.CreateFileAsync(Path.FileName);
        }

        public override Task<FileContainer> GetFile(string Path)
        {
            return Task.FromResult((FileContainer)new CoreFileContainer(Path));
        }

        public override async Task<FileContainer> GetFile(FilePath Path)
        {
            var folder = await GetFolder(Path);
            return await folder.GetFileAsync(Path.FileName);
        }

        public override Task<FolderContainer> GetFolder(string Path)
        {
            return Task.FromResult((FolderContainer)new CoreFolderContainer(Path));
        }

        public override async Task<FolderContainer> GetFolder(FolderPath Path)
        {
            FolderContainer Folder = GetBaseFolder(Path.Root);

            foreach (var piece in PlatformBindingHelpers.GetPathPieces(Path.Path))
            {
                Folder = await Folder.GetFolderAsync(piece);
            }
            return Folder;
        }

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

        public override ISettingsContainer GetLocalSettingsContainer()
        {
            var name = Application.Context.PackageName.Split('.').First();
            return new AndroidSettingsContainer(name, null);
        }

        public override async Task OpenFile(FileContainer File)
        {
            var activity = AndroidHelpers.GetCurrentActivity();
            ParcelFileDescriptor fd = null;
            Android.Net.Uri uri = null;
            try
            {
                if (File is AndroidSAFFileContainer providerfile)
                {
                    var originaluri = providerfile.File.Uri;
                    fd = activity.ContentResolver.OpenFileDescriptor(originaluri, "r");
                    uri = FileReshareProvider.GetShareableURI(fd, originaluri);
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

        public override Task OpenFolder(FolderContainer Folder, FolderOpenOptions Options)
        {
            throw new NotImplementedException();
        }

        private async Task<ActivityResult> CreateFilePicker(FilePickerProperties Properties, bool Multiple)
        {
            Intent intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            if (Multiple) intent.PutExtra(Intent.ExtraAllowMultiple, Multiple);
            bool HasNoTypes = true;

            if (Properties != null)
            {
                if (Properties.FileTypes.Any())
                {
                    HasNoTypes = false;

                    intent.SetType(Properties.FileTypes.First().MimeType);

                    if (Properties.FileTypes.Count > 1)
                    {
                        string Altmimes = "";
                        var others = Properties.FileTypes.Skip(1);
                        int count = others.Count();
                        for (int i = 0; i < count; i++)
                        {
                            Altmimes += others.ElementAt(i).MimeType;
                            if (i + 1 < count) Altmimes += "|";
                        }
                        intent.PutExtra(Intent.ExtraMimeTypes, Altmimes);
                    }
                }
            }

            if (HasNoTypes) intent.SetType("*/*");
            return await AndroidHelpers.GetCurrentActivity().StartActivityForResultAsync(intent);
        }

        public override async Task<FileContainer> PickFile(FilePickerProperties Properties)
        {
            var result = await CreateFilePicker(Properties, false);
            if (result != null && result.ResultCode == Result.Ok && result.Data != null)
            {
                return new AndroidSAFFileContainer(result.Data.Data);
            }
            else return null;
        }

        public override async Task<IReadOnlyList<FileContainer>> PickFiles(FilePickerProperties Properties)
        {
            List<FileContainer> Files = new List<FileContainer>();

            var result = await CreateFilePicker(Properties, true);
            if (result != null && result.ResultCode == Result.Ok && result.Data != null)
            {
                var clipData = result.Data.ClipData;
                if (clipData != null)
                {
                    for (int i = 0; i < clipData.ItemCount; i++)
                    {
                        var item = clipData.GetItemAt(i);
                        Files.Add(new AndroidSAFFileContainer(item.Uri));
                    }
                }
                else
                {
                    Files.Add(new AndroidSAFFileContainer(result.Data.Data));
                }
                return Files;
            }
            return null;
        }

        public override async Task<FolderContainer> PickFolder(FolderPickerProperties Properties)
        {
            Intent intent = new Intent(Intent.ActionOpenDocumentTree);
            var activity = AndroidHelpers.GetCurrentActivity();

            var result = await activity.StartActivityForResultAsync(intent);
            if (result != null && result.ResultCode == Result.Ok && result.Data != null)
            {
                var uri = result.Data.Data;
                var flags = ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission;
                activity.GrantUriPermission(activity.PackageName, uri, flags);
                activity.ContentResolver.TakePersistableUriPermission(uri, result.Data.Flags & flags);
                return new AndroidSAFFolderContainer(uri);
            }
            else return null;
        }

        //NOT SUPPORTED

        public override ISettingsContainer GetRoamingSettingsContainer()
        {
            return GetLocalSettingsContainer();
        }

        public override string GetFutureAccessToken(FileSystemContainer Item)
        {
            var uri = Item is AndroidSAFFolderContainer folder ? folder.Folder.Uri : Item is AndroidSAFFileContainer file ? file.File.Uri : null;
            if (uri != null)
            {
                AndroidHelpers.GetCurrentActivity().ContentResolver.TakePersistableUriPermission(uri, ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);
                return uri.SchemeSpecificPart;
            }
            else return null;
        }

        public override void RemoveFutureAccessToken(string Token)
        {
            var resolver = AndroidHelpers.GetCurrentActivity().ContentResolver;
            var permission = resolver.PersistedUriPermissions.FirstOrDefault(item => item.Uri.SchemeSpecificPart == Token);
            if (permission != null)
            {
                resolver.PersistedUriPermissions.Remove(permission);
            }
        }

        public override bool RequiresFutureAccessToken => true;

        public override bool SupportsRoaming => false;

        public override bool SupportsOpenFolder => false;

        public override bool SupportsOpenFile => true;

        public override bool SupportsPickFile => true;

        public override bool SupportsPickFolder => true;
    }
}