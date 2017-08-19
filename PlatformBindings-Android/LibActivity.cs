using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using PlatformBindings.Models;
using PlatformBindings.Services;

namespace PlatformBindings
{
    public class LibActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            var uibinding = AppServices.UI.DefaultUIBinding as AndroidUIBindingInfo;
            uibinding.Activity = this;
            base.OnCreate(bundle);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            ActivityReturned?.Invoke(this, new ActivityResult(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public void AttachContextMenu(Controls.MenuLayout.Menu Menu, AndroidContextMenuBinding Binding)
        {
            var element = new Tuple<Controls.MenuLayout.Menu, AndroidContextMenuBinding>(Menu, Binding);

            if (!ContextMenuActivations.ContainsKey(Binding.TargetElement))
            {
                ContextMenuActivations.Add(Binding.TargetElement, element);
            }
            else ContextMenuActivations[Binding.TargetElement] = element;

            RegisterForContextMenu(Binding.TargetElement);
        }

        public void OpenContextMenuForDisplay(Controls.MenuLayout.Menu Menu, AndroidContextMenuBinding Binding)
        {
            AttachContextMenu(Menu, Binding);
            OpenContextMenu(Binding.TargetElement);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (ContextMenuActivations.ContainsKey(v))
            {
                var binding = ContextMenuActivations[v];
                AndroidMenuRenderer.Attach(binding.Item1, binding.Item2, menu);
            }
            else base.OnCreateContextMenu(menu, v, menuInfo);
        }

        public event EventHandler<ActivityResult> ActivityReturned;

        private Dictionary<View, Tuple<Controls.MenuLayout.Menu, AndroidContextMenuBinding>> ContextMenuActivations { get; } = new Dictionary<View, Tuple<Controls.MenuLayout.Menu, AndroidContextMenuBinding>>();
    }
}