// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Threading.Tasks;
using PlatformBindings.Common;
using PlatformBindings.Enums;
using Windows.UI.Xaml.Controls;
using System;
using Windows.UI.Core;
using PlatformBindings.Controls.MenuLayout;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Text;
using PlatformBindings.Models;

namespace PlatformBindings.Services
{
    public class UWPUIBindings : UIBindings
    {
        public UWPUIBindings(CoreDispatcher MainDispatcher) : base(Platform.UWP)
        {
            DefaultUIBinding = new UWPUIBindingInfo(MainDispatcher);
            InteractionManager = new UWPInteractionManager(DefaultUIBinding);
            TitleManager = new UWPTitleManager();
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
            TaskCompletionSource<DialogResult> waiter = new TaskCompletionSource<DialogResult>();
            PlatformBindingHelpers.OnUIThread(async () =>
            {
                var binding = UIBinding as UWPUIBindingInfo;

                var content = new ScrollViewer
                {
                    Content = ConvertSpansToFormattedBlock(Message),
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollMode = ScrollMode.Disabled
                };

                var dlg = new ContentDialog { Title = ConvertSpansToFormattedBlock(Title), Content = content, PrimaryButtonText = PrimaryButtonText };
                if (!string.IsNullOrWhiteSpace(SecondaryButtonText)) dlg.SecondaryButtonText = SecondaryButtonText;
                var result = await dlg.CreateContentDialogAsync(false);
                binding?.AttachedDialog?.CreateContentDialog(false);
                switch (result)
                {
                    case ContentDialogResult.Primary:
                        waiter.TrySetResult(DialogResult.Primary);
                        break;

                    case ContentDialogResult.Secondary:
                        waiter.TrySetResult(DialogResult.Secondary);
                        break;

                    default:
                        waiter.TrySetResult(DialogResult.Closed);
                        break;
                }
            });

            return await waiter.Task;
        }

        public override async void OpenLink(Uri Uri)
        {
            await Windows.System.Launcher.LaunchUriAsync(Uri);
        }

        public override async Task<string> RequestTextFromUserAsync(string Title, string Message, string OKButtonText, string CancelButtonText, IUIBindingInfo UIBinding)
        {
            TaskCompletionSource<string> waiter = new TaskCompletionSource<string>();
            PlatformBindingHelpers.OnUIThread(async () =>
            {
                var binding = UIBinding as UWPUIBindingInfo;

                var content = new TextBox
                {
                    Header = ConvertSpansToFormattedBlock(Message)
                };

                var dlg = new ContentDialog { Title = ConvertSpansToFormattedBlock(Title), Content = content, PrimaryButtonText = OKButtonText };
                if (!string.IsNullOrWhiteSpace(CancelButtonText)) dlg.SecondaryButtonText = CancelButtonText;
                var result = await dlg.CreateContentDialogAsync(false);
                binding?.AttachedDialog?.CreateContentDialog(false);
                if (result == ContentDialogResult.Primary)
                {
                    waiter.TrySetResult(content.Text);
                }
                else waiter.TrySetResult(null);
            });

            return await waiter.Task;
        }

        public override void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            PlatformBindingHelpers.OnUIThread(() =>
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
            });
        }

        public override void ShowMenu(Menu Menu, object UIElement)
        {
            if (UIElement is CommandBar bar)
            {
                RegisterMenu(Menu, bar);
            }
            else if (UIElement is FrameworkElement element)
            {
                ShowMenu(Menu, new ContextMenuBinding(element, null));
            }
        }

        public override void RegisterMenu(Menu Menu, object UIElement)
        {
            PlatformBindingHelpers.OnUIThread(() =>
            {
                if (UIElement is CommandBar bar)
                {
                    MenuRenderer.AttachTo(Menu, bar);
                }
                else if (UIElement is FrameworkElement element)
                {
                    element.RightTapped += (s, e) =>
                    {
                        MenuRenderer.ShowAt(Menu, element, e.GetPosition(element));
                    };
                }
            });
        }

        public override InteractionManager InteractionManager { get; }
        public override NavigationManager NavigationManager { get; set; }
        public override ITitleManager TitleManager { get; set; }

        public override IUIBindingInfo DefaultUIBinding { get; }
    }
}