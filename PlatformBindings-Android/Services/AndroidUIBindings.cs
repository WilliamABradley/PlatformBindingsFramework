using Android.App;
using System;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Controls.MenuLayout;

namespace PlatformBindings.Services
{
    public class AndroidUIBindings : IUIBindings
    {
        public AndroidUIBindings()
        {
            DefaultUIBinding = new AndroidUIBindingInfo();
        }

        public InteractionManagerBase InteractionManager => throw new NotImplementedException();

        public IUIBindingInfo DefaultUIBinding { get; }
        public INavigationManager NavigationManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void OpenLink(Uri Uri)
        {
            throw new NotImplementedException();
        }

        public async void PromptUser(string Title, string Message, string PrimaryButtonText, IUIBindingInfo UIBinding)
        {
            await PromptUserAsync(Title, Message, PrimaryButtonText, null, UIBinding);
        }

        public async Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText, string SecondaryButtonText, IUIBindingInfo UIBinding)
        {
            TaskCompletionSource<DialogResult> Waiter = new TaskCompletionSource<DialogResult>();

            var uibinding = (UIBinding ?? AppServices.Services.UI.DefaultUIBinding) as AndroidUIBindingInfo;
            var builder = new AlertDialog.Builder(uibinding.Activity);

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

        public void SetTitlebarText(string Text)
        {
            throw new NotImplementedException();
        }

        public void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            var context = Binding as AndroidContextMenuBinding;
            context.Activity.OpenContextMenuForDisplay(Menu, context);
        }

        public void UpdateTheme()
        {
            throw new NotImplementedException();
        }
    }
}