using System.Threading;
using System.Threading.Tasks;

namespace RedSharp.Sys.Repositories.Interfaces
{
    public interface IMutableItemsRepository<TIdentifier, TItem> : IRepository<TIdentifier, TItem>
    {
        /// <summary>
        /// Apply changes for the given item.
        /// </summary>
        void Update(TItem item);

        /// <inheritdoc cref="Update"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask UpdateAsync(TItem item, CancellationToken token = default);
    }
}
