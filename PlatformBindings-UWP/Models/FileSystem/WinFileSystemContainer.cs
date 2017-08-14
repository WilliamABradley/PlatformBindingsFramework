using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace PlatformBindings.Models.FileSystem
{
    public class WinFileSystemContainer : IFileSystemContainer
    {
        public WinFileSystemContainer(IStorageItem Item)
        {
            this.Item = Item;
        }

        public async Task<bool> Delete()
        {
            try
            {
                await Item.DeleteAsync();
                return true;
            }
            catch { return false; }
        }

        public IStorageItem Item { get; }

        public string Name => Item.Name;

        public string Path => Item.Path;
    }
}