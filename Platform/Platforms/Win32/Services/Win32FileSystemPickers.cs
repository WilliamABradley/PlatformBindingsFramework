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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ookii.Dialogs.Wpf;
using PlatformBindings.Common;
using PlatformBindings.Models.FileSystem;

namespace PlatformBindings.Services
{
    public class Win32FileSystemPickers : FileSystemPickers
    {
        public override bool SupportsPickFile => true;

        public override bool SupportsPickFolder => true;

        public override bool SupportsSaveFile => true;

        public override async Task<FileContainer> PickFile(FilePickerProperties Properties)
        {
            var result = await OpenPicker(Properties);
            if (result != null)
            {
                return (FileContainer)result;
            }

            return null;
        }

        public override async Task<IReadOnlyList<FileContainer>> PickFiles(FilePickerProperties Properties)
        {
            var result = await OpenPicker(Properties, true);
            if (result != null)
            {
                return (List<FileContainer>)result;
            }

            return null;
        }

        private Task<object> OpenPicker(FilePickerProperties Properties, bool Multi = false)
        {
            TaskCompletionSource<object> pickerresult = new TaskCompletionSource<object>();

            PlatformBindingHelpers.OnUIThread(() =>
            {
                using (var dialog = new OpenFileDialog())
                {
                    dialog.CheckFileExists = true;
                    dialog.Multiselect = Multi;

                    ConfigureDialog(dialog, Properties);

                    object fileresult = null;
                    var result = dialog.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        if (Multi)
                        {
                            if (dialog.FileNames != null)
                            {
                                fileresult = dialog.FileNames
                                    .Select(item => (FileContainer)new CoreFileContainer(item))
                                    .ToList();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(dialog.FileName))
                            {
                                fileresult = new CoreFileContainer(dialog.FileName);
                            }
                        }
                    }

                    pickerresult.TrySetResult(fileresult);
                }
            });

            return pickerresult.Task;
        }

        public override Task<FolderContainer> PickFolder(FolderPickerProperties Properties)
        {
            TaskCompletionSource<FolderContainer> pickerresult = new TaskCompletionSource<FolderContainer>();

            PlatformBindingHelpers.OnUIThread(() =>
            {
                var dialog = new VistaFolderBrowserDialog();
                if (Properties != null)
                {
                    if (Properties.StartingLocation != null)
                    {
                        dialog.SelectedPath = AppServices.Current.IO.GetBaseFolder(Properties.StartingLocation.Value).Path;
                    }

                    if (!string.IsNullOrWhiteSpace(Properties.Title))
                    {
                        dialog.Description = Properties.Title;
                    }

                    if (Properties.SuggestedStorageItem != null)
                    {
                        dialog.SelectedPath = Properties.SuggestedStorageItem.Path;
                    }
                }

                FolderContainer container = null;

                var result = dialog.ShowDialog();
                if (result == true)
                {
                    container = new CoreFolderContainer(dialog.SelectedPath);
                }

                pickerresult.TrySetResult(container);
            });

            return pickerresult.Task;
        }

        public override Task<FileContainer> SaveFile(FileSavePickerProperties Properties)
        {
            TaskCompletionSource<FileContainer> pickerresult = new TaskCompletionSource<FileContainer>();

            PlatformBindingHelpers.OnUIThread(() =>
            {
                using (var dialog = new SaveFileDialog())
                {
                    ConfigureDialog(dialog, Properties);
                    dialog.OverwritePrompt = true;
                    dialog.CheckPathExists = true;
                    if (Properties != null)
                    {
                        if (!string.IsNullOrWhiteSpace(Properties.DefaultFileExtension))
                        {
                            dialog.DefaultExt = Properties.DefaultFileExtension;
                        }

                        if (Properties.SuggestedStorageItem != null)
                        {
                            var path = Properties.SuggestedStorageItem.Path;
                            if (File.Exists(path))
                            {
                                dialog.InitialDirectory = Path.GetDirectoryName(path);
                            }

                            dialog.FileName = Path.GetFileName(path);
                        }

                        if (!string.IsNullOrWhiteSpace(Properties.SuggestedName))
                        {
                            dialog.FileName = Properties.SuggestedName;
                        }
                    }

                    FileContainer container = null;

                    var result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        container = new CoreFileContainer(dialog.FileName);
                    }

                    pickerresult.TrySetResult(container);
                }
            });

            return pickerresult.Task;
        }

        private void ConfigureDialog(FileDialog Dialog, FilePickerProperties Properties)
        {
            if (Properties != null)
            {
                if (Properties.FileTypes != null)
                {
                    var types = Properties.FileTypes
                        .Where(ext => ext != "*")
                        .Select(ext => "*" + ext)
                        .ToList();

                    string filters = string.Empty;
                    if (Dialog is OpenFileDialog)
                    {
                        if (Properties.FileTypes.Contains("*"))
                        {
                            filters = "All Files|*.*" + filters;
                        }

                        if (types.Any())
                        {
                            var all = string.Join(";", types);

                            if (!string.IsNullOrWhiteSpace(filters))
                            {
                                filters += "|";
                            }

                            filters += $"{all}|{all}";
                        }
                    }

                    if (types.Count > 1)
                    {
                        foreach (var filter in types)
                        {
                            if (!string.IsNullOrWhiteSpace(filters))
                            {
                                filters += "|";
                            }
                            filters += $"{filter}|{filter}";
                        }
                    }

                    Dialog.Filter = filters;
                }

                if (Properties.StartingLocation != null)
                {
                    Dialog.InitialDirectory = AppServices.Current.IO.GetBaseFolder(Properties.StartingLocation.Value).Path;
                }

                if (!string.IsNullOrWhiteSpace(Properties.Title))
                {
                    Dialog.Title = Properties.Title;
                }
            }
        }
    }
}