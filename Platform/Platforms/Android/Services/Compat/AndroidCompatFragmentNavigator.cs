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

using Android.Support.V4.App;
using Android.Support.V7.App;
using PlatformBindings.Common;

namespace PlatformBindings.Services.Compat
{
    public abstract class AndroidCompatFragmentNavigator<T> : Navigator<T>
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

        public FragmentManager Manager => ((AppCompatActivity)AndroidHelpers.GetCurrentActivity())?.SupportFragmentManager;
    }
}