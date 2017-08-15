using System.Runtime.CompilerServices;
using PlatformBindings.Common;
using PlatformBindings.Models.Settings.Properties;

namespace PlatformBindings.Models.Settings
{
    public class SerialAppSettingList<T> : SerialPropertyList<T>
    {
        public SerialAppSettingList(bool IsLocal = false, [CallerMemberName]string PropertyName = "") : base(PropertyName)
        {
            this.IsLocal = IsLocal;

            //Will Throw if Settings Containers unwired.
            Attach(PlatformBindingHelpers.GetSettingsContainer(IsLocal));
        }

        public bool IsLocal { get; private set; }
    }
}