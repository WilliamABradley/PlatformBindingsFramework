using System;
using System.Threading.Tasks;
using PlatformBindings.Services;

namespace PlatformBindings.Common
{
    public static partial class CoreExtensions
    {
        public static async void ContinueWith(this Task Task, Action action)
        {
            await Task.ContinueWith(task => action());
        }

        public static async void ContinueOnUIThread(this Task Task, IUIBindingInfo UIBinding, Action action)
        {
            await ContinueOnUIThreadAsync(Task, UIBinding, action);
        }

        public static async void ContinueOnUIThread(this Task Task, Action action)
        {
            await ContinueOnUIThreadAsync(Task, AppServices.UI.DefaultUIBinding, action);
        }

        public static async void ContinueOnUIThread(this Task Task, IUIBindingInfo UIBinding, Action<Task> action)
        {
            await ContinueOnUIThreadAsync(Task, UIBinding, action);
        }

        public static async void ContinueOnUIThread<T>(this Task<T> Task, Action<Task<T>> action)
        {
            await ContinueOnUIThreadAsync(Task, AppServices.UI.DefaultUIBinding, action);
        }

        public static async void ContinueOnUIThread<T>(this Task<T> Task, IUIBindingInfo UIBinding, Action<Task<T>> action)
        {
            await ContinueOnUIThreadAsync(Task, UIBinding, action);
        }

        public static async Task ContinueOnUIThreadAsync(this Task Task, Action<Task> action)
        {
            await ContinueOnUIThreadAsync(Task, AppServices.UI.DefaultUIBinding, () => action(Task));
        }

        public static async Task ContinueOnUIThreadAsync(this Task Task, IUIBindingInfo UIBinding, Action<Task> action)
        {
            await ContinueOnUIThreadAsync(Task, UIBinding, () => action(Task));
        }

        public static async Task ContinueOnUIThreadAsync<T>(this Task<T> Task, Action<Task<T>> action)
        {
            await ContinueOnUIThreadAsync(Task, AppServices.UI.DefaultUIBinding, () => action(Task));
        }

        public static async Task ContinueOnUIThreadAsync<T>(this Task<T> Task, IUIBindingInfo UIBinding, Action<Task<T>> action)
        {
            await ContinueOnUIThreadAsync(Task, UIBinding, () => action(Task));
        }

        public static async Task ContinueOnUIThreadAsync(this Task Task, IUIBindingInfo UIBinding, Action action)
        {
            await Task.ContinueWith(async task => await UIBinding.ExecuteAsync(action));
        }
    }
}