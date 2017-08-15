using System;
using PlatformBindings.Common;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace PlatformBindings.Services
{
    public abstract class WinNavigationManager : INavigationManager
    {
        public WinNavigationManager(Frame Frame)
        {
            this.Frame = Frame;
            SystemNavigationManager.GetForCurrentView().BackRequested += WinNavigationManager_BackRequested;
        }

        private void WinNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            GoBack();
        }

        public bool CanGoBack => Frame.CanGoBack;

        private bool _ShowBackButton = false;

        public bool ShowBackButton
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

        public void ClearBackStack()
        {
            Frame.BackStack.Clear();
        }

        public void GoBack()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                ShowBackButton = Frame.CanGoBack;
            }
        }

        public abstract void Navigate(object PageRequest, object Parameter = null);

        public Frame Frame { get; }
        public abstract bool MenuOpen { get; set; }

        public event EventHandler<bool> BackButtonStateChanged;
    }
}