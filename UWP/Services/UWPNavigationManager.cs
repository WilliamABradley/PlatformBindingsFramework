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
using PlatformBindings.Common;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace PlatformBindings.Services
{
    public class UWPNavigationManager : NavigationManager
    {
        public UWPNavigationManager(Navigator Navigator, Frame Frame) : base(Navigator)
        {
            this.Frame = Frame;
            SystemNavigationManager.GetForCurrentView().BackRequested += WinNavigationManager_BackRequested;
        }

        public override void GoBack()
        {
            PlatformBindingHelpers.OnUIThread(() =>
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    ShowBackButton = Frame.CanGoBack;
                }
            });
        }

        private void WinNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            GoBack();
        }

        public override bool CanGoBack => Frame.CanGoBack;
        public Frame Frame { get; }

        public override bool ShowBackButton
        {
            get { return _ShowBackButton; }
            set
            {
                _ShowBackButton = value;
                BackButtonStateChanged?.Invoke(this, value);
                PlatformBindingHelpers.OnUIThread(() =>
                {
                    if (ShowBackButton)
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    }
                    else
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    }
                });
            }
        }

        private bool _ShowBackButton = false;

        public override event EventHandler<bool> BackButtonStateChanged;
    }
}