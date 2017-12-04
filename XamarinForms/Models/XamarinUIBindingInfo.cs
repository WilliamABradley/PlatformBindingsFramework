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
using Xamarin.Forms;

namespace PlatformBindings.Models
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