﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Interfaces.Shared;

namespace RedSharp.Sys.Interfaces.Entities
{
    /// <summary>
    /// This interface was created for the objects that work as wrapper for another application.
    /// </summary>
    public interface IProcessKeeper : IDisposable, IDisposeIndication
    {
        /// <summary>
        /// Associated process.
        /// </summary>
        /// <remarks>
        /// Please do not turn off this: <see cref="Process.EnableRaisingEvents"/>
        /// </remarks>
        Process AssociatedProcess { get; }
    }
}
