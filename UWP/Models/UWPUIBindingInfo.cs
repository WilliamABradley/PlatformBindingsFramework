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
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace PlatformBindings.Models
{
    public class UWPUIBindingInfo : IUIBindingInfo
    {
        public UWPUIBindingInfo(CoreDispatcher Dispatcher, ContentDialog AttachedDialog = null)
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