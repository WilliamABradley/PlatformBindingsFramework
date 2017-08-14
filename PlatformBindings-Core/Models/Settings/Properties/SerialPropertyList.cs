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
            var listContainer = Parent.CreateContainer(PropertyName);
            foreach (var item in listContainer.GetValues().OrderBy(item => Convert.ToInt32(item.Key)))
            {
                Add(JsonConvert.DeserializeObject<T>((string)item.Value));
            }
        }

        public void Update()
        {
            var listContainer = Parent.CreateContainer(PropertyName);
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