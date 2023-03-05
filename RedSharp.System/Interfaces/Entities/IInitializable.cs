using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Interfaces.Entities
{
    public interface IInitializable
    {
        public void Initialize();
    }

    public interface IInitializable<TArguments>
    {
        public void Initialize(TArguments arguments);
    }
}
