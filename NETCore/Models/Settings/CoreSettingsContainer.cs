using Newtonsoft.Json;
using PlatformBindings.Models.FileSystem;
using System.Collections.Generic;

namespace PlatformBindings.Models.Settings
{
    public class CoreSettingsContainer : ISettingsContainer
    {
        public CoreSettingsContainer(string Name, CoreSettingsContainer Parent)
        {
            this.Parent = Parent;
            this.Name = Name;
            CreateDirectory();
        }

        internal CoreSettingsContainer(CoreFolderContainer Directory, CoreSettingsContainer Parent)
        {
            this.Directory = Directory;
            this.Parent = Parent;
            Name = Directory.Name;
        }

        private void CreateDirectory()
        {
            var parent = Parent as CoreSettingsContainer;
            Directory = parent.Directory.CreateFolderAsync(Name).Result as CoreFolderContainer;
        }

        public void Clear()
        {
            Directory.DeleteAsync().RunSynchronously();
            CreateDirectory();
        }

        public bool ContainsKey(string Key)
        {
            var file = Directory.GetFileAsync(Key).Result;
            return file != null;
        }

        public ISettingsContainer CreateContainer(string ContainerName)
        {
            return new CoreSettingsContainer(ContainerName, this);
        }

        public async void RemoveContainer(string ContainerName)
        {
            try
            {
                var folder = Directory.GetFolderAsync(ContainerName).Result;
                await folder?.DeleteAsync();
            }
            catch { }
        }

        public IReadOnlyList<ISettingsContainer> GetContainers()
        {
            List<ISettingsContainer> Directories = new List<ISettingsContainer>();
            var folders = Directory.GetFoldersAsync().Result;
            foreach (CoreFolderContainer folder in folders)
            {
                Directories.Add(new CoreSettingsContainer(folder, this));
            }
            return Directories;
        }

        public CoreFileContainer GetFile(string Key)
        {
            try
            {
                return Directory.GetFileAsync(Key).Result as CoreFileContainer;
            }
            catch { return null; }
        }

        public string GetFileText(CoreFileContainer File)
        {
            return File.ReadFileAsText().Result;
        }

        public void SetValue<T>(string Key, T Value)
        {
            string raw = Value.GetType() == typeof(string) ? Value as string : JsonConvert.SerializeObject(Value);

            var file = Directory.CreateFileAsync(Key).Result;
            file.SaveText(raw).RunSynchronously();
        }

        public T GetValue<T>(string Key)
        {
            var file = GetFile(Key);
            if (file != null)
            {
                var raw = GetFileText(file);
                return typeof(T) == typeof(string) ? (T)(object)raw : JsonConvert.DeserializeObject<T>(raw);
            }
            return (T)(object)null;
        }

        public Dictionary<string, object> GetValues()
        {
            Dictionary<string, object> Vals = new Dictionary<string, object>();
            var files = Directory.GetFilesAsync().Result;
            foreach (CoreFileContainer file in files)
            {
                var raw = GetFileText(file);
                Vals.Add(file.Name, raw);
            }
            return Vals;
        }

        public async void RemoveKey(string Key)
        {
            var file = GetFile(Key);
            await file?.DeleteAsync();
        }

        public void Remove()
        {
            Directory.DeleteAsync().RunSynchronously();
        }

        public string Name { get; }
        public ISettingsContainer Parent { get; }
        public CoreFolderContainer Directory { get; set; }
    }
}