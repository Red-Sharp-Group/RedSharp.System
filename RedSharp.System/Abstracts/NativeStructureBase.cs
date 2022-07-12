using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Helpers;
using RedSharp.Sys.Interfaces.Entities;

namespace RedSharp.Sys.Abstracts
{
    /// <summary>
    /// Basic class for object which hold structure in unmanaged memory.
    /// </summary>
    public abstract unsafe class NativeStructureBase : DisposableBase, INativeStructure
    {
        public const String AlreadyHasAllocatedError = "Memory is already allocated";
        public const String OutOfStructureSizeError = "The method tries to access memory outside of the valid range.";
        public const String MethodReturnedUnexpectedValueError = "Method returns an unexpected result.";

        protected byte* _pointer;

        /// <inheritdoc/>
        public int Size { get; private set; }

        /// <inheritdoc/>
        public bool IsHandleOwner { get; private set; }

        /// <inheritdoc/>
        public IntPtr UnsafeHandle => new IntPtr(_pointer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfInitialized()
        {
            if (UnsafeHandle != IntPtr.Zero)
                throw new Exception(AlreadyHasAllocatedError);
        }

        /// <inheritdoc cref="SetMemory(byte*, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetMemory(IntPtr pointer, int size)
        {
            SetMemory((byte*)(void*)pointer, size);
        }

        /// <summary>
        /// Sets memory pointer.
        /// </summary>
        /// <remarks>
        /// <see cref="IsHandleOwner"/> will be false.
        /// </remarks>
        /// <exception cref="Exception">In case when you try to set pointer twice.</exception>
        /// <exception cref="ArgumentNullException">If input pointer is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If input size is less of equal zero.</exception>
        /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetMemory(byte* pointer, int size)
        {
            ThrowIfDisposed();
            ThrowIfInitialized();

            NativeGuard.ThrowIfNull(pointer);
            ArgumentsGuard.ThrowIfLessOrEqualZero(size);

            _pointer = pointer;
            Size = size;
            IsHandleOwner = false;
        }

        /// <summary>
        /// Creates a new piece of memory by given size.
        /// </summary>
        /// <remarks>
        /// <see cref="IsHandleOwner"/> will be true.
        /// </remarks>
        /// <exception cref="Exception">In case when you try to allocate memory twice.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If input size is less of equal zero.</exception>
        /// <exception cref="Exception">If <see cref="Allocate(int, out int)"/> returns an unexpected value.</exception>
        /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void AllocateMemory(int size)
        {
            ThrowIfDisposed();
            ThrowIfInitialized();

            ArgumentsGuard.ThrowIfLessOrEqualZero(size);

            _pointer = Allocate(size, out int allocatedSize);

            if (_pointer == null || allocatedSize <= 0)
                throw new Exception($"{nameof(Allocate)} {MethodReturnedUnexpectedValueError}");

            Size = allocatedSize;
            IsHandleOwner = false;
        }

        /// <inheritdoc cref="CopyMemory(byte*, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CopyMemory(IntPtr pointer, int size)
        {
            CopyMemory((byte*)(void*)pointer, size);
        }

        /// <summary>
        /// Creates a new piece of memory by given size and copies given data.
        /// </summary>
        /// <remarks>
        /// <see cref="IsHandleOwner"/> will be true.
        /// </remarks>
        /// <exception cref="Exception">In case when you try to allocate memory twice.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If input size is less of equal zero.</exception>
        /// <exception cref="Exception">If <see cref="Allocate(int, out int)"/> returns an unexpected value.</exception>
        /// <exception cref="ObjectDisposedException">If object is disposed.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CopyMemory(byte* pointer, int size)
        {
            ThrowIfDisposed();
            ThrowIfInitialized();

            NativeGuard.ThrowIfNull(pointer);
            ArgumentsGuard.ThrowIfLessOrEqualZero(size);

            _pointer = Allocate(size, out int allocatedSize);

            if (_pointer == null || allocatedSize <= 0)
                throw new Exception($"{nameof(Allocate)} {MethodReturnedUnexpectedValueError}");

            var inputSpan = new Span<byte>(pointer, allocatedSize);
            var destinationSpan = new Span<byte>(_pointer, allocatedSize);

            inputSpan.CopyTo(destinationSpan);

            Size = allocatedSize;
            IsHandleOwner = false;
        }

        /// <summary>
        /// Has to allocate new piece of memory for this object.
        /// </summary>
        /// <param name="size">Needed size.</param>
        /// <param name="allocatedSize">Actual size.</param>
        /// <returns>Pointer to the allocated memory</returns>
        protected virtual byte* Allocate(int size, out int allocatedSize)
        {
            allocatedSize = size;

            return (byte*)NativeMemory.AllocZeroed((nuint)size);
        }

        /// <summary>
        /// Frees allocated piece of memory.
        /// </summary>
        /// <remarks>
        /// Algorithm guarantees that the pointer is valid. 
        /// </remarks>
        protected virtual void Free(byte* pointer)
        {
            NativeMemory.Free(pointer);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected String GetString(byte* pointer, int size, Encoding encoding)
        {
            ThrowIfDisposed();

            NativeGuard.ThrowIfNull(pointer);
            ArgumentsGuard.ThrowIfLessOrEqualZero(size);
            NativeGuard.ThrowIfPointerIsOutOfRange(_pointer, Size, pointer, size);

            return FixedBuffersHelper.GetString(pointer, size, encoding);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetString(byte* pointer, int size, Encoding encoding, String value)
        {
            ThrowIfDisposed();

            NativeGuard.ThrowIfNull(pointer);
            ArgumentsGuard.ThrowIfLessOrEqualZero(size);
            NativeGuard.ThrowIfPointerIsOutOfRange(_pointer, Size, pointer, size);

            FixedBuffersHelper.SetString(pointer, size, encoding, value);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected TValue GetValue<TValue>(TValue* pointer) where TValue : unmanaged
        {
            ThrowIfDisposed();

            NativeGuard.ThrowIfNull(pointer);
            NativeGuard.ThrowIfPointerIsOutOfRange(_pointer, Size, pointer, sizeof(TValue));

            return *pointer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetValue<TValue>(TValue* pointer, TValue value) where TValue : unmanaged
        {
            ThrowIfDisposed();

            NativeGuard.ThrowIfNull(pointer);
            NativeGuard.ThrowIfPointerIsOutOfRange(_pointer, Size, pointer, sizeof(TValue));

            *pointer = value;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected TItem[] GetArray<TItem>(TItem* pointer, int length) where TItem : unmanaged
        {
            ThrowIfDisposed();

            NativeGuard.ThrowIfNull(pointer);
            NativeGuard.ThrowIfPointerIsOutOfRange(_pointer, Size, pointer, length * sizeof(TItem));

            return FixedBuffersHelper.GetArray(pointer, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetArray<TItem>(TItem* pointer, int length, TItem[] array) where TItem : unmanaged
        {
            ThrowIfDisposed();

            NativeGuard.ThrowIfNull(pointer);
            NativeGuard.ThrowIfPointerIsOutOfRange(_pointer, Size, pointer, length * sizeof(TItem));

            FixedBuffersHelper.SetArray(pointer, length, array);
        }


        protected override void InternalDispose(bool manual)
        {
            if (IsHandleOwner)
                Free(_pointer);

            _pointer = null;
            Size = 0;

            base.InternalDispose(manual);
        }
    }
}
