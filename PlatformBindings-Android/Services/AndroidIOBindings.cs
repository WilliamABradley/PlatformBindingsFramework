using Android.App;
using Android.Content;
using Java.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformBindings.Common;
using PlatformBindings.Enums;
using PlatformBindings.Models;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Models.Settings;

namespace PlatformBindings.Services
{
    public class AndroidIOBindings : IOBindings
    {
        public override bool SupportsFutureAccess => false;

        public override bool SupportsRoaming => false;

        public override bool SupportsOpenFolder => false;

        public override bool SupportsOpenFile => true;

        public override async Task<IFileContainer> CreateFile(FilePath Path)
        {
            var folder = await GetFolder(Path);
            return await folder.CreateFile(Path.FileName);
        }

        public override Task<IFileContainer> GetFile(string Path)
        {
            return Task.FromResult((IFileContainer)new CoreFileContainer(Path));
        }

        public override async Task<IFileContainer> GetFile(FilePath Path)
        {
            var folder = await GetFolder(Path);
            return await folder.GetFile(Path.FileName);
        }

        public override Task<IFolderContainer> GetFolder(string Path)
        {
            return Task.FromResult((IFolderContainer)new CoreFolderContainer(Path));
        }

        public override async Task<IFolderContainer> GetFolder(FolderPath Path)
        {
            IFolderContainer Folder = GetBaseFolder(Path.Root);

            foreach (var piece in IOHelpers.GetPathPieces(Path.Path))
            {
                Folder = await Folder.GetFolder(piece);
            }
            return Folder;
        }

        public override IFolderContainer GetBaseFolder(PathRoot Root)
        {
            IFolderContainer Folder = null;
            switch (Root)
            {
                case PathRoot.TempAppStorage:
                    Folder = new CoreFolderContainer(Application.Context.CacheDir.Path);
                    break;

                case PathRoot.Application:
                    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
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

        public override Task<bool> OpenFile(IFileContainer File)
        {
            bool success = false;
            try
            {
                Intent intent = new Intent(Intent.ActionView);
                var mimeType = URLConnection.GuessContentTypeFromName(File.Name);
                intent.SetDataAndType(Android.Net.Uri.Parse(File.Path), mimeType);

                TaskCompletionSource<ActivityResult> Waiter = new TaskCompletionSource<ActivityResult>();

                var uibinding = AppServices.Services.UI.DefaultUIBinding as AndroidUIBindingInfo;
                uibinding.Activity.StartActivityForResult(intent, 24);

                success = true;
            }
            catch { }

            return Task.FromResult(success);
        }

        public override Task<bool> OpenFolder(IFolderContainer Folder, FolderOpenOptions Options)
        {
            throw new NotImplementedException();
        }

        private async Task<ActivityResult> CreateFilePicker(FilePickerProperties Properties, bool Multiple)
        {
            Intent intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.PutExtra(Intent.ExtraAllowMultiple, Multiple);
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

            int requestCode = 42;
            TaskCompletionSource<ActivityResult> Waiter = new TaskCompletionSource<ActivityResult>();
            var uibinding = AppServices.Services.UI.DefaultUIBinding as AndroidUIBindingInfo;
            uibinding.Activity.StartActivityForResult(intent, requestCode);
            uibinding.Activity.ActivityReturned += (s, e) => Waiter.TrySetResult(e);

            var result = await Waiter.Task;
            return result.RequestCode == requestCode ? result : null;
        }

        public override async Task<IFileContainer> PickFile(FilePickerProperties Properties)
        {
            var result = await CreateFilePicker(Properties, false);
            if (result != null && result.ResultCode == Result.Ok && result.Data != null)
            {
                return ResolveToFile(result.Data.Data);
            }
            else return null;
        }

        private IFileContainer ResolveToFile(Android.Net.Uri Uri)
        {
            var path = PathResolver.ResolveFile(Application.Context, Uri);
            return new CoreFileContainer(path);
        }

        public override async Task<IReadOnlyList<IFileContainer>> PickFiles(FilePickerProperties Properties)
        {
            List<IFileContainer> Files = new List<IFileContainer>();

            var result = await CreateFilePicker(Properties, true);
            if (result != null && result.ResultCode == Result.Ok && result.Data != null)
            {
                var clipData = result.Data.ClipData;
                if (clipData != null)
                {
                    for (int i = 0; i < clipData.ItemCount; i++)
                    {
                        var item = clipData.GetItemAt(i);
                        Files.Add(ResolveToFile(item.Uri));
                    }
                }
                else
                {
                    Files.Add(ResolveToFile(result.Data.Data));
                }
                return Files;
            }
            return null;
        }

        public override async Task<IFolderContainer> PickFolder(FolderPickerProperties Properties)
        {
            AppServices.Services.UI.PromptUser("Error", "Folder resolving fails", "OK", null);

            Intent intent = new Intent(Intent.ActionOpenDocumentTree);

            int requestCode = 42;
            TaskCompletionSource<ActivityResult> Waiter = new TaskCompletionSource<ActivityResult>();
            var uibinding = AppServices.Services.UI.DefaultUIBinding as AndroidUIBindingInfo;
            uibinding.Activity.StartActivityForResult(intent, requestCode);
            uibinding.Activity.ActivityReturned += (s, e) => Waiter.TrySetResult(e);

            var result = await Waiter.Task;
            if (result != null && result.ResultCode == Result.Ok && result.Data != null)
            {
                var path = PathResolver.ResolveFile(Application.Context, result.Data.Data);
                return new CoreFolderContainer(path);
            }
            else return null;
        }

        //NOT SUPPORTED

        public void DeleteCredentials()
        {
            throw new NotImplementedException();
        }

        public override ISettingsContainer GetRoamingSettingsContainer()
        {
            throw new NotImplementedException();
        }

        public override string GetFutureAccessToken(IFolderContainer Folder)
        {
            throw new NotImplementedException();
        }

        public override void RemoveFutureAccessToken(string Token)
        {
            throw new NotImplementedException();
        }
    }
}