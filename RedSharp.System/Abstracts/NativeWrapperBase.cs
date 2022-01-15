using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Abstracts
{
    public abstract class NativeWrapperBase : DisposableBase
    {
        public abstract bool IsHandleOwner { get; }

        public abstract IntPtr UnsafeHandle { get; }
    }
}
