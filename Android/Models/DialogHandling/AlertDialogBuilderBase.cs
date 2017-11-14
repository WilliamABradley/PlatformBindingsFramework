using Android.Content;
using Android.Views;
using Java.Lang;
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
            Show();
            return await Waiter.Task;
        }

        public Context Context { get; }
        protected TaskCompletionSource<DialogResult> Waiter = new TaskCompletionSource<DialogResult>();
    }
}