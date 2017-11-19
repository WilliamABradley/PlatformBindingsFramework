using Android.App;
using PlatformBindings.Common;
using System.Security;

namespace PlatformBindings.Models.Encryption
{
    public class DefaultKeyGenerator : IKeyGenerator
    {
        public SecureString GetSecureKey()
        {
            var raw = Application.Context.PackageName + Application.Context.ApplicationInfo.Uid.ToString();
            var data = PlatformBindingHelpers.ConvertToBase64(raw);

            var secure = new SecureString();
            foreach (var c in data)
            {
                secure.AppendChar(c);
            }
            return secure;
        }
    }
}