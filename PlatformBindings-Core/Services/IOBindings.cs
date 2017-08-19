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
        /// <summary>
        /// Gets a File from a Raw Path, this path must observe the Operating System's Path Structure to work correctly.
        /// </summary>
        /// <param name="Path">OS Style Path</param>
        /// <returns>Specified File</returns>
        public abstract Task<FileContainerBase> GetFile(string Path);

        /// <summary>
        /// Gets a File from the FileSystem, the Path is Resolved and is OS Independent.
        /// </summary>
        /// <param name="Path">File Path</param>
        /// <returns>Specified File</returns>
        public abstract Task<FileContainerBase> GetFile(FilePath Path);

        /// <summary>
        /// Creates a File from the Resolved Path.
        /// </summary>
        /// <param name="Path">File Path</param>
        /// <returns>Created File</returns>
        public abstract Task<FileContainerBase> CreateFile(FilePath Path);

        /// <summary>
        /// Gets a Folder from a Raw Path, this path must observe the Operating System's Path Structure to work correctly.
        /// </summary>
        /// <param name="Path">OS Style Path</param>
        /// <returns>Specified Folder</returns>
        public abstract Task<FolderContainerBase> GetFolder(string Path);

        /// <summary>
        /// Gets a Folder from the FileSystem, the Path is Resolved and is OS Independent.
        /// </summary>
        /// <param name="Path">Folder Path</param>
        /// <returns>Specified Folder</returns>
        public abstract Task<FolderContainerBase> GetFolder(FolderPath Path);

        /// <summary>
        /// Gets the closest OS Equivalent Folder from the provided PathRoot.
        /// </summary>
        /// <param name="Root">Requested Root Folder</param>
        /// <returns>Root Folder</returns>
        public abstract FolderContainerBase GetBaseFolder(PathRoot Root);

        /// <summary>
        /// Opens a File Picker to Pick Files from the Operating System. See <see cref="SupportsPickFile"/> to ensure that this is Supported.
        /// </summary>
        /// <returns>A List of User Picked Files, or <see cref="null"/> if Cancelled</returns>
        public Task<IReadOnlyList<FileContainerBase>> PickFiles()
        {
            return PickFiles(null);
        }

        /// <summary>
        /// Opens a File Picker to Pick Files from the Operating System. See <see cref="SupportsPickFile"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="Properties">Filters to narrow down the Observable Files.</param>
        /// <returns>A List of User Picked Files, or <see cref="null"/> if Cancelled</returns>
        public abstract Task<IReadOnlyList<FileContainerBase>> PickFiles(FilePickerProperties Properties);

        /// <summary>
        /// Opens a File Picker to Pick a File from the Operating System. See <see cref="SupportsPickFile"/> to ensure that this is Supported.
        /// </summary>
        /// <returns>A Picked File, or <see cref="null"/> if Cancelled</returns>
        public Task<FileContainerBase> PickFile()
        {
            return PickFile(null);
        }

        /// <summary>
        /// Opens a File Picker to Pick a File from the Operating System. See <see cref="SupportsPickFile"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="Properties">Filters to narrow down the Observable Files.</param>
        /// <returns>A Picked File, or <see cref="null"/> if Cancelled</returns>
        public abstract Task<FileContainerBase> PickFile(FilePickerProperties Properties);

        /// <summary>
        /// Opens a Folder Picker to Pick a Folder from the Operating System. See <see cref="SupportsPickFolder"/> to ensure that this is Supported.
        /// </summary>
        /// <returns>A Picked Folder, or <see cref="null"/> if Cancelled</returns>
        public Task<FolderContainerBase> PickFolder()
        {
            return PickFolder(null);
        }

        /// <summary>
        /// Opens a Folder Picker to Pick a Folder from the Operating System. See <see cref="SupportsPickFolder"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="Properties">Filters to narrow down the Observable Files.</param>
        /// <returns>A Picked Folder, or <see cref="null"/> if Cancelled</returns>
        public abstract Task<FolderContainerBase> PickFolder(FolderPickerProperties Properties);

        /// <summary>
        /// Opens a Folder for Viewing using the File Manager for the Operating System. See <see cref="SupportsOpenFolder"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="Folder">Folder to Open</param>
        /// <returns>Task Success</returns>
        public Task<bool> OpenFolder(FolderContainerBase Folder)
        {
            return OpenFolder(Folder);
        }

        /// <summary>
        /// Opens a Folder for Viewing using the File Manager for the Operating System. See <see cref="SupportsOpenFolder"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="Folder">Folder to Open</param>
        /// <param name="Options">Options for modifying how the Folder is Displayed, such as Pre-Selecting Files/Folders if supported</param>
        /// <returns>Task Success</returns>
        public abstract Task<bool> OpenFolder(FolderContainerBase Folder, FolderOpenOptions Options);

        /// <summary>
        /// Opens a File for Viewing in the Default Application for the File in the Operating System. See <see cref="SupportsOpenFile"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="File">File to Open</param>
        /// <returns>Task Success</returns>
        public abstract Task<bool> OpenFile(FileContainerBase File);

        /// <summary>
        /// Gets the Roaming Settings Cluster. See <see cref="SupportsRoaming"/> to ensure that this is Supported, or use <see cref="Common.PlatformBindingHelpers.GetSettingsContainer"/> to Attempt getting Roaming Settings before defaulting to Local Settings.
        /// </summary>
        /// <returns>The Roaming Settings Cluster if Supported.</returns>
        public abstract ISettingsContainer GetRoamingSettingsContainer();

        /// <summary>
        /// Gets the Local Settings Cluster.
        /// </summary>
        /// <returns>The Local Settings Cluster.</returns>
        public abstract ISettingsContainer GetLocalSettingsContainer();

        /// <summary>
        /// Creates a Future Access Token for a Specified Folder. <see cref="RequiresFutureAccessToken"/> to ensure that this is Required. <para/>
        /// This is Required on UWP to access User Files and Folders in future Sessions of the App, if the App is closed. You can store this Token in Local Settings to Access this file/folder Again.
        /// </summary>
        /// <param name="Item">File/Folder to get Access Token for.</param>
        /// <returns>Future Access Token.</returns>
        public abstract string GetFutureAccessToken(FileSystemContainerBase Item);

        /// <summary>
        /// Removes a file/folder from the Future Access List, using the Future Access Token.
        /// </summary>
        /// <param name="Token">Token of File/Folder to Remove</param>
        public abstract void RemoveFutureAccessToken(string Token);

        /// <summary>
        /// Used to Determine if the OS Requires Future Access Tokens to access Files/Folders in Future Sessions.
        /// </summary>
        public abstract bool RequiresFutureAccessToken { get; }

        /// <summary>
        /// Used to Determine if the OS supports Roaming AppData accross Devices.
        /// </summary>
        public abstract bool SupportsRoaming { get; }

        /// <summary>
        /// Used to Determine if the OS Supports Picking Files using a File Picker.
        /// </summary>
        public abstract bool SupportsPickFile { get; }

        /// <summary>
        /// Used to Determine if the OS Supports Picking Folders using a Folder Picker.
        /// </summary>
        public abstract bool SupportsPickFolder { get; }

        /// <summary>
        /// Used to Determine if the OS Supports Displaying Folders using the OS File Manager.
        /// </summary>
        public abstract bool SupportsOpenFolder { get; }

        /// <summary>
        /// Used to Determine if the OS Supports Opening Files with the Default Application for a File.
        /// </summary>
        public abstract bool SupportsOpenFile { get; }
    }
}