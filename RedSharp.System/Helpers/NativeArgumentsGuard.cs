using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Helpers
{
    public static unsafe class NativeArgumentsGuard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(IntPtr value, String name)
        {
            if (value == IntPtr.Zero)
                throw new ArgumentNullException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(void* value, String name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfPointerIsOutOfRange(void* structurePointer, int structureSize, void* toCheckPointer, int toCheckSize, String name)
        {
            if ((long)toCheckPointer < (long)structurePointer ||
                (long)toCheckPointer + toCheckSize > (long)structurePointer + structureSize)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}
