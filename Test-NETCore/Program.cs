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
            ConsoleHelpers.SystemWriteLine("Hello World!");
            Init();
            ConsoleHelpers.PreventClose();
        }

        private static async void Init()
        {
            var result = await AppServices.UI.PromptUserAsync("Hello", "This is an Example", "Opt1", "Opt2");
            ConsoleHelpers.SystemWriteLine("Result: " + result);
        }
    }
}