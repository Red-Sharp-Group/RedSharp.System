using System;
using RedSharp.Sys.Models;

namespace RedSharp.Sys.Interfaces.Shared
{
    /// <summary>
    /// Special interface for the cases when you need to know when object starter was disposed.
    /// </summary>
    public interface INotifyDisposed
    {
        /// <summary>
        /// Invokes after disposing process
        /// </summary>
        event EventHandler<DisposeEventArgs> Disposed;
    }
}
