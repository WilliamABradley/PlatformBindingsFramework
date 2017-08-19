using System;

namespace PlatformBindings.ConsoleTools
{
    public class ConsoleServices : AppServices
    {
        public ConsoleServices() : base(true)
        {
            UI = new ConsoleUIBindings();
            IO = new ConsoleIOBindings();
        }

        //Placeholder until .NET Standard 2.0
        public static string EntryAssembly { get; set; }

        public override Version GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}