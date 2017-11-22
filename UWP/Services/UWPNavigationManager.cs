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