using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedSharp.Sys.Repositories.Interfaces
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
        /// Returns exact number of items on the invocation moment.
        /// </summary>
        int Count();

        /// <inheritdoc cref="GetCount"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<int> CountAsync(CancellationToken token = default);


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
