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