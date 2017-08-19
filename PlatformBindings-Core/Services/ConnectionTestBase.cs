namespace PlatformBindings.Services
{
    public abstract class ConnectionTestBase
    {
        /// <summary>
        /// Gets whether the Application can access the Internet.
        /// </summary>
        public abstract bool HasInternetConnection { get; }
    }
}
