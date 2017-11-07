using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace PlatformBindings.Models.Settings
{
    public class WinSettingsContainer : ISettingsContainer
    {
        public WinSettingsContainer(string ContainerName, WinSettingsContainer Parent)
        {
            this.Parent = Parent;
            Name = ContainerName;
            Container = Parent.Container.CreateContainer(ContainerName, ApplicationDataCreateDisposition.Always);
        }

        internal WinSettingsContainer(ApplicationDataContainer Container, WinSettingsContainer Parent)
        {
            this.Parent = Parent;
            this.Container = Container;
            Name = Parent != null ? Container.Name : Container.Locality.ToString();
        }

        public ISettingsContainer CreateContainer(string ContainerName)
        {
            return new WinSettingsContainer(ContainerName, this);
        }

        public T GetValue<T>(string Key)
        {
            return (T)Container.Values[Key];
        }

        public void SetValue<T>(string Key, T Value)
        {
            Container.Values[Key] = Value;
        }

        public bool ContainsKey(string Key)
        {
            return Container.Values.ContainsKey(Key);
        }

        public Dictionary<string, object> GetValues()
        {
            return Container.Values.ToDictionary(item => item.Key, item => item.Value);
        }

        public IReadOnlyList<ISettingsContainer> GetContainers()
        {
            var children = new List<ISettingsContainer>();
            foreach (var child in Container.Containers)
            {
                children.Add(new WinSettingsContainer(child.Value, this));
            }
            return children;
        }

        public void RemoveContainer(string ContainerName)
        {
            Container.DeleteContainer(ContainerName);
        }

        public void RemoveKey(string Key)
        {
            Container.Values.Remove(Key);
        }

        public void Remove()
        {
            Parent.RemoveContainer(Name);
        }

        public void Clear()
        {
            Container.Values.Clear();
        }

        public string Name { get; private set; }
        public ApplicationDataContainer Container { get; }
        public ISettingsContainer Parent { get; }
    }
}