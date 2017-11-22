using PlatformBindings.Models;
using System;

namespace PlatformBindings.Services
{
    public abstract class NavigationManager
    {
        public NavigationManager(Navigator Navigator)
        {
            this.Navigator = Navigator;
        }

        public void Navigate(object Page)
        {
            Navigator.Navigate(Page);
        }

        public void Navigate(object Page, string Parameter)
        {
            Navigator.Navigate(Page, Parameter);
        }

        public void Navigate(object Page, string Parameter, bool ClearBackStack)
        {
            Navigator.Navigate(Page, Parameter, ClearBackStack);
        }

        public abstract void GoBack();

        public abstract bool CanGoBack { get; }

        public abstract bool ShowBackButton { get; set; }

        public virtual event EventHandler<bool> BackButtonStateChanged;

        public INavigationMenuHandler Menu { get; set; }

        public Navigator Navigator { get; }
    }
}