using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace PlatformBindings.Models.Settings.Properties
{
    public abstract class PropertyList<T> : ObservableCollection<T>, IProperty where T : IProperty
    {
        public PropertyList([CallerMemberName]string PropertyName = "")
        {
            this.PropertyName = PropertyName;
            CollectionChanged += Values_CollectionChanged;
        }

        public void Attach(ISettingsContainer Parent)
        {
            this.Parent = Parent;
            if (Container == null)
            {
                Container = Parent.GetContainer(PropertyName);
                foreach (var item in Container.GetContainers())
                {
                    var newElement = Activator.CreateInstance<T>();
                    newElement.PropertyName = item.Name;
                    Add(newElement);
                }
            }
        }

        private void Values_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Container != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (IProperty item in e.NewItems)
                        {
                            item.Attach(Container);
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (IProperty item in e.OldItems)
                        {
                            item.Remove();
                        }
                        break;
                }
            }
        }

        public void Remove()
        {
            Parent.RemoveContainer(PropertyName);
        }

        public ISettingsContainer Parent { get; private set; }
        public ISettingsContainer Container { get; private set; }

        public string PropertyName { get; set; }
    }
}