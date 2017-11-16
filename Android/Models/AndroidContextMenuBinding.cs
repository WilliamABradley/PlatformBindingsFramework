using Android.App;

namespace PlatformBindings.Models
{
    public class AndroidContextMenuBinding : IMenuBinding
    {
        public AndroidContextMenuBinding(Activity Activity, Android.Views.View TargetElement)
        {
            this.Activity = Activity;
            this.TargetElement = TargetElement;
        }

        public object DataContext { get; set; }

        public Activity Activity { get; }
        public Android.Views.View TargetElement { get; }
    }
}