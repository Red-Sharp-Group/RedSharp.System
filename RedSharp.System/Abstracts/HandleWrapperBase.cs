using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Abstracts
{
    public abstract class HandleWrapperBase : NativeWrapperBase
    {
        public const String SecondTryToSetHandleError = "An attempt to set handle twice was detect.";
        public const String MethodReturnedUnexpectedValueError = "Method returns an unexpected result.";

        protected bool _isHandleOwner;
        protected IntPtr _handle;

        public override bool IsHandleOwner => _isHandleOwner;

        public override IntPtr UnsafeHandle => _handle;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetHandle(IntPtr handle)
        {
            if(_handle != IntPtr.Zero)
                throw new Exception(SecondTryToSetHandleError);

            ThrowIfDisposed();

            NativeArgumentsGuard.ThrowIfNull(handle, nameof(handle));

            _handle = handle;
            _isHandleOwner = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CreateHandle()
        {
            if (_handle != IntPtr.Zero)
                throw new Exception(SecondTryToSetHandleError);

            ThrowIfDisposed();

            _handle = Create();

            if (_handle == IntPtr.Zero)
                throw new Exception($"{nameof(Create)} {MethodReturnedUnexpectedValueError}");

            _isHandleOwner = true;
        }

        protected abstract IntPtr Create();

        protected abstract void Free(IntPtr handle);

        protected override void InternalDispose(bool manual)
        {
            if (_isHandleOwner)
                Free(_handle);

            _handle = IntPtr.Zero;

            base.InternalDispose(manual);
        }
    }
}
