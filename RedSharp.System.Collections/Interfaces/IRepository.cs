using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RedSharp.Sys.Interfaces.Shared;

namespace RedSharp.Sys.Collections.Interfaces
{
    public interface IRepository<TIdentifier, TItem> : IReadOnlyRepository<TIdentifier, TItem>, IReadableIndication
    {
        /// <summary>
        /// Adds new item to the repository.
        /// </summary>
        void Add(TItem item);

        /// <inheritdoc cref="Add"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask AddAsync(TItem item, CancellationToken token = default);


        /// <summary>
        /// Apply changes for the given item.
        /// </summary>
        void Update(TItem item);

        /// <inheritdoc cref="Update"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask UpdateAsync(TItem item, CancellationToken token = default);


        /// <summary>
        /// Removes item by identifier from the repository.
        /// <br/>Returns true if the item was actually removed.
        /// </summary>
        bool Remove(TIdentifier identifier);

        /// <inheritdoc cref="RemoveByIdentifier"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<bool> RemoveAsync(TIdentifier identifier, CancellationToken token = default);
    }
}
