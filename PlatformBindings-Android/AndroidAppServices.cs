using PlatformBindings.Services;

namespace PlatformBindings
{
    public class AndroidAppServices : AppServices
    {
        public AndroidAppServices(bool HasUI) : base(new AndroidServiceBindings(HasUI))
        {
        }
    }
}