using System;
using System.Threading.Tasks;

namespace PlatformBindings.Services
{
    public class AndroidUIBindingInfo : IUIBindingInfo
    {
        public AndroidUIBindingInfo()
        {
        }

        public async void Execute(Action action)
        {
            await ExecuteAsync(action);
        }

        public Task ExecuteAsync(Action action)
        {
            if (Activity != null) Activity.RunOnUiThread(action);
            else action();
            return Task.FromResult(0);
        }

        public LibActivity Activity { get; set; }
    }
}