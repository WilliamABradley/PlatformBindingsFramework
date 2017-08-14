using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Models.Settings;

namespace PlatformBindings.Services
{
    public abstract partial class IOBindings
    {
        public abstract Task<IFileContainer> GetFile(string Path);

        public abstract Task<IFileContainer> GetFile(FilePath Path);

        public abstract Task<IFileContainer> CreateFile(FilePath Path);

        public abstract Task<IFolderContainer> GetFolder(string Path);

        public abstract Task<IFolderContainer> GetFolder(FolderPath Path);

        public abstract IFolderContainer GetBaseFolder(PathRoot Root);

        public abstract Task<IReadOnlyList<IFileContainer>> PickFiles(FilePickerProperties Properties);

        public abstract Task<IFileContainer> PickFile(FilePickerProperties Properties);

        public abstract Task<IFolderContainer> PickFolder(FolderPickerProperties Properties);

        public abstract Task<bool> OpenFolder(IFolderContainer Folder, FolderOpenOptions Options);

        public abstract Task<bool> OpenFile(IFileContainer File);

        public abstract ISettingsContainer GetRoamingSettingsContainer();

        public abstract ISettingsContainer GetLocalSettingsContainer();

        public abstract string GetFutureAccessToken(IFolderContainer Folder);

        public abstract void RemoveFutureAccessToken(string Token);

        public abstract bool SupportsFutureAccess { get; }

        public abstract bool SupportsRoaming { get; }

        public abstract bool SupportsOpenFolder { get; }

        public abstract bool SupportsOpenFile { get; }
    }
}