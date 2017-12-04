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