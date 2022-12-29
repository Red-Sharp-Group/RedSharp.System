using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RedSharp.Sys.Helpers;
using RedSharp.Sys.Repositories.Enums;
using RedSharp.Sys.Repositories.Interfaces;

namespace RedSharp.Sys.Repositories.Abstracts
{
    public abstract class MemoryRepositoryBase<TIdentifier, TItem> : IMutableItemsRepository<TIdentifier, TItem>
    {
        private IDictionary<TIdentifier, TItem> _collection;

        public MemoryRepositoryBase(RepositoryCapabilities caps = RepositoryCapabilities.Reading) : this(new Dictionary<TIdentifier, TItem>(), caps)
        { }

        public MemoryRepositoryBase(IDictionary<TIdentifier, TItem> collection, RepositoryCapabilities caps = RepositoryCapabilities.Reading)
        {
            ArgumentsGuard.ThrowIfNull(collection);

            _collection = collection;

            Capabilities = caps;
        }


        /// <inheritdoc/>
        public bool IsReadOnly => Capabilities == RepositoryCapabilities.Reading;

        /// <inheritdoc/>
        public RepositoryCapabilities Capabilities { get; private set; }


        /// <inheritdoc/>
        public virtual IEnumerable<TItem> GetEnumerable(int? offset = null, int? limit = null)
        {
            IEnumerable<TItem> result = _collection.Values;

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
        public ValueTask<IEnumerable<TItem>> GetEnumerableAsync(int? offset = null, int? limit = null, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.FromResult(GetEnumerable(offset, limit));
        }


        /// <inheritdoc/>
        public virtual TItem Get(TIdentifier identifier)
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
        public int Count()
        {
            return _collection.Count;
        }

        /// <inheritdoc/>
        public ValueTask<int> CountAsync(CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.FromResult(Count());
        }


        /// <inheritdoc/>
        public virtual void Add(TItem item)
        {
            if (!Capabilities.HasFlag(RepositoryCapabilities.Adding))
                throw new NotSupportedException();

            ArgumentsGuard.ThrowIfNull(item);

            _collection.Add(GetIdentifier(item), item);
        }

        /// <inheritdoc/>
        public ValueTask AddAsync(TItem item, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            Add(item);

            return ValueTask.CompletedTask;
        }


        /// <inheritdoc/>
        public virtual bool Contains(TIdentifier identifier)
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
        public virtual void Update(TItem item)
        {
            if (!Capabilities.HasFlag(RepositoryCapabilities.Updating))
                throw new NotSupportedException();

            ArgumentsGuard.ThrowIfNull(item);
            /* ... and do nothing, I left the checking in the case when this repository will be changed in the future on another */
        }

        /// <inheritdoc/>
        public ValueTask UpdateAsync(TItem item, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            Update(item);

            return ValueTask.CompletedTask;
        }


        /// <inheritdoc/>
        public virtual bool Remove(TIdentifier identifier)
        {
            if (!Capabilities.HasFlag(RepositoryCapabilities.Removing))
                throw new NotSupportedException();

            ArgumentsGuard.ThrowIfNull(identifier);

            return _collection.Remove(identifier);
        }

        /// <inheritdoc/>
        public ValueTask<bool> RemoveAsync(TIdentifier identifier, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return ValueTask.FromResult(_collection.Remove(identifier));
        }

        protected abstract TIdentifier GetIdentifier(TItem item);
    }
}
