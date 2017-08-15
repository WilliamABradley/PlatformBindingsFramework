using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Models.Settings;
using PlatformBindings.Services;

namespace PlatformBindings.ConsoleTools
{
    public class ConsoleIOBindings : IOBindings
    {
        public override bool SupportsFutureAccess => false;

        public override bool SupportsRoaming => false;

        public override bool SupportsOpenFolder => false;

        public override bool SupportsOpenFile => false;

        private DirectoryInfo GetSettingsCluster()
        {
            var root = Path.GetDirectoryName(ConsoleServices.EntryAssembly);
            var settings = new DirectoryInfo(root).CreateSubdirectory("Settings");
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
            throw new NotImplementedException();
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

        public override string GetFutureAccessToken(FolderContainerBase Folder)
        {
            throw new NotImplementedException();
        }

        public override void RemoveFutureAccessToken(string Token)
        {
            throw new NotImplementedException();
        }
    }
}