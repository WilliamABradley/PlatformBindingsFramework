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

using Android.App;
using System;
using System.Threading.Tasks;

namespace PlatformBindings.Models
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