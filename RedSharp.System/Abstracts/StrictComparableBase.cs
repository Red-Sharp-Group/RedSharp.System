﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSharp.Sys.Abstracts
{
    public abstract class StrictComparableBase
    {
        public override bool Equals(object obj)
        {
            return InternalEquals(this, obj as StrictComparableBase);
        }

        public static bool operator ==(StrictComparableBase first, StrictComparableBase second)
        {
            return InternalEquals(first, second);
        }

        public static bool operator !=(StrictComparableBase first, StrictComparableBase second)
        {
            return !InternalEquals(first, second);
        }

        public override int GetHashCode()
        {
            return this.ToString()
                       .ToLower()
                       .GetHashCode();
        }

        private static bool InternalEquals(StrictComparableBase first, StrictComparableBase second)
        {
            if (Object.ReferenceEquals(first, second))
                return true;
            else if (!Object.ReferenceEquals(first, null) && !Object.ReferenceEquals(second, null))
                return first.CompareTo(second);
            else
                return false;
        }

        protected abstract bool CompareTo(StrictComparableBase another);
    }
}
