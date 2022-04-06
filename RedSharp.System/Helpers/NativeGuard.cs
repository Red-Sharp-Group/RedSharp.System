using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Helpers
{
    public static unsafe class NativeGuard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(IntPtr value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value == IntPtr.Zero)
                throw new ArgumentNullException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(void* value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfPointerIsOutOfRange(void* structurePointer, 
                                                      int structureSize, 
                                                      void* toCheckPointer, 
                                                      int toCheckSize, 
                                                      [CallerArgumentExpression("toCheckPointer")] String name = "toCheckPointer")
        {
            if ((long)toCheckPointer < (long)structurePointer ||
                (long)toCheckPointer + toCheckSize > (long)structurePointer + structureSize)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfPointerIsOutOfRange(IntPtr structurePointer, 
                                                      int structureSize,
                                                      IntPtr toCheckPointer, 
                                                      int toCheckSize, 
                                                      [CallerArgumentExpression("toCheckPointer")] String name = "toCheckPointer")
        {
            if ((long)toCheckPointer < (long)structurePointer ||
                (long)toCheckPointer + toCheckSize > (long)structurePointer + structureSize)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}
