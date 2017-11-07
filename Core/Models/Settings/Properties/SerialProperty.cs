using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace PlatformBindings.Models.Settings.Properties
{
    public class SerialProperty<T> : Property<string>
    {
        public SerialProperty([CallerMemberName]string SettingName = "", T Default = default(T)) : base(SettingName, null)
        {
            this.Default = Default;
        }

        private T Default;

        private object _Value { get; set; }

        public new T Value
        {
            get
            {
                if (_Value == null)
                {
                    if (string.IsNullOrWhiteSpace(base.Value)) return Default;
                    _Value = JsonConvert.DeserializeObject<T>(base.Value);
                }
                return (T)_Value;
            }
            set
            {
                _Value = value;
                base.Value = JsonConvert.SerializeObject(_Value);
            }
        }

        public void Update()
        {
            Value = Value;
        }
    }
}