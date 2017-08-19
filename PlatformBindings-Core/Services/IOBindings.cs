using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Models.Settings;

namespace PlatformBindings.Services
{
    /// <summary>
    /// Methods for handling IO through the File System, and Settings Clusters
    /// </summary>
    public abstract partial class IOBindings
    {
        public abstract Task<FileContainerBase> GetFile(string Path);

        public abstract Task<FileContainerBase> GetFile(FilePath Path);

        public abstract Task<FileContainerBase> CreateFile(FilePath Path);

        public abstract Task<FolderContainerBase> GetFolder(string Path);

        public abstract Task<FolderContainerBase> GetFolder(FolderPath Path);

        public abstract FolderContainerBase GetBaseFolder(PathRoot Root);

        public abstract Task<IReadOnlyList<FileContainerBase>> PickFiles(FilePickerProperties Properties);

        public abstract Task<FileContainerBase> PickFile(FilePickerProperties Properties);

        public abstract Task<FolderContainerBase> PickFolder(FolderPickerProperties Properties);

        public abstract Task<bool> OpenFolder(FolderContainerBase Folder, FolderOpenOptions Options);

        public abstract Task<bool> OpenFile(FileContainerBase File);

        public abstract ISettingsContainer GetRoamingSettingsContainer();

        public abstract ISettingsContainer GetLocalSettingsContainer();

        public abstract string GetFutureAccessToken(FolderContainerBase Folder);

        public abstract void RemoveFutureAccessToken(string Token);

        public abstract bool SupportsFutureAccess { get; }

        public abstract bool SupportsRoaming { get; }

        public abstract bool SupportsOpenFolder { get; }

        public abstract bool SupportsOpenFile { get; }
    }
}