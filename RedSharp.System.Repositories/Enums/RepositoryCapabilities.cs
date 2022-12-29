using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Repositories.Enums
{
    [Flags]
    public enum RepositoryCapabilities
    {
        Reading = 0,
        Adding = 1,
        Updating = 2,
        Removing = 4,
    }
}
