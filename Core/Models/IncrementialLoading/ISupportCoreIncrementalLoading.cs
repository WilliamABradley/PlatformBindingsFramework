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