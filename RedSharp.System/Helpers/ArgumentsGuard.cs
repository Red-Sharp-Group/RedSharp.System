using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Helpers
{
    public static class ArgumentsGuard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(Object value, String name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessOrEqualZero(int value, String name)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(name);
        }
    }
}
