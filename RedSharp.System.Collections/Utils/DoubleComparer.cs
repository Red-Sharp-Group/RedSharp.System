using System;
using RedSharp.Sys.Collections.Abstracts;

namespace RedSharp.Sys.Collections.Utils
{
    public class DoubleComparer : ComparerBase<double>
    {
        public DoubleComparer(double approximationValue)
        {
            ApproximationValue = approximationValue;
        }

        public double ApproximationValue { get; private set;}

        public override int Compare(double first, double second)
        {
            if (Math.Abs(first - second) < ApproximationValue)
                return Equal;
            else if (first > second)
                return Greater;
            else
                return Less;
        }
    }
}
