using System.Threading.Tasks;
using PlatformBindings.Common;
using PlatformBindings.Enums;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using System;
using Windows.UI.Core;
using PlatformBindings.Controls.MenuLayout;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Text;

namespace PlatformBindings.Services
{
    public class WinUIBindings : UIBindingsBase
    {
        public WinUIBindings(CoreDispatcher MainDispatcher)
        {
            DefaultUIBinding = new WinUIBindingInfo(MainDispatcher);
            InteractionManager = new InteractionManager(DefaultUIBinding);
        }

        private TextBlock ConvertSpansToFormattedBlock(string RawText)
        {
            var sections = RawText.Split(new string[] { "^B^" }, StringSplitOptions.None);
            var block = new TextBlock { TextWrapping = TextWrapping.Wrap };
            for (int i = 0; i < sections.Length; i++)
            {
                var run = new Run { Text = sections[i] };
                if (i % 2 != 0) run.FontWeight = FontWeights.Bold;
                block.Inlines.Add(run);
            }
            return block;
        }

        public override async Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText = null, string SecondaryButtonText = null, IUIBindingInfo UIBinding = null)
        {
            var binding = UIBinding as WinUIBindingInfo;

            var dlg = new ContentDialog { Title = ConvertSpansToFormattedBlock(Title), Content = ConvertSpansToFormattedBlock(Message), PrimaryButtonText = PrimaryButtonText };
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

        public override void SetWindowText(string Text = "")
        {
            ApplicationView.GetForCurrentView().Title = Text;
        }

        public override void ShowMenu(Menu Menu, IMenuBinding Binding)
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

        public override async void OpenLink(Uri Uri)
        {
            await Windows.System.Launcher.LaunchUriAsync(Uri);
        }

        public override InteractionManagerBase InteractionManager { get; }

        public override INavigationManager NavigationManager { get; set; }

        public override IUIBindingInfo DefaultUIBinding { get; }
    }
}