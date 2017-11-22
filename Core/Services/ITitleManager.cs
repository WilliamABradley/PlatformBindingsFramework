namespace PlatformBindings.Services
{
    /// <summary>
    /// APIs for setting and getting the Titles for the Page and Window.
    /// </summary>
    public interface ITitleManager
    {
        /// <summary>
        /// Sets the Title of the Page in the App/
        /// </summary>
        string PageTitle { get; set; }

        /// <summary>
        /// Sets the Additional Text on the App Window.
        /// </summary>
        string WindowTitle { get; set; }

        /// <summary>
        /// Determines if the Platform supports Window Titles.
        /// </summary>
        bool SupportsWindowTitle { get; }

        /// <summary>
        /// Determines if the App/Platform Supports Page Titles.
        /// </summary>
        bool SupportsPageTitle { get; }
    }
}