using PlatformBindings.Services;

namespace PlatformBindings
{
    public class XamarinAppServices : AppServices
    {
        public XamarinAppServices(bool HasUI) : base(new XamarinServiceBindings(HasUI))
        {
        }
    }
}