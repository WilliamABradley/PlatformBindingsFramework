using Android.App;
using PlatformBindings.Common;
using System;

namespace PlatformBindings.Services
{
    public class AndroidNavigationManager : INavigationManager
    {
        public bool CanGoBack => throw new NotImplementedException();

        public bool ShowBackButton { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool MenuOpen { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<bool> BackButtonStateChanged;

        public void ClearBackStack()
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public virtual void Navigate(object PageRequest, object Parameter)
        {
            //CurrentActivity.ActionBar.add
        }

        private Activity CurrentActivity => AndroidHelpers.GetCurrentActivity();
    }
}