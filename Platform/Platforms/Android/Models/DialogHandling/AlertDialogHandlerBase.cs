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
using Android.Content;
using Android.Views;
using Java.Lang;
using PlatformBindings.Common;
using PlatformBindings.Enums;
using System;
using System.Threading.Tasks;

namespace PlatformBindings.Models.DialogHandling
{
    public abstract class AlertDialogHandlerBase
    {
        public static AlertDialogHandlerBase Pick(Context Context)
        {
            if (AndroidAppServices.UseAppCompatUI) return new CompatAlertDialogHandler(Context);
            else return new AlertDialogHandler(Context);
        }

        public AlertDialogHandlerBase(Context Context)
        {
            this.Context = Context;
            PrimaryButtonClicked += (s, e) =>
            {
                if (e.Cancel) Show();
                else Waiter.TrySetResult(DialogResult.Primary);
            };

            SecondaryButtonClicked += (s, e) =>
            {
                if (e.Cancel) Show();
                else Waiter.TrySetResult(DialogResult.Secondary);
            };
        }

        public void SetTitle(string text)
        {
            SetTitle(new Java.Lang.String(text));
        }

        public abstract void SetTitle(ICharSequence text);

        public void SetMessage(string text)
        {
            SetMessage(new Java.Lang.String(text));
        }

        public abstract void SetMessage(ICharSequence text);

        public void SetPrimaryButton(string text)
        {
            SetPrimaryButton(new Java.Lang.String(text));
        }

        public abstract void SetPrimaryButton(ICharSequence text);

        public void SetSecondaryButton(string text)
        {
            SetSecondaryButton(new Java.Lang.String(text));
        }

        public abstract void SetSecondaryButton(ICharSequence text);

        public virtual void SetView(View View)
        {
            this.View = View;
            SetViewInternal(View);
        }

        protected abstract void SetViewInternal(View View);

        public virtual void SetView(int LayoutResId)
        {
            var activity = Context as Activity;
            var layout = activity.LayoutInflater.Inflate(LayoutResId, null);
            SetView(layout);
        }

        public abstract void Show();

        public virtual void Hide()
        {
            Dialog?.Hide();
        }

        public async Task<DialogResult> ShowAsync()
        {
            PlatformBindingHelpers.OnUIThread(() =>
            {
                Show();
            });
            return await Waiter.Task;
        }

        ~AlertDialogHandlerBase()
        {
            Waiter = null;
        }

        public Context Context { get; }

        public abstract event EventHandler<DialogButtonEventArgs> PrimaryButtonClicked;

        public abstract event EventHandler<DialogButtonEventArgs> SecondaryButtonClicked;

        private TaskCompletionSource<DialogResult> Waiter = new TaskCompletionSource<DialogResult>();

        public abstract Dialog Dialog { get; }

        public View View { get; private set; }

        protected class AlertDialogButtonHandler : Java.Lang.Object, View.IOnClickListener
        {
            public AlertDialogButtonHandler(AlertDialogHandlerBase Handler)
            {
                this.Handler = Handler;
            }

            public void OnClick(View v)
            {
                var args = new DialogButtonEventArgs();
                Click?.Invoke(v, args);

                if (!args.Cancel)
                {
                    Handler.Hide();
                }
            }

            private readonly AlertDialogHandlerBase Handler;

            public event EventHandler<DialogButtonEventArgs> Click;
        }
    }
}