using Android.App;
using System;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Controls.MenuLayout;
using Android.Content;
using PlatformBindings.Common;

namespace PlatformBindings.Services
{
    public class AndroidUIBindings : UIBindingsBase
    {
        public AndroidUIBindings()
        {
            DefaultUIBinding = new AndroidUIBindingInfo();
        }

        public override InteractionManagerBase InteractionManager { get; }
        public override IUIBindingInfo DefaultUIBinding { get; }
        public override INavigationManager NavigationManager { get; set; }

        public override void OpenLink(Uri Uri)
        {
            var activity = AndroidHelpers.GetCurrentActivity(null);
            Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(Uri.ToString()));
            activity.StartActivity(browserIntent);
        }

        public override async Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText, string SecondaryButtonText, IUIBindingInfo UIBinding)
        {
            TaskCompletionSource<DialogResult> Waiter = new TaskCompletionSource<DialogResult>();

            var activity = AndroidHelpers.GetCurrentActivity(UIBinding);
            var builder = new
#if APPCOMPAT
                Android.Support.V7.App.AlertDialog
#else
                AlertDialog
#endif
                .Builder(activity);

            builder.SetTitle(Title);
            builder.SetMessage(Message);
            builder.SetNegativeButton(PrimaryButtonText, (s, e) => Waiter.TrySetResult(DialogResult.Primary));

            if (!string.IsNullOrWhiteSpace(SecondaryButtonText))
            {
                builder.SetPositiveButton(SecondaryButtonText, (s, e) => Waiter.TrySetResult(DialogResult.Secondary));
            }

            var dialog = builder.Show();

            return await Waiter.Task;
        }

        public override void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            var context = Binding as AndroidContextMenuBinding;
            context.Activity.GetHandler().OpenContextMenuForDisplay(Menu, context);
        }

        //Unsupported
        public override void SetWindowText(string Text)
        {
        }
    }
}