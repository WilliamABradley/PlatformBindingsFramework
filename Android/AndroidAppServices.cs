using Android.App;
using PlatformBindings.Enums;
using PlatformBindings.Models.Encryption;
using PlatformBindings.Services;
using System;

namespace PlatformBindings
{
    public class AndroidAppServices : AppServices
    {
        public AndroidAppServices(bool HasUI, bool UseAppCompatUI) : this(HasUI)
        {
            KeyGenerator = new DefaultKeyGenerator();
            AndroidAppServices.UseAppCompatUI = UseAppCompatUI;
        }

        public AndroidAppServices(bool HasUI) : base(HasUI, Platform.Android)
        {
            if (HasUI) UI = new AndroidUIBindings();
            IO = new AndroidIOBindings();
            Credentials = new AndroidCredentialManager();
        }

        public override Version GetAppVersion()
        {
            var info = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, Android.Content.PM.PackageInfoFlags.MetaData);
            return new Version(info.VersionName);
        }

        public static IKeyGenerator KeyGenerator { get; protected set; }
        public static bool UseAppCompatUI { get; private set; } = false;
    }
}