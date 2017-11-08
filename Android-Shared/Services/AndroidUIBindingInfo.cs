using Android.App;
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

#if APPCOMPAT
        public Android.Support.V7.App.AppCompatActivity Activity { get; set; }
#else
        public Activity Activity { get; set; }
#endif
    }
}