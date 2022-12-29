using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using RedSharp.Sys.Collections.Abstracts;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections
{
    public class WrappingCollection<TInput, TOutput> : NotifyingWrapperBase<TInput, TOutput>, IReadOnlyCollection<TOutput>
    {
        private class WrappingCollectionEnumerator : IEnumerator<TOutput>
        {
            private List<WrappedItem> _collection;
            private int _index;

            public WrappingCollectionEnumerator(List<WrappedItem> items)
            {
                _collection = items;

                Reset();
            }

            object IEnumerator.Current => Current;

            public TOutput Current { get; private set; }

            public bool MoveNext()
            {
                _index++;

                if (_index >= _collection.Count)
                    return false;

                Current = _collection[_index].Output;

                return true;
            }

            public void Reset()
            {
                Current = default;

                _index = -1;
            }

            public void Dispose()
            {
                Reset();
            }
        }

        private struct WrappedItem
        {
            public TInput Input;
            public TOutput Output;

            public override bool Equals(object obj)
            {
                return Input.Equals(obj);
            }

            public override int GetHashCode()
            {
                return Input.GetHashCode();
            }

            public override string ToString()
            {
                return Input.ToString();
            }
        }

        private List<WrappedItem> _collection;
        private List<TOutput> _notificationCollection;
        private Func<TInput, TOutput> _wrapperCallback;

        public WrappingCollection(IEnumerable<TInput> collection, Func<TInput, TOutput> wrapperCallback)
        {
            ArgumentsGuard.ThrowIfNull(collection);
            ArgumentsGuard.ThrowIfNull(wrapperCallback);
            ArgumentsGuard.ThrowIfNotType(collection, out INotifyCollectionChanged notifier);

            _wrapperCallback = wrapperCallback;

            _notificationCollection = new List<TOutput>();
            _collection = new List<WrappedItem>();

            AssociateCollection(notifier);
        }

        public int Count => _collection.Count;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new WrappingCollectionEnumerator(_collection);
        }

        public IEnumerator<TOutput> GetEnumerator()
        {
            return new WrappingCollectionEnumerator(_collection);
        }

        private int IndexOf(TInput item)
        {
            var dummy = new WrappedItem()
            {
                Input = item
            };

            return _collection.IndexOf(dummy);
        }

        protected override void InternalAddItems(IEnumerable<TInput> items)
        {
            foreach (var item in items)
            {
                var wrappedItem = new WrappedItem()
                {
                    Input = item,
                    Output = _wrapperCallback.Invoke(item)
                };

                _collection.Add(wrappedItem); 
                _notificationCollection.Add(wrappedItem.Output);
            }

            RaiseAdding(_notificationCollection);

            _notificationCollection.Clear();
        }

        protected override void InternalRemoveItems(IEnumerable<TInput> items)
        {
            foreach (var item in items)
            {
                var index = IndexOf(item);

                if (index == -1)
                    continue;

                var wrappedItem = _collection[index];

                _collection.RemoveAt(index);

                _notificationCollection.Add(wrappedItem.Output);

                RaiseRemoving(_notificationCollection, index);

                _notificationCollection.Clear();
            }
        }

        protected override void InternalClearItems()
        {
            _collection.Clear();

            RaiseClearing();
        }
    }
}
