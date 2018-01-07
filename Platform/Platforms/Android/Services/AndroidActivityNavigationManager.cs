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
using Android.App;
using Android.Support.V7.App;
using PlatformBindings.Common;

namespace PlatformBindings.Services
{
    public class AndroidActivityNavigationManager : NavigationManager
    {
        public AndroidActivityNavigationManager(Navigator Navigator) : base(Navigator)
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

        protected Activity CurrentActivity => AndroidHelpers.GetCurrentActivity();

        public override event EventHandler<bool> BackButtonStateChanged;
    }
}