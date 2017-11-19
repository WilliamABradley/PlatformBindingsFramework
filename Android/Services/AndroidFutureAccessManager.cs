using Android.Content;
using Newtonsoft.Json;
using PlatformBindings.Common;
using PlatformBindings.Models.FileSystem;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformBindings.Services
{
    public class AndroidFutureAccessManager : IFutureAccessManager
    {
        public string GetFutureAccessPermission(FileSystemContainer Item)
        {
            var isFolder = Item is AndroidSAFFolderContainer;
            var uri = Item is IAndroidSAFContainer saf ? saf.Uri : null;
            if (uri != null)
            {
                var activity = AndroidHelpers.GetCurrentActivity();
                var flags = ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission;
                activity.GrantUriPermission(activity.PackageName, uri, flags);
                activity.ContentResolver.TakePersistableUriPermission(uri, flags);

                var result = JsonConvert.SerializeObject(new SAFToken
                {
                    URI = uri.SchemeSpecificPart,
                    IsFolder = isFolder
                });
                return PlatformBindingHelpers.ConvertToBase64(result);
            }
            else return null;
        }

        public Task<FileSystemContainer> RedeemFutureAccessTokenAsync(string Token)
        {
            FileSystemContainer item = null;

            var result = GetToken(Token);

            var activity = AndroidHelpers.GetCurrentActivity();
            if (activity.ContentResolver.PersistedUriPermissions.Any(perm => perm.Uri == result.uri))
            {
                if (result.props.IsFolder) item = new AndroidSAFFolderContainer(result.uri);
                else item = new AndroidSAFFileContainer(result.uri);
            }
            return Task.FromResult(item);
        }

        public void RemoveFutureAccessPermission(string Token)
        {
            var resolver = AndroidHelpers.GetCurrentActivity().ContentResolver;
            var permission = resolver.PersistedUriPermissions.FirstOrDefault(item => item.Uri.SchemeSpecificPart == Token);
            if (permission != null)
            {
                resolver.PersistedUriPermissions.Remove(permission);
            }
        }

        public bool TokenValid(string Token)
        {
            var result = GetToken(Token);
            var resolver = AndroidHelpers.GetCurrentActivity().ContentResolver;

            return resolver.PersistedUriPermissions.FirstOrDefault(item => item.Uri == result.uri) != null;
        }

        private (SAFToken props, Android.Net.Uri uri) GetToken(string EncodedString)
        {
            var result = JsonConvert.DeserializeObject<SAFToken>(PlatformBindingHelpers.ConvertFromBase64(EncodedString));
            var uri = Android.Net.Uri.Parse(result.URI);
            return (result, uri);
        }

        public bool FutureAccessFull => false;

        private class SAFToken
        {
            public string URI { get; set; }
            public bool IsFolder { get; set; }
        }
    }
}