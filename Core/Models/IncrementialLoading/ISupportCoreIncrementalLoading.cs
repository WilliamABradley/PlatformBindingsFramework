using System.Threading.Tasks;

namespace PlatformBindings.Models.IncrementalLoading
{
    public interface ISupportCoreIncrementalLoading
    {
        /// <summary>
        /// Loads content from an Async Source, returns how many items loaded.
        /// </summary>
        /// <param name="count">How many items to attempt to load.</param>
        /// <returns>How many results loaded</returns>
        Task<uint> LoadMoreItemsAsync(uint count);

        /// <summary>
        /// Returns true if there are unloaded Items not in the view.
        /// </summary>
        bool HasMoreItems { get; }
    }
}