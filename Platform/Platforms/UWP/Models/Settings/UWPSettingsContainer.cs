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
using System.Linq;
using Windows.Storage;

namespace PlatformBindings.Models.Settings
{
    public class UWPSettingsContainer : ISettingsContainer
    {
        public UWPSettingsContainer(string ContainerName, UWPSettingsContainer Parent)
        {
            this.Parent = Parent;
            Name = ContainerName;
            Container = Parent.Container.CreateContainer(ContainerName, ApplicationDataCreateDisposition.Always);
        }

        internal UWPSettingsContainer(ApplicationDataContainer Container, UWPSettingsContainer Parent)
        {
            this.Parent = Parent;
            this.Container = Container;
            Name = Parent != null ? Container.Name : Container.Locality.ToString();
        }

        public ISettingsContainer GetContainer(string ContainerName)
        {
            return new UWPSettingsContainer(ContainerName, this);
        }

        public T GetValue<T>(string Key)
        {
            var value = Container.Values[Key];
            if (!(value is T))
            {
                return default(T);
            }
            else return (T)value;
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
                children.Add(new UWPSettingsContainer(child.Value, this));
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