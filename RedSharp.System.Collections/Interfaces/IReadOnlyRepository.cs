using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using RedSharp.Sys.Interfaces.Shared;

namespace RedSharp.Sys.Collections.Interfaces
{
    public interface IReadOnlyRepository<TIdentifier, TItem> : IEnumerableRepository<TItem>
    {
        /// <summary>
        /// Returns first item with correspond identifier or default.
        /// </summary>
        TItem Get(TIdentifier identifier);

        /// <inheritdoc cref="GetByIdentifier"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<TItem> GetAsync(TIdentifier identifier, CancellationToken token = default);


        /// <summary>
        /// Check item existing in the repository
        /// <br/>Returns true if the item was actually removed.
        /// </summary>
        bool Contains(TIdentifier identifier);

        /// <inheritdoc cref="Contains"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<bool> ContainsAsync(TIdentifier identifier, CancellationToken token = default);
    }
}
