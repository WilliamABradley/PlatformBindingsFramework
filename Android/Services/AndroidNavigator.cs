using Android.App;
using Android.Content;
using PlatformBindings.Activities;
using PlatformBindings.Common;
using System;

namespace PlatformBindings.Services
{
    public abstract class AndroidNavigator<T> : Navigator<T>
    {
        public AndroidNavigator()
        {
            ActivityHandler.ActivityChanged += ActivityHandler_ActivityChanged;
        }

        private void ActivityHandler_ActivityChanged(object sender, EventArgs e)
        {
            if (sender is Activity activity)
            {
                var showBack = activity.Intent.GetBooleanExtra("ShowBack", false);
                if (AppServices.Current?.UI?.NavigationManager is NavigationManager manager)
                {
                    manager.ShowBackButton = showBack;
                }
            }
        }

        protected virtual bool InternalNavigate(Type Type, string Parameter)
        {
            return InternalNavigate(Type, Parameter, true);
        }

        protected virtual bool InternalNavigate(Type Type, string Parameter, bool ShowBack)
        {
            return InternalNavigate(Type, Parameter, ShowBack, false);
        }

        protected virtual bool InternalNavigate(Type Type, string Parameter, bool ShowBack, bool ClearBackStack)
        {
            var activity = CurrentActivity;

            var intent = new Intent(activity, Type);
            intent.PutExtra("ShowBack", ShowBack);
            if (!string.IsNullOrWhiteSpace(Parameter))
            {
                intent.PutExtra("Parameter", Parameter);
            }
            if (ClearBackStack)
            {
                intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            }
            activity.StartActivity(intent);
            return true;
        }

        public override string Parameter
        {
            get
            {
                var intent = CurrentActivity?.Intent;
                return intent?.GetStringExtra("Parameter");
            }
        }

        private Activity CurrentActivity => AndroidHelpers.GetCurrentActivity();
    }
}