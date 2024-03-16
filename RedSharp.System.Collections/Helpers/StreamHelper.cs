using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RedSharp.Sys.Collections.Interfaces;
using RedSharp.Sys.Collections.Utils;
using RedSharp.Sys.Helpers;

namespace System.Collections.Generic
{
    public class StreamEnumerableWrapper : Stream
    {
        private IEnumerable<byte> _enumerable;
        private IEnumerator<byte> _enumerator;
        private long _position;

        public StreamEnumerableWrapper(IEnumerable<byte> enumerable)
        {
            ArgumentsGuard.ThrowIfNull(enumerable);

            _enumerable = enumerable;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => _enumerable.Count();

        public override long Position
        {
            get => _position;
            set => Seek(value, SeekOrigin.Begin);
        }

        public override void Flush()
        { /* do nothing */ }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ArgumentsGuard.ThrowIfNull(buffer);
            ArgumentsGuard.ThrowIfLessZero(offset);

            var length = count - offset;

            if (buffer.Length > count)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (_enumerator == null)
                _enumerator = _enumerable.GetEnumerator();

            for (int i = offset; i < length; i++)
            {
                if (_enumerator.MoveNext())
                {
                    buffer[i] = _enumerator.Current;

                    _position++;
                }
                else
                {
                    return i - offset + 1;
                }
            }

            return length;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (offset == 0 && origin == SeekOrigin.Current)
                return _position;

            if (offset == 0 && origin == SeekOrigin.Begin)
            {
                _enumerator = null;

                _position = 0;
            }

            //TODO

            return _position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }

    public static class EnumerableHelper
    {
        public static Stream ToStream(this IEnumerable<byte> enumerable)
        {
            ArgumentsGuard.ThrowIfNull(enumerable);

            return new StreamEnumerableWrapper(enumerable);
        }
    }
}

namespace System.IO
{
    public static class StreamHelper
    {
        public const int DefaultBufferSize = 64;

        public static IDisposableEnumerable<byte> ToDisposableEnumerable(this Stream stream, int bufferSize = DefaultBufferSize)
        {
            ArgumentsGuard.ThrowIfNull(stream);

            return new EnumerableStreamWrapper(stream, bufferSize, true);
        }

        public static IEnumerable<byte> ToEnumerable(this Stream stream, int bufferSize = DefaultBufferSize)
        {
            ArgumentsGuard.ThrowIfNull(stream);

            return new EnumerableStreamWrapper(stream, bufferSize, false);
        }

        public static IDisposableAsyncEnumerable<byte> ToDisposableAsyncEnumerable(this Stream stream, int bufferSize = DefaultBufferSize)
        {
            ArgumentsGuard.ThrowIfNull(stream);

            return new EnumerableStreamWrapper(stream, bufferSize, true);
        }

        public static IAsyncEnumerable<byte> ToAsyncEnumerable(this Stream stream, int bufferSize = DefaultBufferSize)
        {
            ArgumentsGuard.ThrowIfNull(stream);

            return new EnumerableStreamWrapper(stream, bufferSize, false);
        }
    }
}
