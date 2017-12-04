using Android.Support.V4.App;
using Android.Support.V7.App;

namespace PlatformBindings.Services.Compat
{
    public class AndroidCompatFragmentNavigationManager : AndroidActivityNavigationManager
    {
        public AndroidCompatFragmentNavigationManager(Navigator Navigator) : base(Navigator)
        {
        }

        public override bool CanGoBack => Manager.BackStackEntryCount > 0;

        public override void GoBack()
        {
            Manager.PopBackStackImmediate();
        }

        public FragmentManager Manager => ((AppCompatActivity)CurrentActivity)?.SupportFragmentManager;
    }
}