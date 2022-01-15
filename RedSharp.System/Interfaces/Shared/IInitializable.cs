using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Interfaces.Shared
{
    public interface IInitializable
    {
        void Initialize();
    }

    public interface IInitializable<TArgument>
    {
        void Initialize(TArgument argument);
    }
}
