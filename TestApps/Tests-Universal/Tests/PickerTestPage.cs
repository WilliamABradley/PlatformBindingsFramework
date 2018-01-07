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

using PlatformBindings;
using PlatformBindings.Enums;
using PlatformBindings.Exceptions;
using PlatformBindings.Models.FileSystem;
using System;
using System.Threading.Tasks;
using Tests.TestGenerator;

namespace Tests.Tests
{
    public class PickerTestPage : TestPage
    {
        public PickerTestPage(ITestPageGenerator PageGenerator) : base("Picker Tests", PageGenerator)
        {
            AddTestItem(new TestTask
            {
                Name = "Pick File",
                Test = context => Task.Run(async () =>
                {
                    var file = await AppServices.Current.IO.Pickers.PickFile(null);
                    if (file != null)
                    {
                        var result = await AppServices.Current.UI.PromptUserAsync("File", file.Path, "Open", "Close", null);
                        if (result == DialogResult.Primary)
                        {
                            try
                            {
                                await AppServices.Current.IO.OpenFileForDisplay(file);
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

            AddTestItem(new TestTask
            {
                Name = "Pick Files",
                Test = context => Task.Run(async () =>
                {
                    var files = await AppServices.Current.IO.Pickers.PickFiles(null);
                    if (files != null)
                    {
                        string fileList = "";
                        foreach (var file in files)
                        {
                            fileList += file.Path + Environment.NewLine;
                        }

                        var result = await AppServices.Current.UI.PromptUserAsync("Files", fileList, "Open", "Close", null);
                        if (result == DialogResult.Primary)
                        {
                            foreach (var file in files)
                            {
                                try
                                {
                                    await AppServices.Current.IO.OpenFileForDisplay(file);
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

            AddTestItem(new TestTask
            {
                Name = "Pick Folder",
                Test = context => Task.Run(async () =>
                {
                    var folder = await AppServices.Current.IO.Pickers.PickFolder(null);
                    if (folder != null)
                    {
                        var result = await AppServices.Current.UI.PromptUserAsync("Folder", folder.Path, "Open", "Close", null);
                        if (result == DialogResult.Primary)
                        {
                            await AppServices.Current.IO.OpenFolderForDisplay(folder, null);
                        }
                        return null;
                    }
                    else return PickerCancelled;
                })
            });

            AddTestItem(new TestTask
            {
                Name = "Save File W/ Ext Filters",
                Test = context => Task.Run(async () =>
                {
                    var props = new FileSavePickerProperties();
                    props.FileTypes.Add("*");
                    props.FileTypes.Add(".mkv");
                    props.FileTypes.Add(".wmv");
                    props.StartingLocation = PathRoot.Application;

                    var result = await AppServices.Current.IO.Pickers.SaveFile(props);

                    return result?.Path;
                })
            });

            AddTestItem(new TestTask
            {
                Name = "Open File W/ Ext Filters",
                Test = context => Task.Run(async () =>
                {
                    var props = new FilePickerProperties();
                    props.FileTypes.Add("*");
                    props.FileTypes.Add(".mkv");
                    props.FileTypes.Add(".wmv");
                    props.StartingLocation = PathRoot.Application;

                    var result = await AppServices.Current.IO.Pickers.PickFile(props);

                    return result?.Path;
                })
            });
        }

        private string PickerCancelled => "Picker Cancelled";
    }
}