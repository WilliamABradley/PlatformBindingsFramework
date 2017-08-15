using PlatformBindings.Common;
using PlatformBindings.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.IO;

namespace PlatformBindings.Models.FileSystem
{
    /// <summary>
    /// The Folder Wrapper for PlatformBindings, use this for Folder/Directory handling on each platform
    /// </summary>
    public abstract class FolderContainerBase : FileSystemContainerBase
    {
        /// <summary>
        /// Gets the Subfolder of a Specified Name if exists
        /// </summary>
        /// <param name="FolderName">Name of the folder to Fetch</param>
        /// <returns>The Specified Subfolder</returns>
        public abstract Task<FolderContainerBase> GetFolderAsync(string FolderName);

        /// <summary>
        /// Creates a Subfolder with the Specified Name
        /// </summary>
        /// <param name="FolderName">Name of the new Folder</param>
        /// <returns>The new Subfolder</returns>
        public Task<FolderContainerBase> CreateFolderAsync(string FolderName)
        {
            return CreateFolderAsync(FolderName, CreationCollisionOption.FailIfExists);
        }

        /// <summary>
        /// Creates a Subfolder with the Specified Name
        /// </summary>
        /// <param name="FolderName">Name of the new Folder</param>
        /// <param name="Options">How to handle a Folder that has the same name and already exists</param>
        /// <returns>The new Subfolder</returns>
        public abstract Task<FolderContainerBase> CreateFolderAsync(string FolderName, CreationCollisionOption Options);

        /// <summary>
        /// Gets all Subfolders for this Folder
        /// </summary>
        /// <returns>A List of Subfolders</returns>
        public abstract Task<IReadOnlyList<FolderContainerBase>> GetFoldersAsync();

        /// <summary>
        /// Gets all files in this Folder
        /// </summary>
        /// <returns>A List of Files</returns>
        public abstract Task<IReadOnlyList<FileContainerBase>> GetFilesAsync();

        /// <summary>
        /// Gets the File of a Specified Name if exists
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns>The Specified File</returns>
        public abstract Task<FileContainerBase> GetFileAsync(string FileName);

        /// <summary>
        /// Creates a File with the Specified Name
        /// </summary>
        /// <param name="FileName">Name of the new File</param>
        /// <returns>The new File</returns>
        public Task<FileContainerBase> CreateFileAsync(string FileName)
        {
            return CreateFileAsync(FileName, CreationCollisionOption.FailIfExists);
        }

        /// <summary>
        /// Creates a File with the Specified Name
        /// </summary>
        /// <param name="FileName">Name of the new File</param>
        /// <param name="Options">How to handle a File that has the same name and already exists</param>
        /// <returns>The new File</returns>
        public abstract Task<FileContainerBase> CreateFileAsync(string FileName, CreationCollisionOption Options);

        /// <summary>
        /// Queries for all files of a specified Extension
        /// </summary>
        /// <param name="Options">Specifies the Constraints of the Query</param>
        /// <returns>Results of the Query</returns>
        public async Task<FileQueryResult> CreateFileQueryAsync(QueryOptions Options)
        {
            FileQueryResult result = new FileQueryResult();
            foreach (var item in await GetItemsAsync())
            {
                if (item is FolderContainerBase subFolder && Options.Depth == FolderDepth.Deep)
                {
                    var subresult = await subFolder.CreateFileQueryAsync(Options);
                    result.Files.AddRange(subresult.Files);
                }
                else if (item is FileContainerBase file)
                {
                    var extension = System.IO.Path.GetExtension(item.Path);
                    if (Options.FileTypes.FirstOrDefault(ext => ext.FileExtension == extension) != null)
                    {
                        result.Files.Add(file);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get all Files and SubFolders for this folder
        /// </summary>
        /// <returns>A List of SubFolders and Files</returns>
        public async Task<IReadOnlyList<FileSystemContainerBase>> GetItemsAsync()
        {
            var folders = await GetFoldersAsync();
            var files = await GetFilesAsync();

            var result = new List<FileSystemContainerBase>();
            result.AddRange(folders);
            result.AddRange(files);
            return result;
        }
    }
}