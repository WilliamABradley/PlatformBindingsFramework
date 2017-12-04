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