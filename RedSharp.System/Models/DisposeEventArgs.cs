using System;

namespace RedSharp.Sys.Models
{
    /// <summary>
    /// Information about disposing process
    /// </summary>
    public class DisposeEventArgs : EventArgs
    {
        public DisposeEventArgs(bool isManual)
        {
            IsManual = isManual;
        }

        /// <summary>
        /// True if Dispose() was invoked manually, not by finalizer
        /// </summary>
        public bool IsManual { get; private set; }
    }
}
