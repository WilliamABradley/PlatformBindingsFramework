using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Models.Settings;
using System.Reflection;
using PlatformBindings.Common;

namespace PlatformBindings.Services
{
    public class CoreIOBindings : IOBindings
    {
        public override bool RequiresFutureAccessToken => false;

        public override bool SupportsRoaming => false;

        public override bool SupportsOpenFolder => false;

        public override bool SupportsOpenFile => false;

        public override bool SupportsPickFile => false;

        public override bool SupportsPickFolder => false;

        private CoreFolderContainer GetSettingsCluster()
        {
            var root = GetBaseFolder(PathRoot.Application);
            var settings = root.CreateFolderAsync("Settings").Result as CoreFolderContainer;
            return settings;
        }

        public override ISettingsContainer GetLocalSettingsContainer()
        {
            var root = GetSettingsCluster();
            return new CoreSettingsContainer(root, null);
        }

        public override ISettingsContainer GetRoamingSettingsContainer()
        {
            return GetLocalSettingsContainer();
        }

        public override Task<FileContainerBase> GetFile(string Path)
        {
            throw new NotImplementedException();
        }

        public override Task<FileContainerBase> GetFile(FilePath Path)
        {
            throw new NotImplementedException();
        }

        public override Task<FileContainerBase> CreateFile(FilePath Path)
        {
            throw new NotImplementedException();
        }

        public override Task<FolderContainerBase> GetFolder(string Path)
        {
            throw new NotImplementedException();
        }

        public override Task<FolderContainerBase> GetFolder(FolderPath Path)
        {
            throw new NotImplementedException();
        }

        public override FolderContainerBase GetBaseFolder(PathRoot Root)
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

        public override Task<IReadOnlyList<FileContainerBase>> PickFiles(FilePickerProperties Properties)
        {
            throw new NotImplementedException();
        }

        public override Task<FileContainerBase> PickFile(FilePickerProperties Properties)
        {
            throw new NotImplementedException();
        }

        public override Task<FolderContainerBase> PickFolder(FolderPickerProperties Properties)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> OpenFolder(FolderContainerBase Folder, FolderOpenOptions Options)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> OpenFile(FileContainerBase File)
        {
            throw new NotImplementedException();
        }

        public override string GetFutureAccessToken(FileSystemContainerBase Item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveFutureAccessToken(string Token)
        {
            throw new NotImplementedException();
        }
    }
}