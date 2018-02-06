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
using Newtonsoft.Json;
using PlatformBindings.Activities;
using PlatformBindings.Common;
using PlatformBindings.Models;
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

        protected virtual bool InternalNavigate(Type Type)
        {
            return InternalNavigate(Type, null, true);
        }

        protected virtual bool InternalNavigate(Type Type, NavigationParameters Parameters)
        {
            return InternalNavigate(Type, Parameters, true);
        }

        protected virtual bool InternalNavigate(Type Type, NavigationParameters Parameters, bool ShowBack)
        {
            return InternalNavigate(Type, Parameters, ShowBack, false);
        }

        protected virtual bool InternalNavigate(Type Type, NavigationParameters Parameters, bool ShowBack, bool ClearBackStack)
        {
            var activity = CurrentActivity;

            var intent = new Intent(activity, Type);
            intent.PutExtra("ShowBack", ShowBack);

            if (Parameters != null)
            {
                var param = JsonConvert.SerializeObject(Parameters);
                intent.PutExtra(ParameterKey, param);
            }

            if (ClearBackStack)
            {
                intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            }
            activity.StartActivity(intent);
            return true;
        }

        public override NavigationParameters Parameters
        {
            get
            {
                try
                {
                    var intent = CurrentActivity?.Intent;
                    var param = intent?.GetStringExtra(ParameterKey);
                    return JsonConvert.DeserializeObject<NavigationParameters>(param);
                }
                catch
                {
                    return null;
                }
            }
        }

        private Activity CurrentActivity => AndroidHelpers.GetCurrentActivity();

        private const string ParameterKey = "Parameters";
    }
}