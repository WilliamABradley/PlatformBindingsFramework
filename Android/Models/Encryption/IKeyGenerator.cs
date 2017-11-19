using System.Security;

namespace PlatformBindings.Models.Encryption
{
    /// <summary>
    /// Generates a Key to Encrypt/Decrypt the Credential Data, this key must be the same during Encryption/Decryption otherwise you will not be able to read any of the credentials.
    /// </summary>
    public interface IKeyGenerator
    {
        SecureString GetSecureKey();
    }
}