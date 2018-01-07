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
        /// Used to Determine if Picking Files is supported using a File Picker.
        /// </summary>
        public abstract bool SupportsPickFile { get; }

        /// <summary>
        /// Used to Determine if Picking Folders is supported using a Folder Picker.
        /// </summary>
        public abstract bool SupportsPickFolder { get; }

        /// <summary>
        /// Used to Determine if Saving Files is supported using a File Save Picker.
        /// </summary>
        public abstract bool SupportsSaveFile { get; }

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

        /// <summary>
        /// Opens a File Save Picker to save a file from the Operating System. See <see cref="SupportsPickFile"/> to ensure that this is Supported.
        /// </summary>
        /// <returns>A saved file, or <see cref="null"/> if Cancelled</returns>
        public Task<FileContainer> SaveFile()
        {
            return SaveFile(null);
        }

        /// <summary>
        /// Opens a File Save Picker to save a file from the Operating System. See <see cref="SupportsPickFile"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="Properties">Properties for saving the file.</param>
        /// <returns>A saved file, or <see cref="null"/> if Cancelled</returns>
        public abstract Task<FileContainer> SaveFile(FileSavePickerProperties Properties);
    }
}