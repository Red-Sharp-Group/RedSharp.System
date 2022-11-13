using System;

namespace RedSharp.Sys.Abstracts
{
    public abstract class WideComparableBase : IComparable
    {
        public override bool Equals(object obj)
        {
            return InternalEquals(this, obj as WideComparableBase) == 0;
        }

        public static bool operator ==(WideComparableBase first, WideComparableBase second)
        {
            return InternalEquals(first, second) == 0;
        }

        public static bool operator !=(WideComparableBase first, WideComparableBase second)
        {
            return InternalEquals(first, second) != 0;
        }

        public static bool operator >(WideComparableBase first, WideComparableBase second)
        {
            return InternalEquals(first, second) == 1;
        }

        public static bool operator <(WideComparableBase first, WideComparableBase second)
        {
            return InternalEquals(first, second) == -1;
        }

        public static bool operator >=(WideComparableBase first, WideComparableBase second)
        {
            var result = InternalEquals(first, second);

            return result == 1 || result == 0;
        }

        public static bool operator <=(WideComparableBase first, WideComparableBase second)
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
            var another = obj as WideComparableBase;

            if (another == null)
                return 1;

            return CompareTo(another);
        }

        private static int InternalEquals(WideComparableBase first, WideComparableBase second)
        {
            if (Object.ReferenceEquals(first, second))
                return 0;
            else if (Object.ReferenceEquals(second, null))
                return 1;
            else if (Object.ReferenceEquals(first, null))
                return -1;
            else
                return first.CompareTo(second);
        }

        protected abstract int CompareTo(WideComparableBase another);
    }
}
