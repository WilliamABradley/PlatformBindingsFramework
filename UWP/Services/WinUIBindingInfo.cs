using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace PlatformBindings.Services
{
    public class WinUIBindingInfo : IUIBindingInfo
    {
        public WinUIBindingInfo(CoreDispatcher Dispatcher, ContentDialog AttachedDialog = null)
        {
            this.Dispatcher = Dispatcher;
            this.AttachedDialog = AttachedDialog;
        }

        public async void Execute(Action action)
        {
            await ExecuteAsync(action);
        }

        public async Task ExecuteAsync(Action action)
        {
            TaskCompletionSource<bool> Waiter = new TaskCompletionSource<bool>();
            if (Dispatcher != null)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    action();
                    Waiter.TrySetResult(true);
                });
                await Waiter.Task;
            }
            else
            {
                action();
                Waiter.TrySetResult(true);
            }
        }

        public CoreDispatcher Dispatcher { get; }
        public ContentDialog AttachedDialog { get; }
    }
}