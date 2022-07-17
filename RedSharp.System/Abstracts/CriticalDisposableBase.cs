﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using RedSharp.Sys.Interfaces.Shared;

namespace RedSharp.Sys.Abstracts
{
    /// <summary>
    /// Basic class which includes implementation of <see cref="IDisposable"/>, <see cref="IDisposeIndication"/> and <see cref="CriticalFinalizerObject"/>
    /// </summary>
    public abstract class CriticalDisposableBase : CriticalFinalizerObject, IDisposable, IDisposeIndication
    {
        ~CriticalDisposableBase()
        {
            SafeDisposed(false);
        }

        /// <inheritdoc/>
        public bool IsDisposed { get; private set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            SafeDisposed(true);

            IsDisposed = true;
        }

        /// <summary>
        /// Special private method that guaranties that dispose will not throw an exception in finalizer.
        /// </summary>
        /// <remarks>
        /// Do not change the method signature.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SafeDisposed(bool manual)
        {
            try
            {
                InternalDispose(manual);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                Trace.WriteLine(exception.StackTrace);
            }
        }

        /// <summary>
        /// Override this method to define disposing logic.
        /// </summary>
        protected virtual void InternalDispose(bool manual)
        { }

        /// <summary>
        /// Helper method that throws <see cref="ObjectDisposedException"/> if this object is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        protected virtual void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(IDisposable), DisposableBase.ObjectDisposedError);
        }
    }
}
