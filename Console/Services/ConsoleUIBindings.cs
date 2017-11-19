using System;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Controls.MenuLayout;
using PlatformBindings.Models;
using PlatformBindings.ConsoleTools;

namespace PlatformBindings.Services
{
    public abstract class ConsoleUIBindings : UIBindings
    {
        public ConsoleUIBindings() : base(Platform.Console)
        {
        }

        public override InteractionManager InteractionManager => null;

        public override IUIBindingInfo DefaultUIBinding => new ConsoleUIBindingInfo();

        public override INavigationManager NavigationManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private string CleanseFormatMarkers(string RawText)
        {
            return RawText.Replace("^B^", "");
        }

        public override Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText, string SecondaryButtonText, IUIBindingInfo UIBinding)
        {
            Console.WriteLine(CleanseFormatMarkers(Title));
            Console.WriteLine(CleanseFormatMarkers(Message));

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

        public override Task<string> RequestTextFromUserAsync(string Title, string Message, string OKButtonText, string CancelButtonText, IUIBindingInfo UIBinding)
        {
            throw new NotImplementedException();
        }

        public override void SetWindowText(string Text = "")
        {
            Console.Title = Text;
        }

        public override void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            throw new NotImplementedException();
        }
    }
}