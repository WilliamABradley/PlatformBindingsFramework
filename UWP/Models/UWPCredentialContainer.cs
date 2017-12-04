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