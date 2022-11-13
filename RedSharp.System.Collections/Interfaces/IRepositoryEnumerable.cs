using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace RedSharp.Sys.Collections.Interfaces
{
    public interface IRepositoryEnumerable<TItem>
    {
        /// <summary>
        /// Returns a set of items by given offset, limit and predicate.
        /// <br/> If all parameters are null has to return whole internal collection.
        /// <br/> Uses <see cref="Expression"/> because it can be compiled into <see cref="Delegate"/> but otherwise - not.
        /// </summary>
        IEnumerable<TItem> GetEnumerable(int? offset = null, int? limit = null, Expression<Func<TItem, bool>> predicate = null);

        /// <inheritdoc cref="Get"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<IEnumerable<TItem>> GetEnumerableAsync(int? offset = null, int? limit = null, Expression<Func<TItem, bool>> predicate = null, CancellationToken token = default);
    }
}
