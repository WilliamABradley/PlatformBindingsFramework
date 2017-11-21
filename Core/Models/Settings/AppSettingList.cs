using System.Runtime.CompilerServices;
using PlatformBindings.Models.Settings.Properties;

namespace PlatformBindings.Models.Settings
{
    public class AppSettingList<T> : PropertyList<T> where T : IProperty
    {
        public AppSettingList([CallerMemberName] string SettingName = "") : this(false, SettingName)
        {
        }

        public AppSettingList(bool Roam, [CallerMemberName] string SettingName = "") : base(SettingName)
        {
            this.Roam = Roam;
            Attach(Roam ? AppServices.Current.IO.RoamingSettings : AppServices.Current.IO.LocalSettings);
        }

        public bool Roam { get; private set; }
    }
}