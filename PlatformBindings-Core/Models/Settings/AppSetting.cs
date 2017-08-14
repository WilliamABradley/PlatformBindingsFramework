using System;
using System.Runtime.CompilerServices;
using PlatformBindings.Models.Settings.Properties;
using PlatformBindings.Common;

namespace PlatformBindings.Models.Settings
{
    public class AppSetting<T> : Property<T>
    {
        public AppSetting(bool IsLocal = false, [CallerMemberName]string SettingName = "", T Default = default(T)) : base(SettingName, Default)
        {
            this.IsLocal = IsLocal;

            //Will Throw if Settings Containers unwired.
            Attach(CoreHelpers.GetSettingsContainer(IsLocal));
        }

        public bool IsLocal { get; private set; }
    }
}