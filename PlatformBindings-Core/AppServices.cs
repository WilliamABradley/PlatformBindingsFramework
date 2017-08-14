using PlatformBindings.Services;

namespace PlatformBindings
{
    public abstract class AppServices
    {
        public AppServices(IServiceBindings Services)
        {
            AppServices.Services = Services;
        }

        public static IServiceBindings Services { get; protected set; }
    }
}