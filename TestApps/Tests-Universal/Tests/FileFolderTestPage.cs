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
using PlatformBindings.Models.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.TestGenerator;

namespace Tests.Tests
{
    public class FileFolderTestPage : TestPage
    {
        public FileFolderTestPage(ITestPageGenerator PageGenerator) : base("File/Folder Tests", PageGenerator)
        {
            AddTestItem(new TestTask
            {
                Name = "Get Directory Contents",
                Test = context => Task.Run(async () =>
               {
                   var folder = await AppServices.Current.IO.Pickers.PickFolder();
                   if (folder != null)
                   {
                       var items = await folder.GetItemsAsync();
                       var result = "Items: \n";
                       result += items.Any() ? string.Join(Environment.NewLine, items.Select(item => $"{item.Name} - {item.GetType().Name}")) : "No Items in Folder";
                       return result;
                   }
                   return "Folder is Null, Picker Cancelled or Folder not Accessed Properly.";
               })
            });

            AddTestItem(new TestTask
            {
                Name = "Get Sub Folders Sub Folders",
                Test = context => Task.Run(async () =>
                {
                    var folder = await AppServices.Current.IO.Pickers.PickFolder();
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
                            var result = "SubSubFolders: \n";
                            result += SubSubFolders.Count > 0 ? string.Join(Environment.NewLine, SubSubFolders.Select(item => item.Name)) : "No Subsubfolders";
                            return result;
                        }
                        return "No SubFolders";
                    }

                    return "Folder is Null, Picker Cancelled or Folder not Accessed Properly.";
                })
            });

            AddTestItem(new TestTask
            {
                Name = "Create Test Text File",
                Test = context => Task.Run(async () =>
                {
                    var folder = await AppServices.Current.IO.Pickers.PickFolder();
                    if (folder != null)
                    {
                        var file = await folder.CreateFileAsync("TestFile.txt");
                        if (file != null)
                        {
                            await file.SaveText("This is a Test File!");
                        }
                        return $"File Creation:" + file != null ? "Success" : "Failed";
                    }
                    return "Folder is Null, Picker Cancelled or Folder not Accessed Properly.";
                })
            });

            AddTestItem(new TestTask
            {
                Name = "Read Text From File",
                Test = context => Task.Run(async () =>
                {
                    var props = new FilePickerProperties { };
                    props.FileTypes.Add(".txt");
                    var file = await AppServices.Current.IO.Pickers.PickFile(props);
                    if (file != null)
                    {
                        var content = await file.ReadFileAsText();
                        return $"{file.Name} contents: \n" + content;
                    }
                    return "File is Null, Picker Cancelled or File not Accessed Properly.";
                })
            });
        }
    }
}