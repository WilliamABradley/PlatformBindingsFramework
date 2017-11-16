using Windows.Foundation;
using Windows.UI.Xaml;

namespace PlatformBindings.Models
{
    public class ContextMenuBinding : IMenuBinding
    {
        public ContextMenuBinding(FrameworkElement Element, Point? Position)
        {
            this.Element = Element;
            this.Position = Position;
        }

        public FrameworkElement Element { get; }
        public Point? Position { get; }

        public object DataContext => Element.DataContext;
    }
}