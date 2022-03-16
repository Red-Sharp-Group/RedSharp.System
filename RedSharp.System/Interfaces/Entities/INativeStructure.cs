using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Interfaces.Entities
{
    /// <summary>
    /// Wraps the native structure. Extends the <see cref="INativeHandle"/> interface.
    /// </summary>
    public interface INativeStructure : INativeHandle
    {
        /// <summary>
        /// Shows the real size of the wrapped structure, which can be less than your expectation.
        /// </summary>
        int Size { get; }
    }
}
