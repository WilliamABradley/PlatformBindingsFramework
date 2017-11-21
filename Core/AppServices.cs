using PlatformBindings.Enums;
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
        public AppServices(bool HasUI, Platform Platform)
        {
            Current = this;
            this.HasUI = HasUI;
            ServicePlatform = Platform;
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
        public UIBindings UI { get; protected set; }

        /// <summary>
        /// Functions for Platform Independent IO Functions.
        /// </summary>
        public IOBindings IO { get; protected set; }

        /// <summary>
        /// Functions for Platform Independent Credential Management.
        /// </summary>
        public ICredentialManager Credentials { get; protected set; }

        /// <summary>
        /// Functions for Authenticating with OAuth.
        /// </summary>
        public IOAuthBroker OAuth { get; protected set; }

        /// <summary>
        /// Functions for Testing Connection to the Internet and other Sources.
        /// </summary>
        public NetworkUtilities NetworkUtilities { get; protected set; }

        /// <summary>
        /// The Current Platform the Framework is running on.
        /// </summary>
        public Platform ServicePlatform { get; private set; }

        /// <summary>
        /// The Current AppServices Instance.
        /// </summary>
        public static AppServices Current { get; protected set; }
    }
}