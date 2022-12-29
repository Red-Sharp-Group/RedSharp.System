using System.Threading;
using System.Threading.Tasks;

namespace RedSharp.Sys.Repositories.Interfaces
{
    public interface IImmutableItemsRepository<TIdentifier, TItem, TParameters> : IRepository<TIdentifier, TItem>
    {
        /// <summary>
        /// Apply changes for the given item.
        /// </summary>
        TItem Update(TItem item, TParameters paramters);

        /// <inheritdoc cref="Update"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<TItem> UpdateAsync(TItem item, TParameters paramters, CancellationToken token = default);
    }
}
