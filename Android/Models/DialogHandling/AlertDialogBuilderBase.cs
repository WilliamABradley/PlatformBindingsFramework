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

using Android.Content;
using Android.Views;
using Java.Lang;
using PlatformBindings.Common;
using PlatformBindings.Enums;
using System.Threading.Tasks;

namespace PlatformBindings.Models.DialogHandling
{
    public abstract class AlertDialogBuilderBase
    {
        public static AlertDialogBuilderBase Pick(Context Context)
        {
            if (AndroidAppServices.UseAppCompatUI) return new CompatAlertDialogBuilder(Context);
            else return new AlertDialogBuilder(Context);
        }

        public AlertDialogBuilderBase(Context Context)
        {
            this.Context = Context;
        }

        public void SetTitle(string text)
        {
            SetTitle(new String(text));
        }

        public abstract void SetTitle(ICharSequence text);

        public void SetMessage(string text)
        {
            SetMessage(new String(text));
        }

        public abstract void SetMessage(ICharSequence text);

        public void SetPrimaryButton(string text)
        {
            SetPrimaryButton(new String(text));
        }

        public abstract void SetPrimaryButton(ICharSequence text);

        public void SetSecondaryButton(string text)
        {
            SetSecondaryButton(new String(text));
        }

        public abstract void SetSecondaryButton(ICharSequence text);

        public abstract void SetView(View view);

        public abstract void SetView(int LayoutResId);

        public abstract void Show();

        public async Task<DialogResult> ShowAsync()
        {
            PlatformBindingHelpers.OnUIThread(() =>
            {
                Show();
            });
            return await Waiter.Task;
        }

        public Context Context { get; }
        protected TaskCompletionSource<DialogResult> Waiter = new TaskCompletionSource<DialogResult>();
    }
}