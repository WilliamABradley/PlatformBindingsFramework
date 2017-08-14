using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PlatformBindings.Services;

namespace PlatformBindings
{
    public class AndroidAppServices : AppServices
    {
        public AndroidAppServices(bool HasUI) : base(new AndroidServiceBindings(HasUI))
        {
        }
    }
}