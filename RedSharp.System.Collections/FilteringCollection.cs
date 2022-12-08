using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using RedSharp.Sys.Collections.Abstracts;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections
{
    public class FilteringCollection<TItem, TFilter> : NotifiableWrapperBase<TItem, TItem>, IReadOnlyCollection<TItem>
    {
        private class FilteringCollectionEnumerator : IEnumerator<TItem>
        {
            private List<FilteredItem> _collection;
            private int _index;

            public FilteringCollectionEnumerator(List<FilteredItem> items)
            {
                _collection = items;

                Reset();
            }

            object IEnumerator.Current => Current;

            public TItem Current { get; private set; }

            public bool MoveNext()
            {
                _index++;

                for (; _index < _collection.Count; _index++)
                {
                    var item = _collection[_index];

                    if (item.IsMatch)
                    {
                        Current = item.Item;

                        return true;
                    }
                }

                return false;
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

        private struct FilteredItem
        {
            public TItem Item;
            public bool IsMatch;

            public override bool Equals(object obj)
            {
                return Item.Equals(obj);
            }

            public override int GetHashCode()
            {
                return Item.GetHashCode();
            }

            public override string ToString()
            {
                return Item.ToString();
            }
        }

        private List<FilteredItem> _collection;

        private TFilter _filter;
        private Func<TItem, TFilter, bool> _filterCallback;

        private int _fakeCount;

        private List<TItem> _notificationCollection;

        public FilteringCollection(IEnumerable<TItem> collection, Func<TItem, TFilter, bool> filterCallback, TFilter initialFilter)
        {
            ArgumentsGuard.ThrowIfNull(collection);
            ArgumentsGuard.ThrowIfNull(filterCallback);
            ArgumentsGuard.ThrowIfNotType(collection, out INotifyCollectionChanged notifier);

            _filterCallback = filterCallback;
            _filter = initialFilter;

            _notificationCollection = new List<TItem>();
            _collection = new List<FilteredItem>();

            AssociateCollection(notifier);
        }

        public TFilter Filter
        {
            get => _filter;
            set
            {
                if (EqualityComparer<TFilter>.Default.Equals(_filter, value))
                    return;

                _filter = value;

                ForceFiltering();
            }
        }

        public int Count => _fakeCount;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FilteringCollectionEnumerator(_collection);
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return new FilteringCollectionEnumerator(_collection);
        }

        public void ForceFiltering()
        {
            var fakeIndex = 0;

            for (int i = 0; i < _collection.Count; i++)
            {
                var filteredItem = _collection[i];

                var oldResult = filteredItem.IsMatch;
                var newResult = _filterCallback.Invoke(filteredItem.Item, _filter);

                //if item was filtered as true move the fake index
                if (oldResult)
                    fakeIndex++;

                //result didn't change
                if (oldResult == newResult)
                    continue;

                filteredItem = new FilteredItem()
                {
                    Item = filteredItem.Item,
                    IsMatch = newResult
                };

                _collection[i] = filteredItem;

                if (!oldResult && newResult)
                {
                    //was false became true

                    fakeIndex++;

                    _notificationCollection.Add(filteredItem.Item);

                    RaiseAdding(_notificationCollection, fakeIndex);

                    _notificationCollection.Clear();
                }
                else if (oldResult && !newResult)
                {
                    //was true became false

                    _notificationCollection.Add(filteredItem.Item);

                    RaiseRemoving(_notificationCollection, fakeIndex);

                    _notificationCollection.Clear();

                    fakeIndex--;
                }
            }
        }

        private int IndexOf(TItem item)
        {
            var dummy = new FilteredItem()
            {
                Item = item
            };

            return _collection.IndexOf(dummy);
        }

        private int FakeIndexOf(int realIndex)
        {
            var fakeIndex = 0;

            for (int i = 0; i <= realIndex; i++)
                if (_collection[i].IsMatch)
                    fakeIndex++;

            return fakeIndex;
        }

        protected override void InternalAddItems(IEnumerable<TItem> items)
        {
            foreach(var item in items)
            {
                var filteredItem = new FilteredItem()
                {
                    Item = item,
                    IsMatch = _filterCallback.Invoke(item, _filter)
                };

                _collection.Add(filteredItem);

                if (filteredItem.IsMatch)
                {
                    _notificationCollection.Add(item);
                    
                    _fakeCount++;
                }
            }

            RaiseAdding(_notificationCollection);

            _notificationCollection.Clear();
        }

        protected override void InternalRemoveItems(IEnumerable<TItem> items)
        {
            foreach(var item in items)
            {
                var index = IndexOf(item);

                if (index == -1)
                    continue;

                var filteredItem = _collection[index];

                if (filteredItem.IsMatch)
                {
                    _collection.RemoveAt(index);
                }
                else
                {
                    var fakeIndex = FakeIndexOf(index);

                    _collection.RemoveAt(index);

                    _notificationCollection.Add(item);

                    RaiseRemoving(_notificationCollection, fakeIndex);

                    _notificationCollection.Clear();
                }
            }
        }

        protected override void InternalClearItems()
        {
            _collection.Clear();
            _fakeCount = 0;

            RaiseClearing();
        }
    }
}
