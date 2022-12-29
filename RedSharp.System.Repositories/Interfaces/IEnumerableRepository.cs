using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RedSharp.Sys.Repositories.Interfaces
{
    public interface IEnumerableRepository<TItem>
    {
        //It is so much comfortable to work with only these two method in helper classes

        /// <summary>
        /// Returns a set of items by given offset and limit.
        /// <br/> If all parameters are null has to return whole internal collection.
        /// </summary>
        IEnumerable<TItem> GetEnumerable(int? offset = null, int? limit = null);

        /// <inheritdoc cref="GetEnumerable"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<IEnumerable<TItem>> GetEnumerableAsync(int? offset = null, int? limit = null, CancellationToken token = default);
    }
}
