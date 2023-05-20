using RedSharp.Sys.Collections.Abstracts;

namespace RedSharp.Sys.Collections.Utils
{
    public class IntComparer : ComparerBase<int>
    {
        /// <summary>
        /// Default instance with ascending order.
        /// </summary>
        public static readonly IntComparer Ascending;

        /// <summary>
        /// Default instance with descending order.
        /// </summary>
        public static readonly IntComparer Descending;

        static IntComparer()
        {
            Ascending = new IntComparer(true);
            Descending = new IntComparer(false);
        }

        public IntComparer(bool isAscending = true) : base(isAscending)
        { }

        protected override int InternalCompare(int first, int second)
        {
            return first - second;
        }
    }
}
