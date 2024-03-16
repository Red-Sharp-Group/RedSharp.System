using System;
using System.Runtime.CompilerServices;

namespace RedSharp.Sys.Abstracts
{
    public abstract class ComparableBase : IComparable
    {
        public override bool Equals(object obj)
        {
            return InternalEquals(this, obj as ComparableBase) == 0;
        }

        public static bool operator ==(ComparableBase first, ComparableBase second)
        {
            return InternalEquals(first, second) == 0;
        }

        public static bool operator !=(ComparableBase first, ComparableBase second)
        {
            return InternalEquals(first, second) != 0;
        }

        public static bool operator >(ComparableBase first, ComparableBase second)
        {
            return InternalEquals(first, second) == 1;
        }

        public static bool operator <(ComparableBase first, ComparableBase second)
        {
            return InternalEquals(first, second) == -1;
        }

        public static bool operator >=(ComparableBase first, ComparableBase second)
        {
            var result = InternalEquals(first, second);

            return result == 1 || result == 0;
        }

        public static bool operator <=(ComparableBase first, ComparableBase second)
        {
            var result = InternalEquals(first, second);

            return result == -1 || result == 0;
        }

        public override int GetHashCode()
        {
            return this.ToString()
                       .ToLower()
                       .GetHashCode();
        }

        int IComparable.CompareTo(object obj)
        {
            var another = obj as ComparableBase;

            if (another == null)
                return 1;

            return CompareTo(another);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InternalEquals(ComparableBase first, ComparableBase second)
        {
            if (object.ReferenceEquals(first, second))
                return 0;
            else if (object.ReferenceEquals(second, null))
                return 1;
            else if (object.ReferenceEquals(first, null))
                return -1;
            else
                return first.CompareTo(second);
        }

        protected abstract int CompareTo(ComparableBase another);
    }
}
