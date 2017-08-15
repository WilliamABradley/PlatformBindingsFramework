using System;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Services;
using PlatformBindings.Controls.MenuLayout;

namespace PlatformBindings.ConsoleTools
{
    public class ConsoleUIBindings : UIBindingsBase
    {
        public override InteractionManagerBase InteractionManager => null;

        public override IUIBindingInfo DefaultUIBinding => new ConsoleUIBindingInfo();

        public override INavigationManager NavigationManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void OpenLink(Uri Uri)
        {
            throw new NotImplementedException();
        }

        public override Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText = null, string SecondaryButtonText = null, IUIBindingInfo UIBinding = null)
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

        public override void SetTitlebarText(string Text = "")
        {
            Console.Title = Text;
        }

        public override void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            throw new NotImplementedException();
        }
    }
}