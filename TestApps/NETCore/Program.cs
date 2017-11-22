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