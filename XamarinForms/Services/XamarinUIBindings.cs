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
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models;
using Xamarin.Forms;

namespace PlatformBindings.Services
{
    public class XamarinUIBindings : UIBindings
    {
        public XamarinUIBindings() : base(Platform.XamarinForms)
        {
        }

        public override InteractionManager InteractionManager => null;
        public override NavigationManager NavigationManager { get; set; }
        public override ITitleManager TitleManager { get; set; }

        public override IUIBindingInfo DefaultUIBinding { get; }

        public override void OpenLink(Uri Uri)
        {
            Device.OpenUri(Uri);
        }

        public override async Task<DialogResult> PromptUserAsync(string Title, string Message, string PrimaryButtonText, string SecondaryButtonText, IUIBindingInfo UIBinding)
        {
            var binding = (UIBinding ?? AppServices.Current.UI.DefaultUIBinding) as XamarinUIBindingInfo;
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

        public override Task<string> RequestTextFromUserAsync(string Title, string Message, string OKButtonText, string CancelButtonText, IUIBindingInfo UIBinding)
        {
            throw new NotImplementedException();
        }

        public override void ShowMenu(Controls.MenuLayout.Menu Menu, IMenuBinding Binding)
        {
            throw new NotImplementedException();
        }

        public override void ShowMenu(Controls.MenuLayout.Menu Menu, object UIElement)
        {
            throw new NotImplementedException();
        }

        public override void RegisterMenu(Controls.MenuLayout.Menu Menu, object UIElement)
        {
            throw new NotImplementedException();
        }
    }
}