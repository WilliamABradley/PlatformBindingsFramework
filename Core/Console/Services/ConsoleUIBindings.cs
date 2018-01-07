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
using PlatformBindings.Controls.MenuLayout;
using PlatformBindings.Models;

namespace PlatformBindings.Services
{
    public abstract class ConsoleUIBindings : UIBindings
    {
        public ConsoleUIBindings() : base(Platform.Console)
        {
        }

        public override InteractionManager InteractionManager => null;
        public override NavigationManager NavigationManager { get; set; }
        public override ITitleManager TitleManager { get; set; } = new ConsoleTitleManager();

        public override IUIBindingInfo DefaultUIBinding => new ConsoleUIBindingInfo();

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
                Console.WriteLine($"{PrimaryButtonText}/{SecondaryButtonText}?" + " (1/2)");
                while (true)
                {
                    var res = Console.ReadKey();
                    if (res.Key == ConsoleKey.D1)
                    {
                        Console.WriteLine();
                        result = DialogResult.Primary;
                        break;
                    }
                    else if (res.Key == ConsoleKey.D2)
                    {
                        Console.WriteLine();
                        result = DialogResult.Secondary;
                        break;
                    }
                }
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

        public override void ShowMenu(Menu Menu, IMenuBinding Binding)
        {
            throw new NotImplementedException();
        }

        public override void ShowMenu(Menu Menu, object UIElement)
        {
            throw new NotImplementedException();
        }

        public override void RegisterMenu(Menu Menu, object UIElement)
        {
            throw new NotImplementedException();
        }
    }
}