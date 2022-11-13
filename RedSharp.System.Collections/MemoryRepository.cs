using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RedSharp.Sys.Collections.Helpers;
using RedSharp.Sys.Collections.Interfaces;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections
{
    public class MemoryRepository<TIdentifier, TItem> : IRepositoryDictionary<TIdentifier, TItem> where TItem : IRepositoryItem<TIdentifier>
    {
        private IDictionary<TIdentifier, TItem> _collection;

        public MemoryRepository(IDictionary<TIdentifier, TItem> collection, bool isReadOnly = true)
        {
            _collection = collection;
            IsReadOnly = isReadOnly;
        }

        /// <inheritdoc/>
        public bool IsReadOnly { get; private set; }


        /// <inheritdoc/>
        public int GetCount(Expression<Func<TItem, bool>> predicate = null)
        {
            if (predicate == null)
                return _collection.Count;
            else
                return _collection.Values.Count(predicate.Compile());
        }

        /// <inheritdoc/>
        public ValueTask<int> GetCountAsync(Expression<Func<TItem, bool>> predicate = null, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.FromResult(GetCount(predicate));
        }


        /// <inheritdoc/>
        public void Add(TItem item)
        {
            ThrowIfReadOnly();
            ArgumentsGuard.ThrowIfNull(item);

            _collection.Add(item.Identifier, item);
        }

        /// <inheritdoc/>
        public ValueTask AddAsync(TItem item, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            Add(item);

            return ValueTask.CompletedTask;
        }


        /// <inheritdoc/>
        public TItem GetByIdentifier(TIdentifier identifier)
        {
            ArgumentsGuard.ThrowIfNull(identifier);

            return _collection[identifier];
        }

        /// <inheritdoc/>
        public ValueTask<TItem> GetByIdentifierAsync(TIdentifier identifier, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.FromResult(GetByIdentifier(identifier));
        }


        /// <inheritdoc/>
        public bool Contains(TItem item)
        {
            ArgumentsGuard.ThrowIfNull(item);

            return ContainsIdentifier(item.Identifier);
        }

        /// <inheritdoc/>
        public ValueTask<bool> ContainsAsync(TItem item, CancellationToken token = default)
        {
            return ContainsIdentifierAsync(item.Identifier, token);
        }


        /// <inheritdoc/>
        public bool ContainsIdentifier(TIdentifier identifier)
        {
            ArgumentsGuard.ThrowIfNull(identifier);

            return _collection.ContainsKey(identifier);
        }

        /// <inheritdoc/>
        public ValueTask<bool> ContainsIdentifierAsync(TIdentifier identifier, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.FromResult(ContainsIdentifier(identifier));
        }


        /// <inheritdoc/>
        public void Update(TItem item)
        {
            ThrowIfReadOnly();
            ArgumentsGuard.ThrowIfNull(item);
            /* ... and do nothing, I left the checking in the case when this repository will be changed in the future on another */
        }

        /// <inheritdoc/>
        public ValueTask UpdateAsync(TItem item, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.CompletedTask;
        }


        /// <inheritdoc/>
        public bool Remove(TItem item)
        {
            ThrowIfReadOnly();
            ArgumentsGuard.ThrowIfNull(item);

            return RemoveByIdentifier(item.Identifier);
        }

        /// <inheritdoc/>
        public ValueTask<bool> RemoveAsync(TItem item, CancellationToken token = default)
        {
            return RemoveByIdentifierAsync(item.Identifier);
        }


        /// <inheritdoc/>
        public bool RemoveByIdentifier(TIdentifier identifier)
        {
            ThrowIfReadOnly();
            ArgumentsGuard.ThrowIfNull(identifier);

            return _collection.Remove(identifier);
        }

        /// <inheritdoc/>
        public ValueTask<bool> RemoveByIdentifierAsync(TIdentifier identifier, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.FromResult(_collection.Remove(identifier));
        }

        /// <inheritdoc/>
        public IEnumerable<TItem> GetEnumerable(int? offset = null, int? limit = null, Expression<Func<TItem, bool>> predicate = null)
        {
            IEnumerable<TItem> result = _collection.Values;

            if (predicate != null)
                result = result.Where(predicate.Compile());

            if (offset.HasValue)
            {
                ArgumentsGuard.ThrowIfLessZero(offset.Value);

                result = result.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                ArgumentsGuard.ThrowIfLessOrEqualZero(limit.Value);

                result = result.Take(limit.Value);
            }

            return result;
        }

        /// <inheritdoc/>
        public ValueTask<IEnumerable<TItem>> GetEnumerableAsync(int? offset = null, int? limit = null, Expression<Func<TItem, bool>> predicate = null, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.FromResult(GetEnumerable(offset, limit, predicate));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfReadOnly()
        {
            if (IsReadOnly)
                throw new NotSupportedException("This is read-only repository.");
        }
    }
}
