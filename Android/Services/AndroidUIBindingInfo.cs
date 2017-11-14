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

        /// <summary>
        /// AppCompatActivity Accessor to Access Extended Properties and Functions, will be null if not using a derivative of AppCompatActivity, such as <see cref="Activities.PlatformBindingCompatActivity"/>.
        /// </summary>
        public Android.Support.V7.App.AppCompatActivity CompatActivity { get; set; }

        public Activity Activity { get; set; }
    }
}