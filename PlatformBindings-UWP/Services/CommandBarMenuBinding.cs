using Windows.UI.Xaml.Controls;

namespace PlatformBindings.Services
{
    public class CommandBarMenuBinding : IMenuBinding
    {
        public CommandBarMenuBinding(CommandBar Bar)
        {
            this.Bar = Bar;
        }

        public CommandBar Bar { get; }

        public object DataContext => Bar.DataContext;
    }
}