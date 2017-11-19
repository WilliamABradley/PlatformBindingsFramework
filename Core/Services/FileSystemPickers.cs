using PlatformBindings.Models.FileSystem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformBindings.Services
{
    /// <summary>
    /// Provides ways for Users to Pick Files and Folders.
    /// </summary>
    public abstract class FileSystemPickers
    {
        /// <summary>
        /// Used to Determine if the OS Supports Picking Files using a File Picker.
        /// </summary>
        public abstract bool SupportsPickFile { get; }

        /// <summary>
        /// Used to Determine if the OS Supports Picking Folders using a Folder Picker.
        /// </summary>
        public abstract bool SupportsPickFolder { get; }

        /// <summary>
        /// Opens a File Picker to Pick Files from the Operating System. See <see cref="SupportsPickFile"/> to ensure that this is Supported.
        /// </summary>
        /// <returns>A List of User Picked Files, or <see cref="null"/> if Cancelled</returns>
        public Task<IReadOnlyList<FileContainer>> PickFiles()
        {
            return PickFiles(null);
        }

        /// <summary>
        /// Opens a File Picker to Pick Files from the Operating System. See <see cref="SupportsPickFile"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="Properties">Filters to narrow down the Observable Files.</param>
        /// <returns>A List of User Picked Files, or <see cref="null"/> if Cancelled</returns>
        public abstract Task<IReadOnlyList<FileContainer>> PickFiles(FilePickerProperties Properties);

        /// <summary>
        /// Opens a File Picker to Pick a File from the Operating System. See <see cref="SupportsPickFile"/> to ensure that this is Supported.
        /// </summary>
        /// <returns>A Picked File, or <see cref="null"/> if Cancelled</returns>
        public Task<FileContainer> PickFile()
        {
            return PickFile(null);
        }

        /// <summary>
        /// Opens a File Picker to Pick a File from the Operating System. See <see cref="SupportsPickFile"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="Properties">Filters to narrow down the Observable Files.</param>
        /// <returns>A Picked File, or <see cref="null"/> if Cancelled</returns>
        public abstract Task<FileContainer> PickFile(FilePickerProperties Properties);

        /// <summary>
        /// Opens a Folder Picker to Pick a Folder from the Operating System. See <see cref="SupportsPickFolder"/> to ensure that this is Supported.
        /// </summary>
        /// <returns>A Picked Folder, or <see cref="null"/> if Cancelled</returns>
        public Task<FolderContainer> PickFolder()
        {
            return PickFolder(null);
        }

        /// <summary>
        /// Opens a Folder Picker to Pick a Folder from the Operating System. See <see cref="SupportsPickFolder"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="Properties">Filters to narrow down the Observable Files.</param>
        /// <returns>A Picked Folder, or <see cref="null"/> if Cancelled</returns>
        public abstract Task<FolderContainer> PickFolder(FolderPickerProperties Properties);
    }
}