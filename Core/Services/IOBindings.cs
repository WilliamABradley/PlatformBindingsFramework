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

using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Models.Settings;
using PlatformBindings.Common;
using PlatformBindings.Models.FileSystem.Options;
using System.Linq;

namespace PlatformBindings.Services
{
    /// <summary>
    /// Methods for handling IO through the File System, and Settings Clusters
    /// </summary>
    public abstract class IOBindings
    {
        static IOBindings()
        {
            PathResolvers.Add(new CorePathResolver());
        }

        /// <summary>
        /// Adds a Path Resolver to the top of the Stack, this Resolver will evaluate first.
        /// </summary>
        /// <param name="Resolver">Resolver</param>
        public static void AddResolver(IPathResolver Resolver)
        {
            PathResolvers.Add(Resolver);
        }

        /// <summary>
        /// Adds a Path Resolver to the Bottom of the Stack, this Resolver will evaluate last.
        /// </summary>
        /// <param name="Resolver">Resolver</param>
        public static void AddResolverFirst(IPathResolver Resolver)
        {
            PathResolvers.Insert(0, Resolver);
        }

        /// <summary>
        /// Removes a Resolver from the evaluation Stack.
        /// </summary>
        /// <param name="Resolver">Resolver Type</param>
        public static void RemoveResolver(System.Type Resolver)
        {
            var resolver = PathResolvers.FirstOrDefault(item => item.GetType().Equals(Resolver));
            if (resolver != null) PathResolvers.Remove(resolver);
        }

        /// <summary>
        /// Gets the closest OS Equivalent Folder from the provided PathRoot.
        /// </summary>
        /// <param name="Root">Requested Root Folder</param>
        /// <returns>Root Folder</returns>
        public abstract FolderContainer GetBaseFolder(PathRoot Root);

        /// <summary>
        /// Attempts to get a File/Folder from a Raw Path, this path will be evaulated by any Available <see cref="IPathResolver"/>. <para/>
        /// To add a Path Resolver, use <see cref="AddResolver(IPathResolver)"/> or <see cref="AddResolverFirst(IPathResolver)"/>.
        /// </summary>
        /// <param name="Path">Path to File System Item</param>
        /// <returns>Found Item</returns>
        public async Task<StorageContainer> GetFileSystemItem(string Path)
        {
            foreach (var resolver in PathResolvers)
            {
                var result = await resolver.TryResolve(Path);
                if (result != null) return result;
            }
            return null;
        }

        /// <summary>
        /// Attempts to get a Folder from a Raw Path, this path will be evaulated by any Available <see cref="IPathResolver"/>. <para/>
        /// To add a Path Resolver, use <see cref="AddResolver(IPathResolver)"/> or <see cref="AddResolverFirst(IPathResolver)"/>.
        /// </summary>
        /// <param name="Path">Path to Folder</param>
        /// <returns>Found Folder</returns>
        public async Task<FolderContainer> GetFolder(string Path)
        {
            return (await GetFileSystemItem(Path)) as FolderContainer;
        }

        /// <summary>
        /// Gets a Folder from the FileSystem, the Path is Resolved and is OS Independent.
        /// </summary>
        /// <param name="Path">Folder Path</param>
        /// <returns>Specified Folder</returns>
        public virtual async Task<FolderContainer> GetFolder(FolderPath Path)
        {
            var folder = GetBaseFolder(Path.Root);
            foreach (var piece in PlatformBindingHelpers.GetPathPieces(Path.Path))
            {
                folder = await folder.GetFolderAsync(piece);
            }
            return folder;
        }

        /// <summary>
        /// Gets a File from a Raw Path, this path must observe the Operating System's Path Structure to work correctly.
        /// </summary>
        /// <param name="Path">OS Style Path</param>
        /// <returns>Specified File</returns>
        public async Task<FileContainer> GetFile(string Path)
        {
            return (await GetFileSystemItem(Path)) as FileContainer;
        }

        /// <summary>
        /// Gets a File from the FileSystem, the Path is Resolved and is OS Independent.
        /// </summary>
        /// <param name="Path">File Path</param>
        /// <returns>Specified File</returns>
        public virtual async Task<FileContainer> GetFile(FilePath Path)
        {
            var folder = await GetFolder(Path);
            return await folder.GetFileAsync(Path.FileName);
        }

        /// <summary>
        /// Creates a File from the Resolved Path.
        /// </summary>
        /// <param name="Path">File Path</param>
        /// <returns>Created File</returns>
        public async Task<FileContainer> CreateFile(string Path)
        {
            return await CreateFile(Path, DefaultFileCreationCollision);
        }

        /// <summary>
        /// Creates a File from the Resolved Path.
        /// </summary>
        /// <param name="Path">File Path</param>
        /// <returns>Created File</returns>
        public async Task<FileContainer> CreateFile(FilePath Path)
        {
            return await CreateFile(Path, DefaultFileCreationCollision);
        }

