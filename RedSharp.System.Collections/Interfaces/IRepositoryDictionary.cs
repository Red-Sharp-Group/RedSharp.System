using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RedSharp.Sys.Collections.Interfaces
{
    /// <summary>
    /// Special interface to make item compatible with <see cref="IRepositoryDictionary{TIdentifier, TItem}"/>
    /// without constructions like <see cref="KeyValuePair{TIdentifier, TItem}"/>
    /// </summary>
    /// <typeparam name="TIdentifier"></typeparam>
    public interface IRepositoryItem<TIdentifier>
    {
        /// <summary>
        /// Item identifier.
        /// </summary>
        TIdentifier Identifier { get; }
    }

    public interface IRepositoryDictionary<TIdentifier, TItem> : IRepositoryCollection<TItem> where TItem : IRepositoryItem<TIdentifier>
    {
        /// <summary>
        /// Returns first item with correspond identifier or default.
        /// </summary>
        TItem GetByIdentifier(TIdentifier identifier);

        /// <inheritdoc cref="GetByIdentifier"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<TItem> GetByIdentifierAsync(TIdentifier identifier, CancellationToken token = default);


        /// <summary>
        /// Check that the item identifier presented in the repository.
        /// </summary>
        /// <br/>Returns true if the item was actually removed.
        /// </summary>
        bool ContainsIdentifier(TIdentifier identifier);

        /// <inheritdoc cref="ContainsIdentifier"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<bool> ContainsIdentifierAsync(TIdentifier identifier, CancellationToken token = default);


        /// <summary>
        /// Removes item by identifier from the repository.
        /// <br/>Returns true if the item was actually removed.
        /// </summary>
        bool RemoveByIdentifier(TIdentifier identifier);

        /// <inheritdoc cref="RemoveByIdentifier"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<bool> RemoveByIdentifierAsync(TIdentifier identifier, CancellationToken token = default);
    }
}
