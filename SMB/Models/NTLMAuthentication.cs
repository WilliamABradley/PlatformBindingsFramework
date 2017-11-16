namespace PlatformBindings.Models
{
    public class NTLMAuthentication
    {
        public NTLMAuthentication(string Username, string Password) : this(null, Username, Password)
        {
        }

        public NTLMAuthentication(string Domain, string Username, string Password)
        {
            this.Domain = Domain;
            this.Username = Username;
            this.Password = Password;
        }

        public string Domain { get; }
        public string Username { get; }
        public string Password { get; }
    }
}