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
using Android.OS;
using PlatformBindings.Models;
using System;
using System.Threading.Tasks;

namespace PlatformBindings.Common
{
    public static class ActivityExtensions
    {
        public static void AttachContextMenu(this Activity Activity, Controls.MenuLayout.Menu Menu, AndroidContextMenuBinding Binding)
        {
            Activity.GetHandler().AttachContextMenu(Menu, Binding);
        }

        public static void OpenContextMenuForDisplay(this Activity Activity, Controls.MenuLayout.Menu Menu, AndroidContextMenuBinding Binding)
        {
            Activity.GetHandler().OpenContextMenuForDisplay(Menu, Binding);
        }

        public static Task<ActivityResult> StartActivityForResultAsync(this Activity Activity, Type activityType)
        {
            return Activity.GetHandler().StartActivityForResultAsync(activityType);
        }

        public static Task<ActivityResult> StartActivityForResultAsync(this Activity Activity, Intent intent)
        {
            return Activity.GetHandler().StartActivityForResultAsync(intent);
        }

        public static Task<ActivityResult> StartActivityForResultAsync(this Activity Activity, Intent intent, Bundle options)
        {
            return Activity.GetHandler().StartActivityForResultAsync(intent, options);
        }
    }
}