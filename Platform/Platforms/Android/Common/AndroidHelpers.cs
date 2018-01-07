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
using PlatformBindings.Activities;
using PlatformBindings.Models;

namespace PlatformBindings.Common
{
    public static class AndroidHelpers
    {
        public static Activity GetCurrentActivity()
        {
            return GetCurrentActivity(null);
        }

        public static Activity GetCurrentActivity(this IUIBindingInfo UIBinding)
        {
            var uibinding = (UIBinding ?? AppServices.Current.UI.DefaultUIBinding) as AndroidUIBindingInfo;
            return uibinding.Activity;
        }

        public static ActivityHandler GetCurrentActivityHandler(this IUIBindingInfo UIBinding)
        {
            var uibinding = (UIBinding ?? AppServices.Current.UI.DefaultUIBinding) as AndroidUIBindingInfo;
            return ActivityHandler.GetActivityHandler(uibinding.Activity);
        }

        public static ActivityHandler GetHandler(this Activity Activity)
        {
            return ActivityHandler.GetActivityHandler(Activity);
        }
    }
}