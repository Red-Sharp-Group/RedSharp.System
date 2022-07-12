using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Interfaces.Shared
{
    public interface IFreezable
    {
        void Freeze();

        void Unfreeze();
    }

    public interface IFreezeIndication
    {
        bool IsFrozen { get; }
    }
}
