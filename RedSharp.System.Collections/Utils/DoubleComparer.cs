using System;
using RedSharp.Sys.Collections.Abstracts;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections.Utils
{
    public class DoubleComparer : ComparerBase<double>
    {
        /// <summary>
        /// Default instance of the class with <see cref="ArgumentsGuard.ApproximationValue"/> and ascending order.
        /// </summary>
        public static readonly DoubleComparer Ascending;

        /// <summary>
        /// Default instance of the class with <see cref="ArgumentsGuard.ApproximationValue"/> and descending order.
        /// </summary>
        public static readonly DoubleComparer Descending;

        static DoubleComparer()
        {
            Ascending = new DoubleComparer(ArgumentsGuard.ApproximationValue, true);
            Descending = new DoubleComparer(ArgumentsGuard.ApproximationValue, false);
        }

        public DoubleComparer(double approximationValue, bool isAscending = true) : base(isAscending)
        {
            ApproximationValue = approximationValue;
        }

        public double ApproximationValue { get; private set;}

        protected override int InternalCompare(double first, double second)
        {
            if (Math.Abs(first - second) < ApproximationValue)
                return 0;
            else if (first > second)
                return 1;
            else
                return -1;
        }
    }
}
