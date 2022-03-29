using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Abstracts;
using RedSharp.Sys.Helpers;

//TODO comment this

namespace RedSharp.Sys.Utils
{
    public unsafe class NativeWrapper<TStructure> : NativeStructureBase where TStructure : unmanaged
    {
        private TStructure* _structure;

        public NativeWrapper() : this(sizeof(TStructure))
        { }

        public NativeWrapper(int size)
        {
            ArgumentsGuard.ThrowIfLessZero(size);
            ThrowIfSizeIsLessThanStructure(size);

            AllocateMemory(size);

            _structure = (TStructure*)_pointer;
        }

        public NativeWrapper(IntPtr pointer, bool makeCopy = true) : this(pointer, sizeof(TStructure), makeCopy)
        { }

        public NativeWrapper(IntPtr pointer, int size, bool makeCopy = true)
        {
            NativeGuard.ThrowIfNull(pointer);
            ArgumentsGuard.ThrowIfLessZero(size);
            ThrowIfSizeIsLessThanStructure(size);

            if (makeCopy)
                CopyMemory((byte*)(void*)pointer, size);
            else
                SetMemory((byte*)(void*)pointer, size);

            _structure = (TStructure*)_pointer;
        }

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
                throw new ArgumentOutOfRangeException(name, "Input size has to be more or equal to wrapped structure.");
        }

        protected override void InternalDispose(bool manual)
        {
            _structure = null;

            base.InternalDispose(manual);
        }
    }
}
