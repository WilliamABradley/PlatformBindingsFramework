using System.Collections.Generic;
using System.Linq;
using PlatformBindings.Models;
using Windows.Security.Credentials;

namespace PlatformBindings.Services.Bindings
{
    public class UWPCredentialManager : ICredentialManager
    {
        public UWPCredentialManager()
        {
        }

        public IReadOnlyList<CredentialContainer> FetchByResource(string Resource)
        {
            List<CredentialContainer> Creds = new List<CredentialContainer>();
            foreach (var cred in Vault.FindAllByResource(Resource))
            {
                Creds.Add(ConvertToContainer(cred));
            }
            return Creds;
        }

        public void Store(CredentialContainer Credentials)
        {
            Vault.Add(ConvertToLocal(Credentials));
        }

        public CredentialContainer Retrieve(string Resource, string Username)
        {
            return ConvertToContainer(Vault.Retrieve(Resource, Username));
        }

        private static CredentialContainer ConvertToContainer(PasswordCredential Credentials)
        {
            return new CredentialContainer
            {
                ResourceName = Credentials.Resource,
                Username = Credentials.UserName,
                Password = Credentials.Password
            };
        }

        private static PasswordCredential ConvertToLocal(CredentialContainer Credentials)
        {
            return new PasswordCredential(Credentials.ResourceName, Credentials.Username, Credentials.Password);
        }

        public void Remove(CredentialContainer Credential)
        {
            var cred = Vault.Retrieve(Credential.ResourceName, Credential.Username);
            Vault.Remove(cred);
        }

        public void Update(CredentialContainer Credential)
        {
            var cred = Vault.Retrieve(Credential.ResourceName, Credential.Username);
            cred.Password = Credential.Password;
        }

        public void Clear()
        {
            foreach (var cred in Vault.RetrieveAll()) Vault.Remove(cred);
        }

        private PasswordVault Vault { get; } = new PasswordVault();

        public IReadOnlyList<CredentialContainer> AllCredentials => Vault.RetrieveAll().Select(item => ConvertToContainer(item)).ToList();
    }
}