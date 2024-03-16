using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedSharp.Sys.Repositories.Interfaces
{
    public interface ICountableRepository
    {
        /// <summary>
        /// Returns exact number of items on the invocation moment.
        /// </summary>
        int Count();

        /// <inheritdoc cref="GetCount"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        Task<int> CountAsync(CancellationToken token = default);
    }
}
