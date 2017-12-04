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

using PlatformBindings;
using PlatformBindings.ConsoleTools;

namespace Test_NETCore
{
    internal class Program
    {
        public static NETCoreServices Services { get; private set; }

        private static void Main(string[] args)
        {
            Services = new NETCoreServices();
            AppServices.Current.UI.TitleManager.WindowTitle = "Platform Bindings - Console";
            Init();
            ConsoleHelpers.PreventClose();
        }

        private static async void Init()
        {
            var result = await AppServices.Current.UI.PromptUserAsync("Hello", "This is an Example", "Opt1", "Opt2");
            ConsoleHelpers.SystemWriteLine("Result: " + result);
        }
    }
}