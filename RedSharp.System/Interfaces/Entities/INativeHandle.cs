﻿using RedSharp.Sys.Interfaces.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Interfaces.Entities
{
    /// <summary>
    /// Generic interfaces for native structures, has to be used for example for the GDI handle
    /// </summary>
    public interface INativeHandle : IDisposable, IDisposeIndication
    {
        /// <summary>
        /// UNSAFE to use, the direct handle/pointer to the structure.
        /// </summary>
        IntPtr UnsafeHandle { get; }

        /// <summary>
        /// Shows that by <see cref="IDisposable.Dispose"/> the wrapped structure will be deleted too.
        /// </summary>
        bool IsHandleOwner { get; }
    }
}
