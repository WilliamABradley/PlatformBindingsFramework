using Android.App;
using PlatformBindings.Common;

namespace PlatformBindings.Services
{
    public abstract class AndroidFragmentNavigator<T> : Navigator<T>
    {
        /// <summary>
        /// Replaces the PrimaryNavigationFragment with a new Activity.
        /// </summary>
        /// <param name="Fragment">New Navigation Fragment.</param>
        /// <param name="Parameter">Parameter to remember after the Navigation.</param>
        /// <param name="ClearBackStack">Removes the ability to go to the previous Fragment.</param>
        /// <returns>Navigation Handled</returns>
        protected bool NavigatePrimaryFragment(Fragment Fragment, string Parameter, bool ClearBackStack)
        {
            var currentNavigationActivity = Manager.PrimaryNavigationFragment;
            _Parameter = Parameter;

            var transaction = Manager.BeginTransaction();
            transaction.Replace(currentNavigationActivity.Id, Fragment);
            transaction.SetPrimaryNavigationFragment(Fragment);
            if (!ClearBackStack)
            {
                transaction.AddToBackStack(null);
            }
            transaction.Commit();
            return true;
        }

        public override string Parameter => _Parameter;

        /// <summary>
        /// Parameter Storage, as Fragment Navigation doesn't natively support Parameters that I'm aware of.
        /// </summary>
        protected string _Parameter { get; set; }

        public FragmentManager Manager => AndroidHelpers.GetCurrentActivity().FragmentManager;
    }
}