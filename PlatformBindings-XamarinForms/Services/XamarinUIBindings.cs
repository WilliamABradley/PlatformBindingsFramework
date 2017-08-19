using System;
using System.Threading.Tasks;
using PlatformBindings.Controls.MenuLayout;
using PlatformBindings.Enums;

namespace PlatformBindings.Services
{
    public class XamarinUIBindings : UIBindingsBase
    {
        public override InteractionManagerBase InteractionManager => throw new NotImplementedException();

        public override INavigationManager NavigationManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IUIBindingInfo DefaultUIBinding => throw new NotImplementedException();

        public override void OpenLink(Uri Uri)
        {
            throw new NotImplementedException();
        }

        public override async Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText, string SecondaryButtonText, IUIBindingInfo UIBinding)
        {
            var binding = (UIBinding ?? AppServices.UI.DefaultUIBinding) as XamarinUIBindingInfo;
            if (binding?.Page == null) throw new Exception("Xamarin UI Unbound");

            if (!string.IsNullOrWhiteSpace(SecondaryButtonText))
            {
                var answer = await binding.Page.DisplayAlert(Title, Message, PrimaryButtonText, SecondaryButtonText);
                return answer ? DialogResult.Primary : DialogResult.Secondary;
            }
            else
            {
                await binding.Page.DisplayAlert(Title, Message, PrimaryButtonText);
                return DialogResult.Primary;
            }
        }

        public override void SetTitlebarText(string Text)
        {
            throw new NotImplementedException();
        }

        public override void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            throw new NotImplementedException();
        }
    }
}