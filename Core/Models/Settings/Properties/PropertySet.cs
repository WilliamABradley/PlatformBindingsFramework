using System.Collections.Generic;

namespace PlatformBindings.Models.Settings.Properties
{
    public abstract class PropertySet : IProperty
    {
        public PropertySet()
        {
        }

        public virtual void Attach(ISettingsContainer Parent)
        {
            this.Parent = Parent;
            if (Container == null)
            {
                Container = Parent.GetContainer(PropertyName);
                foreach (IProperty item in Properties)
                {
                    item.Attach(Container);
                }
                OnAttached();
            }
        }

        public void Remove()
        {
            Parent.RemoveContainer(PropertyName);
            OnRemoved();
        }

        public ISettingsContainer Parent { get; private set; }
        public ISettingsContainer Container { get; private set; }
        public IReadOnlyList<IProperty> Properties { get; set; }

        public string PropertyName { get; set; }

        public virtual void OnAttached()
        {
        }

        public virtual void OnRemoved()
        {
        }
    }
}