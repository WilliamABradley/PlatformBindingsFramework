using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformBindings.Models.FileSystem;
using Android.App;
using Android.Content;
using PlatformBindings.Common;
using PlatformBindings.Models;

namespace PlatformBindings.Services
{
    public class AndroidFileSystemPickers : FileSystemPickers
    {
        public override bool SupportsPickFile => true;
        public override bool SupportsPickFolder => true;

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

        private async Task<ActivityResult> CreateFilePicker(FilePickerProperties Properties, bool Multiple)
        {
            Intent intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            if (Multiple) intent.PutExtra(Intent.ExtraAllowMultiple, Multiple);
            bool HasNoTypes = true;

            if (Properties != null)
            {
                if (Properties.FileTypes.Any())
                {
                    HasNoTypes = false;

                    intent.SetType(Properties.FileTypes.First().MimeType);

                    if (Properties.FileTypes.Count > 1)
                    {
                        string Altmimes = "";
                        var others = Properties.FileTypes.Skip(1);
                        int count = others.Count();
                        for (int i = 0; i < count; i++)
                        {
                            Altmimes += others.ElementAt(i).MimeType;
                            if (i + 1 < count) Altmimes += "|";
                        }
                        intent.PutExtra(Intent.ExtraMimeTypes, Altmimes);
                    }
                }
            }

            if (HasNoTypes) intent.SetType("*/*");
            return await AndroidHelpers.GetCurrentActivity().StartActivityForResultAsync(intent);
        }
    }
}