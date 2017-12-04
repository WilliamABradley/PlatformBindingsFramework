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

using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace PlatformBindings.Models.IncrementalLoading
{
    public class UWPSupportsIncrementalLoading : ISupportIncrementalLoading
    {
        public UWPSupportsIncrementalLoading(ISupportCoreIncrementalLoading Source)
        {
            this.Source = Source;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                var result = await Source.LoadMoreItemsAsync(count);
                return new LoadMoreItemsResult
                {
                    Count = result
                };
            }).AsAsyncOperation();
        }

        public bool HasMoreItems => Source.HasMoreItems;

        public ISupportCoreIncrementalLoading Source { get; }
    }
}