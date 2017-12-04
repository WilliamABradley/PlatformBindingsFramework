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