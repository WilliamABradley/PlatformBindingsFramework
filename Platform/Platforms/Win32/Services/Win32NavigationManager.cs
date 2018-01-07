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

using PlatformBindings.Common;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PlatformBindings.Services
{
    public class Win32NavigationManager : NavigationManager
    {
        public Win32NavigationManager(Navigator Navigator, Frame Frame) : base(Navigator)
        {
            this.Frame = Frame;
        }

        public override bool CanGoBack => Frame.CanGoBack;

        public override bool ShowBackButton
        {
            get => Frame.NavigationUIVisibility == NavigationUIVisibility.Automatic || Frame.NavigationUIVisibility == NavigationUIVisibility.Visible;
            set
            {
                PlatformBindingHelpers.OnUIThread(() =>
                {
                    Frame.NavigationUIVisibility = value ? NavigationUIVisibility.Visible : NavigationUIVisibility.Hidden;
                });
            }
        }

        public override void GoBack()
        {
            Frame.GoBack();
        }

        public Frame Frame { get; }
    }
}