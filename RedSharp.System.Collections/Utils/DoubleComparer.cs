using System;
using RedSharp.Sys.Collections.Abstracts;

namespace RedSharp.Sys.Collections.Utils
{
    public class DoubleComparer : ComparerBase<double>
    {
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
