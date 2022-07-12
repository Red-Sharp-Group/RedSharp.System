using System;
using RedSharp.Sys.Abstracts;
using RedSharp.Sys.Enums;

namespace RedSharp.Sys.Utils
{
    public class DoubleComparer : ComparerBase<double>
    {
        public DoubleComparer(double approximationValue)
        {
            ApproximationValue = approximationValue;
        }

        public double ApproximationValue { get; private set;}

        public override Equality Compare(double first, double second)
        {
            if (Math.Abs(first - second) < ApproximationValue)
                return Equality.Equal;
            else if (first > second)
                return Equality.Greater;
            else
                return Equality.Less;
        }
    }
}
