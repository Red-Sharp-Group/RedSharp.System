﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Enums;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Abstracts
{
    /// <summary>
    /// Base class for comparing several objects.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public abstract class ComparerBase<TItem> : IComparer<TItem>, IEqualityComparer<TItem>, IComparer, IEqualityComparer
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether 
        /// one is less than, equal to, or greater than the other.
        /// </summary>
        public abstract Equality Compare(TItem first, TItem second);

        int IComparer<TItem>.Compare(TItem first, TItem second)
        {
            return (int)Compare(first, second);
        }
        
        int IComparer.Compare(Object first, Object second)
        {
            return (int)Compare((TItem)first, (TItem)second);
        }

        bool IEqualityComparer<TItem>.Equals(TItem first, TItem second)
        {
            return Compare(first, second) == Equality.Equal;
        }

        bool IEqualityComparer.Equals(Object first, Object second)
        {
            return Compare((TItem)first, (TItem)second) == Equality.Equal;
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
