using Android.App;

namespace PlatformBindings.Services
{
    public class AndroidFragmentNavigationManager : AndroidActivityNavigationManager
    {
        public AndroidFragmentNavigationManager(Navigator Navigator) : base(Navigator)
        {
        }

        public override bool CanGoBack => Manager.BackStackEntryCount > 0;

        public override void GoBack()
        {
            Manager.PopBackStackImmediate();
        }

        public FragmentManager Manager => CurrentActivity.FragmentManager;
    }
}