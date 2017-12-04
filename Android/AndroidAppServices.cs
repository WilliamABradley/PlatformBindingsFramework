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

using Android.App;
using PlatformBindings.Enums;
using PlatformBindings.Models.Encryption;
using PlatformBindings.Services;
using System;

namespace PlatformBindings
{
    public class AndroidAppServices : AppServices
    {
        public AndroidAppServices(bool HasUI, bool UseAppCompatUI) : this(HasUI)
        {
            AndroidAppServices.UseAppCompatUI = UseAppCompatUI;
        }

        public AndroidAppServices(bool HasUI) : base(HasUI, Platform.Android)
        {
            if (HasUI) UI = new AndroidUIBindings();
            IO = new AndroidIOBindings();
            Credentials = new AndroidCredentialManager();
            OAuth = new AndroidOAuthBroker();
            NetworkUtilities = new NetworkUtilities();
        }

        public override Version GetAppVersion()
        {
            var info = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, Android.Content.PM.PackageInfoFlags.MetaData);
            return new Version(info.VersionName);
        }

        public static IKeyGenerator KeyGenerator { get; set; } = new DefaultKeyGenerator();
        public static bool UseAppCompatUI { get; set; } = false;
        public static TimeSpan OAuthTimeOut { get; set; } = TimeSpan.FromSeconds(10);
    }
}