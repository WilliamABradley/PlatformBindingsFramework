namespace PlatformBindings.Services
{
    /// <summary>
    /// Navigation Object in charge of Navigating to the Desired Page.
    /// </summary>
    public abstract class Navigator
    {
        /// <summary>
        /// Navigates to the Request Page.
        /// </summary>
        /// <param name="Page">Page to Navigate to.</param>
        /// <returns>Navigation Handled</returns>
        public abstract bool Navigate(object Page);

        /// <summary>
        /// Navigates to the requested Page, supplying a Parameter.
        /// </summary>
        /// <param name="Page">Page to Navigate to.</param>
        /// <param name="Parameter">Parameter, can be serialised.</param>
        /// <returns>Navigation Handled</returns>
        public abstract bool Navigate(object Page, string Parameter);

        /// <summary>
        /// Navigates to the requested Page, supplying a Parameter.
        /// </summary>
        /// <param name="Page">Page to Navigate to.</param>
        /// <param name="Parameter">Parameter, can be serialised.</param>
        /// <param name="ClearBackStack">Clear the Navigation Back Stack?</param>
        /// <returns>Navigation Handled</returns>
        public abstract bool Navigate(object Page, string Parameter, bool ClearBackStack);

        /// <summary>
        /// Gets the Parameter from the Current Page's Navigation Event.
        /// </summary>
        public abstract string Parameter { get; }
    }

    /// <summary>
    /// Navigation Object in charge of Navigating to the Desired Page.
    /// </summary>
    /// <typeparam name="TPageIdentifier">Page Identifier, can be a string or enum or anything to identify a Page.</typeparam>
    public abstract class Navigator<TPageIdentifier> : Navigator
    {
        /// <summary>
        /// Navigates to the requested Page, supplying a Parameter.
        /// </summary>
        /// <param name="Page">Page to Navigate to.</param>
        /// <param name="Parameter">Parameter, can be serialised.</param>
        /// <param name="ClearBackStack">Clear the Navigation Back Stack?</param>
        /// <returns>Navigation Handled</returns>
        public abstract bool Navigate(TPageIdentifier Page, string Parameter, bool ClearBackStack);

        /// <summary>
        /// Checks Casting
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        public override bool Navigate(object Page)
        {
            return Navigate(Page, null);
        }

        /// <summary>
        /// Checks casting for Valid Navigation.
        /// </summary>
        /// <param name="Page">Page to Navigate to (Must derrive of Generic)</param>
        /// <param name="Parameter">Page Parameter.</param>
        /// <returns>Navigation Handled</returns>
        public override bool Navigate(object Page, string Parameter)
        {
            if (Page is TPageIdentifier generic)
            {
                return Navigate(generic, Parameter, false);
            }
            else return false;
        }

        /// <summary>
        /// Checks casting for Valid Navigation.
        /// </summary>
        /// <param name="Page">Page to Navigate to (Must derrive of Generic)</param>
        /// <param name="Parameter">Page Parameter.</param>
        /// <param name="ClearBackStack">Clear the Navigation Back Stack?</param>
        /// <returns>Navigation Handled</returns>
        public override bool Navigate(object Page, string Parameter, bool ClearBackStack)
        {
            if (Page is TPageIdentifier generic)
            {
                return Navigate(generic, Parameter, ClearBackStack);
            }
            else return false;
        }
    }
}