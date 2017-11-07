using System;
using System.Threading.Tasks;

namespace PlatformBindings.ConsoleTools
{
    public static class ConsoleHelpers
    {
        private static TaskCompletionSource<int> CloseWaiter = new TaskCompletionSource<int>();
        private static ConsoleColor SystemColor = ConsoleColor.DarkCyan;

        public static void AnyKeyContinue()
        {
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey();
        }

        public static bool PromptYesNo(string Prompt)
        {
            Console.WriteLine(Prompt + " (y/n)");
            while (true)
            {
                var res = Console.ReadKey();
                if (res.Key == ConsoleKey.Y)
                {
                    Console.WriteLine();
                    return true;
                }
                else if (res.Key == ConsoleKey.N)
                {
                    Console.WriteLine();
                    return false;
                }
            }
        }

        public static void SystemWrite(string SystemText)
        {
            Console.ForegroundColor = SystemColor;
            Console.Write(SystemText);
            Console.ResetColor();
        }

        public static void SystemWriteLine(string SystemText)
        {
            Console.ForegroundColor = SystemColor;
            Console.WriteLine(SystemText);
            Console.ResetColor();
        }

        public static void PreventClose()
        {
            CloseWaiter.Task.Wait();
        }

        public static void Close(int Code = 0)
        {
            CloseWaiter.TrySetResult(Code);
        }

        public static void PrintProperty(string PropertyName, object Value)
        {
            Console.WriteLine($"{PropertyName}: {Value?.ToString() ?? "null"}");
        }

        public static string EnterPassword()
        {
            string pass = string.Empty;
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            return pass;
        }
    }
}