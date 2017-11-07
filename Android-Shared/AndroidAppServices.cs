using Android.App;
using PlatformBindings.Services;
using System;

namespace PlatformBindings
{
    public class AndroidAppServices : AppServices
    {
        public AndroidAppServices(bool HasUI) : base(HasUI)
        {
            if (HasUI) UI = new AndroidUIBindings();
            IO = new AndroidIOBindings();
        }

        public override Version GetAppVersion()
        {
            var info = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, Android.Content.PM.PackageInfoFlags.MetaData);
            return new Version(info.VersionName);
        }
    }
}