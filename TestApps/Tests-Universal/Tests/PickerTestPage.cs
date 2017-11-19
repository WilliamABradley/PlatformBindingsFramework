using PlatformBindings;
using PlatformBindings.Enums;
using PlatformBindings.Exceptions;
using System;
using System.Threading.Tasks;
using Tests.TestGenerator;

namespace Tests.Tests
{
    public class PickerTestPage : TestPage
    {
        public PickerTestPage(ITestPageGenerator PageGenerator) : base(PageGenerator)
        {
            AddTest(new TestTask
            {
                Name = "Pick File",
                Test = context => Task.Run(async () =>
                {
                    var file = await AppServices.IO.Pickers.PickFile(null);
                    if (file != null)
                    {
                        var result = await AppServices.UI.PromptUserAsync("File", file.Path, "Open", "Close", null);
                        if (result == DialogResult.Primary)
                        {
                            try
                            {
                                await AppServices.IO.OpenFileForDisplay(file);
                            }
                            catch (DefaultAppNotFoundException)
                            {
                                return "No App available that supports this file format.";
                            }
                        }
                        return null;
                    }
                    else return PickerCancelled;
                })
            });

            AddTest(new TestTask
            {
                Name = "Pick Files",
                Test = context => Task.Run(async () =>
                {
                    var files = await AppServices.IO.Pickers.PickFiles(null);
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
                                try
                                {
                                    await AppServices.IO.OpenFileForDisplay(file);
                                }
                                catch (DefaultAppNotFoundException)
                                {
                                    return "No App available that supports this file format.";
                                }
                            }
                        }
                        return null;
                    }
                    else return PickerCancelled;
                })
            });

            AddTest(new TestTask
            {
                Name = "Pick Folder",
                Test = context => Task.Run(async () =>
                {
                    var folder = await AppServices.IO.Pickers.PickFolder(null);
                    if (folder != null)
                    {
                        var result = await AppServices.UI.PromptUserAsync("Folder", folder.Path, "Open", "Close", null);
                        if (result == DialogResult.Primary)
                        {
                            await AppServices.IO.OpenFolderForDisplay(folder, null);
                        }
                        return null;
                    }
                    else return PickerCancelled;
                })
            });
        }

        private string PickerCancelled => "Picker Cancelled";
    }
}