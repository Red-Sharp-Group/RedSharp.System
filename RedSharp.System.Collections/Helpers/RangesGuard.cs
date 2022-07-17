using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Collections.Utils;

namespace RedSharp.Sys.Collections.Helpers
{
    public static class RangesGuard
    {
        //=========================================================================//
        //CHECK RANGE

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the input item is not in range.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotInRange<TItem>(TItem value, FlexibleRange<TItem> range, [CallerArgumentExpression("value")] String name = "value")
        {
            if (!range.IsInRange(value))
                throw new ArgumentOutOfRangeException(name);
        }
    }
}
