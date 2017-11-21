using System.Runtime.CompilerServices;
using PlatformBindings.Models.Settings.Properties;

namespace PlatformBindings.Models.Settings
{
    public class AppSetting<T> : Property<T>
    {
        public AppSetting([CallerMemberName] string SettingName = "") : this(false, SettingName)
        {
        }

        public AppSetting(bool Roam, [CallerMemberName] string SettingName = "") : this(default(T), Roam, SettingName)
        {
        }

        public AppSetting(T Default, [CallerMemberName] string SettingName = "") : this(Default, false, SettingName)
        {
        }

        public AppSetting(T Default, bool Roam, [CallerMemberName] string SettingName = "") : base(SettingName, Default)
        {
            this.Roam = Roam;
            Attach(Roam ? AppServices.Current.IO.RoamingSettings : AppServices.Current.IO.LocalSettings);
        }

        public bool Roam { get; private set; }
    }
}