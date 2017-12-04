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

using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PlatformBindings.Models.Settings.Properties
{
    public class SerialPropertyList<T> : ObservableCollection<T>, IProperty
    {
        public SerialPropertyList([CallerMemberName]string PropertyName = "")
        {
            this.PropertyName = PropertyName;
        }

        public void Attach(ISettingsContainer Parent)
        {
            this.Parent = Parent;
            LoadList();
            CollectionChanged += _Value_CollectionChanged;
        }

        private void LoadList()
        {
            Clear();
            var listContainer = Parent.GetContainer(PropertyName);
            foreach (var item in listContainer.GetValues().OrderBy(item => Convert.ToInt32(item.Key)))
            {
                Add(JsonConvert.DeserializeObject<T>((string)item.Value));
            }
        }

        public void Update()
        {
            var listContainer = Parent.GetContainer(PropertyName);
            listContainer.Clear();
            foreach (var value in this)
            {
                var index = IndexOf(value);
                listContainer.SetValue(index.ToString(), JsonConvert.SerializeObject(value));
            }
            ValueUpdated?.Invoke(this, null);
        }

        public void Remove()
        {
            if (Parent != null)
            {
                Parent.RemoveContainer(PropertyName);
            }
        }

        private void _Value_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Update();
        }

        public ISettingsContainer Parent { get; set; }
        public string PropertyName { get; set; }

        public event EventHandler ValueUpdated;
    }
}