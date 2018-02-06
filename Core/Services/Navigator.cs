// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using PlatformBindings.Models;

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
        public bool Navigate(object Page)
        {
            return Navigate(Page, null);
        }

        /// <summary>
        /// Navigates to the requested Page, supplying a Parameter.
        /// </summary>
        /// <param name="Page">Page to Navigate to.</param>
        /// <param name="Parameter">Parameter, can be serialised.</param>
        /// <returns>Navigation Handled</returns>
        public bool Navigate(object Page, NavigationParameters Parameters)
        {
            return Navigate(Page, Parameters, false);
        }

        /// <summary>
        /// Navigates to the requested Page, supplying a Parameter.
        /// </summary>
        /// <param name="Page">Page to Navigate to.</param>
        /// <param name="Parameter">Parameter, can be serialised.</param>
        /// <param name="ClearBackStack">Clear the Navigation Back Stack?</param>
        /// <returns>Navigation Handled</returns>
        public virtual bool Navigate(object Page, NavigationParameters Parameters, bool ClearBackStack)
        {
            return false;
        }

        /// <summary>
        /// Gets the Parameter from the Current Page's Navigation Event.
        /// </summary>
        public abstract NavigationParameters Parameters { get; }
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
        public abstract bool Navigate(TPageIdentifier Page, NavigationParameters Parameters, bool ClearBackStack);

        /// <summary>
        /// Navigates to the requested Page, supplying a Parameter.
        /// </summary>
        /// <param name="Page">Page to Navigate to.</param>
        /// <param name="Parameter">Parameter, can be serialised.</param>
        /// <returns>Navigation Handled</returns>
        public bool Navigate(TPageIdentifier Page, NavigationParameters Parameters)
        {
            return Navigate(Page, Parameters, false);
        }

        /// <summary>
        /// Navigates to the requested Page, supplying a Parameter.
        /// </summary>
        /// <param name="Page">Page to Navigate to.</param>
        /// <returns>Navigation Handled</returns>
        public bool Navigate(TPageIdentifier Page)
        {
            return Navigate(Page, null);
        }

        /// <summary>
        /// Checks casting for Valid Navigation.
        /// </summary>
        /// <param name="Page">Page to Navigate to (Must derrive of Generic)</param>
        /// <param name="Parameter">Page Parameter.</param>
        /// <param name="ClearBackStack">Clear the Navigation Back Stack?</param>
        /// <returns>Navigation Handled</returns>
        public override bool Navigate(object Page, NavigationParameters Parameters, bool ClearBackStack)
        {
            if (Page is TPageIdentifier generic)
            {
                return Navigate(generic, Parameters, ClearBackStack);
            }
            else return base.Navigate(Page, Parameters, ClearBackStack);
        }
    }
}