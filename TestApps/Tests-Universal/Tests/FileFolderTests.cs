using PlatformBindings;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Tests
{
    public class FileFolderTests : ViewModelBase
    {
        public FileFolderTests()
        {
        }

        public async void PickFolderGetItems()
        {
            var folder = await AppServices.IO.PickFolder();
            if (folder != null)
            {
                var items = await folder.GetItemsAsync();
                await AppServices.UI.PromptUserAsync("Items:", string.Join(Environment.NewLine, items.Select(item => $"{item.Name} - {item.GetType().Name}")), "OK");
                return;
            }
            await AppServices.UI.PromptUserAsync("Folder is Null", "Picker Cancelled or Folder not Accessed Properly.", "OK");
        }

        public async void PickFolderCreateFile()
        {
            var folder = await AppServices.IO.PickFolder();
            if (folder != null)
            {
                var file = await folder.CreateFileAsync("TestFile.txt");
                if (file != null)
                {
                    await file.SaveText("This is a Test File!");
                }
                await AppServices.UI.PromptUserAsync($"File Creation:", file != null ? "Success" : "Failed", "OK");
                return;
            }
            await AppServices.UI.PromptUserAsync("Folder is Null", "Picker Cancelled or Folder not Accessed Properly.", "OK");
        }

        public async void PickFileReadText()
        {
            var file = await AppServices.IO.PickFile();
            if (file != null)
            {
                var content = await file.ReadFileAsText();
                await AppServices.UI.PromptUserAsync($"{file.Name} contents:", content, "OK");
                return;
            }
            await AppServices.UI.PromptUserAsync("File is Null", "Picker Cancelled or File not Accessed Properly.", "OK");
        }

        public async void PickFolderGetSubFolderFolders()
        {
            var folder = await AppServices.IO.PickFolder();
            if (folder != null)
            {
                var subfolders = await folder.GetFoldersAsync();
                if (subfolders.Count > 0)
                {
                    var SubSubFolders = new List<FolderContainer>();
                    foreach (var element in subfolders)
                    {
                        SubSubFolders.AddRange(await element.GetFoldersAsync());
                    }
                    await AppServices.UI.PromptUserAsync("SubSubFolders:", SubSubFolders.Count > 0 ? string.Join(Environment.NewLine, SubSubFolders.Select(item => item.Name)) : "No Subsubfolders", "OK");
                }
                return;
            }
            await AppServices.UI.PromptUserAsync("Folder is Null", "Picker Cancelled or Folder not Accessed Properly.", "OK");
        }
    }
}