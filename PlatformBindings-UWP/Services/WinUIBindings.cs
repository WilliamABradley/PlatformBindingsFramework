using System.Threading.Tasks;
using PlatformBindings.Common;
using PlatformBindings.Enums;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using System;
using Windows.UI.Core;
using PlatformBindings.Controls.MenuLayout;

namespace PlatformBindings.Services
{
    public class WinUIBindings : IUIBindings
    {
        public static WinUIBindings Current;

        public WinUIBindings(CoreDispatcher MainDispatcher)
        {
            Current = this;
            DefaultUIBinding = new WinUIBindingInfo(MainDispatcher);
            InteractionManager = new InteractionManager(DefaultUIBinding);
        }

        public async void PromptUser(string Title, string Message, string PrimaryButtonText = null, IUIBindingInfo UIBinding = null)
        {
            await PromptUserAsync(Title, Message, PrimaryButtonText, null, UIBinding);
        }

        public async Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText = null, string SecondaryButtonText = null, IUIBindingInfo UIBinding = null)
        {
            var binding = UIBinding as WinUIBindingInfo;

            var dlg = new ContentDialog { Title = Title, Content = Message, PrimaryButtonText = PrimaryButtonText };
            if (!string.IsNullOrWhiteSpace(SecondaryButtonText)) dlg.SecondaryButtonText = SecondaryButtonText;
            var result = await dlg.CreateContentDialogAsync(false);
            binding?.AttachedDialog?.CreateContentDialog(false);
            switch (result)
            {
                case ContentDialogResult.Primary:
                    return DialogResult.Primary;

                case ContentDialogResult.Secondary:
                    return DialogResult.Secondary;

                default:
                    return DialogResult.Closed;
            }
        }

        public void SetTitlebarText(string Text = "")
        {
            ApplicationView.GetForCurrentView().Title = Text;
        }

        public void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            switch (Binding)
            {
                case CommandBarMenuBinding barBind:
                    MenuRenderer.AttachTo(Menu, barBind.Bar);
                    break;

                case ContextMenuBinding contextBind:
                    MenuRenderer.ShowAt(Menu, contextBind.Element, contextBind.Position);
                    break;
            }
        }

        public async void OpenLink(Uri Uri)
        {
            await Windows.System.Launcher.LaunchUriAsync(Uri);
        }

        public InteractionManagerBase InteractionManager { get; }

        public INavigationManager NavigationManager { get; set; }

        public IUIBindingInfo DefaultUIBinding { get; }
    }
}