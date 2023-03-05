using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Interfaces.Shared
{
    /// <summary>
    /// This interface is need to distinguish one interface implementation from other, in case when you have several of them.
    /// </summary>
    public interface IIdentity
    {
        /// <summary>
        /// Any kind of information that is unique for the current realization.
        /// </summary>
        string Identity { get; }
    }
}
