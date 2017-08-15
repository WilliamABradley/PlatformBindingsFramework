using System;
using Android.App;

namespace PlatformBindings.Services
{
    public class AndroidServiceBindings : IServiceBindings
    {
        public AndroidServiceBindings(bool HasUI)
        {
            this.HasUI = HasUI;
            if (HasUI) UI = new AndroidUIBindings();
        }

        public bool HasUI { get; }

        public UIBindingsBase UI { get; }

        public IOBindings IO => new AndroidIOBindings();

        public ICredentialManager Credentials => throw new NotImplementedException();

        public IOAuthBroker OAuth => throw new NotImplementedException();

        public bool HasInternetConnection => throw new NotImplementedException();

        public Version GetAppVersion()
        {
            var info = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, Android.Content.PM.PackageInfoFlags.MetaData);
            return new Version(info.VersionName);
        }
    }
}