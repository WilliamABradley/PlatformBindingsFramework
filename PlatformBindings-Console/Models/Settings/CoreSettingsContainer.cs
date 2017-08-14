using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PlatformBindings.Models.Settings;

namespace PlatformBindings.ConsoleTools
{
    public class CoreSettingsContainer : ISettingsContainer
    {
        public CoreSettingsContainer(string Name, CoreSettingsContainer Parent)
        {
            this.Parent = Parent;
            this.Name = Name;
            CreateDirectory();
        }

        internal CoreSettingsContainer(DirectoryInfo Directory, CoreSettingsContainer Parent)
        {
            this.Directory = Directory;
            this.Parent = Parent;
            Name = Directory.Name;
        }

        private void CreateDirectory()
        {
            var parent = Parent as CoreSettingsContainer;
            Directory = parent.Directory.CreateSubdirectory(Name);
        }

        public void Clear()
        {
            Directory.Delete(true);
            CreateDirectory();
        }

        public bool ContainsKey(string Key)
        {
            var files = Directory.GetFiles(Key);
            return files.FirstOrDefault(item => item.Name == Key) != null;
        }

        public ISettingsContainer CreateContainer(string ContainerName)
        {
            return new CoreSettingsContainer(ContainerName, this);
        }

        public void RemoveContainer(string ContainerName)
        {
            var folders = Directory.GetDirectories(ContainerName);
            var folder = folders.FirstOrDefault(item => item.Name == ContainerName);
            folder?.Delete();
        }

        public IReadOnlyList<ISettingsContainer> GetContainers()
        {
            List<ISettingsContainer> Directories = new List<ISettingsContainer>();
            var folders = Directory.GetDirectories();
            foreach (var folder in folders)
            {
                Directories.Add(new CoreSettingsContainer(folder, this));
            }
            return Directories;
        }

        public FileInfo GetFile(string Key)
        {
            var files = Directory.GetFiles(Key);
            return files.FirstOrDefault(item => item.Name == Key);
        }

        public string GetFileText(FileInfo File)
        {
            using (var reader = File.OpenText())
            {
                return reader.ReadToEnd();
            }
        }

        public void SetValue<T>(string Key, T Value)
        {
            string raw = Value.GetType() == typeof(string) ? Value as string : JsonConvert.SerializeObject(Value);

            using (var writer = File.CreateText($"{Directory.FullName}\\{Key}"))
            {
                writer.Write(raw);
            }
        }

        public T GetValue<T>(string Key)
        {
            var file = GetFile(Key);
            var raw = GetFileText(file);
            return typeof(T) == typeof(string) ? (T)(object)raw : JsonConvert.DeserializeObject<T>(raw);
        }

        public Dictionary<string, object> GetValues()
        {
            Dictionary<string, object> Vals = new Dictionary<string, object>();
            var files = Directory.GetFiles();
            foreach (var file in files)
            {
                var raw = GetFileText(file);
                Vals.Add(file.Name, raw);
            }
            return Vals;
        }

        public void RemoveKey(string Key)
        {
            var file = GetFile(Key);
            file?.Delete();
        }

        public void Remove()
        {
            Directory.Delete(true);
        }

        public string Name { get; }
        public ISettingsContainer Parent { get; }
        public DirectoryInfo Directory { get; set; }
    }
}