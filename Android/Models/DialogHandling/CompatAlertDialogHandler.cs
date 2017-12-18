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
using Java.Lang;
using Android.Content;
using Android.Views;
using Android.App;

namespace PlatformBindings.Models.DialogHandling
{
    public class CompatAlertDialogHandler : AlertDialogHandlerBase
    {
        public CompatAlertDialogHandler(Context Context) : base(Context)
        {
            Builder = new Android.Support.V7.App.AlertDialog.Builder(Context);
        }

        public override event EventHandler<DialogButtonEventArgs> PrimaryButtonClicked;

        public override event EventHandler<DialogButtonEventArgs> SecondaryButtonClicked;

        public override void SetMessage(ICharSequence text)
        {
            Builder.SetMessage(text);
        }

        public override void SetPrimaryButton(ICharSequence text)
        {
            Builder.SetPositiveButton(text, (IDialogInterfaceOnClickListener)null);
        }

        public override void SetSecondaryButton(ICharSequence text)
        {
            Builder.SetNegativeButton(text, (IDialogInterfaceOnClickListener)null);
        }

        public override void SetTitle(ICharSequence text)
        {
            Builder.SetTitle(text);
        }

        protected override void SetViewInternal(View view)
        {
            Builder.SetView(view);
        }

        public override void Show()
        {
            var log = Builder.Show();
            _Dialog = log;

            var primary = log.GetButton((int)DialogButtonType.Positive);
            var secondary = log.GetButton((int)DialogButtonType.Negative);

            var primaryHandler = new AlertDialogButtonHandler(this);
            primary.SetOnClickListener(primaryHandler);
            primaryHandler.Click += (s, e) => { PrimaryButtonClicked?.Invoke(this, e); };

            var secondaryHandler = new AlertDialogButtonHandler(this);
            secondary.SetOnClickListener(secondaryHandler);
            secondaryHandler.Click += (s, e) => { SecondaryButtonClicked?.Invoke(this, e); };
        }

        private Android.Support.V7.App.AlertDialog.Builder Builder { get; }

        public override Dialog Dialog => _Dialog;
        private Dialog _Dialog;
    }
}