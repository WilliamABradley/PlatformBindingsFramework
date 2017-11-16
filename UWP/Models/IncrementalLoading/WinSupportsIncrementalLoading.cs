using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace PlatformBindings.Models.IncrementalLoading
{
    public class WinSupportsIncrementalLoading : ISupportIncrementalLoading
    {
        public WinSupportsIncrementalLoading(ISupportCoreIncrementalLoading Source)
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