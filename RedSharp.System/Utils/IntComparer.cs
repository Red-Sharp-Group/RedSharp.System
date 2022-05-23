using RedSharp.Sys.Abstracts;
using RedSharp.Sys.Enums;

namespace RedSharp.Sys.Utils
{
    public class IntComparer : ComparerBase<int>
    {
        public override Equality Compare(int first, int second)
        {
            if (first == second)
                return Equality.Equal;
            else if (first > second)
                return Equality.Greater;
            else
                return Equality.Less;
        }
    }
}
