using System;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Controls.MenuLayout;

namespace PlatformBindings.Services
{
    public abstract class UIBindingsBase
    {
        public abstract InteractionManagerBase InteractionManager { get; }

        public abstract INavigationManager NavigationManager { get; set; }

        public abstract IUIBindingInfo DefaultUIBinding { get; }

        public abstract Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText, string SecondaryButtonText, IUIBindingInfo UIBinding);

        public Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText, string SecondaryButtonText)
        {
            return PromptUserAsync(Title, Message, PrimaryButtonText, SecondaryButtonText, null);
        }

        public Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText)
        {
            return PromptUserAsync(Title, Message, PrimaryButtonText, null, null);
        }

        public async void PromptUser(string Title, string Message, string PrimaryButtonText, IUIBindingInfo UIBinding)
        {
            await PromptUserAsync(Title, Message, PrimaryButtonText, null, UIBinding);
        }

        public async void PromptUser(string Title, string Message, string PrimaryButtonText)
        {
            await PromptUserAsync(Title, Message, PrimaryButtonText, null, null);
        }

        public abstract void SetTitlebarText(string Text);

        public abstract void ShowMenu(Menu Menu, IMenuBinding Binding);

        public abstract void OpenLink(Uri Uri);
    }
}