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