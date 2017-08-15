using System.Runtime.CompilerServices;
using PlatformBindings.Common;
using PlatformBindings.Models.Settings.Properties;

namespace PlatformBindings.Models.Settings
{
    public class SerialisedAppSetting<T> : SerialProperty<T>
    {
        public SerialisedAppSetting(bool IsLocal = false, [CallerMemberName] string SettingName = "", T Default = default(T)) : base(SettingName, Default)
        {
            this.IsLocal = IsLocal;

            //Will Throw if Settings Containers unwired.
            Attach(PlatformBindingHelpers.GetSettingsContainer(IsLocal));
        }

        public bool IsLocal { get; private set; }
    }
}