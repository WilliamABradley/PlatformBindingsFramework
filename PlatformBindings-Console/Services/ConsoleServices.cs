namespace PlatformBindings.ConsoleTools
{
    public class ConsoleServices : AppServices
    {
        public ConsoleServices() : base(new ConsoleServiceBindings())
        {
        }

        //Placeholder until .NET Standard 2.0
        public static string EntryAssembly { get; set; }
    }
}