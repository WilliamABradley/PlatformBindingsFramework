using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using PlatformBindings.Models;
using PlatformBindings.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformBindings.Activities
{
    public class ActivityHandler
    {
        private ActivityHandler(Activity Activity)
        {
            this.Activity = Activity;
        }

        public static ActivityHandler GetActivityHandler(Activity Activity)
        {
            if (!Handlers.ContainsKey(Activity))
            {
                Handlers.Add(Activity, new ActivityHandler(Activity));
            }
            return Handlers[Activity];
        }

        public void UpdateCurrentActivity()
        {
            if (AppServices.UI == null)
            {
                throw new Exception("Please Initialise a new AndroidAppServices Instance for PlatformBindings to work.");
            }

            var uibinding = AppServices.UI.DefaultUIBinding as AndroidUIBindingInfo;
            uibinding.Activity =
#if APPCOMPAT
                (Android.Support.V7.App.AppCompatActivity)
#endif
                Activity;
        }

        public Task<ActivityResult> StartActivityForResultAsync(Type ActivityType)
        {
            var (requestCode, task) = CreateActivityRequest();
            Activity.StartActivityForResult(ActivityType, requestCode);
            return task.Task;
        }

        public Task<ActivityResult> StartActivityForResultAsync(Intent intent)
        {
            var (requestCode, task) = CreateActivityRequest();
            Activity.StartActivityForResult(intent, requestCode);
            return task.Task;
        }

        public Task<ActivityResult> StartActivityForResultAsync(Intent intent, Bundle options)
        {
            var (requestCode, task) = CreateActivityRequest();
            Activity.StartActivityForResult(intent, requestCode, options);
            return task.Task;
        }

        private (int RequestCode, TaskCompletionSource<ActivityResult> task) CreateActivityRequest()
        {
            var rand = new Random();
            int requestCode = 0;
            do
            {
                requestCode = rand.Next(short.MaxValue - 1);
            }
            while (ActivityResultWaiters.ContainsKey(requestCode));

            var task = new TaskCompletionSource<ActivityResult>();
            ActivityResultWaiters.Add(requestCode, task);
            task.Task.ContinueWith(t =>
            {
                ActivityResultWaiters.Remove(requestCode);
            });
            return (requestCode, task);
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (ActivityResultWaiters.ContainsKey(requestCode))
            {
                ActivityResultWaiters[requestCode].SetResult(new ActivityResult(requestCode, resultCode, data));
            }
        }

        public void AttachContextMenu(Controls.MenuLayout.Menu Menu, AndroidContextMenuBinding Binding)
        {
            var element = new Tuple<Controls.MenuLayout.Menu, AndroidContextMenuBinding>(Menu, Binding);

            if (!ContextMenuActivations.ContainsKey(Binding.TargetElement))
            {
                ContextMenuActivations.Add(Binding.TargetElement, element);
            }
            else ContextMenuActivations[Binding.TargetElement] = element;

            Activity.RegisterForContextMenu(Binding.TargetElement);
        }

        public void OpenContextMenuForDisplay(Controls.MenuLayout.Menu Menu, AndroidContextMenuBinding Binding)
        {
            AttachContextMenu(Menu, Binding);
            Activity.OpenContextMenu(Binding.TargetElement);
        }

        public bool OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (ContextMenuActivations.ContainsKey(v))
            {
                var binding = ContextMenuActivations[v];
                AndroidMenuRenderer.Attach(binding.Item1, binding.Item2, menu);
                return true;
            }
            else return false;
        }

        public Activity Activity { get; }

        private Dictionary<View, Tuple<Controls.MenuLayout.Menu, AndroidContextMenuBinding>> ContextMenuActivations { get; } = new Dictionary<View, Tuple<Controls.MenuLayout.Menu, AndroidContextMenuBinding>>();
        private Dictionary<int, TaskCompletionSource<ActivityResult>> ActivityResultWaiters { get; } = new Dictionary<int, TaskCompletionSource<ActivityResult>>();

        private static Dictionary<Activity, ActivityHandler> Handlers { get; } = new Dictionary<Activity, ActivityHandler>();
    }
}