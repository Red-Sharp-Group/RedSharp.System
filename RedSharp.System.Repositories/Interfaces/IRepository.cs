using System.Threading;
using System.Threading.Tasks;
using RedSharp.Sys.Interfaces.Shared;
using RedSharp.Sys.Repositories.Enums;

namespace RedSharp.Sys.Repositories.Interfaces
{
    public interface IRepository<TIdentifier, TItem> : IReadOnlyRepository<TIdentifier, TItem>, IReadableIndication
    {
        /// <summary>
        /// This is FLAG!
        /// Describes potential actions that you can do with this repository.
        /// </summary>
        /// <remarks>
        /// Flags can be out of range than defined in <see cref="RepositoryCapabilities"/>.
        /// </remarks>
        RepositoryCapabilities Capabilities { get; }


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
