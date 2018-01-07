using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ookii.Dialogs.Wpf;
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
            using (var dialog = new OpenFileDialog())
            {
                object pickerresult = null;
                dialog.CheckFileExists = true;
                dialog.Multiselect = Multi;

                ConfigureDialog(dialog, Properties);

                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (Multi)
                    {
                        if (dialog.FileNames != null)
                        {
                            pickerresult = dialog.FileNames
                                .Select(item => new CoreFileContainer(item))
                                .ToList();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(dialog.FileName))
                        {
                            pickerresult = new CoreFileContainer(dialog.FileName);
                        }
                    }
                }

                return Task.FromResult(pickerresult);
            }
        }

        public override Task<FolderContainer> PickFolder(FolderPickerProperties Properties)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (Properties != null)
            {
                if (Properties.StartingLocation != null)
                {
                    dialog.SelectedPath = AppServices.Current.IO.GetBaseFolder(Properties.StartingLocation.Value).Path;
                }
            }

            FolderContainer pickerresult = null;

            var result = dialog.ShowDialog();
            if (result == true)
            {
                pickerresult = new CoreFolderContainer(dialog.SelectedPath);
            }

            return Task.FromResult(pickerresult);
        }

        public override Task<FileContainer> SaveFile(FileSavePickerProperties Properties)
        {
            using (var dialog = new SaveFileDialog())
            {
                ConfigureDialog(dialog, Properties);
                if (Properties != null)
                {
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

                return Task.FromResult(container);
            }
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
            }
        }
    }
}