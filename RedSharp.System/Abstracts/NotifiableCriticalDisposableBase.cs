﻿using System;
using System.Diagnostics;
using RedSharp.Sys.Interfaces.Shared;
using RedSharp.Sys.Models;

namespace RedSharp.Sys.Abstracts
{
    public abstract class NotifiableCriticalDisposableBase : CriticalDisposableBase, INotifyDisposing, INotifyDisposed
    {
        /// <inheritdoc/>
        public event EventHandler<DisposeEventArgs> Disposing;

        /// <inheritdoc/>
        public event EventHandler<DisposeEventArgs> Disposed;

        /// <inheritdoc/>
        protected internal override void SafeDispose(bool manual)
        {
            DisposeEventArgs arguments = null;

            if (Disposing != null)
            {
                arguments = new DisposeEventArgs(manual);

                try
                {
                    Disposing.Invoke(this, arguments);
                }
                catch (Exception exception)
                {
                    Trace.WriteLine(exception.Message);
                    Trace.WriteLine(exception.StackTrace);
                }
            }

            base.SafeDispose(manual);

            if (Disposed != null)
            {
                if (arguments == null)
                    arguments = new DisposeEventArgs(manual);

                try
                {
                    Disposed.Invoke(this, arguments);
                }
                catch (Exception exception)
                {
                    Trace.WriteLine(exception.Message);
                    Trace.WriteLine(exception.StackTrace);
                }
            }
        }
    }
}
