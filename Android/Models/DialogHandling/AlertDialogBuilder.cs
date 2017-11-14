using System;
using Java.Lang;
using Android.App;
using Android.Content;
using PlatformBindings.Enums;
using Android.Views;
using System.Threading.Tasks;

namespace PlatformBindings.Models.DialogHandling
{
    public class AlertDialogBuilder : AlertDialogBuilderBase
    {
        public AlertDialogBuilder(Context Context) : base(Context)
        {
            Builder = new AlertDialog.Builder(Context);
        }

        public override void SetMessage(ICharSequence text)
        {
            Builder.SetMessage(text);
        }

        public override void SetPrimaryButton(ICharSequence text)
        {
            Builder.SetNegativeButton(text, new EventHandler<DialogClickEventArgs>((s, e) => Waiter.TrySetResult(DialogResult.Primary)));
        }

        public override void SetSecondaryButton(ICharSequence text)
        {
            Builder.SetPositiveButton(text, new EventHandler<DialogClickEventArgs>((s, e) => Waiter.TrySetResult(DialogResult.Secondary)));
        }

        public override void SetTitle(ICharSequence text)
        {
            Builder.SetTitle(text);
        }

        public override void SetView(View view)
        {
            Builder.SetView(view);
        }

        public override void SetView(int LayoutResId)
        {
            Builder.SetView(LayoutResId);
        }

        public override void Show()
        {
            Builder.Show();
        }

        private AlertDialog.Builder Builder { get; }
    }
}