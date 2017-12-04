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
            Attach(Roam ? AppServices.Current.IO.RoamingSettings : AppServices.Current.IO.LocalSettings);
        }

        public bool Roam { get; private set; }
    }
}