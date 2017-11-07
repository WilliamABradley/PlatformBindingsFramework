using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlatformBindings.Services
{
    public class XamarinUIBindingInfo : IUIBindingInfo
    {
        public XamarinUIBindingInfo(Page Page = null)
        {
            this.Page = Page;
        }

        public async void Execute(Action action)
        {
            await ExecuteAsync(action);
        }

        public async Task ExecuteAsync(Action action)
        {
            TaskCompletionSource<bool> Waiter = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(() =>
            {
                action();
                Waiter.TrySetResult(true);
            });
            await Waiter.Task;
        }

        public Page Page { get; set; }
    }
}