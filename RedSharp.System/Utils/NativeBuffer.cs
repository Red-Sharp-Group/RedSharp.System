using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Abstracts;
using RedSharp.Sys.Helpers;
using RedSharp.Sys.Interfaces.Entities;

namespace RedSharp.Sys.Utils
{
    /// <summary>
    /// This is like a array but in the native heap.
    /// </summary>
    /// <remarks>
    /// This guy can take zero length just to not make exclusions for the zero length cases (but this is pointless actually)
    /// <br/>But I'm not sure that the system allocator on another system can allocate zero length piece of memory (Windows can)
    /// </remarks>
    public unsafe class NativeBuffer<TSructure> : NativeStructureBase, IEnumerable<TSructure> where TSructure : unmanaged
    {
        /// <summary>
        /// Just to have an ability to LINQ this thing.
        /// </summary>
        private class NativeBufferEnumerator : IEnumerator<TSructure>
        {
            private NativeBuffer<TSructure> _buffer;
            private int _index;

            public NativeBufferEnumerator(NativeBuffer<TSructure> owner)
            {
                _buffer = owner;

                Reset();
            }

            public TSructure Current => _buffer[_index];

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                _index++;

                return _index < _buffer.Length;
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose()
            {
                Reset();
                _buffer = null;
            }
        }

        private INativeStructure _basicStructure;
        private int _length;

        /// <summary>
        /// Initialize an empty buffer with desired length.
        /// </summary>
        /// <remarks>
        /// This is stupid remark but the LENGTH is not a SIZE.
        /// </remarks>
        /// <exception cref="ArgumentException">If length is less then zero.</exception>
        public NativeBuffer(int length)
        {
            ArgumentsGuard.ThrowIfLessZero(length);

            AllocateMemory(length * sizeof(TSructure));

            _length = length;
        }

        /// <summary>
        /// Initialize a buffer from the input pointer with desired length.
        /// </summary>
        /// <remarks>
        /// This is stupid remark but the LENGTH is not a SIZE.
        /// <br/> This is a pretty UNSAFE constructor. Think twice before using it.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If pointer is null.</exception>
        /// <exception cref="ArgumentException">If length is less then zero.</exception>
        public NativeBuffer(IntPtr pointer, int length, bool makeCopy = true)
        {
            NativeGuard.ThrowIfNull(pointer);
            ArgumentsGuard.ThrowIfLessZero(length);

            if (makeCopy)
                CopyMemory(pointer, length * sizeof(TSructure));
            else
                SetMemory(pointer, length * sizeof(TSructure));
        }

        /// <summary>
        /// Creates a new buffer based on the input native structure with zero offset and input length.
        /// </summary>
        /// <remarks>
        /// This is stupid remark but the LENGTH is not a SIZE.
        /// </remarks>
        /// <exception cref="ArgumentException">If length is less then zero.</exception>
        /// <exception cref="ArgumentNullException">If the input structure is null.</exception>
        /// <exception cref="ObjectDisposedException">If the input structure is disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If a new buffer is bigger than the basic structure.</exception>
        public NativeBuffer(INativeStructure basic, int length, bool makeCopy = true) : this(basic, 0, length, makeCopy)
        { }

        /// <summary>
        /// Creates a new buffer based on the input native structure with desired offset and input length.
        /// </summary>
        /// <remarks>
        /// This is stupid remark but the LENGTH is not a SIZE, but offset in bytes.
        /// </remarks>
        /// <exception cref="ArgumentException">If length is less then zero.</exception>
        /// <exception cref="ArgumentNullException">If the input structure is null.</exception>
        /// <exception cref="ObjectDisposedException">If the input structure is disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If a new buffer is bigger than the basic structure.</exception>
        public NativeBuffer(INativeStructure basic, int offset, int length, bool makeCopy = true)
        {
            ArgumentsGuard.ThrowIfNull(basic);
            ArgumentsGuard.ThrowIfDisposed(basic);
            ArgumentsGuard.ThrowIfLessZero(length);

            var targetPointer = basic.UnsafeHandle + offset;
            var size = length * sizeof(TSructure);

            NativeGuard.ThrowIfPointerIsOutOfRange(basic.UnsafeHandle, basic.Size, targetPointer, size, "computed pointer");

            if (makeCopy)
            {
                CopyMemory(targetPointer, size);

                //We don't need to hold a basic structure because we copied this thing.
            }
            else
            {
                SetMemory(targetPointer, size);

                _basicStructure = basic;
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// If structure is created from other <see cref="INativeStructure"/> without coping 
        /// then <see cref="IsDisposed"/> will be connected to the basic <see cref="INativeStructure"/>
        /// </remarks>
        public override bool IsDisposed
        {
            get
            {
                if (_basicStructure == null)
                    return base.IsDisposed;
                else
                    return base.IsDisposed || _basicStructure.IsDisposed;
            }
        }

        /// <summary>
        /// Gets structure by index.
        /// </summary>
        /// <remarks>
        /// Still have no idea how it works, but it works.
        /// </remarks>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentOutOfRangeException">If you tries to get something outside of structure.</exception>
        public ref TSructure this[int index]
        {
            get 
            {
                ThrowIfDisposed();

                if (index < 0 || index >= _length)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return ref *(((TSructure*)_pointer) + index);
            }
        }

        /// <summary>
        /// Returns number of elements stored in buffer.
        /// </summary>
        /// <remarks>
        /// Do not confuse with size.
        /// </remarks>
        /// <exception cref="ObjectDisposedException"/>
        public int Length
        {
            get
            {
                ThrowIfDisposed();

                return _length;
            }
        }

        /// <summary>
        /// Returns <see cref="Span{T}"/> structure with desired length (or with computed length).
        /// </summary>
        /// <remarks>
        /// This is stupid remark but the LENGTH is not a SIZE.
        /// <br/> This is a pretty UNSAFE method, because <see cref="Span{T}"/> can stay live after buffer disposing. 
        /// Think twice before using it.
        /// </remarks>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="ArgumentOutOfRangeException">If the length is outside of buffer.</exception>
        public Span<TNSructure> AsSpanOf<TNSructure>(int? length = null) where TNSructure : unmanaged
        {
            ThrowIfDisposed();

            if (length.HasValue)
            {
                if (length * sizeof(TNSructure) > Size)
                    throw new ArgumentOutOfRangeException(nameof(length), "The desired length is more than the buffer size.");
            }
            else
            {
                length = Size / sizeof(TNSructure);
            }

            return new Span<TNSructure>(_pointer, length.Value);
        }

        /// <inheritdoc/>
        public IEnumerator<TSructure> GetEnumerator()
        {
            ThrowIfDisposed();

            return new NativeBufferEnumerator(this);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected override void InternalDispose(bool manual)
        {
            _basicStructure = null;

            base.InternalDispose(manual);
        }
    }
}
