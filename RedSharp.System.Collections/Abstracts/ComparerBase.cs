using System;
using System.Collections;
using System.Collections.Generic;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections.Abstracts
{
    /// <summary>
    /// Base class for comparing several objects.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public abstract class ComparerBase<TItem> : IComparer<TItem>, IEqualityComparer<TItem>, IComparer, IEqualityComparer
    {
        public const int Less = -1;
        public const int Equal = 0;
        public const int Greater = 1;

        /// <summary>
        /// Compares two objects and returns a value indicating whether 
        /// one is less than, equal to, or greater than the other.
        /// </summary>
        public abstract int Compare(TItem first, TItem second);
        
        int IComparer.Compare(Object first, Object second)
        {
            return Compare((TItem)first, (TItem)second);
        }

        bool IEqualityComparer<TItem>.Equals(TItem first, TItem second)
        {
            return Compare(first, second) == Equal;
        }

        bool IEqualityComparer.Equals(Object first, Object second)
        {
            return Compare((TItem)first, (TItem)second) == Equal;
        }

        int IEqualityComparer<TItem>.GetHashCode(TItem obj)
        {
            ArgumentsGuard.ThrowIfNull(obj);

            return obj.GetHashCode();
        }

        int IEqualityComparer.GetHashCode(Object obj)
        { 
            ArgumentsGuard.ThrowIfNull(obj);
            ArgumentsGuard.ThrowIfNotType<TItem>(obj);

            return obj.GetHashCode();
        }
    }
}
