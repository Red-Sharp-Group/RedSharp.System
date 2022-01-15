using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Interfaces.Shared;

namespace RedSharp.Sys.Abstracts
{
    public abstract class DisposableBase : IDisposable, IDisposeIndication
    {
        public const String ObjectDisposedError = "This object was disposed.";

        ~DisposableBase()
        {
            SafeDisposed(false);
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            SafeDisposed(true);

            IsDisposed = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SafeDisposed(bool manual)
        {
            try
            {
                InternalDispose(manual);
            }
            catch(Exception exception)
            {
                Trace.WriteLine(exception.Message);
                Trace.WriteLine(exception.StackTrace);
            }
        }

        protected virtual void InternalDispose(bool manual)
        { }

        protected virtual void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(IDisposable), ObjectDisposedError);
        }
    }
}
