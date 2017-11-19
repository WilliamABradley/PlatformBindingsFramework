using System.Runtime.CompilerServices;
using PlatformBindings.Models.Settings.Properties;

namespace PlatformBindings.Models.Settings
{
    public class SerialAppSettingList<T> : SerialPropertyList<T>
    {
        public SerialAppSettingList([CallerMemberName] string SettingName = "") : this(false, SettingName)
        {
        }

        public SerialAppSettingList(bool Roam, [CallerMemberName] string SettingName = "") : base(SettingName)
        {
            this.Roam = Roam;
            Attach(Roam ? AppServices.IO.RoamingSettings : AppServices.IO.LocalSettings);
        }

        public bool Roam { get; private set; } = false;
    }
}