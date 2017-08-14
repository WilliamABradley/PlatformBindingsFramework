using System;

namespace PlatformBindings.Services
{
    public interface INavigationManager
    {
        void Navigate(object PageRequest, object Parameter);

        void ClearBackStack();

        void GoBack();

        bool CanGoBack { get; }

        bool ShowBackButton { get; set; }

        bool MenuOpen { get; set; }

        event EventHandler<bool> BackButtonStateChanged;
    }
}