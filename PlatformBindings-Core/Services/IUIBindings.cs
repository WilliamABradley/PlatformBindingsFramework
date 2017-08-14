using System;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Controls.MenuLayout;

namespace PlatformBindings.Services
{
    public interface IUIBindings
    {
        InteractionManagerBase InteractionManager { get; }

        INavigationManager NavigationManager { get; set; }

        IUIBindingInfo DefaultUIBinding { get; }

        Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText, string SecondaryButtonText, IUIBindingInfo UIBinding);

        void PromptUser(string Title, string Message, string PrimaryButtonText, IUIBindingInfo UIBinding);

        void SetTitlebarText(string Text);

        void ShowMenu(Menu Menu, IMenuBinding Binding);

        void OpenLink(Uri Uri);
    }
}