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

using System;
using PlatformBindings.Services;
using Windows.UI.Core;
using Windows.ApplicationModel;
using PlatformBindings.Services.Bindings;
using PlatformBindings.Enums;

namespace PlatformBindings
{
    /// <summary>
    /// Main Entrypoint for UWP, In order to use UI Functions, you need to use <see cref="AttachDispatcher(CoreDispatcher)"/> in your <see cref="Application.OnLaunched(Windows.ApplicationModel.Activation.LaunchActivatedEventArgs)"/> method or later.
    /// </summary>
    public class UWPAppServices : AppServices
    {
        public UWPAppServices(bool HasUI) : base(HasUI, Platform.UWP)
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