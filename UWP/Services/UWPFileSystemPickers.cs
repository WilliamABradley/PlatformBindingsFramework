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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformBindings.Models.FileSystem;
using Windows.Storage.Pickers;
using PlatformBindings.Enums;
using PlatformBindings.Common;

namespace PlatformBindings.Services
{
    public class UWPFileSystemPickers : FileSystemPickers
    {
        public override bool SupportsPickFile => true;
        public override bool SupportsPickFolder => true;
        public override bool SupportsSaveFile => true;

        public override async Task<FileContainer> PickFile(FilePickerProperties Properties)
        {
            TaskCompletionSource<FileContainer> waiter = new TaskCompletionSource<FileContainer>();
            PlatformBindingHelpers.OnUIThread(async () =>
            {
                var picker = GetFilePicker(Properties);
                var file = await picker.PickSingleFileAsync();
                waiter.TrySetResult(file != null ? new UWPFileContainer(file) : null);
            });
            return await waiter.Task;
        }

        public override async Task<IReadOnlyList<FileContainer>> PickFiles(FilePickerProperties Properties)
        {
            TaskCompletionSource<IReadOnlyList<FileContainer>> waiter = new TaskCompletionSource<IReadOnlyList<FileContainer>>();
            PlatformBindingHelpers.OnUIThread(async () =>
            {
                var picker = GetFilePicker(Properties);
                var files = await picker.PickMultipleFilesAsync();
                waiter.TrySetResult(files?.Select(item => new UWPFileContainer(item)).ToList());
            });
            return await waiter.Task;
        }

        public override async Task<FolderContainer> PickFolder(FolderPickerProperties Properties)
        {
            TaskCompletionSource<FolderContainer> waiter = new TaskCompletionSource<FolderContainer>();
            PlatformBindingHelpers.OnUIThread(async () =>
            {
                var picker = new FolderPicker();
                if (Properties != null)
                {
                    foreach (var property in Properties.FileTypes)
                    {
                        picker.FileTypeFilter.Add(property);
                    }
                    if (Properties.StartingLocation.HasValue)
                    {
                        var location = GetPickerLocation(Properties.StartingLocation);
                        if (location != PickerLocationId.Unspecified) picker.SuggestedStartLocation = location;
                    }
                }

                if (Properties == null || !Properties.FileTypes.Any()) picker.FileTypeFilter.Add("*");

                var folder = await picker.PickSingleFolderAsync();

                waiter.TrySetResult(folder != null ? new UWPFolderContainer(folder) : null);
            });
            return await waiter.Task;
        }

        public override async Task<FileContainer> SaveFile(FileSavePickerProperties Properties)
        {
            TaskCompletionSource<FileContainer> waiter = new TaskCompletionSource<FileContainer>();
            PlatformBindingHelpers.OnUIThread(async () =>
            {
                FileSavePicker picker = new FileSavePicker();

                if (Properties != null)
                {
                    if (!string.IsNullOrWhiteSpace(Properties.SuggestedName))
                    {
                        picker.SuggestedFileName = Properties.SuggestedName;
                    }

                    if (Properties.FileTypes != null)
                    {
                        foreach (var filter in Properties.FileTypes.Where(ext => ext != "*"))
                        {
                            picker.FileTypeChoices.Add(filter, new string[] { filter });
                        }
                    }
                }

                var file = await picker.PickSaveFileAsync();
                waiter.TrySetResult(file != null ? new UWPFileContainer(file) : null);
            });

            return await waiter.Task;
        }

        private FileOpenPicker GetFilePicker(FilePickerProperties Properties = null)
        {
            FileOpenPicker picker = new FileOpenPicker();
            if (Properties != null)
            {
                foreach (var property in Properties.FileTypes)
                {
                    picker.FileTypeFilter.Add(property);
                }
                if (Properties.StartingLocation.HasValue)
                {
                    var location = GetPickerLocation(Properties.StartingLocation);
                    if (location != PickerLocationId.Unspecified) picker.SuggestedStartLocation = location;
                }
            }

            if (Properties == null || !Properties.FileTypes.Any()) picker.FileTypeFilter.Add("*");
            return picker;
        }

        private PickerLocationId GetPickerLocation(PathRoot? Root)
        {
            switch (Root)
            {
                case PathRoot.Documents:
                    return PickerLocationId.DocumentsLibrary;

                case PathRoot.Downloads:
                    return PickerLocationId.Downloads;

                case PathRoot.Videos:
                    return PickerLocationId.VideosLibrary;

                case PathRoot.Music:
                    return PickerLocationId.MusicLibrary;

                default:
                    return PickerLocationId.Unspecified;
            }
        }
    }
}