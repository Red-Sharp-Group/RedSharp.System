using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Abstracts;
using RedSharp.Sys.Helpers;
using RedSharp.Sys.Interfaces.Entities;

namespace RedSharp.Sys.Utils
{
    /// <summary>
    /// Simple wrapper to manage a native structure 
    /// or create it in the managed environment.
    /// </summary>
    public unsafe class NativeWrapper<TStructure> : NativeStructureBase where TStructure : unmanaged
    {
        private const String SizesAreNotSameError = "Input size has to be more or equal to wrapped structure.";

        private INativeStructure _basicStructure;
        private TStructure* _structure;

        /// <summary>
        /// Allocates a zeroed piece of memory just in size of the structure.
        /// </summary>
        public NativeWrapper() : this(sizeof(TStructure))
        { }

        /// <summary>
        /// Allocates a zeroed piece of memory in the given size.
        /// </summary>
        /// <remarks>
        /// Size must be equal or more then size of the structure.
        /// </remarks>
        /// <exception cref="ArgumentException">If size is wrong.</exception>
        public NativeWrapper(int size)
        {
            ArgumentsGuard.ThrowIfLessZero(size);
            ThrowIfSizeIsLessThanStructure(size);

            AllocateMemory(size);

            _structure = (TStructure*)_pointer;
        }

        /// <summary>
        /// Sets or makes a copy of the structure by given pointer.
        /// </summary>
        /// <remarks>
        /// <br/> This is a pretty UNSAFE constructor. Think twice before using it.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If pointer is null.</exception>
        public NativeWrapper(IntPtr pointer, bool makeCopy = true) : this(pointer, sizeof(TStructure), makeCopy)
        { }

        /// <summary>
        /// Sets or makes a copy of the structure by given pointer.
        /// </summary>
        /// <remarks>
        /// Size must be equal or more then size of the structure.
        /// <br/> This is a pretty UNSAFE constructor. Think twice before using it.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If pointer is null.</exception>
        /// <exception cref="ArgumentException">If size is wrong.</exception>
        public NativeWrapper(IntPtr pointer, int size, bool makeCopy = true)
        {
            NativeGuard.ThrowIfNull(pointer);
            ArgumentsGuard.ThrowIfLessZero(size);
            ThrowIfSizeIsLessThanStructure(size);

            if (makeCopy)
                CopyMemory(pointer, size);
            else
                SetMemory(pointer, size);

            _structure = (TStructure*)_pointer;
        }

        /// <summary>
        /// Creates a new wrapper based on the input native structure with zero offset and default size.
        /// </summary>
        /// <exception cref="ArgumentException">If size is wrong.</exception>
        /// <exception cref="ArgumentNullException">If the input structure is null.</exception>
        /// <exception cref="ObjectDisposedException">If the input structure is disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If new structure is outside of the basic structure.</exception>
        public NativeWrapper(INativeStructure basic, bool makeCopy = true) : this (basic, 0, sizeof(TStructure), makeCopy)
        { }

        /// <summary>
        /// Creates a new wrapper based on the input native structure with zero offset and desired size.
        /// </summary>
        /// <exception cref="ArgumentException">If size is wrong.</exception>
        /// <exception cref="ArgumentNullException">If the input structure is null.</exception>
        /// <exception cref="ObjectDisposedException">If the input structure is disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If new structure is outside of the basic structure.</exception>
        public NativeWrapper(INativeStructure basic, int size, bool makeCopy = true) : this(basic, 0, size, makeCopy)
        { }

        /// <summary>
        /// Creates a new wrapper based on the input native structure with desired offset and default size.
        /// </summary>
        /// <exception cref="ArgumentException">If size is wrong.</exception>
        /// <exception cref="ArgumentNullException">If the input structure is null.</exception>
        /// <exception cref="ObjectDisposedException">If the input structure is disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If new structure is outside of the basic structure.</exception>
        public NativeWrapper(INativeStructure basic, int offset, int size, bool makeCopy = true)
        {
            ArgumentsGuard.ThrowIfNull(basic);
            ArgumentsGuard.ThrowIfDisposed(basic);
            ArgumentsGuard.ThrowIfLessZero(size);
            ThrowIfSizeIsLessThanStructure(size);

            var targetPointer = basic.UnsafeHandle + offset;

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

            _structure = (TStructure*)_pointer;
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
        /// Dereferenced pointer passed by ref.
        /// </summary>
        /// <remarks>
        /// I don't really know how it works, 
        /// but it works without creation a copy of a structure on the stack.
        /// </remarks>
        /// <exception cref="ObjectDisposedException"/>
        public ref TStructure Value
        {
            get
            {
                ThrowIfDisposed();

                return ref *_structure;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfSizeIsLessThanStructure(int value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value < sizeof(TStructure))
                throw new ArgumentOutOfRangeException(name, SizesAreNotSameError);
        }

        protected override void InternalDispose(bool manual)
        {
            _structure = null;
            _basicStructure = null;

            base.InternalDispose(manual);
        }
    }
}
