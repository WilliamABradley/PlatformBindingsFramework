namespace PlatformBindings.Models
{
    public class CredentialContainer
    {
        public CredentialContainer()
        {
        }

        public CredentialContainer(string Resource, string Username, string Password)
        {
            this.ResourceName = Resource;
            this.Username = Username;
            this.Password = Password;
        }

        public string ResourceName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}