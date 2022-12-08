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
    public class MemoryRepository<TIdentifier, TItem> : IRepository<TIdentifier, TItem>
    {
        private IDictionary<TIdentifier, TItem> _collection;
        private Func<TItem, TIdentifier> _identifierGetter;

        public MemoryRepository(IDictionary<TIdentifier, TItem> collection, bool isReadOnly = true, Func<TItem, TIdentifier> identifierGetter = null)
        {
            _collection = collection;
            IsReadOnly = isReadOnly;

            if (!isReadOnly)
                ArgumentsGuard.ThrowIfNull(identifierGetter);

            _identifierGetter = identifierGetter;
        }

        /// <inheritdoc/>
        public bool IsReadOnly { get; private set; }


        /// <inheritdoc/>
        public int Count(Expression<Func<TItem, bool>> predicate = null)
        {
            if (predicate == null)
                return _collection.Count;
            else
                return _collection.Values.Count(predicate.Compile());
        }

        /// <inheritdoc/>
        public ValueTask<int> CountAsync(Expression<Func<TItem, bool>> predicate = null, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.FromResult(Count(predicate));
        }


        /// <inheritdoc/>
        public void Add(TItem item)
        {
            ThrowIfReadOnly();
            ArgumentsGuard.ThrowIfNull(item);

            _collection.Add(_identifierGetter.Invoke(item), item);
        }

        /// <inheritdoc/>
        public ValueTask AddAsync(TItem item, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            Add(item);

            return ValueTask.CompletedTask;
        }


        /// <inheritdoc/>
        public TItem Get(TIdentifier identifier)
        {
            ArgumentsGuard.ThrowIfNull(identifier);

            return _collection[identifier];
        }

        /// <inheritdoc/>
        public ValueTask<TItem> GetAsync(TIdentifier identifier, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.FromResult(Get(identifier));
        }


        /// <inheritdoc/>
        public bool Contains(TIdentifier identifier)
        {
            ArgumentsGuard.ThrowIfNull(identifier);

            return _collection.ContainsKey(identifier);
        }

        /// <inheritdoc/>
        public ValueTask<bool> ContainsAsync(TIdentifier identifier, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.FromResult(Contains(identifier));
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
        public bool Remove(TIdentifier identifier)
        {
            ThrowIfReadOnly();
            ArgumentsGuard.ThrowIfNull(identifier);

            return _collection.Remove(identifier);
        }

        /// <inheritdoc/>
        public ValueTask<bool> RemoveAsync(TIdentifier identifier, CancellationToken token = default)
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
