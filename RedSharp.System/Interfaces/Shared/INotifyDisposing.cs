using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Models;

namespace RedSharp.Sys.Interfaces.Shared
{
    /// <summary>
    /// Special interface for the cases when you need to know when object starter disposing.
    /// </summary>
    public interface INotifyDisposing
    {
        /// <summary>
        /// Invokes before disposing process
        /// </summary>
        event EventHandler<DisposeEventArgs> Disposing;
    }
}
