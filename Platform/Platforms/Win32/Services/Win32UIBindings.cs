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

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PlatformBindings.Common;
using PlatformBindings.Controls.MenuLayout;
using PlatformBindings.Enums;
using PlatformBindings.Models;
using WPFCustomMessageBox;

namespace PlatformBindings.Services
{
    public class Win32UIBindings : UIBindings
    {
        public Win32UIBindings() : base(Platform.Win32)
        {
        }

        public override InteractionManager InteractionManager => null;
        public override NavigationManager NavigationManager { get; set; }
        public override ITitleManager TitleManager { get => null; set { } }

        public override IUIBindingInfo DefaultUIBinding => new Win32UIBindingInfo(Application.Current.Dispatcher);

        public override void OpenLink(Uri Uri)
        {
            System.Diagnostics.Process.Start(Uri.ToString());
        }

        public override async Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText, string SecondaryButtonText, IUIBindingInfo UIBinding)
        {
            TaskCompletionSource<DialogResult> Waiter = new TaskCompletionSource<DialogResult>();

            PlatformBindingHelpers.OnUIThread(() =>
            {
                MessageBoxResult? result = null;

                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                if (window != null)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(window);
                }

                if (!string.IsNullOrWhiteSpace(SecondaryButtonText))
                {
                    result = CustomMessageBox.ShowOKCancel(Message, Title, PrimaryButtonText, SecondaryButtonText);
                }
                else
                {
                    result = CustomMessageBox.ShowOK(Message, Title, PrimaryButtonText);
                }

                DialogResult finalresult = DialogResult.Closed;

                if (result == null)
                {
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                        case MessageBoxResult.Yes:
                            finalresult = DialogResult.Primary;
                            break;

                        case MessageBoxResult.Cancel:
                        case MessageBoxResult.No:
                            finalresult = DialogResult.Secondary;
                            break;
                    }
                }

                Waiter.TrySetResult(finalresult);
            });

            return await Waiter.Task;
        }

        public override void RegisterMenu(Menu Menu, object UIElement)
        {
            throw new NotImplementedException();
        }

        public override Task<string> RequestTextFromUserAsync(string Title, string Message, string OKButtonText, string CancelButtonText, IUIBindingInfo UIBinding)
        {
            throw new NotImplementedException();
        }

        public override void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            throw new NotImplementedException();
        }

        public override void ShowMenu(Menu Menu, object UIElement)
        {
            throw new NotImplementedException();
        }
    }
}