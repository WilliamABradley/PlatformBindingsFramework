using System;
using System.Runtime.CompilerServices;

namespace PlatformBindings.Models.Settings.Properties
{
    public class Property<T> : IProperty
    {
        public Property([CallerMemberName]string PropertyName = "", T Default = default(T))
        {
            this.PropertyName = PropertyName;
            this.Default = Default;
        }

        public void Attach(ISettingsContainer Parent)
        {
            this.Parent = Parent;
            if (Temp != null) Value = (T)Temp;
        }

        internal bool IsPropertyAttached()
        {
            try
            {
                return Parent != null && Parent.ContainsKey(PropertyName);
            }
            catch { return false; }
        }

        public override string ToString()
        {
            return Value?.ToString();
        }

        public void Remove()
        {
            if (IsPropertyAttached())
            {
                Parent.RemoveKey(PropertyName);
            }
        }

        public string PropertyName { get; set; }
        public ISettingsContainer Parent { get; private set; }

        private T Default;
        private object Temp;

        public T Value
        {
            get
            {
                if (!IsPropertyAttached()) return Temp != null ? (T)Temp : Default;
                else return Parent.GetValue<T>(PropertyName);
            }
            set
            {
                if (Parent != null)
                {
                    Parent.SetValue(PropertyName, value);
                    ValueUpdated?.Invoke(this, null);
                }
                else Temp = value;
            }
        }

        public event EventHandler ValueUpdated;
    }
}