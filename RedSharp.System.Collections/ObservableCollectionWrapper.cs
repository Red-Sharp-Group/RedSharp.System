using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RedSharp.Sys.Collections.Abstracts;
using RedSharp.Sys.Collections.Interfaces;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections
{
    /// <summary>
    /// A simple wrapper that implements <see cref="IObservableCollection{TItem}"/>.
    /// </summary>
    /// <remarks>
    /// I have to warn you that this is a wrapper object with an additional functionality,
    /// so it may require more actions to do the same things.
    /// </remarks>
    public class ObservableCollectionWrapper<TItem> : NotifiableCollectionBase<TItem>, IObservableCollection<TItem>
    {
        private ICollection<TItem> _internalCollection;

        public ObservableCollectionWrapper(ICollection<TItem> internalCollection)
        {
            ArgumentsGuard.ThrowIfNull(internalCollection);

            _internalCollection = internalCollection;
        }

        /// <inheritdoc/>
        public bool IsReadOnly => _internalCollection.IsReadOnly;

        /// <inheritdoc/>
        /// <remarks>
        /// Can be observed.
        /// </remarks>
        public int Count => _internalCollection.Count;

        /// <inheritdoc/>
        public void Add(TItem item)
        {
            ThrowIfReadOnly();

            RaisePropertyChanging(nameof(Count));

            _internalCollection.Add(item);

            RaisePropertyChanged(nameof(Count));
            RaiseAdding(item);
        }

        /// <inheritdoc/>
        public bool Contains(TItem item)
        {
            return _internalCollection.Contains(item);
        }

        /// <inheritdoc/>
        public bool Remove(TItem item)
        {
            ThrowIfReadOnly();

            if (!_internalCollection.Contains(item))
                return false;

            RaisePropertyChanging(nameof(Count));

            _internalCollection.Remove(item);

            RaisePropertyChanged(nameof(Count));
            RaiseRemoving(item);

            return true;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            ThrowIfReadOnly();

            if (_internalCollection.Count == 0)
                return;

            RaisePropertyChanging(nameof(Count));

            _internalCollection.Clear();

            RaisePropertyChanged(nameof(Count));
            RaiseClearing();
        }

        /// <inheritdoc/>
        public IEnumerator<TItem> GetEnumerator()
        {
            return _internalCollection.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalCollection.GetEnumerator();
        }

        /// <inheritdoc/>
        public void CopyTo(TItem[] array, int arrayIndex)
        {
            _internalCollection.CopyTo(array, arrayIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfReadOnly()
        {
            if (IsReadOnly)
                throw new NotSupportedException("This is read-only collection.");
        }
    }
}
