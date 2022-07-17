using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Interfaces.Shared
{
    /// <summary>
    /// Another implementation of initializable objects 
    /// that allows you to do actions and then freeze object state.
    /// </summary>
    public interface IFreezable
    {
        /// <summary>
        /// Disallows changing the object state.
        /// </summary>
        void Freeze();

        /// <summary>
        /// Allows changing the object state.
        /// </summary>
        /// <remarks>
        /// Be careful: may throw <see cref="NotSupportedException"/>, usually object only freezable.
        /// </remarks>
        void Unfreeze();
    }
}
