using System;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.Settings;
using Windows.ApplicationModel;
using Windows.Storage;
using PlatformBindings.Models.FileSystem;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using System.Linq;
using Windows.Storage.AccessCache;
using Windows.System;
using PlatformBindings.Common;

namespace PlatformBindings.Services
{
    public class WinIOBindings : IOBindings
    {
        public WinIOBindings()
        {
            Current = this;
        }

        public static WinIOBindings Current { get; private set; }

        public override bool SupportsFutureAccess => true;

        public override bool SupportsRoaming => true;

        public override bool SupportsOpenFolder => true;

        public override bool SupportsOpenFile => true;

        public override async Task<FolderContainerBase> GetFolder(FolderPath Path)
        {
            var folder = (GetBaseFolder(Path.Root) as WinFolderContainer).Folder;
            string path = Path.Path;

            foreach (var piece in PlatformBindingHelpers.GetPathPieces(path))
            {
                var task = folder.CreateFolderAsync(piece, Windows.Storage.CreationCollisionOption.OpenIfExists).AsTask();
                await task;
                if (task.Exception != null)
                    throw task.Exception;
                folder = task.Result;
            }
            return new WinFolderContainer(folder);
        }

        public override async Task<FileContainerBase> GetFile(FilePath Path)
        {
            var folder = await GetFolder(Path);
            return await folder.GetFileAsync(Path.FileName);
        }

        public override async Task<FileContainerBase> CreateFile(FilePath Path)
        {
            var folder = await GetFolder(Path);
            return await folder.CreateFileAsync(Path.FileName);
        }

        public override ISettingsContainer GetLocalSettingsContainer()
        {
            return new WinSettingsContainer(ApplicationData.Current.LocalSettings, null);
        }

        public override ISettingsContainer GetRoamingSettingsContainer()
        {
            return new WinSettingsContainer(ApplicationData.Current.RoamingSettings, null);
        }

        public override FolderContainerBase GetBaseFolder(PathRoot Root)
        {
            switch (Root)
            {
                case PathRoot.LocalAppStorage:
                    return new WinFolderContainer(ApplicationData.Current.LocalFolder);

                case PathRoot.RoamingAppStorage:
                    return new WinFolderContainer(ApplicationData.Current.RoamingFolder);

                case PathRoot.AppStorageNoBackup:
                    return new WinFolderContainer(ApplicationData.Current.LocalCacheFolder);

                case PathRoot.TempAppStorage:
                    return new WinFolderContainer(ApplicationData.Current.TemporaryFolder);

                case PathRoot.Documents:
                    return new WinFolderContainer(KnownFolders.DocumentsLibrary);

                case PathRoot.Videos:
                    return new WinFolderContainer(KnownFolders.VideosLibrary);

                case PathRoot.Music:
                    return new WinFolderContainer(KnownFolders.MusicLibrary);

                case PathRoot.Application:
                    return new WinFolderContainer(Package.Current.InstalledLocation);
            }
            return new WinFolderContainer(ApplicationData.Current.RoamingFolder);
        }

        public override async Task<IReadOnlyList<FileContainerBase>> PickFiles(FilePickerProperties Properties = null)
        {
            var picker = GetFilePicker(Properties);
            var files = await picker.PickMultipleFilesAsync();
            return files?.Select(item => new WinFileContainer(item)).ToList();
        }

        public override async Task<FileContainerBase> PickFile(FilePickerProperties Properties = null)
        {
            var picker = GetFilePicker(Properties);
            var file = await picker.PickSingleFileAsync();
            return file != null ? new WinFileContainer(file) : null;
        }

        private FileOpenPicker GetFilePicker(FilePickerProperties Properties = null)
        {
            FileOpenPicker picker = new FileOpenPicker();
            if (Properties != null)
            {
                foreach (var property in Properties.FileTypes)
                {
                    picker.FileTypeFilter.Add(property.FileExtension);
                }
                picker.SuggestedStartLocation = GetPickerLocation(Properties.StartingLocation.Value);
            }

            if (Properties == null || Properties.FileTypes.Any()) picker.FileTypeFilter.Add("*");
            return picker;
        }

        private PickerLocationId GetPickerLocation(PathRoot? Root)
        {
            switch (Root)
            {
                case PathRoot.Documents:
                    return PickerLocationId.DocumentsLibrary;

                case PathRoot.Downloads:
                    return PickerLocationId.Downloads;

                case PathRoot.Videos:
                    return PickerLocationId.VideosLibrary;

                case PathRoot.Music:
                    return PickerLocationId.MusicLibrary;

                default:
                    return PickerLocationId.Unspecified;
            }
        }

        public override async Task<FolderContainerBase> PickFolder(FolderPickerProperties Properties = null)
        {
            var picker = new FolderPicker();
            if (Properties != null)
            {
                foreach (var property in Properties.FileTypes)
                {
                    picker.FileTypeFilter.Add(property.FileExtension);
                }
                picker.SuggestedStartLocation = GetPickerLocation(Properties.StartingLocation.Value);
            }

            if (Properties == null || Properties.FileTypes.Any()) picker.FileTypeFilter.Add("*");

            var folder = await picker.PickSingleFolderAsync();

            return folder != null ? new WinFolderContainer(folder) : null;
        }

        public override string GetFutureAccessToken(FolderContainerBase Folder)
        {
            var folder = Folder as WinFolderContainer;
            return StorageApplicationPermissions.FutureAccessList.Add(folder.Folder);
        }

        public override void RemoveFutureAccessToken(string Token)
        {
            StorageApplicationPermissions.FutureAccessList.Remove(Token);
        }

        public override async Task<FileContainerBase> GetFile(string Path)
        {
            var file = await StorageFile.GetFileFromPathAsync(Path);
            return new WinFileContainer(file);
        }

        public override async Task<FolderContainerBase> GetFolder(string Path)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(Path);
            return new WinFolderContainer(folder);
        }

        public override async Task<bool> OpenFolder(FolderContainerBase Folder, FolderOpenOptions Options = null)
        {
            var container = Folder as WinFolderContainer;
            var folder = container.Folder;

            var LaunchOptions = new FolderLauncherOptions();
            if (Options != null)
            {
                foreach (FileSystemContainerBase FileSystemItem in Options.ItemsToSelect)
                {
                    IStorageItem Item = null;
                    if (FileSystemItem is WinFolderContainer winfolder) Item = winfolder.Folder;
                    else if (FileSystemItem is WinFileContainer winfile) Item = winfile.File;

                    LaunchOptions.ItemsToSelect.Add(Item);
                }
            }

            return await Launcher.LaunchFolderAsync(folder, LaunchOptions);
        }

        public override async Task<bool> OpenFile(FileContainerBase File)
        {
            var container = File as WinFileContainer;
            var file = container.File;

            return await Launcher.LaunchFileAsync(file);
        }
    }
}