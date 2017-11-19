using System;
using System.IO;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Models.Settings;
using System.Reflection;
using PlatformBindings.Common;
using PlatformBindings.Models.FileSystem.Options;

namespace PlatformBindings.Services
{
    public class CoreIOBindings : IOBindings
    {
        public override bool SupportsRoaming => false;
        public override bool SupportsOpenFolderForDisplay => false;
        public override bool SupportsOpenFileForDisplay => false;

        public override IFutureAccessManager FutureAccess => null;
        public override FileSystemPickers Pickers => null;

        public override ISettingsContainer LocalSettings { get; } = new CoreSettingsContainer(GetSettingsCluster(), null);

        private static CoreFolderContainer GetSettingsCluster()
        {
            var root = AppServices.IO.GetBaseFolder(PathRoot.Application);
            var settings = root.CreateFolderAsync("Settings").Result as CoreFolderContainer;
            return settings;
        }

        public override FolderContainer GetBaseFolder(PathRoot Root)
        {
            switch (Root)
            {
                case PathRoot.LocalAppStorage:
                case PathRoot.RoamingAppStorage:
                case PathRoot.AppStorageNoBackup:
                    if (NETCoreServices.UseGlobalAppData)
                    {
                        var folder = Root == PathRoot.LocalAppStorage ? Environment.SpecialFolder.LocalApplicationData : Environment.SpecialFolder.ApplicationData;
                        return new CoreFolderContainer(Environment.GetFolderPath(folder));
                    }
                    else return new CoreFolderContainer(PlatformBindingHelpers.ResolvePath(new FolderPath(PathRoot.Application, "AppData")));

                case PathRoot.Documents:
                    return new CoreFolderContainer(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

                case PathRoot.Pictures:
                    return new CoreFolderContainer(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

                case PathRoot.Videos:
                    return new CoreFolderContainer(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));

                case PathRoot.Music:
                    return new CoreFolderContainer(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));

                case PathRoot.Downloads:
                    var path = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
                    path = Path.Combine(path, "Downloads");
                    return new CoreFolderContainer(path);

                case PathRoot.Application:
                    return new CoreFolderContainer(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

                default:
                    return null;
            }
        }

        public override Task OpenFileForDisplay(FileContainer File)
        {
            throw new NotSupportedException();
        }

        public override Task OpenFolderForDisplay(FolderContainer Folder, FolderOpenOptions Options)
        {
            throw new NotImplementedException();
        }
    }
}