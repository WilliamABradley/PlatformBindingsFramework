using System;
using System.Threading.Tasks;
using PlatformBindings.Common;
using PlatformBindings.Controls.Generic.MenuLayout;
using PlatformBindings.Enums;
using PlatformBindings.Services;

namespace PlatformBindings.ConsoleTools
{
    public class ConsoleUIBindings : IUIBindings
    {
        public InteractionManagerBase InteractionManager => null;

        public INavigationManager NavigationManager => null;

        public IUIBindingInfo DefaultUIBinding => new ConsoleUIBindingInfo();

        public void OpenChangelog()
        {
            throw new NotImplementedException();
        }

        public void OpenFeedback()
        {
            throw new NotImplementedException();
        }

        public void OpenLink(Uri Uri)
        {
            throw new NotImplementedException();
        }

        public async void PromptUser(string Title, string Message, string PrimaryButtonText = null, IUIBindingInfo UIBinding = null)
        {
            await PromptUserAsync(Title, Message, PrimaryButtonText, null, UIBinding);
        }

        public Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText = null, string SecondaryButtonText = null, IUIBindingInfo UIBinding = null)
        {
            Console.WriteLine(Title);
            Console.WriteLine(Message);

            DialogResult result = DialogResult.Primary;

            if (!string.IsNullOrWhiteSpace(SecondaryButtonText))
            {
                result = ConsoleHelpers.PromptYesNo($"{PrimaryButtonText}/{SecondaryButtonText}?") ? result = DialogResult.Primary : DialogResult.Secondary;
            }
            else
            {
                //ConsoleHelpers.AnyKeyContinue();
            }

            return Task.FromResult(result);
        }

        public void SetTitlebarText(string Text = "")
        {
            Console.Title = Text;
        }

        public void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            throw new NotImplementedException();
        }

        public void UpdateTheme()
        {
        }
    }
}