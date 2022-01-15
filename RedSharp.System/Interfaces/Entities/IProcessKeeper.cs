using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Interfaces.Shared;

namespace RedSharp.Sys.Interfaces.Entities
{
    public interface IProcessKeeper : IDisposable, IDisposeIndication
    {
        Process AssociatedProcess { get; }
    }
}
