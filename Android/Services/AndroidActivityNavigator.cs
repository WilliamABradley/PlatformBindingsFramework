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

using Android.App;
using Android.Content;
using PlatformBindings.Activities;
using PlatformBindings.Common;
using System;

namespace PlatformBindings.Services
{
    public abstract class AndroidActivityNavigator<T> : Navigator<T>
    {
        public AndroidActivityNavigator()
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