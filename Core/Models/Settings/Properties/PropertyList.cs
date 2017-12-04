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