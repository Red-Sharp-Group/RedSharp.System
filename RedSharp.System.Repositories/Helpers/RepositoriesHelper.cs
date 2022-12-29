using System;
using System.Linq;
using System.Text;
using System.Transactions;
using RedSharp.Sys.Helpers;
using RedSharp.Sys.Repositories.Utils;

namespace RedSharp.Sys.Repositories.Interfaces
{
    public static class RepositoriesHelper
    {
        public const int DefaultLimit = 32;

        /// <summary>
        /// Wraps <see cref="IEnumerableRepository.GetEnumerable(int?, int?)"/> into an async enumerator pattern
        /// </summary>
        /// <remarks>
        /// As the foreach loop works with duck typing 
        /// I have decided to explicitly return structure
        /// to avoid creation of two new objects 
        /// (it returns enumerator as structure too)
        /// </remarks>
        public static RepositoryAsyncEnumerableWrapper<TItem> GetAsyncEnumerable<TItem>(this IEnumerableRepository<TItem> repository, int? limit = DefaultLimit)
        {
            ArgumentsGuard.ThrowIfNull(repository, nameof(repository));

            return new RepositoryAsyncEnumerableWrapper<TItem>(repository, limit);
        }
    }
}
