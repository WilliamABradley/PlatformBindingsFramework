// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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