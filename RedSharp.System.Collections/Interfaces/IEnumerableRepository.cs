using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace RedSharp.Sys.Collections.Interfaces
{
    public interface IEnumerableRepository<TItem>
    {
        /// <summary>
        /// Returns exact number of items on the invocation moment.
        /// <br/> If the parameter is null has to return total number of items.
        /// <br/> Uses <see cref="Expression"/> because it can be compiled into <see cref="Delegate"/> but otherwise - not.
        /// </summary>
        int Count(Expression<Func<TItem, bool>> predicate = null);

        /// <inheritdoc cref="GetCount"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<int> CountAsync(Expression<Func<TItem, bool>> predicate = null, CancellationToken token = default);


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