        /// <summary>
        /// Creates a File from the Resolved Path. Will use <see cref="DefaultFileCreationCollision"/> if File already exists.
        /// </summary>
        /// <param name="Path">File Path</param>
        /// <returns>Created File</returns>
        public virtual async Task<FileContainer> CreateFile(string Path, CreationCollisionOption Option)
        {
            var path = System.IO.Path.GetDirectoryName(Path);
            var folder = await GetFolder(path);
            return await folder.CreateFileAsync(path, Option);
        }

        /// <summary>
        /// Creates a File from the Resolved Path.
        /// </summary>
        /// <param name="Path">File Path</param>
        /// <returns>Created File</returns>
        public virtual async Task<FileContainer> CreateFile(FilePath Path, CreationCollisionOption Option)
        {
            var folder = await GetFolder(Path);
            return await folder.CreateFileAsync(Path.FileName, Option);
        }

        /// <summary>
        /// Opens a Folder for Viewing using the File Manager for the Operating System. See <see cref="SupportsOpenFolderForDisplay"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="Folder">Folder to Open</param>
        /// <exception cref="NotSupportedException"/>
        public Task OpenFolderForDisplay(FolderContainer Folder)
        {
            return OpenFolderForDisplay(Folder, null);
        }

        /// <summary>
        /// Opens a Folder for Viewing using the File Manager for the Operating System. See <see cref="SupportsOpenFolderForDisplay"/> to ensure that this is Supported.
        /// </summary>
        /// <param name="Folder">Folder to Open</param>
        /// <param name="Options">Options for modifying how the Folder is Displayed, such as Pre-Selecting Files/Folders if supported</param>
        /// <exception cref="NotSupportedException"/>
        public abstract Task OpenFolderForDisplay(FolderContainer Folder, FolderOpenOptions Options);

        /// <summary>
        /// Opens a File for Viewing in the Default Application for the File in the Operating System. See <see cref="SupportsOpenFileForDisplay"/> to ensure that this is Supported. <para/>
        /// </summary>
        /// <param name="File">File to Open</param>
        /// <exception cref="Exceptions.DefaultAppNotFoundException"/>
        /// <exception cref="NotSupportedException"/>
        public abstract Task OpenFileForDisplay(FileContainer File);

        /// <summary>
        /// Gets the Roaming Settings Cluster.
        /// See <see cref="SupportsRoaming"/> to check if this is Supported, returns the Local Settings container if not Supported.
        /// </summary>
        /// <returns>The Roaming Settings Cluster if Supported, otherwise the Local Settings Cluster.</returns>
        public virtual ISettingsContainer RoamingSettings => LocalSettings;

        /// <summary>
        /// Gets the Local Settings Cluster.
        /// </summary>
        /// <returns>The Local Settings Cluster.</returns>
        public abstract ISettingsContainer LocalSettings { get; }

        /// <summary>
        /// Gets the Future Access Manager for accessing Files and Folders across App sessions. <para/>
        /// Check <see cref="RequiresFutureAccessPermission"/> to see if this is Required by the Platform.
        /// </summary>
        /// <exception cref="System.NotSupportedException"/>
        public abstract IFutureAccessManager FutureAccess { get; }

        /// <summary>
        /// Provides ways for Users to Pick Files and Folders, or Open Files and Folders.
        /// </summary>
        /// <exception cref="System.NotSupportedException"/>
        public abstract FileSystemPickers Pickers { get; }

        /// <summary>
        /// Used to Determine if the OS Requires Future Access Permissions to access Files/Folders in Future Sessions. <para/>
        /// Represents availaibility of <see cref="FutureAccess"/>.
        /// </summary>
        public virtual bool RequiresFutureAccessPermission => FutureAccess != null;

        /// <summary>
        /// Used to Determine if the OS Supports creating File/Folder Pickers. <para/>
        /// Represents availaibility of <see cref="Pickers"/>.
        /// </summary>
        public virtual bool SupportsPickers => Pickers != null;

        /// <summary>
        /// Used to Determine if the OS Supports Displaying Folders using the OS File Manager.
        /// </summary>
        public abstract bool SupportsOpenFolderForDisplay { get; }

        /// <summary>
        /// Used to Determine if the OS Supports Opening Files with the Default Application for a File.
        /// </summary>
        public abstract bool SupportsOpenFileForDisplay { get; }

        /// <summary>
        /// Used to Determine if the OS supports Roaming AppData accross Devices.
        /// </summary>
        public virtual bool SupportsRoaming => RoamingSettings != LocalSettings;

        /// <summary>
        /// Customises the default CreationCollisionOption for use in Folder Creation Methods.<para/>
        /// Defaults to <see cref="CreationCollisionOption.FailIfExists"/>.
        /// </summary>
        public static CreationCollisionOption DefaultFolderCreationCollision { get; set; } = CreationCollisionOption.FailIfExists;

        /// <summary>
        /// Customises the default CreationCollisionOption for use in File Creation Methods. <para/>
        /// Defaults to <see cref="CreationCollisionOption.FailIfExists"/>.
        /// </summary>
        public static CreationCollisionOption DefaultFileCreationCollision { get; set; } = CreationCollisionOption.FailIfExists;

        /// <summary>
        /// Stores Resolvers for fetching Files/Folders from string Paths.
        /// </summary>
        private static List<IPathResolver> PathResolvers = new List<IPathResolver>();
    }
}