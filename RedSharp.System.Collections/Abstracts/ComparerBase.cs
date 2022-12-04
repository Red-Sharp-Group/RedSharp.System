using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections.Abstracts
{
    /// <summary>
    /// Base class for comparing several objects.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public abstract class ComparerBase<TItem> : IComparer<TItem>, IEqualityComparer<TItem>, IComparer, IEqualityComparer
    {
        public ComparerBase(bool isAscending = true)
        {
            IsAscending = isAscending;
        }

        public bool IsAscending { get; private set; }

        /// <summary>
        /// Compares two objects and returns a value indicating whether 
        /// one is less than, equal to, or greater than the other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Compare(TItem first, TItem second)
        {
            var result = InternalCompare(first, second);

            if (!IsAscending)
                result *= -1;

            return result;
        }
        
        int IComparer.Compare(Object first, Object second)
        {
            return Compare((TItem)first, (TItem)second);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TItem first, TItem second)
        {
            return Compare(first, second) == 0;
        }

        bool IEqualityComparer.Equals(Object first, Object second)
        {
            return Equals((TItem)first, (TItem)second);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHashCode(TItem obj)
        {
            return InternalGetHashCode(obj);
        }

        int IEqualityComparer.GetHashCode(Object obj)
        { 
            ArgumentsGuard.ThrowIfNotType(obj, out TItem item);

            return GetHashCode(item);
        }

        /// <inheritdoc cref="Compare"/>
        protected abstract int InternalCompare(TItem first, TItem second);

        /// <inheritdoc cref="GetHashCode"/>
        protected virtual int InternalGetHashCode(TItem item)
        {
            ArgumentsGuard.ThrowIfNull(item);

            return item.GetHashCode();
        }
    }
}
