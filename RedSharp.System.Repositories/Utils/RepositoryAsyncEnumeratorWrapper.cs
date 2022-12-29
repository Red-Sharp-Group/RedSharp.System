using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RedSharp.Sys.Helpers;
using RedSharp.Sys.Repositories.Interfaces;

namespace RedSharp.Sys.Repositories.Utils
{
    public struct RepositoryAsyncEnumeratorWrapper<TItem> : IAsyncEnumerator<TItem>
    {
        private IEnumerableRepository<TItem> _repository;
        private CancellationToken _token;

        private IEnumerator<TItem> _enumerator;
        private int _offset;
        private int? _limit;

        public RepositoryAsyncEnumeratorWrapper(IEnumerableRepository<TItem> repository, int? limit, CancellationToken token)
        {
            ArgumentsGuard.ThrowIfNull(repository);

            _repository = repository;
            _token = token;

            _enumerator = null;
            _offset = 0;
            _limit = limit;
        }

        public TItem Current
        {
            get
            {
                if (_enumerator == null)
                    return default;

                return _enumerator.Current;
            }
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            _token.ThrowIfCancellationRequested();

            if (_enumerator == null || !_enumerator.MoveNext())
            {
                var enumerable = await _repository.GetEnumerableAsync(_offset, _limit, _token);

                if (enumerable == null)
                    return false;

                _enumerator = enumerable.GetEnumerator();

                if (!_enumerator.MoveNext())
                    return false;
            }

            _offset++;

            return true;
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
