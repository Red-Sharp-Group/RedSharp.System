using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using RedSharp.Sys.Collections.Abstracts;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections
{
    public class SortingCollection<TItem> : NotifiableWrapperBase<TItem, TItem>, IReadOnlyCollection<TItem>
    {
        private class ItemBuiltInComparer<TImplementation> : ComparerBase<TImplementation>
        {
            protected override int InternalCompare(TImplementation first, TImplementation second)
            {
                //Parameters:
                //  x: The first object to compare.
                //  y: The second object to compare.
                //
                //Returns:
                //    Less than zero – x is less than y.
                //    Zero – x equals y.
                //    Greater than zero – x is greater than y.

                if (first != null)
                    return  ((IComparable<TImplementation>)first).CompareTo(second);
                else if (second != null)
                    return  ((IComparable<TImplementation>)second).CompareTo(first) * (-1);
                else
                    return 0;
            }
        }

        private LinkedList<TItem> _collection;
        private List<TItem> _notificationCollection;
        private IComparer<TItem> _comparer;

        public SortingCollection(IEnumerable<TItem> collection) 
            : this(collection, ReturnBuiltInComparer<TItem>())
        { }

        public SortingCollection(IEnumerable<TItem> collection, IComparer<TItem> comparer)
        {
            ArgumentsGuard.ThrowIfNull(collection);
            ArgumentsGuard.ThrowIfNull(comparer);
            ArgumentsGuard.ThrowIfNotType(collection, out INotifyCollectionChanged notifier);

            _collection = new LinkedList<TItem>();
            _notificationCollection = new List<TItem>();
            _comparer = comparer;

            AssociateCollection(notifier);
        }

        public int Count => _collection.Count;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IComparer<TTempItem> ReturnBuiltInComparer<TTempItem>()
        {
            if (typeof(TTempItem).GetInterface(nameof(IComparable<TTempItem>)) == null)
                throw new Exception("If the given type is not implemented IComparable<T> interface you have to pass it with comparer.");

            return new ItemBuiltInComparer<TTempItem>();
        }

        protected override void InternalAddItems(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                if(_collection.Count == 0)
                {
                    _collection.AddFirst(item);

                    _notificationCollection.Add(item);

                    RaiseAdding(item, 0);

                    _notificationCollection.Clear();
                }
                else
                {
                    var node = _collection.First;

                    //Parameters:
                    //  x: The first object to compare.
                    //  y: The second object to compare.
                    //
                    //Returns:
                    //    Less than zero – x is less than y.
                    //    Zero – x equals y.
                    //    Greater than zero – x is greater than y.

                    var result = _comparer.Compare(item, node.Value);
                    var index = 0;

                    if (result <= 0)
                    {
                        _collection.AddBefore(node, new LinkedListNode<TItem>(item));
                        _notificationCollection.Add(item);

                        RaiseAdding(item, index);

                        _notificationCollection.Clear();
                    }
                    else
                    {
                        index++;

                        while (true)
                        {
                            if (node.Next == null)
                                break;

                            result = _comparer.Compare(item, node.Next.Value);

                            if (result <= 0)
                                break;

                            node = node.Next;
                            
                            index++;
                        }

                        _collection.AddAfter(node, new LinkedListNode<TItem>(item));
                        _notificationCollection.Add(item);

                        RaiseAdding(item, index);

                        _notificationCollection.Clear();
                    }
                }
            }
        }

        protected override void InternalRemoveItems(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                var node = _collection.First;
                var index = 0;

                while (node != null && !object.Equals(node.Value, item))
                {
                    node = node.Next;
                    index++;
                }

                if (node == null)
                    continue;

                _collection.Remove(node);

                _notificationCollection.Add(item);

                RaiseRemoving(item, index);

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
