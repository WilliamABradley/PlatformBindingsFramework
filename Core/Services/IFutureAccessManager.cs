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

using PlatformBindings.Models.FileSystem;
using System.Threading.Tasks;

namespace PlatformBindings.Services
{
    /// <summary>
    /// Future Access Manager allows you to access Files and Folders across App sessions, by using Tokens.
    /// </summary>
    public interface IFutureAccessManager
    {
        /// <summary>
        /// Gets Future Access Token for a Specified File or Folder. <see cref="RequiresFutureAccessPermission"/> to ensure that this is Required. <para/>
        /// This is Required by some Platforms to access User Files and Folders in future Sessions of the App, if the App is closed. You can store this Token in Local Settings to Access this file/folder Again. <para/>
        /// Might return null if it is an unsupported Type, or if the Future Access Manager is full (Which occurs on UWP). To check if it is full, see <see cref="FutureAccessFull"/>.
        /// </summary>
        /// <param name="Item">File/Folder to get Access Token for.</param>
        /// <returns>Future Access Token.</returns>
        string GetFutureAccessPermission(FileSystemContainer Item);

        /// <summary>
        /// Returns the
        /// </summary>
        /// <param name="Permission"></param>
        /// <returns></returns>
        Task<FileSystemContainer> RedeemFutureAccessTokenAsync(string Token);

        /// <summary>
        /// Checks to see if the provided Token is Valid and Redeemable.
        /// </summary>
        /// <param name="Token">Token to Validate</param>
        /// <returns>Is Token Valid</returns>
        bool TokenValid(string Token);

        /// <summary>
        /// Removes a file/folder from the Future Access List, using the Future Access Token.
        /// </summary>
        /// <param name="Token">Token of File/Folder to Remove</param>
        void RemoveFutureAccessPermission(string Token);

        /// <summary>
        /// Determines if the Future Access Manager is full, this might occur on UWP at 1000 Entries. <para/>
        /// If it is full, no more Tokens will be given out, unless you Remove some Permissions.
        /// </summary>
        bool FutureAccessFull { get; }
    }
}