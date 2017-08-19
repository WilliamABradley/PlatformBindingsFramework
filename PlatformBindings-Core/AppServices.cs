using PlatformBindings.Services;
using System;

namespace PlatformBindings
{
    /// <summary>
    /// Main Entrypoint for Accessing the Platform Bindings Framework. Instantiate it using Platform Specific, Inherited Class. 
    /// <para/>E.g. UWPAppServices on UWP.
    /// </summary>
    public abstract class AppServices
    {
        public AppServices(bool HasUI)
        {
            this.HasUI = HasUI;
        }

        /// <summary>
        /// Gets the Application Version, this information will be gathered from the Internal Application Version, such as PackageManifest for UWP.
        /// </summary>
        /// <returns></returns>
        public abstract Version GetAppVersion();

        /// <summary>
        /// Gets whether the current Application Scenario allows for User Input. This will be false in cases such as Background Tasks, Services, Notifications, etc. 
        /// <para/>If false, it likely means that the <see cref="UI"/> Property will be null.
        /// </summary>
        public bool HasUI { get; }

        /// <summary>
        /// Functions for Platform Independent UI Functions.
        /// </summary>
        public static UIBindingsBase UI { get; protected set; }

        /// <summary>
        /// Functions for Platform Independent IO Functions.
        /// </summary>
        public static IOBindings IO { get; protected set; }

        /// <summary>
        /// Functions for Platform Independent Credential Management.
        /// </summary>
        public static ICredentialManager Credentials { get; protected set; }

        /// <summary>
        /// Functions for Authenticating with OAuth.
        /// </summary>
        public static IOAuthBroker OAuth { get; protected set; }

        /// <summary>
        /// Functions for Testing Connection to the Internet and other Sources.
        /// </summary>
        public static ConnectionTestBase Connection { get; protected set; }
    }
}