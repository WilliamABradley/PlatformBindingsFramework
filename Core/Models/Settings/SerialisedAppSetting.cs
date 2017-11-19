using System.Runtime.CompilerServices;
using PlatformBindings.Models.Settings.Properties;

namespace PlatformBindings.Models.Settings
{
    public class SerialisedAppSetting<T> : SerialProperty<T>
    {
        public SerialisedAppSetting([CallerMemberName] string SettingName = "") : this(false, SettingName)
        {
        }

        public SerialisedAppSetting(bool Roam, [CallerMemberName] string SettingName = "") : this(default(T), Roam, SettingName)
        {
        }

        public SerialisedAppSetting(T Default, [CallerMemberName] string SettingName = "") : this(Default, false, SettingName)
        {
        }

        public SerialisedAppSetting(T Default, bool Roam, [CallerMemberName] string SettingName = "") : base(SettingName, Default)
        {
            this.Roam = Roam;
            Attach(Roam ? AppServices.IO.RoamingSettings : AppServices.IO.LocalSettings);
        }

        public bool Roam { get; private set; }
    }
}