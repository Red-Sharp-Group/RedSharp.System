﻿using System;
using System.Runtime.CompilerServices;

namespace RedSharp.Sys.Native.Helpers
{
    public static unsafe class NativeGuard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(IntPtr value, [CallerArgumentExpression("value")] string name = "value")
        {
            if (value == IntPtr.Zero)
                throw new ArgumentNullException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(void* value, [CallerArgumentExpression("value")] string name = "value")
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfPointerIsOutOfRange(void* structurePointer, 
                                                      int structureSize, 
                                                      void* toCheckPointer, 
                                                      int toCheckSize, 
                                                      [CallerArgumentExpression("toCheckPointer")] string name = "toCheckPointer")
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
                                                      [CallerArgumentExpression("toCheckPointer")] string name = "toCheckPointer")
        {
            if ((long)toCheckPointer < (long)structurePointer ||
                (long)toCheckPointer + toCheckSize > (long)structurePointer + structureSize)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}
