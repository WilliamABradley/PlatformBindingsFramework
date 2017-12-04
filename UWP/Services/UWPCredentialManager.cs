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
            try
            {
                foreach (var cred in Vault.FindAllByResource(Resource))
                {
                    Creds.Add(new UWPCredentialContainer(cred));
                }
            }
            catch { }
            return Creds;
        }

        public CredentialContainer Store(CredentialContainer Credential)
        {
            if (Credential is UWPCredentialContainer) return Credential;

            var internalcred = new PasswordCredential(Credential.ResourceName, Credential.Username, Credential.Password);
            Vault.Add(internalcred);
            return new UWPCredentialContainer(internalcred);
        }

        public CredentialContainer Retrieve(string Resource, string Username)
        {
            try
            {
                return new UWPCredentialContainer(Vault.Retrieve(Resource, Username));
            }
            catch { }
            return null;
        }

        public void Remove(CredentialContainer Credential)
        {
            var uwpcred = (Credential as UWPCredentialContainer) ?? Retrieve(Credential.ResourceName, Credential.Username) as UWPCredentialContainer;
            if (uwpcred != null) Vault.Remove(uwpcred.Credential);
        }

        public void Clear()
        {
            foreach (var cred in AllCredentials)
            {
                Remove(cred);
            }
        }

        public IReadOnlyList<CredentialContainer> AllCredentials => Vault.RetrieveAll().Select(item => new UWPCredentialContainer(item)).ToList();
        private PasswordVault Vault { get; } = new PasswordVault();
    }
}