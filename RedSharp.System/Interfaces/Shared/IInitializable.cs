using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Interfaces.Shared
{
    /// <summary>
    /// Interface is created for the objects that requires initialization not by constructor 
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Initializes object
        /// </summary>
        /// <remarks>
        /// Can be invoked several times without exceptions
        /// </remarks>
        public void Initialize();
    }

    /// <summary>
    /// Interface is created for the objects that requires initialization 
    /// with parameters and not by constructor 
    /// </summary>
    public interface IInitializable<TArguments>
    {
        /// <summary>
        /// Initializes object
        /// </summary>
        /// <remarks>
        /// Can be invoked several times without exceptions
        /// </remarks>
        public void Initialize(TArguments arguments);
    }
}
