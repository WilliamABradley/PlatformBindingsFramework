using PlatformBindings;
using PlatformBindings.Enums;
using System;

namespace Tests.Tests
{
    public class PickerTests
    {
        public async void PickFile()
        {
            var file = await AppServices.IO.PickFile(null);
            if (file != null)
            {
                var result = await AppServices.UI.PromptUserAsync("File", file.Path, "Open", "Close", null);
                if (result == DialogResult.Primary)
                {
                    await AppServices.IO.OpenFile(file);
                }
            }
            else PickerCancelled();
        }

        public async void PickFiles()
        {
            var files = await AppServices.IO.PickFiles(null);
            if (files != null)
            {
                string fileList = "";
                foreach (var file in files)
                {
                    fileList += file.Path + Environment.NewLine;
                }

                var result = await AppServices.UI.PromptUserAsync("Files", fileList, "Open", "Close", null);
                if (result == DialogResult.Primary)
                {
                    foreach (var file in files)
                    {
                        await AppServices.IO.OpenFile(file);
                    }
                }
            }
            else PickerCancelled();
        }

        public async void PickFolder()
        {
            var folder = await AppServices.IO.PickFolder(null);
            if (folder != null)
            {
                var result = await AppServices.UI.PromptUserAsync("Folder", folder.Path, "Open", "Close", null);
                if (result == DialogResult.Primary)
                {
                    await AppServices.IO.OpenFolder(folder, null);
                }
            }
            else PickerCancelled();
        }

        private void PickerCancelled()
        {
            AppServices.UI.PromptUser("Warning", "Picker Cancelled", "OK", null);
        }
    }
}