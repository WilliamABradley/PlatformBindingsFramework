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

using System;
using System.Threading.Tasks;
using PlatformBindings.Models.FileSystem;
using Windows.Storage.AccessCache;
using Windows.Storage;
using Newtonsoft.Json;
using PlatformBindings.Common;

namespace PlatformBindings.Services
{
    public class UWPFutureAccessManager : IFutureAccessManager
    {
        public string GetFutureAccessPermission(StorageContainer Item)
        {
            if (FutureAccessFull)
            {
                return null;
            }

            IStorageItem ItemToStore = null;
            if (Item is IUWPStorageContainer container)
            {
                ItemToStore = container.Item;
            }

            if (ItemToStore != null)
            {
                var token = StorageApplicationPermissions.FutureAccessList.Add(ItemToStore);
                var result = JsonConvert.SerializeObject(new UWPToken
                {
                    OriginalPath = Item.Path,
                    Token = token
                });
                return PlatformBindingHelpers.ConvertToBase64(result);
            }
            return null;
        }

        public async Task<StorageContainer> RedeemFutureAccessTokenAsync(string Token)
        {
            var result = GetToken(Token);
            var item = await StorageApplicationPermissions.FutureAccessList.GetItemAsync(result.Token);
            if (item.Path != result.OriginalPath)
            {
                if (item is StorageFolder) item = await StorageFolder.GetFolderFromPathAsync(result.OriginalPath);
                else item = await StorageFile.GetFileFromPathAsync(result.OriginalPath);
            }

            if (item is StorageFolder folder) return new UWPFolderContainer(folder);
            else return new UWPFileContainer(item as StorageFile);
        }

        public void RemoveFutureAccessPermission(string Token)
        {
            StorageApplicationPermissions.FutureAccessList.Remove(Token);
        }

        public bool TokenValid(string Token)
        {
            var result = GetToken(Token);
            return StorageApplicationPermissions.FutureAccessList.ContainsItem(result.Token);
        }

        private UWPToken GetToken(string EncodedString)
        {
            return JsonConvert.DeserializeObject<UWPToken>(PlatformBindingHelpers.ConvertFromBase64(EncodedString));
        }

        private class UWPToken
        {
            // Due to a bug in the FutureAccessList, Redeeming a Token of folders such as C:\users\{user}\Downloads, when the User Downloads Folder was moved to a different Directory results in the Token returning the Wrong Directory.
            public string OriginalPath { get; set; }

            public string Token { get; set; }
        }

        public bool FutureAccessFull => StorageApplicationPermissions.FutureAccessList.Entries.Count == StorageApplicationPermissions.FutureAccessList.MaximumItemsAllowed;
    }
}