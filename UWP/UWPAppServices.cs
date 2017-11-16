using System;
using PlatformBindings.Services;
using Windows.UI.Core;
using Windows.ApplicationModel;
using PlatformBindings.Services.Bindings;

namespace PlatformBindings
{
    /// <summary>
    /// Main Entrypoint for UWP, In order to use UI Functions, you need to use <see cref="AttachDispatcher(CoreDispatcher)"/> in your <see cref="Application.OnLaunched(Windows.ApplicationModel.Activation.LaunchActivatedEventArgs)"/> method or later.
    /// </summary>
    public class UWPAppServices : AppServices
    {
        public UWPAppServices(bool HasUI) : base(HasUI)
        {
            IO = new UWPIOBindings();
            Credentials = new UWPCredentialManager();
            OAuth = new UWPOAuthBroker();
            NetworkUtilities = new UWPNetworkUtilities();
        }

        /// <summary>
        /// Collects the Dispatcher for UI Manipulation, this is required to use any UI Function.
        /// </summary>
        /// <param name="Dispatcher"></param>
        public void AttachDispatcher(CoreDispatcher Dispatcher)
        {
            UI = new UWPUIBindings(Dispatcher);
        }

        public override Version GetAppVersion()
        {
            var appversion = Package.Current.Id.Version;
            return new Version(appversion.Major, appversion.Minor, appversion.Build, appversion.Revision);
        }
    }
}