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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using Windows.UI.Core;

namespace PlatformBindings.Models.IncrementalLoading
{
    public class UWPListSupportsIncrementalLoading : UWPSupportsIncrementalLoading, IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public UWPListSupportsIncrementalLoading(ISupportCoreIncrementalLoading Source) : base(Source)
        {
            Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

            if (Source is INotifyPropertyChanged propChange)
            {
                propChange.PropertyChanged += PropChange_PropertyChanged;
            }
            if (Source is INotifyCollectionChanged collectChange)
            {
                collectChange.CollectionChanged += CollectChange_CollectionChanged; ;
            }
        }

        private void PropChange_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        private async void CollectChange_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CollectionChanged?.Invoke(sender, e);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int Add(object value)
        {
            return _Source.Add(value);
        }

        public void Clear()
        {
            _Source.Clear();
        }

        public bool Contains(object value)
        {
            return _Source.Contains(value);
        }

        public int IndexOf(object value)
        {
            return _Source.IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            _Source.Insert(index, value);
        }

        public void Remove(object value)
        {
            _Source.Remove(value);
        }

        public void RemoveAt(int index)
        {
            _Source.RemoveAt(index);
        }

        public void CopyTo(Array array, int index)
        {
            _Source.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return _Source.GetEnumerator();
        }

        private IList _Source { get { return Source as IList; } }

        public bool IsFixedSize => _Source.IsFixedSize;

        public bool IsReadOnly => _Source.IsReadOnly;

        public int Count => _Source.Count;

        public bool IsSynchronized => _Source.IsSynchronized;

        public object SyncRoot => _Source.SyncRoot;

        public object this[int index] { get => _Source[index]; set => _Source[index] = value; }

        public CoreDispatcher Dispatcher { get; }
    }
}