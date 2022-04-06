using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Interfaces.Shared;

namespace RedSharp.Sys.Helpers
{
    public static class ArgumentsGuard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(Object value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfDisposed(IDisposeIndication value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value.IsDisposed)
                throw new ObjectDisposedException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessOrEqualZero(int value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessZero(int value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotInRange(int value, int start, int end, [CallerArgumentExpression("value")] String name = "value")
        {
            if (start > value || value > end)
                throw new ArgumentOutOfRangeException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNullOrEmpty(String value, [CallerArgumentExpression("value")] String name = "value")
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException(name, "String cannot be null or empty.");
        }
    }
}
