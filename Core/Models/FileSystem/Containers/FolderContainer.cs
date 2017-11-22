using PlatformBindings.Enums;
using PlatformBindings.Models.FileSystem.Options;
using PlatformBindings.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    /// <summary>
    /// The Folder Wrapper for PlatformBindings, use this for Folder/Directory handling on each platform
    /// </summary>
    public abstract class FolderContainer : FileSystemContainer
    {
        /// <summary>
        /// Gets the Subfolder of a Specified Name if exists
        /// </summary>
        /// <param name="FolderName">Name of the folder to Fetch</param>
        /// <returns>The Specified Subfolder</returns>
        public virtual async Task<FolderContainer> GetFolderAsync(string FolderName)
        {
            var folders = await GetFoldersAsync();
            return folders.FirstOrDefault(item => item.Name == FolderName);
        }

        /// <summary>
        /// Gets the File of a Specified Name if exists
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns>The Specified File</returns>
        public virtual async Task<FileContainer> GetFileAsync(string FileName)
        {
            var files = await GetFilesAsync();
            return files.FirstOrDefault(item => item.Name == FileName);
        }

        /// <summary>
        /// Gets all Subfolders for this Folder
        /// </summary>
        /// <returns>A List of Subfolders</returns>
        public abstract Task<IReadOnlyList<FolderContainer>> GetFoldersAsync();

        /// <summary>
        /// Gets all files in this Folder
        /// </summary>
        /// <returns>A List of Files</returns>
        public abstract Task<IReadOnlyList<FileContainer>> GetFilesAsync();

        /// <summary>
        /// Creates a Subfolder with the Specified Name. Will use <see cref="IOBindings.DefaultFolderCreationCollision"/> if Folder already exists.
        /// </summary>
        /// <param name="FolderName">Name of the new Folder</param>
        /// <returns>The new Subfolder</returns>
        public Task<FolderContainer> CreateFolderAsync(string FolderName)
        {
            return CreateFolderAsync(FolderName, IOBindings.DefaultFolderCreationCollision);
        }

        /// <summary>
        /// Creates a File with the Specified Name. Will use <see cref="IOBindings.DefaultFileCreationCollision"/> if File already exists.
        /// </summary>
        /// <param name="FileName">Name of the new File</param>
        /// <returns>The new File</returns>
        public Task<FileContainer> CreateFileAsync(string FileName)
        {
            return CreateFileAsync(FileName, IOBindings.DefaultFileCreationCollision);
        }

        /// <summary>
        /// Creates a Subfolder with the Specified Name
        /// </summary>
        /// <param name="FolderName">Name of the new Folder</param>
        /// <param name="Options">How to handle a Folder that has the same name and already exists</param>
        /// <returns>The new Subfolder</returns>
        public virtual async Task<FolderContainer> CreateFolderAsync(string FolderName, CreationCollisionOption Options)
        {
            string newName = FolderName;
            if (await FolderExists(FolderName))
            {
                switch (Options)
                {
                    case CreationCollisionOption.GenerateUniqueName:
                        for (int num = 2; await FolderExists(newName); num++)
                        {
                            newName = FolderName + $" ({num})";
                        }
                        break;

                    case CreationCollisionOption.ReplaceExisting:
                        var existing = await GetFolderAsync(newName);
                        await existing.DeleteAsync();
                        break;

                    case CreationCollisionOption.FailIfExists:
                        throw new IOException("Folder already exists");
                }
            }
            return await InternalCreateFolderAsync(newName);
        }

        /// <summary>
        /// Creates a File with the Specified Name
        /// </summary>
        /// <param name="FileName">Name of the new File</param>
        /// <param name="Options">How to handle a File that has the same name and already exists</param>
        /// <returns>The new File</returns>
        public virtual async Task<FileContainer> CreateFileAsync(string FileName, CreationCollisionOption Options)
        {
            string NewName = FileName;
            if (await FileExists(FileName))
            {
                switch (Options)
                {
                    case CreationCollisionOption.GenerateUniqueName:
                        string fileNoExt = System.IO.Path.GetFileNameWithoutExtension(NewName);
                        string Ext = System.IO.Path.GetExtension(NewName);
                        for (int num = 2; await FileExists(NewName); num++)
                        {
                            NewName = FileName + $" ({num})" + Ext;
                        }
                        break;

                    case CreationCollisionOption.ReplaceExisting:
                        var existing = await GetFileAsync(NewName);
                        await existing.DeleteAsync();
                        break;

                    case CreationCollisionOption.FailIfExists:
                        throw new IOException("File already exists");
                }
            }
            return await InternalCreateFileAsync(NewName);
        }

        /// <summary>
        /// Internal Folder Creation Factory, Collision Handling is handled automatically.<para/>
        /// To Handle Collision logic, override <see cref="CreateFolderAsync(string, CreationCollisionOption)"/>.
        /// </summary>
        /// <param name="FolderName">Name of the new Folder</param>
        /// <returns>The new Subfolder</returns>
        protected abstract Task<FolderContainer> InternalCreateFolderAsync(string FolderName);

        /// <summary>
        /// Internal File Creation Factory, Collision Handling is handled automatically.<para/>
        /// To Handle Collision logic, override <see cref="CreateFileAsync(string, CreationCollisionOption)"/>.
        /// </summary>
        /// <param name="FileName">Name of the new File</param>
        /// <returns>The new File</returns>
        protected abstract Task<FileContainer> InternalCreateFileAsync(string FileName);

        /// <summary>
        /// Checks if the Desired Folder Exists
        /// </summary>
        /// <param name="FileName">Folder Name to Check</param>
        /// <returns>Does the folder exist?</returns>
        public virtual async Task<bool> FolderExists(string FolderName)
        {
            var folders = await GetFoldersAsync();
            return folders.FirstOrDefault(item => item.Name == FolderName) != null;
        }

        /// <summary>
        /// Checks if the Desired File Exists
        /// </summary>
        /// <param name="FileName">File Name to Check</param>
        /// <returns>Does the file exist?</returns>
        public virtual async Task<bool> FileExists(string FileName)
        {
            var files = await GetFilesAsync();
            return files.FirstOrDefault(item => item.Name == FileName) != null;
        }

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
                if (item is FolderContainer subFolder && Options.Depth == FolderDepth.Deep)
                {
                    var subresult = await subFolder.CreateFileQueryAsync(Options);
                    result.Files.AddRange(subresult.Files);
                }
                else if (item is FileContainer file)
                {
                    var extension = System.IO.Path.GetExtension(item.Path);
                    if (Options.FileTypes.FirstOrDefault(ext => ext == extension) != null)
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
        public virtual async Task<IReadOnlyList<FileSystemContainer>> GetItemsAsync()
        {
            var folders = await GetFoldersAsync();
            var files = await GetFilesAsync();

            var result = new List<FileSystemContainer>();
            result.AddRange(folders);
            result.AddRange(files);
            return result;
        }
    }
}