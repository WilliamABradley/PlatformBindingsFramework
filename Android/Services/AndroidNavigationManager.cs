using System;
using Android.App;
using Android.Support.V7.App;
using PlatformBindings.Common;

namespace PlatformBindings.Services
{
    public class AndroidNavigationManager : NavigationManager
    {
        public AndroidNavigationManager(Navigator Navigator) : base(Navigator)
        {
        }

        public override void GoBack()
        {
            CurrentActivity.Finish();
        }

        private bool BackButtonVisible(Activity Activity)
        {
            ActionBarDisplayOptions options = 0;
            if (Activity is AppCompatActivity compatAct)
            {
                options = (ActionBarDisplayOptions)(compatAct.SupportActionBar?.DisplayOptions ?? 0);
            }
            else if (Activity?.ActionBar != null)
            {
                options = Activity.ActionBar.DisplayOptions;
            }
            var hasState = options & ActionBarDisplayOptions.HomeAsUp;
            return hasState == ActionBarDisplayOptions.HomeAsUp;
        }

        public override bool CanGoBack => !CurrentActivity.IsTaskRoot;

        public override bool ShowBackButton
        {
            get
            {
                return BackButtonVisible(CurrentActivity);
            }
            set
            {
                var activity = CurrentActivity;
                var currentState = BackButtonVisible(activity);

                try
                {
                    if (activity is AppCompatActivity compatAct)
                    {
                        compatAct.SupportActionBar?.SetDisplayHomeAsUpEnabled(value);
                    }
                    else if (activity?.ActionBar != null)
                    {
                        activity.ActionBar?.SetDisplayHomeAsUpEnabled(value);
                    }
                }
                catch { }

                if (value != currentState)
                {
                    BackButtonStateChanged?.Invoke(this, value);
                }
            }
        }

        private Activity CurrentActivity => AndroidHelpers.GetCurrentActivity();

        public override event EventHandler<bool> BackButtonStateChanged;
    }
}