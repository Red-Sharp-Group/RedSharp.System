using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Abstracts
{
    public abstract unsafe class NativeStructureWrapperBase : NativeWrapperBase
    {
        public const String SecondTryToSetMemoryError = "An attempt to set memory twice was detect.";
        public const String OutOfStructureSizeError = "The method tries to access memory outside of the valid range.";
        public const String MethodReturnedUnexpectedValueError = "Method returns an unexpected result.";

        protected int _size;
        protected bool _isStructureOwner;
        protected byte* _pointer;

        public int StructureSize => _size;

        public override bool IsHandleOwner => _isStructureOwner;

        public override IntPtr UnsafeHandle
        {
            get
            {
                ThrowIfDisposed();

                return new IntPtr(_pointer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetMemory(byte* pointer, int size)
        {
            if (_pointer != null)
                throw new Exception(SecondTryToSetMemoryError);

            ThrowIfDisposed();

            NativeArgumentsGuard.ThrowIfNull(pointer, nameof(pointer));
            ArgumentsGuard.ThrowIfLessOrEqualZero(size, nameof(size));

            _pointer = pointer;
            _size = size;
            _isStructureOwner = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void AllocateMemory(int size)
        {
            if (_pointer != null)
                throw new Exception(SecondTryToSetMemoryError);

            ThrowIfDisposed();

            ArgumentsGuard.ThrowIfLessOrEqualZero(size, nameof(size));

            _pointer = Allocate(size, out _size);

            if (_pointer == null || _size <= 0)
                throw new Exception($"{nameof(Allocate)} {MethodReturnedUnexpectedValueError}");

            _isStructureOwner = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CopyMemory(byte* pointer, int size)
        {
            if (_pointer != null)
                throw new Exception(SecondTryToSetMemoryError);

            ThrowIfDisposed();

            NativeArgumentsGuard.ThrowIfNull(pointer, nameof(pointer));
            ArgumentsGuard.ThrowIfLessOrEqualZero(size, nameof(size));

            _pointer = Allocate(size, out _size);

            if (_pointer == null || _size <= 0)
                throw new Exception($"{nameof(Allocate)} {MethodReturnedUnexpectedValueError}");

            var inputSpan = new Span<byte>(pointer, _size);
            var destinationSpan = new Span<byte>(_pointer, _size);

            inputSpan.CopyTo(destinationSpan);

            _isStructureOwner = true;
        }

        protected virtual byte* Allocate(int size, out int allocatedSize)
        {
            allocatedSize = size;

            return (byte*)NativeMemory.AllocZeroed((nuint)size);
        }

        protected virtual void Free(byte* pointer)
        {
            NativeMemory.Free(pointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected String GetString(byte* pointer, int size, Encoding encoding)
        {
            ThrowIfDisposed();

            NativeArgumentsGuard.ThrowIfNull(pointer, nameof(pointer));
            ArgumentsGuard.ThrowIfLessOrEqualZero(size, nameof(size));
            NativeArgumentsGuard.ThrowIfPointerIsOutOfRange(_pointer, _size, pointer, size, nameof(pointer));
            ArgumentsGuard.ThrowIfNull(encoding, nameof(encoding));

            return encoding.GetString(new Span<byte>(pointer, size));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetString(byte* pointer, int size, Encoding encoding, String value)
        {
            ThrowIfDisposed();

            NativeArgumentsGuard.ThrowIfNull(pointer, nameof(pointer));
            ArgumentsGuard.ThrowIfLessOrEqualZero(size, nameof(size));
            NativeArgumentsGuard.ThrowIfPointerIsOutOfRange(_pointer, _size, pointer, size, nameof(pointer));
            ArgumentsGuard.ThrowIfNull(encoding, nameof(encoding));

            var span = new Span<byte>(pointer, size);

            if (String.IsNullOrWhiteSpace(value))
                span.Fill(0);

            encoding.GetBytes(value, span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected TValue GetValue<TValue>(TValue* pointer) where TValue : unmanaged
        {
            ThrowIfDisposed();

            NativeArgumentsGuard.ThrowIfNull(pointer, nameof(pointer));
            NativeArgumentsGuard.ThrowIfPointerIsOutOfRange(_pointer, _size, pointer, sizeof(TValue), nameof(pointer));

            return *pointer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetValue<TValue>(TValue* pointer, TValue value) where TValue : unmanaged
        {
            ThrowIfDisposed();

            NativeArgumentsGuard.ThrowIfNull(pointer, nameof(pointer));
            NativeArgumentsGuard.ThrowIfPointerIsOutOfRange(_pointer, _size, pointer, sizeof(TValue), nameof(pointer));

            *pointer = value;
        }

        protected override void InternalDispose(bool manual)
        {
            if (_isStructureOwner)
                Free(_pointer);

            _pointer = null;
            _size = 0;

            base.InternalDispose(manual);
        }
    }
}
