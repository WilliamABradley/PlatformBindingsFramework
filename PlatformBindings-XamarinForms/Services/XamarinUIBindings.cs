using System;
using System.Threading.Tasks;
using PlatformBindings.Controls.MenuLayout;
using PlatformBindings.Enums;

namespace PlatformBindings.Services
{
    public class XamarinUIBindings : IUIBindings
    {
        public InteractionManagerBase InteractionManager => throw new NotImplementedException();

        public IUIBindingInfo DefaultUIBinding => throw new NotImplementedException();

        public INavigationManager NavigationManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        public async void PromptUser(string Title, string Message, string PrimaryButtonText, IUIBindingInfo UIBinding)
        {
            await PromptUserAsync(Title, Message, PrimaryButtonText, null, UIBinding);
        }

        public async Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText, string SecondaryButtonText, IUIBindingInfo UIBinding)
        {
            var binding = (UIBinding ?? AppServices.Services.UI.DefaultUIBinding) as XamarinUIBindingInfo;
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

        public void SetTitlebarText(string Text)
        {
            throw new NotImplementedException();
        }

        public void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            throw new NotImplementedException();
        }

        public void UpdateTheme()
        {
            throw new NotImplementedException();
        }
    }
}