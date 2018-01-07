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
using System.Linq;
using System.Threading.Tasks;
using PlatformBindings.Models.FileSystem;
using Android.App;
using Android.Content;
using PlatformBindings.Common;
using PlatformBindings.Models;
using Java.Net;

namespace PlatformBindings.Services
{
    public class AndroidFileSystemPickers : FileSystemPickers
    {
        public override bool SupportsPickFile => true;
        public override bool SupportsPickFolder => true;
        public override bool SupportsSaveFile => false;

        public override async Task<FileContainer> PickFile(FilePickerProperties Properties)
        {
            var result = await CreateFilePicker(Properties, false);
            if (result != null && result.ResultCode == Result.Ok && result.Data != null)
            {
                return new AndroidSAFFileContainer(result.Data.Data);
            }
            else return null;
        }

        public override async Task<IReadOnlyList<FileContainer>> PickFiles(FilePickerProperties Properties)
        {
            List<FileContainer> Files = new List<FileContainer>();

            var result = await CreateFilePicker(Properties, true);
            if (result != null && result.ResultCode == Result.Ok && result.Data != null)
            {
                var clipData = result.Data.ClipData;
                if (clipData != null)
                {
                    for (int i = 0; i < clipData.ItemCount; i++)
                    {
                        var item = clipData.GetItemAt(i);
                        Files.Add(new AndroidSAFFileContainer(item.Uri));
                    }
                }
                else
                {
                    Files.Add(new AndroidSAFFileContainer(result.Data.Data));
                }
                return Files;
            }
            return null;
        }

        public override async Task<FolderContainer> PickFolder(FolderPickerProperties Properties)
        {
            Intent intent = new Intent(Intent.ActionOpenDocumentTree);
            var activity = AndroidHelpers.GetCurrentActivity();

            var result = await activity.StartActivityForResultAsync(intent);
            if (result != null && result.ResultCode == Result.Ok && result.Data != null)
            {
                return new AndroidSAFFolderContainer(result.Data.Data);
            }
            else return null;
        }

        public override Task<FileContainer> SaveFile(FileSavePickerProperties Properties)
        {
            throw new System.NotImplementedException();
        }

        private async Task<ActivityResult> CreateFilePicker(FilePickerProperties Properties, bool Multiple)
        {
            var activity = AndroidHelpers.GetCurrentActivity();

            Intent intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            if (Multiple) intent.PutExtra(Intent.ExtraAllowMultiple, Multiple);
            bool HasNoTypes = true;

            if (Properties != null)
            {
                if (Properties.FileTypes.Any())
                {
                    HasNoTypes = false;
                    var mimes = Properties.FileTypes.Select(extension => URLConnection.GuessContentTypeFromName(extension));
                    intent.SetType(mimes.First());

                    if (Properties.FileTypes.Count > 1)
                    {
                        var altmimes = mimes.Skip(1).ToArray();
                        intent.PutExtra(Intent.ExtraMimeTypes, altmimes);
                    }
                }
            }

            if (HasNoTypes) intent.SetType("*/*");
            return await activity.StartActivityForResultAsync(intent);
        }
    }
}