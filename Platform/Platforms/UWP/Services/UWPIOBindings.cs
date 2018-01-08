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
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.Settings;
using Windows.ApplicationModel;
using Windows.Storage;
using PlatformBindings.Models.FileSystem;
using Windows.System;
using PlatformBindings.Models.FileSystem.Options;
using PlatformBindings.Common;

namespace PlatformBindings.Services
{
    public class UWPIOBindings : IOBindings
    {
        public UWPIOBindings()
        {
            RemoveResolver(typeof(CorePathResolver));
            AddResolverFirst(new UWPPathResolver());
        }

        public override bool SupportsOpenFolderForDisplay => true;
        public override bool SupportsOpenFileForDisplay => true;

        public override IFutureAccessManager FutureAccess { get; } = new UWPFutureAccessManager();
        public override FileSystemPickers Pickers { get; } = new UWPFileSystemPickers();

        public override ISettingsContainer LocalSettings { get; } = new UWPSettingsContainer(ApplicationData.Current.LocalSettings, null);
        public override ISettingsContainer RoamingSettings { get; } = new UWPSettingsContainer(ApplicationData.Current.RoamingSettings, null);

        public override FolderContainer GetBaseFolder(PathRoot Root)
        {
            switch (Root)
            {
                case PathRoot.LocalAppStorage:
                    return new UWPFolderContainer(ApplicationData.Current.LocalFolder);

                case PathRoot.RoamingAppStorage:
                    return new UWPFolderContainer(ApplicationData.Current.RoamingFolder);

                case PathRoot.AppStorageNoBackup:
                    return new UWPFolderContainer(ApplicationData.Current.LocalCacheFolder);

                case PathRoot.TempAppStorage:
                    return new UWPFolderContainer(ApplicationData.Current.TemporaryFolder);

                case PathRoot.Documents:
                    return new UWPFolderContainer(KnownFolders.DocumentsLibrary);

                case PathRoot.Pictures:
                    return new UWPFolderContainer(KnownFolders.PicturesLibrary);

                case PathRoot.Videos:
                    return new UWPFolderContainer(KnownFolders.VideosLibrary);

                case PathRoot.Music:
                    return new UWPFolderContainer(KnownFolders.MusicLibrary);

                case PathRoot.Application:
                    return new UWPFolderContainer(Package.Current.InstalledLocation);
            }
            return new UWPFolderContainer(ApplicationData.Current.LocalFolder);
        }

        public override Task OpenFolderForDisplay(FolderContainer Folder, FolderOpenOptions Options = null)
        {
            return PlatformBindingHelpers.OnUIThreadAsync(async () =>
            {
                var container = Folder as UWPFolderContainer;
                var folder = container.Folder;

                var LaunchOptions = new FolderLauncherOptions();
                if (Options != null)
                {
                    foreach (StorageContainer FileSystemItem in Options.ItemsToSelect)
                    {
                        IStorageItem Item = null;
                        if (FileSystemItem is UWPFolderContainer winfolder) Item = winfolder.Folder;
                        else if (FileSystemItem is UWPFileContainer winfile) Item = winfile.File;

                        LaunchOptions.ItemsToSelect.Add(Item);
                    }
                }

                await Launcher.LaunchFolderAsync(folder, LaunchOptions);
            });
        }

        public override Task OpenFileForDisplay(FileContainer File)
        {
            return PlatformBindingHelpers.OnUIThreadAsync(async () =>
            {
                var container = File as UWPFileContainer;
                var file = container.File;

                await Launcher.LaunchFileAsync(file);
            });
        }
    }
}