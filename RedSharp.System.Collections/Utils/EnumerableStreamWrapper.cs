using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RedSharp.Sys.Abstracts;
using RedSharp.Sys.Collections.Interfaces;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections.Utils
{
    public class EnumerableStreamWrapper : DisposableBase, IDisposableEnumerable<byte>, IDisposableAsyncEnumerable<byte>
    {
        private class StreamEnumerator : IEnumerator<byte>
        {
            private Stream _stream;
            private byte[] _buffer;
            private int _length;
            private int _index;

            public StreamEnumerator(Stream stream, int bufferLength)
            {
                _stream = stream;
                _buffer = new byte[bufferLength];

                _index = -1;
            }

            object IEnumerator.Current => Current;

            public byte Current => _buffer[_index];

            public bool MoveNext()
            {
                if (_index == -1 || _index + 1 >= _length)
                {
                    _index = -1;

                    _length = _stream.Read(_buffer, 0, _buffer.Length);

                    if (_length == 0)
                        return false;
                }

                _index++;

                return true;
            }

            public void Reset()
            {
                _stream.Seek(0, SeekOrigin.Begin);
                _index = -1;
            }

            public void Dispose()
            { /* Do nothing */ }
        }

        private class AsyncStreamEnumerator : IAsyncEnumerator<byte>
        {
            private Stream _stream;
            private byte[] _buffer;
            private int _length;
            private int _index;
            private CancellationToken _token;

            public AsyncStreamEnumerator(Stream stream, int bufferLength, CancellationToken token)
            {
                _stream = stream;
                _buffer = new byte[bufferLength];
                _token = token;

                _index = -1;
            }

            public byte Current => _buffer[_index];

            public async ValueTask<bool> MoveNextAsync()
            {
                if (_index == -1 || _index + 1 >= _length)
                {
                    _index = -1;

                    _length = await _stream.ReadAsync(_buffer, 0, _buffer.Length, _token);

                    if (_length == 0)
                        return false;
                }

                _token.ThrowIfCancellationRequested();

                _index++;

                return true;
            }

            public ValueTask DisposeAsync()
            {
                return ValueTask.CompletedTask;
            }
        }

        private Stream _stream;
        private int _bufferSize;
        private bool _isStreamOwner;

        public EnumerableStreamWrapper(Stream stream, int bufferSize, bool isStreamOwner = true)
        {
            ArgumentsGuard.ThrowIfNull(stream);
            ArgumentsGuard.ThrowIfLessOrEqualZero(bufferSize);

            _stream = stream;
            _bufferSize = bufferSize;
            _isStreamOwner = isStreamOwner;

            if (!_isStreamOwner)
                GC.SuppressFinalize(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<byte> GetEnumerator()
        {
            ThrowIfDisposed();

            if (_stream.CanSeek && _stream.Position != 0)
                _stream.Seek(0, SeekOrigin.Begin);

            return new StreamEnumerator(_stream, _bufferSize);
        }

        public IAsyncEnumerator<byte> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (_stream.CanSeek && _stream.Position != 0)
                _stream.Seek(0, SeekOrigin.Begin);

            return new AsyncStreamEnumerator(_stream, _bufferSize, cancellationToken);
        }

        protected override void InternalDispose(bool manual)
        {
            base.InternalDispose(manual);

            if (_isStreamOwner)
                _stream.Dispose();
        }
    }
}
