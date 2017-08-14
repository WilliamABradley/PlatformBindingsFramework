namespace PlatformBindings.Services
{
    public class AndroidContextMenuBinding : IMenuBinding
    {
        public AndroidContextMenuBinding(LibActivity Activity, Android.Views.View TargetElement)
        {
            this.Activity = Activity;
            this.TargetElement = TargetElement;
        }

        public object DataContext { get; set; }

        public LibActivity Activity { get; }
        public Android.Views.View TargetElement { get; }
    }
}