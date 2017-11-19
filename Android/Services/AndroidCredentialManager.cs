using System.Collections.Generic;
using System.Linq;
using PlatformBindings.Models;
using Android.App;
using PlatformBindings.Models.Settings;

namespace PlatformBindings.Services
{
    public class AndroidCredentialManager : ICredentialManager
    {
        public AndroidCredentialManager()
        {
            var package = Application.Context.PackageName.Split().Last();
            Container = new AndroidSettingsContainer("_SPECIAL", null);
        }

        public IReadOnlyList<CredentialContainer> FetchByResource(string Resource)
        {
            return AllCredentials.Where(item => item.ResourceName == Resource).ToList();
        }

        public CredentialContainer Store(CredentialContainer Credential)
        {
            if (Credential is AndroidCredentialContainer) return Credential;
            else
            {
                return new AndroidCredentialContainer(Container, Credential);
            }
        }

        public CredentialContainer Retrieve(string Resource, string Username)
        {
            return FetchByResource(Resource).FirstOrDefault(item => item.Username == Username);
        }

        public void Remove(CredentialContainer Credential)
        {
            var andrcred = Credential as AndroidCredentialContainer ?? Retrieve(Credential.ResourceName, Credential.Username) as AndroidCredentialContainer;
            if (andrcred != null) andrcred.Container.RemoveKey(andrcred.EncryptedHeader);
        }

        public void Clear()
        {
            Container.Clear();
        }

        public IReadOnlyList<CredentialContainer> AllCredentials
        {
            get
            {
                return Container.GetValueHeaders().Select(item => new AndroidCredentialContainer(Container, item)).ToList();
            }
        }

        private AndroidSettingsContainer Container { get; }
    }
}