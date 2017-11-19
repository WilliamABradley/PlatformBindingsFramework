using Windows.Security.Credentials;

namespace PlatformBindings.Models
{
    internal class UWPCredentialContainer : CredentialContainer
    {
        public UWPCredentialContainer(PasswordCredential Credential)
        {
            this.Credential = Credential;
        }

        public override string ResourceName { get => Credential.Resource; }
        public override string Username { get => Credential.UserName; }

        public override string Password
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Credential.Password)) Credential.RetrievePassword();
                return Credential.Password;
            }
            set => Credential.Password = value;
        }

        public PasswordCredential Credential { get; }
    }
}