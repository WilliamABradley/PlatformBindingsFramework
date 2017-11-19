using System.Collections.Generic;
using PlatformBindings.Models;

namespace PlatformBindings.Services
{
    public interface ICredentialManager
    {
        IReadOnlyList<CredentialContainer> FetchByResource(string Resource);

        CredentialContainer Retrieve(string Resource, string Username);

        CredentialContainer Store(CredentialContainer Credential);

        void Remove(CredentialContainer Credential);

        void Clear();

        IReadOnlyList<CredentialContainer> AllCredentials { get; }
    }
}