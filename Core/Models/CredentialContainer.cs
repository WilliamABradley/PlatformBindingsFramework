namespace PlatformBindings.Models
{
    public class CredentialContainer
    {
        public CredentialContainer()
        {
        }

        public CredentialContainer(string ResourceName, string Username, string Password)
        {
            this.ResourceName = ResourceName;
            this.Username = Username;
            this.Password = Password;
        }

        public virtual string ResourceName { get; }
        public virtual string Username { get; }
        public virtual string Password { get; set; }
    }
}