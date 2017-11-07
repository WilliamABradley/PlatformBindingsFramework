using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    /// <summary>
    /// The File System wrapper for each File/Folder. This Class holds important functions for manipulating File System Items
    /// </summary>
    public abstract class FileSystemContainerBase
    {
        /// <summary>
        /// Asyncronously attempts to delete this Item
        /// </summary>
        /// <returns>Operation Success</returns>
        public abstract Task<bool> DeleteAsync();

        /// <summary>
        /// Deletes this Item.
        /// </summary>
        public async void Delete()
        {
            await DeleteAsync();
        }

        /// <summary>
        /// Renames the Specifed Item
        /// </summary>
        /// <param name="NewName">New Name for this Item</param>
        /// <returns>Operation Success</returns>
        public abstract Task<bool> RenameAsync(string NewName);

        /// <summary>
        /// Gets the Name as represented via the File System of this Item
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the Full Path of this Item in the File System
        /// </summary>
        public abstract string Path { get; }
    }
}