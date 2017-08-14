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

        public override Task<IFileContainer> GetFile(string Path)
        {
            throw new NotImplementedException();
        }

        public override Task<IFileContainer> GetFile(FilePath Path)
        {
            throw new NotImplementedException();
        }

        public override Task<IFileContainer> CreateFile(FilePath Path)
        {
            throw new NotImplementedException();
        }

        public override Task<IFolderContainer> GetFolder(string Path)
        {
            throw new NotImplementedException();
        }

        public override Task<IFolderContainer> GetFolder(FolderPath Path)
        {
            throw new NotImplementedException();
        }

        public override IFolderContainer GetBaseFolder(PathRoot Root)
        {
            throw new NotImplementedException();
        }

        public override Task<IReadOnlyList<IFileContainer>> PickFiles(FilePickerProperties Properties)
        {
            throw new NotImplementedException();
        }

        public override Task<IFileContainer> PickFile(FilePickerProperties Properties)
        {
            throw new NotImplementedException();
        }

        public override Task<IFolderContainer> PickFolder(FolderPickerProperties Properties)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> OpenFolder(IFolderContainer Folder, FolderOpenOptions Options)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> OpenFile(IFileContainer File)
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