using System.Collections.Generic;
using System.Threading;
using RedSharp.Sys.Helpers;
using RedSharp.Sys.Repositories.Interfaces;

namespace RedSharp.Sys.Repositories.Utils
{
    public struct RepositoryAsyncEnumerableWrapper<TItem> : IAsyncEnumerable<TItem>
    {
        private IEnumerableRepository<TItem> _repository;
        private int? _limit;

        public RepositoryAsyncEnumerableWrapper(IEnumerableRepository<TItem> repository, int? limit)
        {
            ArgumentsGuard.ThrowIfNull(repository);

            _repository = repository;
            _limit = limit;
        }

        IAsyncEnumerator<TItem> IAsyncEnumerable<TItem>.GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return GetAsyncEnumerator(cancellationToken);
        }

        public RepositoryAsyncEnumeratorWrapper<TItem> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new RepositoryAsyncEnumeratorWrapper<TItem>(_repository, _limit, cancellationToken);
        }
    }
}
