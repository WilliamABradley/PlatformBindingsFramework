using Android.App;
using Android.Support.V7.App;
using PlatformBindings.Activities;
using PlatformBindings.Common;
using System;

namespace PlatformBindings.Services
{
    public class AndroidNavigationManager : INavigationManager
    {
        public bool CanGoBack => throw new NotImplementedException();

        public bool ShowBackButton
        {
            get
            {
                return false;
            }
            set
            {
                var activity = CurrentActivity;
                if (activity is AppCompatActivity compatAct)
                {
                    compatAct.SupportActionBar?.SetDisplayHomeAsUpEnabled(true);
                }
                else if (activity.ActionBar != null)
                {
                    activity.ActionBar.SetDisplayHomeAsUpEnabled(true);
                }
            }
        }

        public bool MenuOpen { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<bool> BackButtonStateChanged;

        public void ClearBackStack()
        {
            foreach (var handle in ActivityHandler.Handlers)
            {
                if (handle.Key != CurrentActivity)
                {
                    handle.Key.Finish();
                    ActivityHandler.Handlers.Remove(handle.Key);
                }
            }
        }

        public void GoBack()
        {
            CurrentActivity.Finish();
        }

        public virtual void Navigate(object PageRequest, object Parameter)
        {
            //CurrentActivity.ActionBar.add
        }

        private Activity CurrentActivity => AndroidHelpers.GetCurrentActivity();
    }
}